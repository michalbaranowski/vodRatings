using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using vod.Domain.Services.Boundary.Interfaces;

namespace vod.Domain.Services
{
    public class BackgroundWorker : IBackgroundWorker
    {
        public void Execute(Func<bool> func)
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                var result = func();
                if(!result)
                    throw new Exception("Background work failed.");
            }).Start();
        }
    }
}
