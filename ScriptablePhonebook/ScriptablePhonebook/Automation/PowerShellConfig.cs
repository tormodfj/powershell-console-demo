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
            InitRunspace();
            InitProfile(profile);
        }

        public string Profile { get; private set; }

        public Runspace Runspace { get; private set; }

        public void SetVariable(string name, object value)
        {
            Runspace.SessionStateProxy.SetVariable(name, value);
        }

        private void InitRunspace()
        {
            Runspace = RunspaceFactory.CreateRunspace();
            Runspace.ThreadOptions = PSThreadOptions.UseCurrentThread;
            Runspace.Open();
        }

        private void InitProfile(string profile)
        {
            Profile = Path.Combine(Environment.CurrentDirectory, profile);
            SetVariable("profile", Profile);
        }
    }
}
