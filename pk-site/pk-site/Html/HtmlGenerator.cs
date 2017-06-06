using System;
using System.Collections.Generic;
using pk_site.Pokemon;

namespace pk_site.Html
{
    public class HtmlGenerator : IHtmlGenerator
    {
        private string _title;
        private IEnumerable<string> _styleSheets;

        public HtmlGenerator(string pageTitle, IEnumerable<string> styleSheets)
        {
            _title = pageTitle;
            _styleSheets = styleSheets;
        }

        public string FileName => _fileName;

        public string Title => _title;

        public IEnumerable<string> StyleSheets => _styleSheets;

        public IEnumerable<IPokemonInfo> PartyPokemon => throw new NotImplementedException();

        public void Write(string outputFile)
        {
            throw new NotImplementedException();
        }
    }
}
