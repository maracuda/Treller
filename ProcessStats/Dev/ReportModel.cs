namespace ProcessStats.Dev
{
    public class ReportModel
    {
        public ReportModel(string name, byte[] content)
        {
            Name = name;
            Content = content;
        }

        public string Name { get; }
        public byte[] Content { get; }
    }
}