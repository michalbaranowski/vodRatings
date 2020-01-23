using System.Collections.Generic;
using vod.Domain.Services.Boundary.Commands;
using vod.Domain.Services.Boundary.Models;

namespace vod.Domain.Services.Boundary.Interfaces
{
    public interface IStoredDataManager
    {
        IEnumerable<FilmwebResult> UseStorageIfPossible(UseStorageIfPossibleCommand command);
    }
}
