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
        private Theme _currentTheme = Theme.Dark;

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

                switch (CurrentTheme)
                {
                    case Theme.Dark:
                        mergedDictionaries.Add(new LightTheme());
                        CurrentTheme = Theme.Light;
                        break;
                    case Theme.Light:
                    default:
                        mergedDictionaries.Add(new DarkTheme());
                        CurrentTheme = Theme.Dark;
                        break;
                }
            }
        }

        public Theme CurrentTheme
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
