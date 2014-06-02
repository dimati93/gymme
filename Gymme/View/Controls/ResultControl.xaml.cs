using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Gymme.View.Controls
{
    public partial class ResultControl : ExecuteInputControl
    {
        public ResultControl()
        {
            InitializeComponent();
        }

        private void InputBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox box = (TextBox)sender;
            box.SelectAll();
        }

        private void InputBox_TextInput(object sender, KeyEventArgs keyEventArgs)
        {
            if (!IsEdited)
            {
                IsEdited = true;
            }

            UpdateDataSources();
        }

        protected override Dictionary<Control, DependencyProperty> GetBindingElements()
        {
            return new Dictionary<Control, DependencyProperty>
            {
                {tbResult, TextBox.TextProperty}
            };
        }
    }

}
