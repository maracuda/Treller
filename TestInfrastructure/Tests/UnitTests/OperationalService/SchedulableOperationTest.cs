using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Rhino.Mocks;
using SKBKontur.Infrastructure.Common;
using SKBKontur.Treller.WebApplication.Implementation.Services.Operationals.Operations;

namespace SKBKontur.Treller.Tests.Tests.UnitTests.OperationalService
{
    public class SchedulableOperationTest : UnitTest
    {
        private IDateTimeFactory dateTimeFactory;

        public override void SetUp()
        {
            base.SetUp();

            dateTimeFactory = mock.Create<IDateTimeFactory>();
        }

        [Test]
        public void TestRunScheduledOperation()
        {
            using (mock.Record())
            {
                dateTimeFactory.Expect(f => f.Now).Return(DateTime.Now.Date.AddHours(11));
            }

            var i = 0;
            var operation = new ScheduledRegularOperation(dateTimeFactory, "zzz", TimeSpan.FromMilliseconds(10), TimeSpan.FromHours(10), TimeSpan.FromHours(12), () => { i++; });
            var operationResult = operation.Run();
            Assert.IsFalse(operationResult.HasValue);
            Assert.AreEqual(1, i);
        }

        [Test]
        public void TestRunScheduledOperationWithException()
        {
            using (mock.Record())
            {
                dateTimeFactory.Expect(f => f.Now).Return(DateTime.Now.Date.AddHours(11));
            }

            var exception = new Exception();
            var operation = new ScheduledRegularOperation(dateTimeFactory, "zzz", TimeSpan.FromMilliseconds(10), TimeSpan.FromHours(10), TimeSpan.FromHours(12), () => { throw exception; });
            var operationResult = operation.Run();
            Assert.IsTrue(operationResult.HasValue);
            Assert.AreEqual(exception, operationResult.Value);
        }

        [Test]
        public void TestIdleRunScheduledOperationWhenGreaterUpperBound()
        {
            using (mock.Record())
            {
                dateTimeFactory.Expect(f => f.Now).Return(DateTime.Now.Date.AddHours(12));
            }

            var i = 0;
            var operation = new ScheduledRegularOperation(dateTimeFactory, "zzz", TimeSpan.FromMilliseconds(10), TimeSpan.FromHours(10), TimeSpan.FromHours(12), () => { i++; });
            var operationResult = operation.Run();
            Assert.IsFalse(operationResult.HasValue);
            Assert.AreEqual(0, i);
        }

        [Test]
        public void TestIdleRunScheduledOperationWhenLesserLowerBound()
        {
            using (mock.Record())
            {
                dateTimeFactory.Expect(f => f.Now).Return(DateTime.Now.Date.AddHours(10));
            }

            var i = 0;
            var operation = new ScheduledRegularOperation(dateTimeFactory, "zzz", TimeSpan.FromMilliseconds(10), TimeSpan.FromHours(10), TimeSpan.FromHours(12), () => { i++; });
            var operationResult = operation.Run();
            Assert.IsFalse(operationResult.HasValue);
            Assert.AreEqual(0, i);
        }

        [Test]
        public void TestRunScheduledOperationTwiceAtOneTime()
        {
            using (mock.Record())
            {
                dateTimeFactory.Stub(f => f.Now).Return(DateTime.Now.Date.AddHours(11));
            }

            var i = 0;
            const int timeoutMs = 500;
            var operation = new ScheduledRegularOperation(dateTimeFactory, "zzz", TimeSpan.FromMilliseconds(10), TimeSpan.FromHours(10), TimeSpan.FromHours(12),
                                        () => { i++; Thread.Sleep(timeoutMs); });

            Task.Run(() =>
            {
                Assert.AreEqual(OperationState.Idle, operation.State);
                var firstRunResult = operation.Run();
                Assert.IsFalse(firstRunResult.HasValue);
            });

            Task.Run(() =>
            {
                Assert.AreEqual(OperationState.Running, operation.State);
                var secondRunResult = operation.Run();
                Assert.IsFalse(secondRunResult.HasValue);
            });
            Thread.Sleep(timeoutMs + 50);
            Assert.AreEqual(OperationState.Idle, operation.State);
            Assert.AreEqual(1, i);
        }
    }
}