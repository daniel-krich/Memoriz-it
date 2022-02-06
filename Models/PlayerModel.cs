using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryCardGame.Models
{
    public class PlayerModel : IComparable
    {
        public string Name { get; set; }
        public int Score { get; set; }
        public int Fails { get; set; }

        public PlayerModel(string name)
        {
            Name = name;
        }

        public void AddScore(int score) => Score += score;
        public void Fail() => Fails++;
        public override string ToString() => Name;

        public int CompareTo(object obj)
        {
            if(obj is not null && obj is PlayerModel)
            {
                return Score.CompareTo((obj as PlayerModel).Score);
            }
            return 0;
        }
    }
}
