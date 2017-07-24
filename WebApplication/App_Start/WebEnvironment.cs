using System.Web;
using Storage;

namespace WebApplication
{
    public class WebEnvironment : IEnvironment
    {
        public string BasePath => HttpRuntime.AppDomainAppPath;
    }
}