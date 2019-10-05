using vod.Domain.Services.Boundary;
using vod.Domain.Services.Boundary.Interfaces.Enums;

namespace vod.Domain.Services
{
    public class BackgroundWorkerStateManager : IBackgroundWorkerStateManager
    {
        private BgWorkerStatesEnum _state;

        public void SetState(BgWorkerStatesEnum state)
        {
            _state = state;
        }

        public bool IsBusy()
        {
            return _state == BgWorkerStatesEnum.Busy;
        }
    }
}
