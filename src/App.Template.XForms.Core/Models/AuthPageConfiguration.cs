namespace App.Template.XForms.Core.Models
{
    /// <summary>
    /// Configure the appereance of the authentication page.
    /// </summary>
    public class AuthPageConfiguration
    {
        public string SubTitle { get; set; } = "Sign in to your Account";
        public bool ShowRegistrationButton { get; set; } = false;
    }
}