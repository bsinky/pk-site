using pk_site.Pokemon;

namespace pk_site.Html
{
    public interface IHtmlGenerator
    {
        string Title { get; }
        void Write(string outputDirectory, IPokemonSaveInfo saveInfo);
    }
}
