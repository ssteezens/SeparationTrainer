using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.OS;
using Android.Widget;
using SeparationTrainer.Controls;
using SeparationTrainer.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Xamarin.Forms.Color;

[assembly: ExportRenderer(typeof(AutoSelectEntry), typeof(AutoSelectEntryRenderer))]
namespace SeparationTrainer.Droid.Renderers
{
    public class AutoSelectEntryRenderer : EntryRenderer
    {
        public AutoSelectEntryRenderer(Context context) : base(context)
        {
            
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                var nativeEditText = (EditText)Control;
                nativeEditText.SetSelectAllOnFocus(true);
            }
        }
    }
}
