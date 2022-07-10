using SeparationTrainer.Services.Theme;
using Xamarin.Forms;
using AndroidApp = Android.App.Application;

[assembly: Dependency(typeof(SeparationTrainer.Droid.Services.Theme.ThemeService))]
namespace SeparationTrainer.Droid.Services.Theme
{
    public class ThemeService : IThemeService
    {
        public void SetTheme(SeparationTrainer.Theme theme)
        {
            switch(theme)
            {
                case SeparationTrainer.Theme.Dark:
                    MainActivity.Instance.SetTheme(Resource.Style.DarkTheme);
                    break;
                case SeparationTrainer.Theme.Light:
                    MainActivity.Instance.SetTheme(Resource.Style.LightTheme);
                    break;
                default:
                    MainActivity.Instance.SetTheme(Resource.Style.LightTheme);
                    break;
            }

            if (CurrentTheme != theme)
                MainActivity.Instance.Recreate();

            CurrentTheme = theme;
        }

        public SeparationTrainer.Theme CurrentTheme { get; set; }
    }
}
