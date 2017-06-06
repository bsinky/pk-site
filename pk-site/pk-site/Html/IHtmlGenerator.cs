using pk_site.Pokemon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pk_site.Html
{
    public interface IHtmlGenerator
    {
        string FileName { get; }
        string Title { get; }
        IEnumerable<string> StyleSheets { get; }
        IEnumerable<IPokemonInfo> PartyPokemon { get; }
        void Write(string outputFile);
    }
}
