using System;

namespace SKBKontur.Treller.Tests.Tests.UnitTests.Mocks
{
    public static class MockExtensions
    {
        public static void Expect<T>(this T mock, Action<T> action, int repeatTimes = 1, params object[] outParameters) where T : class
        {
            var options = Rhino.Mocks.RhinoMocksExtensions.Expect(mock, action).Repeat.Times(repeatTimes);
            if (outParameters != null)
            {
                options.OutRef(outParameters);
            }
        }

        public static TResult Expect<T, TResult>(this T mock, Func<T, TResult> function, TResult expectedResult, int repeatTimes = 1, params object[] outParamters) where T : class
        {
            var result = Rhino.Mocks.RhinoMocksExtensions.Expect(mock, x => function(x)).Return(expectedResult).Repeat.Times(repeatTimes);
            if (outParamters != null)
            {
                result.OutRef(outParamters);
            }
            return expectedResult;
        }
    }
}