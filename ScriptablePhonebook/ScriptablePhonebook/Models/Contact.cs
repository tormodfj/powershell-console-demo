using Caliburn.Micro;

namespace ScriptablePhonebook.Models
{
    public class Contact : PropertyChangedBase
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
