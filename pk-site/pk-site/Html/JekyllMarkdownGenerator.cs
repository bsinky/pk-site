using pk_site.Pokemon;
using RazorEngine;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Permissions;

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

        // TODO: lots of duplication with HtmlGenerator.cs, rethink
        public void Write(string outputDirectory, IPokemonSaveInfo saveInfo)
        {
            AppDomain domain = null;

            if (AppDomain.CurrentDomain.IsDefaultAppDomain())
            {
                // RazorEngine cannot clean up from the default appdomain...
                Console.WriteLine("Switching to secound AppDomain, for RazorEngine...");
                AppDomainSetup adSetup = new AppDomainSetup();
                adSetup.ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                var current = AppDomain.CurrentDomain;

                domain = AppDomain.CreateDomain(
                    "PokeSiteDomain", null,
                    current.SetupInformation, new PermissionSet(PermissionState.Unrestricted));
            }

            Directory.CreateDirectory(outputDirectory);
            string titleForContent = $"{DateTime.Now:yyyy-MM-dd} - {_title}";
            string titleForPostFile = $"{DateTime.Now:yyyy-MM-dd}-{_title.Replace('_', '-').Replace(' ', '-')}";
            string outputFile = Path.Combine(outputDirectory, $"{titleForPostFile}.md");
            string template = File.ReadAllText("Resources/blogpost.cshtml");

            DynamicViewBag viewBag = new DynamicViewBag(new Dictionary<string, object>()
            {
                ["Title"] = titleForContent,
                ["Date"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm")
            });

            IEnumerable<string> cssFiles = CopyCSS(outputDirectory);
            viewBag.AddValue("CSSFiles", cssFiles.ToList());

            // Compile and run Razor template
            using (StreamWriter outputWriter = new StreamWriter(outputFile))
            {
                Engine.Razor.RunCompile(template, "test", outputWriter, saveInfo.GetType(),
                    saveInfo, viewBag);
            }

            if (domain != null)
            {
                // RazorEngine will cleanup. 
                AppDomain.Unload(domain);
            }
        }

        protected virtual IDictionary<string, string> CSSFilesToCopy => new Dictionary<string, string>();

        /// <summary>
        /// Returns an IEnumerable of relative CSS paths copied to relative outputDirectory paths
        /// </summary>
        public IEnumerable<string> CopyCSS(string outputDirectory)
        {
            foreach (var kvp in CSSFilesToCopy)
            {
                File.Copy(kvp.Key, Path.Combine(outputDirectory, kvp.Value), true);
            }

            return CSSFilesToCopy.Values;
        }
    }
}
