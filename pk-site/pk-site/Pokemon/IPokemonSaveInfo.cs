using pk_site.Html;
using System.Collections.Generic;

namespace pk_site.Pokemon
{
    public interface IPokemonSaveInfo
    {
        string SaveFilePath { get; }
        string SaveGameFileName { get; }
        ITrainerInfo TrainerInfo { get; }
        IEnumerable<IPokemonInfo> PartyMembers { get; }
        IEnumerable<IPokemonInfo> BoxPokemon { get; }
        string GetBall(int ball);
        string GetSpecies(int species);
        string GetAbility(int ability);
        string GetMove(int move);
        string GameTitle { get; }
        string Version { get; }
        IHtmlGenerator GetHtmlGenerator();
        string GameImagePath { get; }
    }
}
