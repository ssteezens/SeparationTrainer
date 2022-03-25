using System;
using System.Runtime.Remoting.Contexts;
using IdentityLookup;
using SeparationTrainer.Controls;
using SeparationTrainer.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(AutoSelectEntry), typeof(AutoSelectEntryRenderer))]
namespace SeparationTrainer.iOS.Renderers
{
    public class AutoSelectEntryRenderer : EntryRenderer
    {
        public AutoSelectEntryRenderer()
        {

        }

        public AutoSelectEntryRenderer(Context context) : base()
        {
            
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                Control.EditingDidBegin += (sender, eIos) => {
                    Control.PerformSelector(new ObjCRuntime.Selector("selectAll"), null, 0.0f);
                };
            }
        }
    }
}