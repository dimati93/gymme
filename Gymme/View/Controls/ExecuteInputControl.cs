using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Gymme.View.Controls
{
    public abstract class ExecuteInputControl : AdvancedBindingUC
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
    }
}