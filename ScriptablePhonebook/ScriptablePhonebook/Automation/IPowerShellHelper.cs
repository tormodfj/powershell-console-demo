using System;

namespace ScriptablePhonebook.Automation
{
    public interface IPowerShellHelper
    {
        string ExecutePS(string script);

        void SetVariable(string name, object value);
    }
}
