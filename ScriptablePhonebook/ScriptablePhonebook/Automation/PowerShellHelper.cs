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
        private readonly PowerShell Shell;

        [ImportingConstructor]
        public PowerShellHelper(IPowerShellConfig config)
        {
            Shell = PowerShell.Create();
            Shell.Runspace = config.Runspace;
            if (!string.IsNullOrEmpty(config.Profile) && File.Exists(config.Profile))
            {
                var profileScript = File.ReadAllText(config.Profile);
                Shell.AddScript(profileScript);
                Shell.Invoke();
                Shell.Commands.Clear();
            }
        }

        public string ExecutePS(string script)
        {
            var output = new StringBuilder();
            output.AppendFormat("> {0}", script).AppendLine();

            Shell.AddScript(script);
            Shell.AddCommand("Out-String");

            try
            {
                var results = Shell.Invoke();
                if(Shell.Streams.Error.Any())
                {
                    foreach(var error in Shell.Streams.Error)
                    {
                        AppendError(output, error);
                    }
                    Shell.Streams.Error.Clear();
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

            Shell.Commands.Clear();
            return output.ToString();
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
