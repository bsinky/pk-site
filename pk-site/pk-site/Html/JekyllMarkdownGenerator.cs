using pk_site.Pokemon;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using HandlebarsDotNet;

namespace pk_site.Html
{
    public class JekyllMarkdownGenerator : IHtmlGenerator
    {
        private string _title;

        public JekyllMarkdownGenerator(string pageTitle, IPokemonSaveInfo saveInfo)
        {
            _title = pageTitle;
        }

        public string Title => _title;

        public void Write(string outputDirectory, IPokemonSaveInfo saveInfo)
        {
            Directory.CreateDirectory(outputDirectory);
            string titleForPostFile = $"{DateTime.Now:yyyy-MM-dd}-{_title.Replace('_', '-').Replace(' ', '-')}";
            string outputFile = Path.Combine(outputDirectory, $"{titleForPostFile}.md");

            var data = new
            {
                Title = _title,
                Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                BoxPokemonCount = saveInfo.BoxPokemon.Count(),
                Model = saveInfo
            };

            foreach (string partialFile in Directory.EnumerateFiles(Path.Combine("Resources", "blogpostpartials")))
            {
                string partialFileName = Path.GetFileNameWithoutExtension(partialFile);
                Handlebars.RegisterTemplate(partialFileName, File.ReadAllText(partialFile));
            }

            var template = Handlebars.Compile(File.ReadAllText(Path.Combine("Resources", "blogpost.hb")));

            using (StreamWriter outputWriter = new StreamWriter(outputFile))
            {
                outputWriter.Write(template(data));
            }
        }

        protected virtual IDictionary<string, string> CSSFilesToCopy => new Dictionary<string, string>();

        /// <summary>
        /// Returns an IEnumerable of relative CSS paths copied to relative outputDirectory paths
        /// </summary>
        public IEnumerable<string> CopyCSS(string outputDirectory)
        {
            yield break;
        }
    }
}
