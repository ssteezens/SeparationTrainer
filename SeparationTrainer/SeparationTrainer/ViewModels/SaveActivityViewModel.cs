using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SeparationTrainer.ViewModels
{
    public class SaveActivityViewModel : BaseViewModel
    {
        public SaveActivityViewModel()
        {
            SubmitCommand = new Command(Submit);
        }

        public List<string> StressLevels => new List<string>() {"1", "2", "3", "4", "5", "6", "7"};

        public string SelectedStressLevel { get; set; }

        public string Notes { get; set; }

        public Command SubmitCommand { get; }

        private void Submit()
        {

        }
    }
}
