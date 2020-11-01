using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SeparationTrainer.Services
{
    public interface IDialogService
    {
        Task<bool> ShowAlert(string title, string message, string accept = "Yes", string cancel = "No");

        Task<string> DisplayPrompt(string title, string message, string accept = "Ok", string cancel = "Cancel",
            string placeholder = "",
            int maxLength = -1, Keyboard keyboard = null, string initialValue = "");
    }
}
