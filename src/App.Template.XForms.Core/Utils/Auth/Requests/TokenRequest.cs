using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using App.Template.XForms.Core.Exceptions;
using ModernHttpClient;
using Validation;

namespace App.Template.XForms.Core.Utils.Auth.Requests
{
    /// <summary>
    /// This class represents a OAuth 2.0 token request.
    /// </summary>
    internal abstract class TokenRequest
    {
        public async Task<string> MakeCall(string tokensUri, CancellationToken cancellationToken)
        {
            Requires.NotNull(tokensUri, "tokensUri");
      
            var parameters = GetParameters();

            Verify.Operation(parameters != null, "The GetParameters method must not return a null instance.");

            using (var httpClient = new HttpClient(new NativeMessageHandler()))
            {
                var response = await httpClient.PostAsync(tokensUri, new FormUrlEncodedContent(parameters), cancellationToken)
                    .ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                }
                if (response.StatusCode == HttpStatusCode.RequestTimeout)
                {
                    throw new HttpException((int)HttpStatusCode.RequestTimeout,
                        "The request was aborted.");
                }
            }

            throw new Exception("Could not make call");
        }

        /// <summary>
        /// Gets the parameters representing the request.
        /// </summary>
        /// <returns>The parameters.</returns>
        protected abstract NameValueCollection GetParameters();
    }
}