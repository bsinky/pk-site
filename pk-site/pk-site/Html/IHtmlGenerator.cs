using pk_site.Pokemon;
using System.Collections.Generic;

namespace pk_site.Html
{
    public interface IHtmlGenerator
    {
        string Title { get; }
        void Write(string outputDirectory, IPokemonSaveInfo saveInfo);
        IEnumerable<string> CopyCSS(string outputDirectory);
    }
}
