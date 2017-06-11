using pk_site.Html;
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
        private string _gameSpriteRelativePath;
        private IEnumerable<PKM> _boxPokemon;

        private string outputImageRelativeDirectory => "img";
        private string outputImageDirectory => Path.Combine(_outputDirectory, outputImageRelativeDirectory);

        public PKHeXSaveInfo(string language, string saveFilePath, string outputDirectory)
        {
            _saveFile = SaveUtil.getVariantSAV(File.ReadAllBytes(saveFilePath));
            _gameStrings = GameInfo.getStrings(language);
            _outputDirectory = outputDirectory;
            _boxPokemon = _saveFile.BoxData.Where(bpkm => bpkm.Species > 0);
            Directory.CreateDirectory(outputImageDirectory);
            foreach (var pokemon in _saveFile.PartyData.Concat(_boxPokemon))
            {
                ProcessPokemon(pokemon);
            }
            ProcessGameSprite();
        }

        private (int, int, int, int, bool, bool, int) GetImageKey(PKM pkm)
            => (pkm.Species, pkm.AltForm, pkm.Gender, pkm.SpriteItem, pkm.IsEgg, pkm.IsShiny, pkm.Format);

        private void ProcessPokemon(PKM pkm)
        {
            var imageKey = GetImageKey(pkm);
            Image generatedImage = pkm.Sprite();
            string imageFileName = ImageKeyToFileName(imageKey) + ".png";
            string relativePath = string.Join("/", outputImageRelativeDirectory, imageFileName);
            string outputPath = string.Join("/", outputImageDirectory, imageFileName);

            _pokemonImageCache[imageKey] = (outputPath, relativePath, generatedImage);
        }

        private void ProcessGameSprite()
        {
            var gameSprite = _saveFile.Sprite();
            var imageFileName = "game.png";
            var absolutePath = Path.Combine(outputImageDirectory, imageFileName);
            var relativePath = string.Join("/", outputImageRelativeDirectory, imageFileName);
            gameSprite.Save(absolutePath);
            _gameSpriteRelativePath = relativePath;
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

        public IEnumerable<IPokemonInfo> PartyMembers => _saveFile.PartyData.Select(member => GetPokemonInfo(member));
        public IEnumerable<IPokemonInfo> BoxPokemon => _boxPokemon.Select(member => GetPokemonInfo(member));

        private IPokemonInfo GetPokemonInfo(PKM pkm) => 
            new PokemonInfo(
                GetSpecies(pkm.Species),
                GetBall(pkm.Ball),
                GetAbility(pkm.Ability),
                pkm.Nickname,
                pkm.CurrentLevel,
                pkm.Moves.Select(move => GetMove(move)),
                GetImagePath(pkm),
                _gameStrings.itemlist[pkm.HeldItem]);

        public ITrainerInfo TrainerInfo => new TrainerInfo(
            _saveFile.OT,
            _saveFile.Gender == 0 ? Genders.Boy : Genders.Girl,
            _saveFile.Money,
            Enumerable.Empty<string>(), /* TODO */
            _saveFile.PlayTimeString,
            _saveFile.SeenCount,
            _saveFile.CaughtCount);

        public string GetAbility(int ability) => _gameStrings.abilitylist[ability];
        public string GetBall(int ball) => _gameStrings.balllist[ball];
        public string GetMove(int move) => _gameStrings.movelist[move];
        public string GetSpecies(int species) => _gameStrings.specieslist[species];
        public string GameTitle => _gameStrings.gamelist[_saveFile.Generation];
        public string Version => _saveFile.Version.ToString();
        public string GameImagePath => _gameSpriteRelativePath;

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
