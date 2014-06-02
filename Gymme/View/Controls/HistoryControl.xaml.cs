using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace Gymme.View.Controls
{
    public partial class HistoryControl : UserControl
    {
        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(HistoryControl), new PropertyMetadata(null));

        public static readonly DependencyProperty ItemsSourceProperty = 
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(HistoryControl), new PropertyMetadata(default(IEnumerable)));

        public IEnumerable ItemsSource
        {
            get
            {
                return (IEnumerable)GetValue(ItemsSourceProperty);
            }
            set
            {
                SetValue(ItemsSourceProperty, value);
            }
        }

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set
            {
                SetValue(ItemTemplateProperty, value);
            }
        }

        public HistoryControl()
        {
            InitializeComponent();
        }
    }
}
