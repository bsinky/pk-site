using System.Collections.Generic;

namespace pk_site.Pokemon
{
    public class TrainerInfo : ITrainerInfo
    {
        public string Name { get; private set; }
        public Genders Gender { get; private set; }
        public uint Money { get; private set; }
        public string FormattedMoney => Money.ToString("C0");
        public IEnumerable<string> Badges { get; private set; }
        public string PlayTime { get; private set; }
        public int SeenCount { get; private set; }
        public int CaughtCount { get; private set; }

        public TrainerInfo(string name, Genders gender, uint money,
            IEnumerable<string> badges, string playTime, int seenCount,
            int caughtCount)
        {
            Name = name;
            Gender = gender;
            Money = money;
            Badges = badges;
            PlayTime = playTime;
            SeenCount = seenCount;
            CaughtCount = caughtCount;
        }
    }
}
