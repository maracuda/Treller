using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;
using Tolltech.TollEnnobler.SolutionFixers;

namespace SKBKontur.Tolltech.ProjectMap
{
    public class DboWriter : IFixer
    {
        public void Fix(Document document, DocumentEditor documentEditor)
        {
            Console.WriteLine(document.Name);
        }

        public string Name => "DboWriter";
        public int Order => 0;
    }
}