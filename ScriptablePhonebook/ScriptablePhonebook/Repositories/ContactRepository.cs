using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Caliburn.Micro;
using ScriptablePhonebook.Automation;
using ScriptablePhonebook.Models;

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
		public ContactRepository(IEventAggregator eventAggregator, IPowerShellConfig psConfig)
		{
			this.contacts = new List<Contact>(sampleData);
			this.eventAggregator = eventAggregator;

			psConfig.SetVariable("repository", this);
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
