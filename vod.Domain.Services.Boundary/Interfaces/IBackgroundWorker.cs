using System;
using vod.Domain.Services.Boundary.Interfaces.Enums;

namespace vod.Domain.Services.Boundary.Interfaces
{
    public interface IBackgroundWorker
    {
        void Execute(MovieTypes type, Func<bool> func);
    }
}
