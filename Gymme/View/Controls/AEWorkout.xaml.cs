using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Gymme.View.Controls
{
    public partial class AEWorkout : AdvancedBindingUC, IAEView
    {

        public AEWorkout()
        {
            InitializeComponent();
        }

        protected override Dictionary<Control, DependencyProperty> GetBindingElements()
        {
            return new Dictionary<Control, DependencyProperty>
                {
                    {tbName, TextBox.TextProperty},
                    {tbNote, TextBox.TextProperty}
                };
        }

        private void InputBox_TextInput(object sender, KeyEventArgs keyEventArgs)
        {
            UpdateDataSources();
        }
    }
}
