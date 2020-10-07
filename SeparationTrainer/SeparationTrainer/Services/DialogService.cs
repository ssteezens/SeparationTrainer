using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SeparationTrainer.Services
{
    public class DialogService : IDialogService
    {
        public async Task<bool> ShowAlert(string title, string message, string accept = "Yes", string cancel = "No")
        {
            return await Application.Current.MainPage.DisplayAlert(title, message, accept, cancel);
        }
    }
}
