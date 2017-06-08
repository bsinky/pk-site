﻿using pk_site.Html;
using PKHeX.Core;
using PKHeX.WinForms;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace pk_site.Pokemon
{
    public class PKHeXSaveInfo : IPokemonSaveInfo
    {
        private SaveFile _saveFile;
        private GameInfo.GameStrings _gameStrings;
        private string _outputDirectory;
        private Dictionary<(int, int, int, int, bool, bool, int), (string, string, Image)> _pokemonImageCache = new Dictionary<(int, int, int, int, bool, bool, int), (string, string, Image)>();

        private string outputImageRelativeDirectory => "img";
        private string outputImageDirectory => Path.Combine(_outputDirectory, outputImageRelativeDirectory);

        public PKHeXSaveInfo(string language, string saveFilePath, string outputDirectory)
        {
            _saveFile = SaveUtil.getVariantSAV(File.ReadAllBytes(saveFilePath));
            _gameStrings = GameInfo.getStrings(language);
            _outputDirectory = outputDirectory;
            Directory.CreateDirectory(outputImageDirectory);
            foreach (var pokemon in _saveFile.PartyData)
            {
                ProcessPokemon(pokemon);
            }
        }

        private (int, int, int, int, bool, bool, int) GetImageKey(PKM pkm)
            => (pkm.Species, pkm.AltForm, pkm.Gender, pkm.SpriteItem, pkm.IsEgg, pkm.IsShiny, pkm.Format);

        private void ProcessPokemon(PKM pkm)
        {
            var imageKey = GetImageKey(pkm);
            Image generatedImage = PKMUtil.getSprite(pkm.Species, pkm.AltForm, pkm.Gender, pkm.SpriteItem, pkm.IsEgg, pkm.IsShiny, pkm.Format);
            string imageFileName = ImageKeyToFileName(imageKey) + ".png";
            string relativePath = string.Join("/", outputImageRelativeDirectory, imageFileName);
            string outputPath = string.Join("/", outputImageDirectory, imageFileName);

            _pokemonImageCache[imageKey] = (outputPath, relativePath, generatedImage);
        }

        private string GetImagePath(PKM pkm)
        {
            var imageKey = GetImageKey(pkm);
            if (_pokemonImageCache.TryGetValue(imageKey, out (string, string, Image) foundImage))
            {
                return foundImage.Item2;
            }

            return "";
        }

        public IEnumerable<IPokemonInfo> PartyMembers =>
            _saveFile.PartyData.Select(member => new PokemonInfo(
                GetSpecies(member.Species),
                GetBall(member.Ball),
                GetAbility(member.Ability),
                member.Nickname,
                member.CurrentLevel,
                member.Moves.Select(move => GetMove(move)),
                GetImagePath(member)));

        public ITrainerInfo TrainerInfo => new TrainerInfo(
            _saveFile.OT,
            default(Genders), /* TODO */
            _saveFile.Money,
            Enumerable.Empty<string>(), /* TODO */
            _saveFile.PlayTimeString);

        public string GetAbility(int ability) => _gameStrings.abilitylist[ability];
        public string GetBall(int ball) => _gameStrings.balllist[ball];
        public string GetMove(int move) => _gameStrings.movelist[move];
        public string GetSpecies(int species) => _gameStrings.specieslist[species];
        public string GameTitle => _gameStrings.gamelist[_saveFile.Generation];
        public string Version => _saveFile.Version.ToString();

        public IHtmlGenerator GetHtmlGenerator()
        {
            WriteImagesToDisk();
            return new HtmlGenerator($"{GameTitle} ({Version})", this);
        }

        private void WriteImagesToDisk()
        {
            foreach(var (absolutePath, _, image) in _pokemonImageCache.Values)
            {
                image.Save(absolutePath);
            }
        }

        private string ImageKeyToFileName((int,int,int,int,bool,bool,int) imageKey)
        {
            var (species, form, gender, item, isEgg, isShiny, generation) = imageKey;
            return ImageKeyToFileName(species, form, gender, item, isEgg, isShiny, generation);
        }
        
        private string ImageKeyToFileName(int species, int form, int gender, int item, bool isEgg, bool isShiny, int generation)
        {
            return $"p{species}-{form}-{gender}-{item}-{(isEgg ? 1 : 0)}-{(isShiny ? 1 : 0)}-{generation}";
        }
    }
}
