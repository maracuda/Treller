using System;

namespace SKBKontur.TestInfrastructure.MockWrappers
{
    public static class MockExtensions
    {
        public static void Expect<T>(this T mockedObject, Action<T> action, int repeatTimes = 1, params object[] outParameters) where T : class
        {
            var options = Rhino.Mocks.RhinoMocksExtensions.Expect(mockedObject, action).Repeat.Times(repeatTimes);
            if (outParameters != null)
            {
                options.OutRef(outParameters);
            }
        }

        public static TResult Expect<T, TResult>(this T mockedObject, Func<T, TResult> function, TResult expectedResult, int repeatTimes = 1, params object[] outParamters) where T : class
        {
            var result = Rhino.Mocks.RhinoMocksExtensions.Expect(mockedObject, x => function(x)).Return(expectedResult).Repeat.Times(repeatTimes);
            if (outParamters != null)
            {
                result.OutRef(outParamters);
            }
            return expectedResult;
        }
    }
}