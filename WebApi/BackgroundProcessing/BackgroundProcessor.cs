using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WebApi.BackgroundProcessing
{
    public class BackgroundProcessor:IBackgroundProcessor
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly ReaderWriterLockSlim _readerWriterLockSlim;


        private IList<Task> _tasks;
        private IList<Task> Tasks
        {
            get
            {
                _readerWriterLockSlim.EnterReadLock();
                var tasks = _tasks;
                _readerWriterLockSlim.ExitReadLock();
                return tasks;
            }
            set
            {
                _readerWriterLockSlim.EnterWriteLock();
                _tasks = value;
                _readerWriterLockSlim.ExitWriteLock();
            }
        }

        public BackgroundProcessor()
        {
            _readerWriterLockSlim = new ReaderWriterLockSlim();
            Tasks = new List<Task>();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cancellationTokenSource.Cancel();
            return Task.CompletedTask;
        }

        public Task QueueBackgroundTask(Action workItem)
        {
            Task task = new Task(workItem, _cancellationTokenSource.Token, TaskCreationOptions.LongRunning)
                .ContinueWith(task1 => Tasks.Remove(task1));
            Tasks.Add(task);
            task.Start();
            return Task.CompletedTask;
        }

        public int GetQueuedBackgroundTaskCount()
        {
            return Tasks.Count;
        }
    }
}