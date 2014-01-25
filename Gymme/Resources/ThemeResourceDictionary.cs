using System.ComponentModel;
using System.Windows;

namespace Gymme.Resources
{
    /// <summary>
    /// Provides automatic selection of resources based on the current theme
    /// </summary>
    public class ThemeResourceDictionary : ResourceDictionary
    {
        private ResourceDictionary lightResources;
        private ResourceDictionary darkResources;

        /// <summary>
        /// Gets or sets the <see cref="ResourceDictioary"/> to use when in the "light" theme
        /// </summary>
        public ResourceDictionary LightResources
        {
            get { return lightResources; }
            set
            {
                lightResources = value;

                if (!IsDarkTheme && value != null)
                {
                    MergedDictionaries.Add(value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="ResourceDictioary"/> to use when in the "dark" theme
        /// </summary>
        public ResourceDictionary DarkResources
        {
            get { return darkResources; }
            set
            {
                darkResources = value;

                if (IsDarkTheme && value != null)
                {
                    MergedDictionaries.Add(value);
                }
            }
        }

        /// <summary>
        /// Determines if the application is running in the dark theme
        /// </summary>
        private bool IsDarkTheme
        {
            get
            {
                if (IsDesignMode)
                {
                    return true;
                }
                else
                {
                    return (Visibility)Application.Current.Resources["PhoneDarkThemeVisibility"]
                        == Visibility.Visible;
                }
            }
        }

        /// <summary>
        /// Determines if the application is being run by a design tool
        /// </summary>
        private bool IsDesignMode
        {
            get
            {
                // VisualStudio sometimes returns false for DesignMode, DesignTool is our backup
                return DesignerProperties.GetIsInDesignMode(this) ||
                    DesignerProperties.IsInDesignTool;
            }
        }
    }
}