using System.Reflection;

namespace App.Template.XForms.Core.Bootstrapper
{
    internal class CoreAssemblyHelper
    {
        internal static Assembly CoreAssembly => typeof(CoreAssemblyHelper).GetTypeInfo().Assembly;
    }
}