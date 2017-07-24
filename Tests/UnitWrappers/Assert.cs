using System;

namespace Tests.UnitWrappers
{
    public static class Assert
    {
        public static void AreEqual(dynamic a, dynamic b)
        {
            Xunit.Assert.Equal(a, b);
        }

        public static void AreNotEqual(dynamic a, dynamic b)
        {
            Xunit.Assert.NotEqual(a, b);
        }

        public static void True(bool condition, string message = null)
        {
            Xunit.Assert.True(condition, message);
        }

        public static void False(bool condition, string message = null)
        {
            Xunit.Assert.False(condition, message);
        }

        public static void Fail(string message = null)
        {
            Xunit.Assert.True(false, message);
        }

        public static void Throws(Type exceptionType, Action testAction, string message = null)
        {
            var exception = Xunit.Assert.Throws(exceptionType, () => testAction());
            Xunit.Assert.Equal(message, exception.Message);
        }

        public static void Throws<T>(Action testAction, string message = null) where T : Exception
        {
            var exception = Xunit.Assert.Throws<T>(() => testAction());
            Xunit.Assert.Equal(message, exception.Message);
        }

        public static void AreDeepEqual<T>(T actual, T expected, string message = null)
        {
            Xunit.Assert.True(ObjectComparer.AreEqual(actual, expected), message);
        }

        public static void IsNull(Object obj)
        {
            Xunit.Assert.Null(obj);
        }

        public static void IsNotNull(Object obj)
        {
            Xunit.Assert.NotNull(obj);
        }
    }
}