using pk_site.Html;
using pk_site.Pokemon;
using PKHeX.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pk_site
{
    public class PKHeXFactory
    {
        private SaveFile _saveFile;

        private string[] _speciesList;
        private string[] _itemList;
        private string[] _abilityList;
        private string[] _movesList;

        public PKHeXFactory(string language, string saveFilePath)
        {
            _saveFile = SaveUtil.getVariantSAV(File.ReadAllBytes(saveFilePath));
            _speciesList = Util.getSpeciesList(language);
            _itemList = Util.getItemsList(language);
            _abilityList = Util.getAbilitiesList(language);
            _movesList = Util.getMovesList(language);
            _generationList = Util
        }

        public IEnumerable<IPokemonInfo> PartyMembers =>
            _saveFile.PartyData.Select(member => new PKHeXPokemonInfo(member));

        public IHtmlGenerator GetHtmlGenerator()
        {
            return new HtmlGenerator(_saveFile.Generation)
        }
    }
}
