using Caliburn.Micro;
using ScriptablePhonebook.Models;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace ScriptablePhonebook.Repositories
{
    [Export(typeof(IContactRepository))]
    public class ContactRepository : IContactRepository
    {
        private readonly Contact[] sampleData =
        {
            new Contact { Name="Jens Stoltenberg", Number="12345678" },
            new Contact { Name="Erna Solberg", Number="44332211" },
            new Contact { Name="Siv Jensen", Number="98765432" },
        };
        private readonly IList<Contact> contacts;
        private readonly IEventAggregator eventAggregator;

        [ImportingConstructor]
        public ContactRepository(IEventAggregator eventAggregator)
        {
            this.contacts = new List<Contact>(sampleData);
            this.eventAggregator = eventAggregator;
        }

        public IEnumerable<Contact> GetAll()
        {
            return contacts.ToArray();
        }

        public void Clear()
        {
            contacts.Clear();
            eventAggregator.Publish(new ContactRepositoryModifiedMessage());
        }

        public void AddContact(Contact contact)
        {
            contacts.Add(contact);
            eventAggregator.Publish(new ContactRepositoryModifiedMessage());
        }

        public void RemoveContact(Contact contact)
        {
            contacts.Remove(contact);
            eventAggregator.Publish(new ContactRepositoryModifiedMessage());
        }
    }
}
