using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace WebApi.BackgroundProcessing
{
    public interface IBackgroundProcessor:IHostedService
    {
        Task QueueBackgroundTask(Action workItem);
        int GetQueuedBackgroundTaskCount();
    }
}
