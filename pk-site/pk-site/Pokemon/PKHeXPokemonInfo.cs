using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pk_site.Pokemon
{
    public class PKHeXPokemonInfo : IPokemonInfo
    {
        public PKHeXPokemonInfo(PKHeX.Core.PKM pkm)
        {
            
        }

        public string Species => throw new NotImplementedException();
        public string Pokeball => throw new NotImplementedException();
        public string Ability => throw new NotImplementedException();
        public string Nickname => throw new NotImplementedException();
        public int CurrentLevel => throw new NotImplementedException();

        public IEnumerable<string> Moves => throw new NotImplementedException();
    }
}
