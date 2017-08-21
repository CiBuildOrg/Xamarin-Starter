
namespace App.Template.XForms.Core.Options
{
    public static class ResourceKeys
    {
        public static bool IsDebug
        {
            get
            {
#if DEBUG
                return true;
#else
                return false;
#endif
            }
        }
    }
}