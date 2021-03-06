﻿using System.Collections.Generic;

namespace pk_site.Pokemon
{
    public interface IPokemonInfo
    {
        string Species { get; }
        string Pokeball { get; }
        string Ability { get; }
        string Nickname { get; }
        string ImagePath { get; }
        string HeldItem { get; }

        int CurrentLevel { get; }
        
        IEnumerable<string> Moves { get; }
        string MovesList { get; }
    }
}
