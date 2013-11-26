using System.Windows;
using System.Windows.Controls;

namespace Gymme.View
{
    public partial class SetControl : UserControl
    {
        public SetControl()
        {
            InitializeComponent();
        }

        private void InputBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox box = (TextBox) sender;
            box.SelectAll();
        }
    }
}
