using System;
using System.Management.Automation.Runspaces;

namespace ScriptablePhonebook.Automation
{
    public interface IPowerShellConfig
    {
        string Profile { get; }

        Runspace Runspace { get; }
        
        void SetVariable(string name, object value);
    }
}
