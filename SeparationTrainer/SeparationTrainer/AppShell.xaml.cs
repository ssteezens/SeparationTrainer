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
        }

        private void OnToggleTheme(object sender, EventArgs e)
        {
            var mergedDictionaries = Application.Current.Resources.MergedDictionaries;

            if (mergedDictionaries != null)
            {
                mergedDictionaries.Clear();

                switch (AppState.CurrentTheme)
                {
                    case Theme.Dark:
                        mergedDictionaries.Add(new LightTheme());
                        AppState.CurrentTheme = Theme.Light;
                        break;
                    case Theme.Light:
                    default:
                        mergedDictionaries.Add(new DarkTheme());
                        AppState.CurrentTheme = Theme.Dark;
                        break;
                }
            }
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
