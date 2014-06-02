using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Gymme.View.Controls
{
    public abstract class ExecuteInputControl : UserControl
    {
        public static readonly DependencyProperty IsEditedProperty =
            DependencyProperty.Register("IsEdited", typeof(bool), typeof(ExecuteInputControl), new PropertyMetadata(false));

        public bool IsEdited
        {
            get { return (bool) GetValue(IsEditedProperty); }
            set
            {
                SetValue(IsEditedProperty, value);
            }
        }

        private Dictionary<Control, DependencyProperty> _bindingElements;

        protected ExecuteInputControl()
        {
            Loaded += RegisterBindingElements;
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

        private void RegisterBindingElements(object sender, RoutedEventArgs routedEventArgs)
        {
            _bindingElements = GetBindingElements();
        }

        protected abstract Dictionary<Control, DependencyProperty> GetBindingElements();
    }
}