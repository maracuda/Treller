namespace SKBKontur.Treller.WebApplication.Services.ActiveDirectory
{
    public class AdService : IAdService
    {
        public AdCredentials GetDeliveryCredentials()
        {
            return new AdCredentials("kontur", "maylo", "KorANgot22");
        }
    }
}