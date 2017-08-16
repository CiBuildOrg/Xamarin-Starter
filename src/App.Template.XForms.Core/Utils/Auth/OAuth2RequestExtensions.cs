using System;
using Xamarin.Auth;

namespace App.Template.XForms.Core.Utils.Auth
{
    public static class OAuth2RequestExtensions
    {
        public static Uri GetAuthenticatedUrl(AccessToken token, Uri unauthenticatedUrl, string accessTokenParameterName = "access_token")
        {
            if (token == null) throw new ArgumentNullException(nameof(token));

            if (unauthenticatedUrl == null) throw new ArgumentNullException(nameof(unauthenticatedUrl));

            var absUri = unauthenticatedUrl.AbsoluteUri;
            var url = $"{absUri}{(absUri.Contains("?") ? "&" : "?")}{accessTokenParameterName}={token.Token}";
            return new Uri(url);
        }

        public static string GetAuthorizationHeader(this OAuth2Request request, AccessToken token)
        {
            if (token == null) throw new ArgumentNullException(nameof(token));

            return $"Bearer {token.Token}";
        }
    }
}