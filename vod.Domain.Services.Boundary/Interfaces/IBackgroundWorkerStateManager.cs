using vod.Domain.Services.Boundary.Interfaces.Enums;

namespace vod.Domain.Services.Boundary
{
    public interface IBackgroundWorkerStateManager
    {
        void SetState(BgWorkerStatesEnum state);
        bool IsBusy();
    }
}
