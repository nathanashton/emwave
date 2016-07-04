using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace WpfApplication2
{
    /// <summary>
    ///     Interaction logic for DebugWindow.xaml
    /// </summary>
    public partial class DebugWindow : Window
    {
        public DebugWindow()
        {
            InitializeComponent();
            var _viewModel = new ViewModel(this);
            DataContext = _viewModel;
            if (File.Exists("test.data"))
            {
                _viewModel.Data2 = ViewModel.ReadFromBinaryFile<ObservableCollection<HR>>("test.data");
            }
        }
    }
}