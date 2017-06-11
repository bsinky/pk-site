using System.Collections.Generic;

namespace pk_site.Pokemon
{
    public class PokemonInfo : IPokemonInfo
    {
        public string Species { get; private set; }
        public string Pokeball { get; private set; }
        public string Ability { get; private set; }
        public string Nickname { get; private set; }
        public int CurrentLevel { get; private set; }
        public IEnumerable<string> Moves { get; private set; }
        public string ImagePath { get; private set; }
        public string HeldItem { get; private set; }

        public PokemonInfo(string species, string ball, string ability,
            string nickname, int level, IEnumerable<string> moves, string imagePath,
            string heldItem)
        {
            Species = species;
            Pokeball = ball;
            Ability = ability;
            Nickname = nickname;
            CurrentLevel = level;
            Moves = moves;
            ImagePath = imagePath;
            HeldItem = heldItem;
        }
    }
}
