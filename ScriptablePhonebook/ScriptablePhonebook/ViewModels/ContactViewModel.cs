using System.ComponentModel;
using System.Diagnostics;
using Caliburn.Micro;
using ScriptablePhonebook.Models;

namespace ScriptablePhonebook.ViewModels
{
    [DebuggerDisplay("{Name} ({Number})")]
    public class ContactViewModel : PropertyChangedBase
    {
        private Contact model;

        public ContactViewModel(Contact model)
        {
            this.model = model;
            model.PropertyChanged += HandleModelPropertyChanged;
        }

        public Contact Model
        {
            get { return model; }
        }

        public string Name
        {
            get { return model.Name; }
            set { model.Name = value; }
        }

        public string Number
        {
            get { return model.Number; }
            set { model.Number = value; }
        }

        private void HandleModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NotifyOfPropertyChange(e.PropertyName);
        }
    }
}
