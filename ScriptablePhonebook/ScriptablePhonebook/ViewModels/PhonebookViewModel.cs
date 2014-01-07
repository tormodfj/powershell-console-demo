using Caliburn.Micro;
using ScriptablePhonebook.Models;
using ScriptablePhonebook.Repositories;
using System.ComponentModel.Composition;
using System.Linq;

namespace ScriptablePhonebook.ViewModels
{
    [Export(typeof(PhonebookViewModel))]
    public class PhonebookViewModel : Screen, IHandle<ContactRepositoryModifiedMessage>
    {
        private readonly IContactRepository contactRepository;

        [ImportingConstructor]
        public PhonebookViewModel(IContactRepository contactRepository)
        {
            this.contactRepository = contactRepository;

            DisplayName = "Scriptable Phonebook";
            Contacts = new BindableCollection<ContactViewModel>();
        }

        public BindableCollection<ContactViewModel> Contacts { get; private set; }

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
                Number = contact.Number
            };
        }
    }
}
