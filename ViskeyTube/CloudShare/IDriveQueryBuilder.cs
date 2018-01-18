namespace ViskeyTube.CloudShare
{
    public interface IDriveQueryBuilder
    {
        IDriveQueryBuilder InFolder(string folderId);
        IDriveQueryBuilder And();
        string ToQueryString();
    }
}