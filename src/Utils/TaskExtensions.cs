using System;
using System.Threading;
using System.Threading.Tasks;

namespace HeyGen.StreamingAvatar
{
    internal static class TaskExtensions
    {
        public static async Task TimeoutAfter(this Task task, int milliseconds)
        {
            using var cts = new CancellationTokenSource();
            var timeoutTask = Task.Delay(milliseconds, cts.Token);
            var completedTask = await Task.WhenAny(task, timeoutTask);
            
            if (completedTask == timeoutTask)
            {
                throw new TimeoutException();
            }
            
            cts.Cancel();
            await task; // Propagate exceptions
        }

        public static async Task<T> TimeoutAfter<T>(this Task<T> task, int milliseconds)
        {
            using var cts = new CancellationTokenSource();
            var timeoutTask = Task.Delay(milliseconds, cts.Token);
            var completedTask = await Task.WhenAny(task, timeoutTask);
            
            if (completedTask == timeoutTask)
            {
                throw new TimeoutException();
            }
            
            cts.Cancel();
            return await task; // Propagate exceptions
        }

        public static Task WaitForConditionAsync(Func<bool> condition, int checkInterval = 100, int timeout = 30000)
        {
            return Task.Run(async () =>
            {
                var startTime = DateTime.UtcNow;

                while (!condition())
                {
                    if ((DateTime.UtcNow - startTime).TotalMilliseconds > timeout)
                    {
                        throw new TimeoutException("Condition was not met within the specified timeout.");
                    }

                    await Task.Delay(checkInterval);
                }
            });
        }
    }
}