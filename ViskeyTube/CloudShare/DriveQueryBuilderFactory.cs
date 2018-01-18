namespace ViskeyTube.CloudShare
{
    public class DriveQueryBuilderFactory : IDriveQueryBuilderFactory
    {
        public IDriveQueryBuilder Create()
        {
            return new DriveQueryBuilder();
        }
    }
}