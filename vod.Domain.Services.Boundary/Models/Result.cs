using vod.Domain.Services.Boundary.Interfaces.Enums;

namespace vod.Domain.Services
{
    public class Result
    {
        public string Title { get; set; }
        public string ProviderName { get; set; }
        public MovieTypes MovieType { get; set; }
    }
}