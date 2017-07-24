using Tests.Tests.UnitTests.Mocks;
using Xunit;

namespace Tests.Tests.UnitTests
{
    public class MockExtensionTest : UnitTest
    {
        [Fact]
        public void MainWorkerShouldOrganizeWork()
        {
            const string enterstring = "enterString";

            var assistent1 = mockRepository.Create<IAssistent>();
            var assistent2 = mockRepository.Create<IAssistent>();
            var assistent3 = mockRepository.Create<IAssistent>();

            var mainWorker = new MainWorker(assistent1, assistent2, assistent3);

            var outstring1 = "outString1";
            var outstring2 = "outString2";
            var expected = "outString3";
            
            using(mockRepository.Record())
            {
                assistent1.Expect(x => x.EditText(enterstring), outstring1);
                assistent2.Expect(x => x.EditText(outstring1), outstring2);
                assistent3.Expect(x => x.EditText(outstring2), expected);
            }

            var actual = mainWorker.Organize(enterstring);

            UnitWrappers.Assert.AreEqual(actual, expected);
        }

        [Fact]
        public void AssembleAssistantShouldAssembleText()
        {
            var assistant = new AssembleAssistant();

            var actual = assistant.EditText("text");

            UnitWrappers.Assert.AreEqual(actual, "text text");
        }

        [Fact]
        public void SummurizeAssistantShouldSummurize()
        {
            var assistant = new SummurizeAssistant();

            var actual = assistant.EditText("text");

            UnitWrappers.Assert.AreEqual(actual, "text 4");
        }
    }

    public class SummurizeAssistant : IAssistent
    {
        public string EditText(string text)
        {
            return $"{text} {text.Length}";
        }
    }

    public class AssembleAssistant : IAssistent
    {
        public string EditText(string text)
        {
            return $"{text} {text}";
        }
    }


    public class MainWorker
    {
        private readonly IAssistent[] assistents;

        public MainWorker(params IAssistent[] assistents)
        {
            this.assistents = assistents;
        }
        
        public string Organize(string text)
        {
            string result = text;

            foreach (var assistent in assistents)
            {
                result = assistent.EditText(result);
            }

            return result;
        }
    }

    public interface IAssistent
    {
        string EditText(string text);
    }
    
    // Есть главный работник, к нему присылают статью, он ее обрабатывает и возвращает
    // Сам он ничего не делает, он просит помочь своим подчиненным:
    // Первый подчиненный берет текст добавляет к нему пробел и сам текст и возвращает
    // Второй подчиненный берет текст добавляет к нему пробел и длинну текста (число)
}