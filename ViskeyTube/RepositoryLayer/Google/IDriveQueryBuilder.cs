namespace ViskeyTube.RepositoryLayer.Google
{
    public interface IDriveQueryBuilder
    {
        IDriveQueryBuilder InFolder(string folderId);
        IDriveQueryBuilder And();
        string ToQueryString();
    }
}