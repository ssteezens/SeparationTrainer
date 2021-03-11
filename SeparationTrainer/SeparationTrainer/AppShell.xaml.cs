using SeparationTrainer.ViewModels;
using SeparationTrainer.Views;
using System;
using System.Collections.Generic;
using SeparationTrainer.Themes;
using Xamarin.Forms;

namespace SeparationTrainer
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();

            if (!Application.Current.Properties.ContainsKey("CurrentTheme"))
            {
                Application.Current.Properties["CurrentTheme"] = AppState.CurrentTheme;
            }

            var currentTheme = Application.Current.Properties["CurrentTheme"];

            if (currentTheme == null)
                AppState.CurrentTheme = Theme.Light;
            else
                AppState.CurrentTheme = (Theme)currentTheme;

            SaveThemeToDisk();
            LoadThemeStyleSheet();
        }

        private void LoadThemeStyleSheet()
        {
            var mergedDictionaries = Application.Current.Resources.MergedDictionaries;

            if (mergedDictionaries != null)
            {
                mergedDictionaries.Clear();

                switch (AppState.CurrentTheme)
                {
                    case Theme.Dark:
                        mergedDictionaries.Add(new DarkTheme());
                        break;
                    case Theme.Light:
                    default:
                        mergedDictionaries.Add(new LightTheme());
                        break;
                }
            }
        }

        public bool DarkThemeIsToggled
        {
            get => AppState.CurrentTheme == Theme.Dark;
            set
            {
                AppState.CurrentTheme = value 
                    ? Theme.Dark 
                    : Theme.Light;
                SaveThemeToDisk();
                LoadThemeStyleSheet();
                ThemeToggleSwitch.IsToggled = DarkThemeIsToggled;
            }
        }

        private void SaveThemeToDisk()
        {
            Application.Current.Properties["CurrentTheme"] = AppState.CurrentTheme;
        }
    }

    public static class AppState
    {
        private static Theme _currentTheme = Theme.Dark;

        public static Theme CurrentTheme
        {
            get => _currentTheme;
            set => _currentTheme = value;
        }
    }

    public enum Theme
    {
        Light,
        Dark
    }
}
