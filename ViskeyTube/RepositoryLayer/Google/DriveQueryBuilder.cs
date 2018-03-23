using System.Text;

namespace ViskeyTube.RepositoryLayer.Google
{
    public class DriveQueryBuilder : IDriveQueryBuilder
    {
        private readonly StringBuilder sb = new StringBuilder();

        public IDriveQueryBuilder InFolder(string folderId)
        {
            sb.Append($" '{folderId}' in parents");
            return this;
        }

        public IDriveQueryBuilder And()
        {
            sb.Append(" and");
            return this;
        }

        public string ToQueryString()
        {
            return sb.ToString();
        }
    }
}