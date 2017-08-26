namespace App.Template.XForms.Core.Utils.Auth
{
    public class GetAccessTokenResponse
    {
        public AccessToken AccessToken { get; set; }
        public ErrorResult Error { get; set; }

        public bool HasAccessToken => AccessToken != null;
    }
}