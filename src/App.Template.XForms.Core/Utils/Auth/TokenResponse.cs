namespace App.Template.XForms.Core.Utils.Auth
{
    public class TokenResponse
    {
        public AccessToken AccessToken { get; set; }
        public ErrorResult Error { get; set; }

        public bool IsSuccess => AccessToken != null && Error == null;
        public bool IsError => !IsSuccess;
    }
}