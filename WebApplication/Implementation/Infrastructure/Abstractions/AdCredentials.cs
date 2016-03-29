namespace SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Abstractions
{
    public class AdCredentials
    {
        public string Domain { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public string DomainLogin { get { return string.Format(@"{0}\{1}", Domain ?? ".", Login); } }
    }
}