using CommandLine;
using CommandLine.Text;
using pk_site.Html;
using pk_site.Pokemon;
using System;

namespace pk_site
{
    class Program
    {
        public static void Main(string[] args)
        {
            var options = new Options();

            if (!Parser.Default.ParseArguments(args, options))
            {
                return;
            }

            IPokemonSaveInfo saveInfo = new PKHeXSaveInfo(options.Language, options.SaveFilePath, 
                options.OutputDirectory, options.Type, options.AbsoluteImageDirectory, options.RelativeImagePath);
            IHtmlGenerator renderer = saveInfo.GetHtmlGenerator();

            renderer.Write(options.OutputDirectory, saveInfo);

            Console.WriteLine($"Successfully wrote to {options.OutputDirectory}");
            Console.WriteLine("DONE");
        }

        public class Options
        {
            [Option('p', "path", Required = true, HelpText = "Path to the save file")]
            public string SaveFilePath { get; set; }

            [Option('o', "output", DefaultValue = "output/", HelpText = "Path to the output directory")]
            public string OutputDirectory { get; set; }

            [Option('l', "language", DefaultValue = "en", HelpText = "Language of PKHeX resource files to use")]
            public string Language { get; set; }

            [Option('a', "absoluteimagedirectory", HelpText = "Absolute path to image directory")]
            public string AbsoluteImageDirectory { get; set; }

            [Option('r', "relativeimagepath", HelpText = "Relative to image directory")]
            public string RelativeImagePath { get; set; }

            [Option('t', "type", HelpText = "Type of output to generate")]
            public string Type { get; set; }

            [ParserState]
            public IParserState LastParserState { get; set; }

            [HelpOption]
            public string GetUsage()
            {
                return HelpText.AutoBuild(this,
                    (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
            }
        }
    }
}

