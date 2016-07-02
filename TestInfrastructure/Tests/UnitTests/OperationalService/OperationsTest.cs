using System;
using NUnit.Framework;
using Rhino.Mocks;
using SKBKontur.Infrastructure.Common;
using SKBKontur.Treller.WebApplication.Implementation.Services.Operationals.Operations;

namespace SKBKontur.Treller.Tests.Tests.UnitTests.OperationalService
{
    public class OperationsTest : UnitTest
    {
        private IDateTimeFactory dateTimeFactory;

        public override void SetUp()
        {
            base.SetUp();

            dateTimeFactory = mock.Create<IDateTimeFactory>();
        }

        [Test]
        public void TestRunSimpleOperation()
        {
            var i = 0;
            var operation = new SimpleOperation("zzz", TimeSpan.FromMilliseconds(10), () => { i++; });
            var operationResult = operation.Run();
            Assert.IsFalse(operationResult.HasValue);
            Assert.AreEqual(1, i);
        }

        [Test]
        public void TestRunSimpleOperationWithException()
        {
            var exception = new Exception();
            var operation = new SimpleOperation("zzz", TimeSpan.FromMilliseconds(10), () => { throw exception; });
            var operationResult = operation.Run();
            Assert.IsTrue(operationResult.HasValue);
            Assert.AreEqual(exception, operationResult.Value);
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
    }
}