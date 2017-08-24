using System.Threading;
using System.Threading.Tasks;

namespace App.Template.XForms.Core.Extensions
{
    public static class TaskExtensions
    {
        public static Task<T> WaitAsync<T>(this Task<T> task)
        {
            // Ensure that awaits were called with .ConfigureAwait(false)

            var wait = new ManualResetEventSlim(false);

            var continuation = task.ContinueWith(_ =>
            {
                wait.Set();
                return _.Result;
            });

            wait.Wait();

            return continuation;
        }

        public static Task WaitAsync(this Task task)
        {
            // Ensure that awaits were called with .ConfigureAwait(false)

            var wait = new ManualResetEventSlim(false);

            var continuation = task.ContinueWith(_ =>
            {
                wait.Set();
            });

            wait.Wait();

            return continuation;
        }
    }
}
