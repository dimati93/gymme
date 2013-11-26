using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Gymme.View
{
    public partial class SetControl : UserControl
    {
        private Dictionary<Control, DependencyProperty> _bindingElements;
        public SetControl()
        {
            InitializeComponent(); Loaded += (o, e) => RegisterBindingElements();
        }

        public void UpdateDataSources()
        {
            UpdateBindingElements(ex => ex.UpdateSource());
        }

        private void UpdateBindingElements(Action<BindingExpression> beAction)
        {
            foreach (var bindingElement in _bindingElements)
            {
                BindingExpression expression = bindingElement.Key.GetBindingExpression(bindingElement.Value);
                if (expression != null)
                {
                    beAction(expression);
                }
            }
        }

        private void RegisterBindingElements()
        {
            _bindingElements = new Dictionary<Control, DependencyProperty>
            {
                {tbLift, TextBox.TextProperty},
                {tbReps, TextBox.TextProperty}
            };
        }
        private void InputBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox box = (TextBox) sender;
            box.SelectAll();
        }
    }
}
