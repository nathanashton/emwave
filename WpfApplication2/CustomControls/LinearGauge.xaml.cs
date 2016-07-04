using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

namespace WpfApplication2.CustomControls
{
    public partial class LinearGauge
    {
        public static readonly DependencyProperty ActiveGaugeProperty = DependencyProperty.Register("ActiveGauge",
    typeof(bool), typeof(LinearGauge),
    new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                ActiveGaugeChanged));

        private static void ActiveGaugeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = (LinearGauge)d;
            instance.UpdateGauge();
        }

        public static readonly DependencyProperty GaugeFillProperty = DependencyProperty.Register("GaugeFill",
            typeof(Brush), typeof(LinearGauge),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        public static readonly DependencyProperty GaugePercentProperty = DependencyProperty.Register("GaugePercent",
            typeof(double), typeof(LinearGauge),
            new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                GaugePercentChanged));

        public static readonly DependencyProperty GaugeHeightProperty = DependencyProperty.Register("GaugeHeight",
            typeof(double), typeof(LinearGauge),
            new FrameworkPropertyMetadata(double.NaN,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsParentMeasure));


        public LinearGauge()
        {
            InitializeComponent();
            (Content as FrameworkElement).DataContext = this;
        }

        public bool ActiveGauge
        {
            get { return (bool)GetValue(ActiveGaugeProperty); }
            set { SetValueDp(ActiveGaugeProperty, value); }
        }

        public Brush GaugeFill
        {
            get { return (Brush) GetValue(GaugeFillProperty); }
            set { SetValueDp(GaugeFillProperty, value); }
        }

        public double GaugePercent
        {
            get { return (double) GetValue(GaugePercentProperty); }
            set { SetValueDp(GaugePercentProperty, value); }
        }

        public double GaugeHeight
        {
            get { return (double) GetValue(GaugeHeightProperty); }
            set { SetValueDp(GaugeHeightProperty, value); }
        }

        public static void GaugePercentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = (LinearGauge) d;
            instance.UpdateGauge();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void SetValueDp(DependencyProperty property, object value,
            [CallerMemberName] string p = null)
        {
            SetValue(property, value);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateGauge();

        }

        private void UpdateGauge()
        {
            GaugeHeight = border.ActualHeight/100*GaugePercent;
            if (ActiveGauge)
            {
                label.Background = GaugeFill;
            }
            else
            {
                label.Background = new SolidColorBrush(Colors.Transparent);
            }
        }
    }
}