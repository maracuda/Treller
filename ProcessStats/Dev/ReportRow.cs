using System;
using System.Collections.Generic;
using System.Linq;

namespace ProcessStats.Dev
{
    public class ReportRow
    {
        private readonly IList<object> values;

        private ReportRow(object[] initialValues)
        {
            values = new List<object>(initialValues);
        }

        public object[] Values => values.ToArray();

        public void Append(object value)
        {
            values.Add(value);
        }

        public static ReportRow Create(params object[] values)
        {
            if (values == null || values.Length == 0)
            {
                throw new ArgumentException("Fail to create report row from null or empty array of objects.");
            } 

            return new ReportRow(values);
        }

        public static ReportRow Empty = new ReportRow(new object[0]);
    }
}