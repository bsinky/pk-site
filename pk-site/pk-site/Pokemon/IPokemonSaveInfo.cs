using pk_site.Html;
using System.Collections.Generic;
using System.Drawing;

namespace pk_site.Pokemon
{
    public interface IPokemonSaveInfo
    {
        ITrainerInfo TrainerInfo { get; }
        IEnumerable<IPokemonInfo> PartyMembers { get; }
        string GetBall(int ball);
        string GetSpecies(int species);
        string GetAbility(int ability);
        string GetMove(int move);
        string GameTitle { get; }
        string Version { get; }
        IHtmlGenerator GetHtmlGenerator();
    }
}
