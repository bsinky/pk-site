using pk_site.Pokemon;
using RazorEngine;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Security.Permissions;

namespace pk_site.Html
{
    public class HtmlGenerator : IHtmlGenerator
    {
        private string _title;
        private IPokemonSaveInfo _saveInfo;

        public HtmlGenerator(string pageTitle, IPokemonSaveInfo saveInfo)
        {
            _title = pageTitle;
        }

        public string Title => _title;

        public IEnumerable<IPokemonInfo> PartyPokemon => throw new NotImplementedException();

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
            string template = "test";
            string templateFile = "Resources/template.cshtml";
            var templateSource = new LoadedTemplateSource(template, templateFile);

            // TODO: fix
            var result =
                    Engine.Razor.RunCompile(templateFile, GetTemplateKey(), null,
                     new
                     {
                         Title = Title,
                         Trainer = saveInfo.TrainerInfo,
                         PartyMembers = saveInfo.PartyMembers
                     });

            File.WriteAllText(outputFile, result);

            if (domain != null)
                // RazorEngine will cleanup. 
                AppDomain.Unload(domain);
            }
        }

        private ITemplateKey GetTemplateKey()
        {
            return new TemplateKey(Title, ResolveType.Global, null);
        }

        private class TemplateKey : BaseTemplateKey
        {
            public TemplateKey(string name, ResolveType resolveType, 
                ITemplateKey context) : base(name, resolveType, context) { }

            public override string GetUniqueKeyString()
            {
                return Name;
            }
        }
    }
}
