namespace SKBKontur.Treller.WebApplication.Services.ActiveDirectory
{
    public class AdCredentials
    {
        public AdCredentials(string domain, string login, string password)
        {
            Domain = domain;
            Login = login;
            Password = password;
        }

        public string Domain { get; private set; }
        public string Login { get; private set; }
        public string DomainLogin { get { return string.Format(@"{0}\{1}", Domain ?? ".", Login); } }
        public string Password { get; private set; }
    }
}