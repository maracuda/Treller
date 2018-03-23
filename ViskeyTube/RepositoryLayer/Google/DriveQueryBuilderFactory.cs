namespace ViskeyTube.RepositoryLayer.Google
{
    public class DriveQueryBuilderFactory : IDriveQueryBuilderFactory
    {
        public IDriveQueryBuilder Create()
        {
            return new DriveQueryBuilder();
        }
    }
}