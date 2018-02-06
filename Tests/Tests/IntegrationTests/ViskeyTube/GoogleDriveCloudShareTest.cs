﻿using System.IO;
using System.Linq;
using ViskeyTube.CloudShare;
using Xunit;

namespace Tests.Tests.IntegrationTests.ViskeyTube
{
    public class GoogleDriveCloudShareTest : IntegrationTest
    {
        private readonly GoogleDriveCloudShare googleDriveCloudShare;

        private const string BillingChannelId = "UCiGKUGNeK8-KHPpRqxZoYcw";
        private const string BillingGooglePhotoFolderId = "17K6Nj556UL2ylNYzh2lXPpYzq7Bif8tl";

        public GoogleDriveCloudShareTest()
        {
            googleDriveCloudShare = new GoogleDriveCloudShare(credentialsService.GoogleApiKey,
                                                              credentialsService.GoogleClientSecret,
                                                              container.Get<IDriveQueryBuilderFactory>());
        }

        [Fact]
        public void AbleToDownloadFile()
        {
            var bytes = googleDriveCloudShare.DownloadFile("1XNz2OF6xYiKPObWvBpczLx13Xossk6kR");
            Assert.True(bytes.Length > 0);
            File.WriteAllBytes("downloadResult.png", bytes);
        }

        [Fact]
        public void AbleToGetFilesFromFolder()
        {
            var fileNames = googleDriveCloudShare.GetFiles(BillingGooglePhotoFolderId);
            Assert.True(fileNames.Length > 0);
        }

        [Fact]
        public void AbleToMoveFileToYouTube()
        {
            var files = googleDriveCloudShare.GetFiles(BillingGooglePhotoFolderId);
            var videoFile = files.Single(x => x.Name.Contains("29 декабря 2017"));
            var bytes = googleDriveCloudShare.DownloadFile(videoFile.FileId);

            var videoToUpload = new VideoToUpload
            {
                Title = "TEST - " + videoFile.Name,
                Description = "Test NightWhiskey"
            };

            var result = googleDriveCloudShare.UploadToYouTube(bytes, videoToUpload, BillingChannelId);
            Assert.True(result.Success, result.Exception?.Message);
        }

        [Fact]
        public void AbleToGetMyVideos()
        {
            var videos = googleDriveCloudShare.GetVideos(BillingChannelId);
            var files = googleDriveCloudShare.GetFiles(BillingGooglePhotoFolderId);
            Assert.True(videos.Any(v => files.Any(f => v.IsProbablyTheSameAs(f.Name, f.Size))));
        }

        [Fact]
        public void AbleToMoveVideoToPlaylist()
        {
            googleDriveCloudShare.AddVideoToPlayList("MmBhsdxoCYE", "PLsUe9ECHg2WapJxV0CQVUCw_dy9oWomSX");
        }

        [Fact]
        public void AbleToGetMyChannels()
        {
            var files = googleDriveCloudShare.GetMyChannels();
            Assert.True(files.Length > 0);
        }
    }
}