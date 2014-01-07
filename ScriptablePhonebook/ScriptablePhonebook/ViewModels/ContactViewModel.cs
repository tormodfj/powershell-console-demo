using Caliburn.Micro;
using System.Diagnostics;

namespace ScriptablePhonebook.ViewModels
{
    [DebuggerDisplay("{Name} ({Number})")]
    public class ContactViewModel : PropertyChangedBase
    {
        private string name;
        private string number;

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
            }
        }
    }
}
