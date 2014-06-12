using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Telerik.Windows.Controls;

namespace Gymme.View.Controls
{
    public partial class AEExercise : AdvancedBindingUC, IAEView
    {
        public AEExercise()
        {
            InitializeComponent();
        }

        private void InputBox_TextInput(object sender, KeyEventArgs keyEventArgs)
        {
            UpdateDataSources();
        }

        protected override Dictionary<Control, DependencyProperty> GetBindingElements()
        {
            return new Dictionary<Control, DependencyProperty>
                {
                    {tbName, TextBox.TextProperty},
                    {acCategory, TextBox.TextProperty}
                };
        }
    }
}
