using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

using Gymme.View.Helpers;

namespace Gymme.View.Controls
{
    public partial class SetControl : UserControl , IDataContextChangedHandler<SetControl>
    {
        public static readonly DependencyProperty IsEditedProperty =
            DependencyProperty.Register("IsEdited", typeof (bool), typeof (SetControl), new PropertyMetadata(false));

        public bool IsEdited
        {
            get { return (bool) GetValue(IsEditedProperty); }
            set
            {
                SetValue(IsEditedProperty, value);
            }
        }

        private Dictionary<Control, DependencyProperty> _bindingElements;
        public SetControl()
        {
            InitializeComponent(); 
            DataContextChangedHelper<SetControl>.Bind(this);
            Loaded += (o, e) => RegisterBindingElements();
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

        private void InputBox_TextInput(object sender, KeyEventArgs keyEventArgs)
        {
            if (!IsEdited)
            {
                IsEdited = true;
            }

            UpdateDataSources();
        }

        public void DataContextChanged(SetControl sender, DependencyPropertyChangedEventArgs e)
        {
            sender.IsEdited = false;
        }
    }

}
