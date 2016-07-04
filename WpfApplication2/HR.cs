using System;
using PropertyChanged;

namespace WpfApplication2
{
    [Serializable]
    [ImplementPropertyChanged]
    public class HR
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
        public bool Changed { get; set; }
    }
}