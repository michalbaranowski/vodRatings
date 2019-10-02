using System;
using System.Collections.Generic;
using System.Text;

namespace vod.Domain.Services.Boundary.Interfaces
{
    public interface IBackgroundWorker
    {
        void Execute(Func<bool> func);
    }
}
