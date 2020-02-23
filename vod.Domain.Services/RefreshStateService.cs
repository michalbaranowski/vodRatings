using vod.Domain.Services.Boundary.Interfaces;
using vod.Domain.Services.Boundary.Interfaces.Enums;
using vod.Domain.Services.Boundary.Models;

namespace vod.Domain.Services
{
    public class RefreshStateService : IRefreshStateService
    {
        private RefreshState _state;

        public RefreshStateService()
        {
            this.RemoveCurrentRefreshState();
        }

        public RefreshState GetRefreshState()
        {
            return _state;
        }

        public void RemoveCurrentRefreshState()
        {
            _state = new RefreshState();
        }

        public void SetCurrentRefreshState(MovieTypes movieType)
        {
            _state.IsRefreshingNow = true;
            _state.RefreshingType = movieType;
        }
    }
}
