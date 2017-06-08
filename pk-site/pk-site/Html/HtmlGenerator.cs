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
    public class HtmlGenerator : IHtmlGenerator
    {
        private string _title;

        public HtmlGenerator(string pageTitle, IPokemonSaveInfo saveInfo)
        {
            _title = pageTitle;
        }

        public string Title => _title;

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
            string outputFile = Path.Combine(outputDirectory, "index.html");
            string template = File.ReadAllText("Resources/template.cshtml");

            DynamicViewBag viewBag = new DynamicViewBag(new Dictionary<string, object>()
            {
                ["Title"] = Title
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

        protected virtual IDictionary<string, string> CSSFilesToCopy => new Dictionary<string, string>()
        {
            ["Resources/main.css"] = "main.css"
        };

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

        private void CopyImages(string outputDirectory, IPokemonSaveInfo saveInfo)
        {
            foreach (var pokemon in saveInfo.PartyMembers)
            {

            }
        }
    }
}
