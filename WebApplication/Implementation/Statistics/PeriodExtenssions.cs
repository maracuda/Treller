using System;

namespace SKBKontur.Treller.WebApplication.Implementation.Statistics
{
    public static class PeriodExtenssions
    {
        public static TimeSpan CalculatePeriod(this DateTime beginDate, DateTime endDate)
        {
            TimeSpan result = endDate - beginDate;
            if (result.TotalDays < 1)
            {
                return result;
            }

            while (true)
            {
                if (beginDate >= endDate)
                {
                    return result;
                }

                beginDate = beginDate.AddDays(1);
                if ((beginDate.Date.DayOfWeek == DayOfWeek.Saturday || beginDate.Date.DayOfWeek == DayOfWeek.Sunday) && result.TotalDays > 1)
                {
                    result = result.Subtract(new TimeSpan(1, 0, 0, 0));
                }
            }
        }

    }
}