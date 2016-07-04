using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using PropertyChanged;
using UsbHid;

namespace WpfApplication2
{
    [ImplementPropertyChanged]
    public class ViewModel
    {
        public Window _window;
        public Stopwatch stopwatch = new Stopwatch();

        public UsbHidDevice usb;

        public ViewModel(Window window)
        {
            _window = window;
            Data = new ObservableCollection<HR>();
            Data2 = new ObservableCollection<HR>();

            usb = new UsbHidDevice(0x0E30, 0x0008);
            usb.DataReceived += Usb_DataReceived;
            usb.OnConnected += Usb_OnConnected;
            usb.OnDisConnected += Usb_OnDisConnected;

            //if (File.Exists("test.data"))
            //{
            //    Data = ViewModel.ReadFromBinaryFile<ObservableCollection<HR>>("test.data");
            //    CalculateAverage();
            //}
        }

        // public ChartValues<HR> HeartRates { get; set; }

        public bool Connected { get; set; }
        public int HRAverage { get; set; }
        public int CurrentHR { get; set; }

        public int RedPercent { get; set; }
        public int BluePercent { get; set; }
        public int GreenPercent { get; set; }


        public bool RedActive { get; set; }
        public bool BlueActive { get; set; }
        public bool GreenActive { get; set; }


        public ObservableCollection<HR> Data { get; set; }
        public ObservableCollection<HR> Data2 { get; set; }

        public void Usb_OnDisConnected()
        {
            Connected = false;
        }

        public void Usb_OnConnected()
        {
            Connected = true;
        }

        public void CalculateAverage()
        {
            var c = Data.Count;
            if (c == 0)
            {
                c = 1;
            }
            HRAverage = Data.Sum(x => x.H)/c;
        }

        public static void WriteToBinaryFile<T>(string filePath, T objectToWrite, bool append = false)
        {
            using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(stream, objectToWrite);
            }
        }

        public static T ReadFromBinaryFile<T>(string filePath)
        {
            using (Stream stream = File.Open(filePath, FileMode.Open))
            {
                var binaryFormatter = new BinaryFormatter();
                return (T) binaryFormatter.Deserialize(stream);
            }
        }

        public void Usb_DataReceived(byte[] data)
        {

            var hex = ByteArrayToString(data);
            var text = ConvertHexToString(hex);

            var h = Regex.Match(text, @"H=[0-9]+");
            var r = Regex.Match(text, @"R=[A-Z]+");
            var i = Regex.Match(text, @"I=[0-9]+");

            var t = Regex.Match(text, @"T=[0-9]+");
            var s = Regex.Match(text, @"S=[0-9]+");
            var a = Regex.Match(text, @"A=[0-9]+");
            var e = Regex.Match(text, @"E=[0-9]+");
            var l = Regex.Match(text, @"L=[0-9]+");

            var heartrate = new HR {ElapsedTime = TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds)};

            if (h.Success)
            {
                var value = Regex.Match(h.Value, @"[0-9]+");
                if (value.Success)
                {
                    heartrate.H = Convert.ToInt32(value.Value);
                }
            }
            if (r.Success)
            {
                var value = Regex.Match(r.Value, @"[A-Z]+");
                if (value.Success)
                {
                    heartrate.R = value.Value;
                }
            }
            if (i.Success)
            {
                var value = Regex.Match(i.Value, @"[0-9]+");
                if (value.Success)
                {
                    heartrate.I = Convert.ToInt32(value.Value);
                }
            }

            if (t.Success)
            {
                var value = Regex.Match(t.Value, @"[0-9]+");
                if (value.Success)
                {
                    heartrate.T = Convert.ToInt32(value.Value);
                }
            }

            if (s.Success)
            {
                var value = Regex.Match(s.Value, @"[0-9]+");
                if (value.Success)
                {
                    heartrate.S = Convert.ToInt32(value.Value);
                    if (heartrate.T != 0)
                    {
                        CalculateCoherence(heartrate.S);
                    }
                }
            }

            if (a.Success)
            {
                var value = Regex.Match(a.Value, @"[0-9]+");
                if (value.Success)
                {
                    heartrate.A = Convert.ToInt32(value.Value);
                }
            }

            if (e.Success)
            {
                var value = Regex.Match(e.Value, @"[0-9]+");
                if (value.Success)
                {
                    heartrate.E = Convert.ToInt32(value.Value);
                }
            }

            if (l.Success)
            {
                var value = Regex.Match(l.Value, @"[0-9]+");
                if (value.Success)
                {
                    heartrate.L = Convert.ToInt32(value.Value);
                }
            }

            CalculateAverage();

            if (heartrate.T != 0)
            {
                Data2.Add(heartrate);
            }


            _window.Dispatcher.Invoke(() => { Data.Add(heartrate); });

            CurrentHR = heartrate.H;
        }

        private void CalculateCoherence(int value)
        {
            if (value == 0)
            {
                RedActive = true;
                BlueActive = false;
                GreenActive = false;
            }
            if (value == 1)
            {
                RedActive = false;
                BlueActive = true;
                GreenActive = false;
            }
            if (value == 2)
            {
                RedActive = false;
                BlueActive = false;
                GreenActive = true;
            }

            //Calculate percentages

            double count = Data2.Count(x => x.T != 0);
            double redcount = Data2.Count(x => x.S == 0);
            double bluecount = Data2.Count(x => x.S == 1);
            double greencount = Data2.Count(x => x.S == 2);
            if (count == 0) return;
            var r = redcount/count*100;
            var b = bluecount/count*100;
            var g = greencount/count*100;

            RedPercent = (int) r;
            BluePercent = (int) b;
            GreenPercent = (int) g;
        }

        public static string ByteArrayToString(byte[] ba)
        {
            var hex = new StringBuilder(ba.Length*2);
            foreach (var b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public string ConvertHexToString1314807815(string hexValue)
        {
            var strValue = "";
            while (hexValue.Length > 0)
            {
                strValue += Convert.ToChar(Convert.ToUInt32(hexValue.Substring(0, 2), 16)).ToString();
                hexValue = hexValue.Substring(2, hexValue.Length - 2);
            }
            return strValue;
        }

        public string ConvertHexToString(string hexValue)
        {
            var strValue = "";
            while (hexValue.Length > 0)
            {
                strValue += Convert.ToChar(Convert.ToUInt32(hexValue.Substring(0, 2), 16)).ToString();
                hexValue = hexValue.Substring(2, hexValue.Length - 2);
            }
            return strValue;
        }
    }
}