using CommandLine;
using CommandLine.Text;
using PKHeX.Core;
using System;
using System.IO;
using System.Linq;

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

            byte[] saveFileBytes = File.ReadAllBytes(options.SaveFilePath);

            var saveFile = SaveUtil.getVariantSAV(saveFileBytes);
            var speciesList = Util.getSpeciesList(options.Language);
            var itemList = Util.getItemsList(options.Language);
            var abilityList = Util.getAbilitiesList(options.Language);
            var movesList = Util.getMovesList(options.Language);

            Console.WriteLine($"Generation: {saveFile.Generation}");
            Console.WriteLine($"Party Count: {saveFile.PartyCount}");

            for (var x = 0; x < saveFile.PartyData.Length; x++)
            {
                var partyMember = saveFile.PartyData[x];
                var species = speciesList[partyMember.Species];
                var pokeball = itemList[partyMember.Ball];
                var ability = abilityList[partyMember.Ability];
                var nickname = partyMember.Nickname;
                var moves = partyMember.Moves.Select((move, index) => $"{index + 1}: {movesList[move]}");

                Console.WriteLine($"Pokemon {x + 1}:");
                Console.WriteLine($"  {nickname}");
                Console.WriteLine($"  Lvl {partyMember.CurrentLevel} {species}");
                Console.WriteLine($"  Ball: {pokeball}");
                Console.WriteLine($"  Ability: {ability}");
                Console.WriteLine("  Moves:");
                foreach (var move in moves)
                {
                    Console.WriteLine($"    {move}");
                }
            }
        }

        public class Options
        {
            [Option('p', "path", Required = true, HelpText = "Path to the save file")]
            public string SaveFilePath { get; set; }

            [Option('l', "language", DefaultValue = "en", HelpText = "Language of PKHeX resource files to use")]
            public string Language { get; set; }

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

