using System.Web;
using SKBKontur.Treller.Storage;

namespace SKBKontur.Treller.WebApplication
{
    public class WebEnvironment : IEnvironment
    {
        public string BasePath => HttpRuntime.AppDomainAppPath;
    }
}