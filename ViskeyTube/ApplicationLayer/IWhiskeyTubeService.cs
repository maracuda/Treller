using System;
using ViskeyTube.CloudShare;

namespace ViskeyTube.ApplicationLayer
{
    public interface IWhiskeyTubeService
    {
        UploadResultDto[] SyncByGoogleDrive(string folderId, string channelId, string playlistId);

        UploadResultDto[] SyncByGoogleDrive(DateTime inclusiveFromDate, DateTime inclusiveEndDate, string wikiArchivePageId, string driveFolderId, string youtubeChannelId);
        UploadResultDto[] SyncByWiki(DateTime inclusiveFromDate, DateTime inclusiveEndDate, string wikiArchivePageId, string driveFolderId, string youtubeChannelId);
    }
}