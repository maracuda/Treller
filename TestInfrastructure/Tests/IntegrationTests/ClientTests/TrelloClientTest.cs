using System.Linq;
using NUnit.Framework;
using SKBKontur.TaskManagerClient;
using SKBKontur.TestInfrastructure.UnitWrappers;
using Assert = SKBKontur.TestInfrastructure.UnitWrappers.Assert;

namespace SKBKontur.TestInfrastructure.Tests.IntegrationTests.ClientTests
{
    public class TrelloClientTest : IntegrationTest
    {
        private ITaskManagerClient trelloClient;

        public override void SetUp()
        {
            base.SetUp();

            trelloClient = container.Get<ITaskManagerClient>();
        }

        [MyTest]
        public void TestGetBoards()
        {
            var actualBoards = trelloClient.GetAllBoards("konturbilling");
            Assert.True(actualBoards.Length > 2);
            var actualOpenBoards = trelloClient.GetOpenBoards("konturbilling");
            Assert.True(actualOpenBoards.Length > 2);
            CollectionAssert.IsSubsetOf(actualOpenBoards.Select(x => x.Name), actualBoards.Select(x => x.Name));
        }
    }
}