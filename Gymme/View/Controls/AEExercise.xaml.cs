﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.Phone.Controls;

namespace Gymme.View.Controls
{
    public partial class AEExercise : UserControl, IAEView
    {
        private Dictionary<Control, DependencyProperty> _bindingElements;

        public AEExercise()
        {
            InitializeComponent();
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
                {tbName, TextBox.TextProperty},
                {tbCategory, AutoCompleteBox.TextProperty}
            };
        }
    }
}
