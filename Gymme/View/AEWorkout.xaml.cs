using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Gymme.View
{
    public partial class AEWorkout : UserControl, IAEView
    {
        private Dictionary<Control, DependencyProperty> _bindingElements;

        public AEWorkout()
        {
            InitializeComponent();
            Loaded += (o, e) => RegisterBindingElements();
        }

        public void UpdateDataSources()
        {
            foreach (var bindingElement in _bindingElements)
            {
                var expression = bindingElement.Key.GetBindingExpression(bindingElement.Value);
                if (expression != null)
                {
                    expression.UpdateSource();
                }
            }
        }

        private void RegisterBindingElements()
        {
            _bindingElements = new Dictionary<Control, DependencyProperty>
            {
                {tbName, TextBox.TextProperty},
                {tbNote, TextBox.TextProperty}
            };
        }
    }
}
