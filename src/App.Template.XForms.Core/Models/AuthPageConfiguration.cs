namespace App.Template.XForms.Core.Models
{
    /// <summary>
    /// Configure the appereance of the authentication page.
    /// </summary>
    public class AuthPageConfiguration
    {
        public string Title { get; set; } = "Authenticate";
        public string SubTitle { get; set; } = "Sign in to your Account";
        public bool ShowCloseButton { get; set; } = false;
        public bool ShowRegistrationButton { get; set; } = false;
        public bool ShowHeader { get; set; } = false;
    }
}