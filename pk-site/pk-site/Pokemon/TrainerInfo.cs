using System.Collections.Generic;

namespace pk_site.Pokemon
{
    public class TrainerInfo : ITrainerInfo
    {
        private string _name;
        private Genders _gender;
        private uint _money;
        private IEnumerable<string> _badges;
        private string _playTime;

        public TrainerInfo(string name, Genders gender, uint money,
            IEnumerable<string> badges, string playTime)
        {
            _name = name;
            _gender = gender;
            _money = money;
            _badges = badges;
            _playTime = playTime;
        }

        public string Name => _name;
        public Genders Gender => _gender;
        public uint Money => _money;
        public string FormattedMoney => _money.ToString("C");
        public IEnumerable<string> Badges => _badges;
        public string PlayTime => _playTime;
    }
}
