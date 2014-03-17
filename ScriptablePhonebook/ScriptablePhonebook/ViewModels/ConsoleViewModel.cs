using System.ComponentModel.Composition;
using System.Windows.Controls;
using System.Windows.Input;
using Caliburn.Micro;
using ScriptablePhonebook.Automation;

namespace ScriptablePhonebook.ViewModels
{
    [Export(typeof(ConsoleViewModel))]
    public class ConsoleViewModel : Screen
    {
        private readonly IPowerShellHelper psHelper;

        private string input;
        private string output;

        [ImportingConstructor]
        public ConsoleViewModel(IPowerShellHelper psHelper)
        {
            this.psHelper = psHelper;

            DisplayName = "Phonebook Console";
        }

        public string Input
        {
            get
            {
                return input;
            }
            set
            {
                input = value;
                NotifyOfPropertyChange(() => Input);
            }
        }

        public string Output
        {
            get
            {
                return output;
            }
            set
            {
                output = value;
                NotifyOfPropertyChange(() => Output);
            }
        }

        public void Run()
        {
            var result = psHelper.ExecutePS(input);
            Output += result;
        }

        public void Clear()
        {
            Output = string.Empty;
            Input = string.Empty;
        }

        public void HandlePreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.F5) Run();
        }

        public void ScrollToEnd(TextBox textBox)
        {
            textBox.ScrollToEnd();
        }
    }
}
