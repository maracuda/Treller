using System;

namespace ViskeyTube.CloudShare
{
    public interface IWhiskeyTubeService
    {
        UploadResult[] SyncByGoogleDrive(string folderId, string channelId, string playlistId);

        UploadResult[] SyncByGoogleDrive(DateTime inclusiveFromDate, DateTime inclusiveEndDate, string wikiArchivePageId, string driveFolderId, string youtubeChannelId);
        UploadResult[] SyncByWiki(DateTime inclusiveFromDate, DateTime inclusiveEndDate, string wikiArchivePageId, string driveFolderId, string youtubeChannelId);
    }
}