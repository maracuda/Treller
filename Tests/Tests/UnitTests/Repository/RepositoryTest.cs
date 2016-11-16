using System;
using System.Collections.Generic;
using Xunit;
using Rhino.Mocks;
using SKBKontur.Infrastructure.Common;
using SKBKontur.TaskManagerClient.Repository;
using SKBKontur.TaskManagerClient.Repository.BusinessObjects;
using SKBKontur.TaskManagerClient.Repository.Clients;
using Assert = SKBKontur.Treller.Tests.UnitWrappers.Assert;

namespace SKBKontur.Treller.Tests.Tests.UnitTests.Repository
{
    public class RepositoryTest : UnitTest
    {
        private IRepositorySettings repositorySettings;
        private IRepositoryClientFactory repositoryClientFactory;
        private TaskManagerClient.Repository.Repository repository;
        private IDateTimeFactory dateTimeFactory;
        private IRepositoryClient repositoryClient;

        public RepositoryTest() : base()
        {
            repositorySettings = mock.Create<IRepositorySettings>();
            repositoryClientFactory = mock.Create <IRepositoryClientFactory>();
            dateTimeFactory = mock.Create<IDateTimeFactory>();
            repositoryClient = mock.Create<IRepositoryClient>();

            using (mock.Record())
            {
                var repoId = DataGenerator.GenDigitString(5);
                repositorySettings.Expect(f => f.RepositoryId).Return(repoId);
                repositoryClientFactory.Expect(f => f.CreateGitLabClient(repoId)).Return(repositoryClient);
            }

            repository = new TaskManagerClient.Repository.Repository(repositorySettings, repositoryClientFactory, dateTimeFactory);
        }

        [Fact]
        public void TestSearchForOldBranches()
        {
            var olderThan = TimeSpan.FromDays(1);
            var now = DateTime.Now;
            var branch1 = GenerateBranch(now);
            var branch2 = GenerateBranch(now.Subtract(olderThan).Subtract(TimeSpan.FromTicks(1)));
            var branch3 = GenerateBranch(now.Subtract(olderThan));

            using (mock.Record())
            {
                repositorySettings.Stub(f => f.NotTrackedBrancheNames).Return(new HashSet<string>());
                dateTimeFactory.Expect(f => f.Now).Return(now);
                repositoryClient.Expect(f => f.SelectAllBranches()).Return(new[] {branch1, branch2, branch3});
            }

            var actual = repository.SearchForOldBranches(olderThan);
            Assert.AreEqual(new[] {branch2}, actual);
        }

        [Fact]
        public void TestSearchForOldBranchesWhenFilterNonTrackingBranch()
        {
            var olderThan = TimeSpan.FromDays(1);
            var now = DateTime.Now;
            var branch = GenerateBranch(now.Subtract(olderThan).Subtract(TimeSpan.FromTicks(1)));
            var nontrackingBranch = GenerateBranch(now.Subtract(olderThan).Subtract(TimeSpan.FromTicks(1)));

            using (mock.Record())
            {
                repositorySettings.Stub(f => f.NotTrackedBrancheNames).Return(new HashSet<string>() { nontrackingBranch.Name });
                dateTimeFactory.Expect(f => f.Now).Return(now);
                repositoryClient.Expect(f => f.SelectAllBranches()).Return(new[] { branch, nontrackingBranch});
            }

            var actual = repository.SearchForOldBranches(olderThan);
            Assert.AreEqual(new[] {branch}, actual);
        }
        [Fact]
        public void TestSearchForOldBranchesInPeriod()
        {
            var now = DateTime.Now;
            var branch1 = GenerateBranch(now);
            var branch2 = GenerateBranch(now.Subtract(TimeSpan.FromDays(1).Add(TimeSpan.FromMilliseconds(1))));
            var branch3 = GenerateBranch(now.Subtract(TimeSpan.FromDays(2)));

            using (mock.Record())
            {
                repositorySettings.Stub(f => f.NotTrackedBrancheNames).Return(new HashSet<string>());
                dateTimeFactory.Expect(f => f.Now).Return(now);
                repositoryClient.Expect(f => f.SelectAllBranches()).Return(new[] { branch1, branch2, branch3 });
            }

            var actual = repository.SearchForOldBranches(TimeSpan.FromDays(1), TimeSpan.FromDays(2));
            Assert.AreEqual(new[] { branch2 }, actual);
        }

        [Fact]
        public void TestSearchForOldBranchesWithInvalidPeriod()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                repository.SearchForOldBranches(TimeSpan.FromDays(2), TimeSpan.FromDays(1));
            });
        }

        private static Branch GenerateBranch(DateTime lastCommitDate)
        {
            return new Branch
            {
                Name = DataGenerator.GenEnglishString(15),
                Commit = new BranchLastCommit
                {
                    Committed_date = lastCommitDate,
                    Message = DataGenerator.GenRussainString(30),
                    Id = DataGenerator.GenEnglishString(15),
                    Committer_email = DataGenerator.GenEmail()
                }
            };
        }
    }
}