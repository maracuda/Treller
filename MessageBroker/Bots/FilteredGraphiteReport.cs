using System.Net;
using Metrics;
using Metrics.Graphite;
using Metrics.MetricData;
using Metrics.Utils;

namespace MessageBroker.Bots
{
    public class FilteredGraphiteReport : GraphiteReport
    {
        private readonly string contextPrefix;

        public FilteredGraphiteReport(GraphiteSender sender) : base(sender)
        {
            contextPrefix = $"Billy.Treller.{Dns.GetHostName()}.";
        }

        protected override void ReportCounter(string name, CounterValue value, Unit unit, MetricTags tags)
        {
            if (value.Items.Length == 0)
            {
                Send(Name(name, unit), value.Count);
            }
            else
            {
                Send(SubfolderName(name, unit, "Total"), value.Count);
            }
        }

        protected override void ReportMeter(string name, MeterValue value, Unit unit, TimeUnit rateUnit, MetricTags tags)
        {
            Send(SubfolderName(name, unit, "Total"), value.Count);
        }

        protected override void ReportHistogram(string name, HistogramValue value, Unit unit, MetricTags tags)
        {
            Send(SubfolderName(name, unit, "Count"), value.Count);
            Send(SubfolderName(name, unit, "Max"), value.Max);
            Send(SubfolderName(name, unit, "Median"), value.Median);
            Send(SubfolderName(name, unit, "p95"), value.Percentile95);
        }

        protected override void ReportTimer(string name, TimerValue value, Unit unit, TimeUnit rateUnit, TimeUnit durationUnit, MetricTags tags)
        {
            Send(SubfolderName(name, unit, "Count"), value.Rate.Count);
            Send(SubfolderName(name, durationUnit.Unit(), "Duration-Max"), value.Histogram.Max);
            Send(SubfolderName(name, durationUnit.Unit(), "Duration-Median"), value.Histogram.Median);
            Send(SubfolderName(name, durationUnit.Unit(), "Duration-p95"), value.Histogram.Percentile95);
        }

        protected override string FormatMetricName<T>(string context, MetricValueSource<T> metric)
        {
            return contextPrefix + metric.Name;
        }
    }
}