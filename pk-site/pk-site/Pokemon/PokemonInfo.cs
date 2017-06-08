﻿using System.Collections.Generic;

namespace pk_site.Pokemon
{
    public class PokemonInfo : IPokemonInfo
    {
        private string _species;
        private string _ball;
        private string _ability;
        private string _nickname;
        private int _level;
        private IEnumerable<string> _moves;
        private string _imagePath;

        public PokemonInfo(string species, string ball, string ability,
            string nickname, int level, IEnumerable<string> moves, string imagePath)
        {
            _species = species;
            _ball = ball;
            _ability = ability;
            _nickname = nickname;
            _level = level;
            _moves = moves;
            _imagePath = imagePath;
        }

        public string Species => _species;
        public string Pokeball => _ball;
        public string Ability => _ability;
        public string Nickname => _nickname;
        public int CurrentLevel => _level;
        public IEnumerable<string> Moves => _moves;
        public string ImagePath => _imagePath;
    }
}
