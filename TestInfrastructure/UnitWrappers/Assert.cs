using System;

namespace SKBKontur.Treller.Tests.UnitWrappers
{
    public static class Assert
    {
        public static void AreEqual(dynamic a, dynamic b)
        {
            NUnit.Framework.Assert.AreEqual(a, b);
        }

        public static void AreNotEqual(dynamic a, dynamic b)
        {
            NUnit.Framework.Assert.AreNotEqual(a, b);
        }

        public static void True(bool condition, string message = null)
        {
            NUnit.Framework.Assert.True(condition, message);
        }

        public static void False(bool condition, string message = null)
        {
            NUnit.Framework.Assert.False(condition, message);
        }

        public static void Fail(string message = null)
        {
            NUnit.Framework.Assert.Fail(message);
        }

        public static void Throws(Type exceptionType, Action testAction, string message = null)
        {
            NUnit.Framework.Assert.Throws(exceptionType, () => testAction(), message);
        }

        public static void Throws<T>(Action testAction, string message = null) where T : Exception
        {
            NUnit.Framework.Assert.Throws<T>(() => testAction(), message);
        }

        public static void AreDeepEqual<T>(T actual, T expected, string message = null)
        {
            NUnit.Framework.Assert.That(actual, new DataContractConstraint<T>(expected), message);
        }
    }
}