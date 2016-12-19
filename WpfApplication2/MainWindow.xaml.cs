using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using PropertyChanged;
using UsbHid.USB.Classes.Messaging;

namespace WpfApplication2
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    [ImplementPropertyChanged]
    public partial class MainWindow : Window
    {
        private readonly ViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new ViewModel(this);
            DataContext = _viewModel;
            var t = h;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Data = new System.Collections.ObjectModel.ObservableCollection<Hr>();
            _viewModel.Usb.Connect();

            if (!_viewModel.Usb.IsDeviceConnected) return;

            var command = new CommandMessage(0x53, new byte[]
            {
                0x00, 0x00, 0x03, 0x4A, 0x2D, 0x0D, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00
            });

            //var command = new CommandMessage(0x72, new byte[62]);

            
         _viewModel.Usb.SendMessage(command);

            _viewModel.Stopwatch.Start();
            _viewModel.Timer.Start();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _viewModel.Usb.Disconnect();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ViewModel.WriteToBinaryFile("test.data", _viewModel.Data);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var debug = new DebugWindow();
            debug.Show();
        }

        private void HeartRateChart_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                var s = tt.PointInfos[0].ValueX;
                var timespan = TimeSpan.Parse(s);

                var highlight = _viewModel.Data.FirstOrDefault(x => x.ElapsedTime.Ticks == timespan.Ticks);

                _viewModel.CurrentHr = highlight;
            }
            catch (Exception)
            {
            }
        }
    }
}