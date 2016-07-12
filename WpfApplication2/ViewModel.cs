using PropertyChanged;
using Syncfusion.UI.Xaml.Charts;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Threading;
using UsbHid;

namespace WpfApplication2
{
    [ImplementPropertyChanged]
    public class ViewModel
    {
        public Window Window;
        public Stopwatch Stopwatch = new Stopwatch();
        public DispatcherTimer Timer;
        public UsbHidDevice Usb;

        public ViewModel(Window window)
        {
            HrAnnotations = new AnnotationCollection();
            var verticalLineAnnotation = new VerticalLineAnnotation
            {
                ShowAxisLabel = true,
                CoordinateUnit = CoordinateUnit.Axis,
                X1 = 1
            };

            HrAnnotations.Add(verticalLineAnnotation);

            Window = window;
            Data = new ObservableCollection<Hr>();
            Data2 = new ObservableCollection<Hr>();

            Usb = new UsbHidDevice(0x0E30, 0x0008);
            Usb.DataReceived += Usb_DataReceived;
            Usb.OnConnected += Usb_OnConnected;
            Usb.OnDisConnected += Usb_OnDisConnected;

            Timer = new DispatcherTimer();
            Timer.Tick += Timer_Tick;
            Timer.Interval = new TimeSpan(0, 0, 0, 0, 1);

            if (File.Exists("test.data"))
            {
                Data = ReadFromBinaryFile<ObservableCollection<Hr>>("test.data");
                CalculateAverages();
                CurrentHr = Data.Last();
            }
        }

        public string Test { get; set; }
        public string ElapsedTime { get; set; }

        public bool Connected { get; set; }
        public int HrAverage { get; set; }
        public int HrMin { get; set; }
        public int HrMax { get; set; }

        public bool HrAverageVisible { get; set; } = false;
        public bool HrMinVisible { get; set; } = false;
        public bool HrMaxVisible { get; set; } = false;

        public Hr CurrentHr { get; set; }

        public AnnotationCollection HrAnnotations { get; set; }

        //public int RedPercent { get; set; }
        //public int BluePercent { get; set; }
        //public int GreenPercent { get; set; }

        //public bool RedActive { get; set; }
        //public bool BlueActive { get; set; }
        //public bool GreenActive { get; set; }

        public ObservableCollection<Hr> Data { get; set; }
        public ObservableCollection<Hr> Data2 { get; set; }

        private void Timer_Tick(object sender, EventArgs e)
        {
            ElapsedTime = Stopwatch.Elapsed.ToString("mm\\:ss\\.ff");
        }

        public void Usb_OnDisConnected()
        {
            Connected = false;
        }

        public void Usb_OnConnected()
        {
            Connected = true;
        }

        public void CalculateAverages()
        {
            var c = Data.Count;
            if (c == 0)
            {
                c = 1;
            }
            HrAverage = Data.Sum(x => x.H) / c;
            if (Data == null || Data.Count <= 0) return;
            {
                HrMin = Data.Min(x => x.H);
                HrMax = Data.Max(x => x.H);
            }
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
                return (T)binaryFormatter.Deserialize(stream);
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

            var heartrate = new Hr { ElapsedTime = TimeSpan.FromMilliseconds(Stopwatch.ElapsedMilliseconds) };

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

            CalculateAverages();

            CalculateCoherence(heartrate);
            CurrentHr = heartrate;
            Window.Dispatcher.Invoke(() => { Data.Add(heartrate); });
        }

        private void CalculateCoherence(Hr hr)
        {
            var value = hr.S;

            if (hr.T != 0)
            {
                if (value == 0)
                {
                    hr.RedActive = true;
                    hr.BlueActive = false;
                    hr.GreenActive = false;
                }
                if (value == 1)
                {
                    hr.RedActive = false;
                    hr.BlueActive = true;
                    hr.GreenActive = false;
                }
                if (value == 2)
                {
                    hr.RedActive = false;
                    hr.BlueActive = false;
                    hr.GreenActive = true;
                }
            }
            else
            {
                if (Data.Count > 0)
                {
                    var previous = Data.Last();
                    hr.RedActive = previous.RedActive;
                    hr.BlueActive = previous.BlueActive;
                    hr.GreenActive = previous.GreenActive;
                }
                else
                {
                    hr.BlueActive = true;
                }
            }

            //Calculate percentages based on whre Red, Green and Blue arent all false
            var count = Data.Count == 0 ? 1 : Data.Count();

            double redcount = Data.Count(x => x.RedActive);
            double bluecount = Data.Count(x => x.BlueActive);
            double greencount = Data.Count(x => x.GreenActive);

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            var r = redcount / count * 100;
            var b = bluecount / count * 100;
            var g = greencount / count * 100;

            hr.RedPercent = (int)r;
            hr.BluePercent = (int)b;
            hr.GreenPercent = (int)g;
        }

        public static string ByteArrayToString(byte[] ba)
        {
            var hex = new StringBuilder(ba.Length * 2);
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

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
    }
}