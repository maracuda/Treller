using System;
using ViskeyTube.DomainLayer.Common;
using Xunit;

namespace Tests.Tests.IntegrationTests.ViskeyTube
{
    public class DateTimeHelpersTest
    {
        [Theory]
        [InlineData("2017-01-24", "Бла бла бла 24 января 2017 г.")]
        [InlineData("2017-02-24", "Бла бла бла 24 февраля 2017 г.")]
        [InlineData("2017-03-24", "Бла бла бла 24 марта 2017 г.")]
        [InlineData("2017-04-24", "Бла бла бла 24 апреля 2017 г.")]
        [InlineData("2017-05-24", "Бла бла бла 24 мая 2017 г.")]
        [InlineData("2017-06-24", "Бла бла бла 24 июня 2017 г.")]
        [InlineData("2017-07-24", "Бла бла бла 24 июля 2017 г.")]
        [InlineData("2017-08-24", "Бла бла бла 24 августа 2017 г.")]
        [InlineData("2017-09-24", "Бла бла бла 24 сентября 2017 г.")]
        [InlineData("2017-10-24", "Бла бла бла 24 октября 2017 г.")]
        [InlineData("2017-11-24", "Бла бла бла 24 ноября 2017 г.")]
        [InlineData("2017-12-24", "Бла бла бла 24 декабря 2017 г.")]
        public void AbleToParseDateTime(string expected, string source)
        {
            var actual = DateTimeHelpers.ExtractRussianDateTime(source);
            Assert.Equal(DateTime.Parse(expected), actual);
        }
    }
}