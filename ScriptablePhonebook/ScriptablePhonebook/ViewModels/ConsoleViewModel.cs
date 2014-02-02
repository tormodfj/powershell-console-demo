using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace ScriptablePhonebook.ViewModels
{
    [Export(typeof(ConsoleViewModel))]
    public class ConsoleViewModel : Screen
    {
        public ConsoleViewModel()
        {
            DisplayName = "Phonebook Console";
        }
    }
}
