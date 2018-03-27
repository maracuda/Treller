using System;

namespace MessageBroker.Messages
{
    public class DataRow
    {
        private DataRow(object[] values)
        {
            Values = values;
        }

        public object[] Values { get; }

        public static DataRow Create(params object[] values)
        {
            if (values == null)
                throw new ArgumentException("Fail to create data row from null.");

            return new DataRow(values);
        }
    }
}