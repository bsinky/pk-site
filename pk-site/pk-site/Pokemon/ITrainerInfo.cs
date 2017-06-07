using System.Collections.Generic;

namespace pk_site.Pokemon
{
    public interface ITrainerInfo
    {
        string Name { get; }
        Genders Gender { get; }
        uint Money { get; }
        string FormattedMoney { get; }
        IEnumerable<string> Badges { get; }
        string PlayTime { get; }
    }
    
    public enum Genders
    {
        Boy,
        Girl
    }
}