using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;
using Tolltech.TollEnnobler.SolutionFixers;

namespace ProjectMap
{
    public class DocumentLister : IFixer
    {
        public void Fix(Document document, DocumentEditor documentEditor)
        {
            Console.WriteLine(document.Name);
        }

        public string Name => "DocumentLister";
        public int Order => 0;
    }
}