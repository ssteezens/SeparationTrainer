using SeparationTrainer.Models;
using Xamarin.Forms;

namespace SeparationTrainer.Converters
{
    public class TagTemplateSelector : DataTemplateSelector
    {
        public DataTemplate TagTemplate { get; set; }

        public DataTemplate AddButtonTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is ActivityTagModel)
                return TagTemplate;
            else
                return AddButtonTemplate;
        }
    }
}
