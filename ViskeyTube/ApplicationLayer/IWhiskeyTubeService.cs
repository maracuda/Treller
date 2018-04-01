using System;
using ViskeyTube.CloudShare;

namespace ViskeyTube.ApplicationLayer
{
    public interface IWhiskeyTubeService
    {
        UploadResultDto[] SyncByWiki(DateTime inclusiveFromDate, DateTime inclusiveEndDate, string wikiArchivePageId, string driveFolderId, string youtubeChannelId);
    }
}