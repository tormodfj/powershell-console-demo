using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using ScriptablePhonebook.Models;
using ScriptablePhonebook.Repositories;

namespace ScriptablePhonebook.ViewModels
{
    [Export(typeof(PhonebookViewModel))]
    public class PhonebookViewModel : Screen, IHandle<ContactRepositoryModifiedMessage>
    {
        private readonly IContactRepository contactRepository;
        private readonly IWindowManager windowManager;
        private ContactViewModel selectedContact;

        [ImportingConstructor]
        public PhonebookViewModel(IEventAggregator eventAggregator, IWindowManager windowManager, IContactRepository contactRepository)
        {
            eventAggregator.Subscribe(this);

            this.contactRepository = contactRepository;
            this.windowManager = windowManager;

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
            var vm = new EditContactViewModel();
            var result = windowManager.ShowDialog(vm);
            if(result == true)
            {
                contactRepository.AddContact(vm.Result);
            }
        }

        public bool CanEditSelectedContact { get { return selectedContact != null; } }
        public void EditSelectedContact()
        {
            var vm = new EditContactViewModel(selectedContact.Model);
            var result = windowManager.ShowDialog(vm);
        }

        public bool CanDeleteSelectedContact { get { return selectedContact != null; } }
        public void DeleteSelectedContact()
        {
            contactRepository.RemoveContact(selectedContact.Model);
        }

        public void OpenConsole()
        {
            var vm = IoC.Get<ConsoleViewModel>();
            var consoleIsAlreadyOpen = vm.IsActive;
            if(consoleIsAlreadyOpen)
            {
                BringToFront(vm);
            }
            else
            {
                windowManager.ShowWindow(vm);
            }
        }

        public void Handle(ContactRepositoryModifiedMessage message)
        {
            RefreshFromRepository();
        }

        protected override void OnInitialize()
        {
            RefreshFromRepository();
        }

        protected override void OnDeactivate(bool close)
        {
            if(close)
            {
                Application.Current.Shutdown();
            }
            base.OnDeactivate(close);
        }

        private void RefreshFromRepository()
        {
            var contactViewModels = contactRepository.GetAll().Select(CreateContactViewModel);
            Contacts.Clear();
            Contacts.AddRange(contactViewModels);
        }

        private ContactViewModel CreateContactViewModel(Contact contact)
        {
            return new ContactViewModel(contact);
        }

        private static void BringToFront(IViewAware vm)
        {
            var window = vm.GetView() as Window;
            if(window != null)
            {
                window.Activate();
            }
        }
    }
}
