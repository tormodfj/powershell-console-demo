using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;

namespace ScriptablePhonebook.Automation
{
    [Export(typeof(IPowerShellHelper))]
    public class PowerShellHelper : IPowerShellHelper
    {
        private readonly IPowerShellConfig config;
        private readonly PowerShell shell;

        [ImportingConstructor]
        public PowerShellHelper(IPowerShellConfig config)
        {
            this.config = config;

            shell = PowerShell.Create();
            shell.Runspace = config.Runspace;
            if (!string.IsNullOrEmpty(config.Profile) && File.Exists(config.Profile))
            {
                var profileScript = File.ReadAllText(config.Profile);
                shell.AddScript(profileScript);
                shell.Invoke();
                shell.Commands.Clear();
            }
        }

        public string ExecutePS(string script)
        {
            var output = new StringBuilder();
            output.AppendFormat("> {0}", script).AppendLine();

            shell.AddScript(script);
            shell.AddCommand("Out-String");

            try
            {
                var results = shell.Invoke();
                if(shell.Streams.Error.Any())
                {
                    foreach(var error in shell.Streams.Error)
                    {
                        AppendError(output, error);
                    }
                    shell.Streams.Error.Clear();
                }
                else
                {
                    foreach(var result in results)
                    {
                        output.Append(result);
                    }
                }
            }
            catch (Exception ex)
            {
                output.Append(ex.Message);
            }

            shell.Commands.Clear();
            return output.ToString();
        }

        public void SetVariable(string name, object value)
        {
            config.SetVariable(name, value);
        }

        private void AppendError(StringBuilder output, ErrorRecord error)
        {
            output.AppendLine(error.ToString());
            output.AppendFormat("   +{0}", error.InvocationInfo.PositionMessage).AppendLine();
            output.AppendFormat("   + CategoryInfo          :{0}", error.CategoryInfo).AppendLine();
            output.AppendFormat("   + FullyQualifiedErrorId :{0}", error.FullyQualifiedErrorId).AppendLine();
        }
    }
}
