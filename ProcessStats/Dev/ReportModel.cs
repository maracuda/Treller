namespace ProcessStats.Dev
{
    public class ReportModel
    {
        public ReportModel(string name, string content)
        {
            Name = name;
            Content = content;
        }

        public string Name { get; set; }
        public string Content { get; set; }
    }
}