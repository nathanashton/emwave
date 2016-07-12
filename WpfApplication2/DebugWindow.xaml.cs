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
            var viewModel = new ViewModel(this);
            DataContext = viewModel;
            if (File.Exists("test.data"))
            {
                viewModel.Data2 = ViewModel.ReadFromBinaryFile<ObservableCollection<Hr>>("test.data");
            }
        }
    }
}