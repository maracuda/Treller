using Google.Apis.YouTube.v3;

namespace ViskeyTube.CloudShare
{
    public static class RequestExtensions
    {
        private static ChannelsResource.ListRequest Mine(this ChannelsResource.ListRequest request, bool mine)
        {
            request.Mine = mine;
            return request;
        }

        public static ChannelsResource.ListRequest Mine(this ChannelsResource.ListRequest request)
        {
            return request.Mine(true);
        }
    }
}