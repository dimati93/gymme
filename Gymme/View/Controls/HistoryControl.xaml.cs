using System.Windows;
using System.Windows.Controls;
using Gymme.View.Helpers;

namespace Gymme.View.Controls
{
    public partial class HistoryControl : UserControl, IDataContextChangedHandler<HistoryControl>
    {
        public HistoryControl()
        {
            InitializeComponent();
            DataContextChangedHelper<HistoryControl>.Bind(this);
        }

        public void DataContextChanged(HistoryControl sender, DependencyPropertyChangedEventArgs e)
        {
        }
    }
}
