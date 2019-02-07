using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace HomesEngland.BackgroundProcessing
{
    public interface IBackgroundProcessor:IHostedService
    {
        Task QueueBackgroundTask(Action workItem);
        CancellationToken GetCancellationToken();
    }
}
