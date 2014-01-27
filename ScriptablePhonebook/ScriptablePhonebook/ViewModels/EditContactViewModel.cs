using Caliburn.Micro;
using ScriptablePhonebook.Models;

namespace ScriptablePhonebook.ViewModels
{
    public class EditContactViewModel : Screen
    {
        private string name;
        private string number;
        private Contact contact;

        public EditContactViewModel(Contact contact = null)
        {
            this.contact = contact;
            if (contact != null)
            {
                this.name = contact.Name;
                this.number = contact.Number;
            }

            DisplayName = "Contact Details";
        }

        public Contact Result
        {
            get { return contact; }
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                NotifyOfPropertyChange(() => Name);
                NotifyOfPropertyChange(() => CanSave);
            }
        }

        public string Number
        {
            get
            {
                return number;
            }
            set
            {
                number = value;
                NotifyOfPropertyChange(() => Number);
                NotifyOfPropertyChange(() => CanSave);
            }
        }

        public bool CanSave
        {
            get
            {
                return !string.IsNullOrWhiteSpace(name)
                    && !string.IsNullOrWhiteSpace(number);
            }
        }

        public void Save()
        {
            contact = contact ?? new Contact();
            contact.Name = name;
            contact.Number = number;

            TryClose(true);
        }

        public void Cancel()
        {
            TryClose(false);
        }
    }
}
