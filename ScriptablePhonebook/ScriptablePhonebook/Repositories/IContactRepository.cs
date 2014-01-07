using ScriptablePhonebook.Models;
using System.Collections.Generic;

namespace ScriptablePhonebook.Repositories
{
    public interface IContactRepository
    {
        IEnumerable<Contact> GetAll();

        void Clear();

        void AddContact(Contact contact);

        void RemoveContact(Contact contact);
    }
}
