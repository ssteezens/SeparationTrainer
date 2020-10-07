using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeparationTrainer.Services
{
    public interface IDialogService
    {
        Task<bool> ShowAlert(string title, string message, string accept = "Yes", string cancel = "No");
    }
}
