using System.Runtime.Remoting.Contexts;
using CoreAnimation;
using CoreGraphics;
using SeparationTrainer.Controls;
using SeparationTrainer.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ThemeEditor), typeof(ThemeEditorRenderer))]
namespace SeparationTrainer.iOS.Renderers
{
    public class ThemeEditorRenderer : EditorRenderer
    {
        public ThemeEditorRenderer() 
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
        
        private void SetUnderlineColor(ThemeEditor themeEditor)
        {
            var underlineColor = themeEditor.BottomLineColor.ToUIColor();
            
            var line  = new CALayer {
                BorderColor = underlineColor.CGColor,
                BackgroundColor = UIColor.FromRGB(174, 174, 174).CGColor,
                Frame = new CGRect (0, Frame.Height / 2, Frame.Width * 2, 1f)
            };

            Control.Layer.AddSublayer (line);
        }
    }
}