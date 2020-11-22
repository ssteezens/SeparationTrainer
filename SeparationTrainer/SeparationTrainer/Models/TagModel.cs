using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeparationTrainer.Models
{
    public class TagModel : ObservableObject
    {
        private string _name;
        public int Id { get; set; }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value, nameof(Name));
        }

        public override string ToString()
        {
            return $"{Id} {Name}";
        }
    }
}
