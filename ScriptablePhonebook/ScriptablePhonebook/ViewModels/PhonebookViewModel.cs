using Caliburn.Micro;
using ScriptablePhonebook.Models;
using ScriptablePhonebook.Repositories;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;

namespace ScriptablePhonebook.ViewModels
{
    [Export(typeof(PhonebookViewModel))]
    public class PhonebookViewModel : Screen, IHandle<ContactRepositoryModifiedMessage>
    {
        private readonly IContactRepository contactRepository;
        private ContactViewModel selectedContact;

        [ImportingConstructor]
        public PhonebookViewModel(IEventAggregator eventAggregator, IContactRepository contactRepository)
        {
            eventAggregator.Subscribe(this);

            this.contactRepository = contactRepository;

            DisplayName = "Scriptable Phonebook";
            Contacts = new BindableCollection<ContactViewModel>();
        }

        public BindableCollection<ContactViewModel> Contacts { get; private set; }
        public ContactViewModel SelectedContact
        {
            get { return selectedContact; }
            set
            {
                selectedContact = value;
                NotifyOfPropertyChange(() => SelectedContact);
                NotifyOfPropertyChange(() => CanEditSelectedContact);
                NotifyOfPropertyChange(() => CanDeleteSelectedContact);
            }
        }

        public void CreateNewContact()
        {
            MessageBox.Show("CreateNewContact");
        }

        public bool CanEditSelectedContact { get { return selectedContact != null; } }
        public void EditSelectedContact()
        {
            MessageBox.Show("EditSelectedContact");
        }

        public bool CanDeleteSelectedContact { get { return selectedContact != null; } }
        public void DeleteSelectedContact()
        {
            contactRepository.RemoveContact(selectedContact.Model);
        }

        public void Handle(ContactRepositoryModifiedMessage message)
        {
            RefreshFromRepository();
        }

        protected override void OnInitialize()
        {
            RefreshFromRepository();
        }

        private void RefreshFromRepository()
        {
            var contactViewModels = contactRepository.GetAll().Select(CreateContactViewModel);
            Contacts.Clear();
            Contacts.AddRange(contactViewModels);
        }

        private ContactViewModel CreateContactViewModel(Contact contact)
        {
            return new ContactViewModel
            {
                Name = contact.Name,
                Number = contact.Number,
                Model = contact
            };
        }
    }
}
