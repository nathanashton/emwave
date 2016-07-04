using System.Windows;
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


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.usb.Connect();


            if (!_viewModel.usb.IsDeviceConnected) return;

            var command = new CommandMessage(0x86, new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 });
            _viewModel.usb.SendMessage(command);



            _viewModel.stopwatch.Start();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _viewModel.usb.Disconnect();
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

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            _viewModel.RedPercent = 50;
            _viewModel.RedActive = true;
        }
    }
}