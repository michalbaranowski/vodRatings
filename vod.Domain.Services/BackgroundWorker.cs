using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using vod.Domain.Services.Boundary;
using vod.Domain.Services.Boundary.Interfaces;
using vod.Domain.Services.Boundary.Interfaces.Enums;

namespace vod.Domain.Services
{
    public class BackgroundWorker : IBackgroundWorker
    {
        private readonly IBackgroundWorkerStateManager _stateManager;
        private readonly Queue<KeyValuePair<MovieTypes,Func<bool>>> _queue;

        public BackgroundWorker(IBackgroundWorkerStateManager stateManager)
        {
            _stateManager = stateManager;
            _queue = new Queue<KeyValuePair<MovieTypes, Func<bool>>>();
        }

        public void Execute(MovieTypes type, Func<bool> func)
        {
            if (_stateManager.IsBusy() && _queue.All(n => n.Key != type))
            {
                _queue.Enqueue(new KeyValuePair<MovieTypes, Func<bool>>(type, func));
                return;
            }

            new Thread(() =>
            {
                try
                {
                    Thread.CurrentThread.IsBackground = true;
                    _stateManager.SetState(BgWorkerStatesEnum.Busy);

                    var result = func();
                    if (!result)
                        throw new Exception("Background work failed.");

                }
                finally
                {
                    _stateManager.SetState(BgWorkerStatesEnum.Active);

                    if (_queue.Any())
                    {
                        var val = _queue.Dequeue();
                        Execute(val.Key, val.Value);
                    }
                }
            }).Start();
        }
    }
}
