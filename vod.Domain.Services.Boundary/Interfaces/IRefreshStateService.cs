using vod.Domain.Services.Boundary.Interfaces.Enums;
using vod.Domain.Services.Boundary.Models;

namespace vod.Domain.Services.Boundary.Interfaces
{
    public interface IRefreshStateService
    {
        RefreshState GetRefreshState();
        void SetCurrentRefreshState(MovieTypes movieType);
        void RemoveCurrentRefreshState();
    }
}
