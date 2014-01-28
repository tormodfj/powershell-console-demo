using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Management.Automation.Runspaces;

namespace ScriptablePhonebook.Automation
{
    [Export(typeof(IPowerShellConfig))]
    public class PowerShellConfig : IPowerShellConfig
    {
        [ImportingConstructor]
        public PowerShellConfig([Import("Profile")] string profile)
        {
            Runspace = RunspaceFactory.CreateRunspace();
            Runspace.ThreadOptions = PSThreadOptions.UseCurrentThread; // Execute on the thread that calls the Invoke method
            Runspace.Open();

            Profile = Path.Combine(Environment.CurrentDirectory, profile);
            AddVariable("profile", Profile);
        }

        public string Profile { get; private set; }

        public Runspace Runspace { get; private set; }

        public void AddVariable(string name, object value)
        {
            Runspace.SessionStateProxy.SetVariable(name, value);
        }
    }
}
