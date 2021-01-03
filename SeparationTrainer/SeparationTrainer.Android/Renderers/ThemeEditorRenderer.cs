using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using SeparationTrainer.Controls;
using SeparationTrainer.Droid.Renderers;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ThemeEditor), typeof(ThemeEditorRenderer))]
namespace SeparationTrainer.Droid.Renderers
{
    public class ThemeEditorRenderer : EditorRenderer
    {
        public ThemeEditorRenderer(Context context) : base(context)
        {
            
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (Control != null && e.NewElement != null)
            {
                var themeEditor = (ThemeEditor)e.NewElement;

                SetUnderlineColor(themeEditor);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            var themeEditor = (ThemeEditor)sender;

            SetUnderlineColor(themeEditor);
        }

        private void SetUnderlineColor(ThemeEditor themeEditor)
        {
            var thing = themeEditor.BottomLineColor.ToAndroid();

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                Control.BackgroundTintList = ColorStateList.ValueOf(thing);
            else
                Control.Background.SetColorFilter(thing, PorterDuff.Mode.SrcAtop);
            
        }
    }
}
