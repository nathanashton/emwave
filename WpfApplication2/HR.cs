using System;
using PropertyChanged;

namespace WpfApplication2
{
    [Serializable]
    [ImplementPropertyChanged]
    public class Hr
    {
        public TimeSpan ElapsedTime { get; set; }
        public int H { get; set; } // Heartrate
        public int I { get; set; }
        public string R { get; set; }

        public int T { get; set; } // Elapsed time in seconds
        public int S { get; set; }
        public int A { get; set; }
        public int E { get; set; }
        public int L { get; set; }

        public int RedPercent { get; set; }
        public int BluePercent { get; set; }
        public int GreenPercent { get; set; }

        public bool RedActive { get; set; }
        public bool BlueActive { get; set; }
        public bool GreenActive { get; set; }
    }
}