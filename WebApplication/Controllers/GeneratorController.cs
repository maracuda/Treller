using System.Web.Mvc;
using SKBKontur.Treller.WebApplication.Implementation.PhraseGeneration;

namespace SKBKontur.Treller.WebApplication.Controllers
{
    public class GeneratorController : Controller
    {
        private readonly IPhraseGenerator phraseGenerator;

        public GeneratorController(IPhraseGenerator phraseGenerator)
        {
            this.phraseGenerator = phraseGenerator;
        }

        public ActionResult Index()
        {
            var phrase = phraseGenerator.GenerateRandomPhrase();
            return View("Index", (object)phrase);
        }
    }
}