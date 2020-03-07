using vod.Domain.Services.Boundary.Interfaces.Enums;

namespace vod.Domain.Services.Boundary.Interfaces
{
    public interface IRefreshDataService
    {
        bool Refresh(MovieTypes type);
    }
}
