using pk_site.Html;
using PKHeX.Core;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace pk_site.Pokemon
{
    public class PKHeXSaveInfo : IPokemonSaveInfo
    {
        private SaveFile _saveFile;

        private GameInfo.GameStrings _gameStrings;

        public PKHeXSaveInfo(string language, string saveFilePath)
        {
            _saveFile = SaveUtil.getVariantSAV(File.ReadAllBytes(saveFilePath));
            _gameStrings = GameInfo.getStrings(language);
        }

        public IEnumerable<IPokemonInfo> PartyMembers =>
            _saveFile.PartyData.Select(member => new PokemonInfo(
                GetSpecies(member.Species),
                GetBall(member.Ball),
                GetAbility(member.Ability),
                member.Nickname,
                member.CurrentLevel,
                member.Moves.Select(move => GetMove(move))));

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
        public string Generation => _gameStrings.gamelist[_saveFile.Generation];

        public IHtmlGenerator GetHtmlGenerator()
        {
            return new HtmlGenerator(Generation, this);
        }
    }
}
