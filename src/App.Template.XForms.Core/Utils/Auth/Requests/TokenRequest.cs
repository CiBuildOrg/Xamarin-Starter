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
    public class CallResult
    {
        public string Content { get; set; }
        public bool IsSuccess { get; set; }
    }



    /// <summary>
    /// This class represents a OAuth 2.0 token request.
    /// </summary>
    internal abstract class TokenRequest
    {
        public async Task<CallResult> MakeCall(string tokensUri, CancellationToken cancellationToken)
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
                    var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return new CallResult
                    {
                        Content = content,
                        IsSuccess = true
                    };
                }

                switch (response.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                    case HttpStatusCode.InternalServerError:
                        var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        return new CallResult
                        {
                            Content = content,
                            IsSuccess = false
                        };
                        // deserialize error middleware
                    case HttpStatusCode.RequestTimeout:
                        throw new HttpException((int)HttpStatusCode.RequestTimeout,
                            "The request was aborted.");
                    default:
                        throw new Exception("Could not make call");

                }
            }
        }

        /// <summary>
        /// Gets the parameters representing the request.
        /// </summary>
        /// <returns>The parameters.</returns>
        protected abstract NameValueCollection GetParameters();
    }
}