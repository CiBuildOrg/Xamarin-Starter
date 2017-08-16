using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using App.Template.XForms.Core.Exceptions;
using RestSharp.Portable;
using Validation;

namespace App.Template.XForms.Core.Utils.Auth
{
    /// <summary>
    /// Extensions to the <see cref="IRestClient"/> interface.
    /// </summary>
    public static class RestClientExtensions
    {
        /// <summary>
        /// Executes a request asynchronously.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="request">The request.</param>
        /// <param name="token">The token.</param>
        /// <returns>The asynchronous request task.</returns>
        public static Task<IRestResponse> ExecuteAsync(this IRestClient client, IRestRequest request,
            CancellationToken token)
        {
            Requires.NotNull(client, "client");
            Requires.NotNull(request, "request");

            var taskCompletionSource = new TaskCompletionSource<IRestResponse>();

            try
            {
                client.ExecuteAsync(request, token).ContinueWith(x =>
                {
                    var response = x.Result;
                    if (token.IsCancellationRequested)
                    {
                        taskCompletionSource.TrySetCanceled();
                    }

                    else if (response.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        taskCompletionSource.TrySetException(new HttpException((int) HttpStatusCode.InternalServerError,
                            "An error occured while processing the request."));
                    }

                    else if (response.StatusCode == HttpStatusCode.RequestTimeout)
                    {
                        taskCompletionSource.TrySetException(new HttpException((int) HttpStatusCode.RequestTimeout,
                            "The request was aborted."));
                    }

                    else
                    {
                        taskCompletionSource.TrySetResult(response);
                    }
                }, token);

                token.Register(() =>
                {
                    taskCompletionSource.TrySetCanceled();
                });
            }
            catch (Exception ex)
            {
                taskCompletionSource.TrySetException(ex);
            }

            return taskCompletionSource.Task;
        }
    }
}