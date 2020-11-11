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

        public async Task ShowError(string title, string message, string confirm = "Ok")
        {
            await Application.Current.MainPage.DisplayAlert(title, message, confirm);
        }

        public async Task<string> DisplayPrompt(string title, string message, string accept = "Ok", string cancel = "Cancel", string placeholder = "", 
            int maxLength = -1, Keyboard keyboard = null, string initialValue = "")
        {
            return await Application.Current.MainPage.DisplayPromptAsync(title, message, accept, cancel, placeholder, maxLength, keyboard, initialValue);
        }
    }
}
