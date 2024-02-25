using RandomUserGeneratorTestApp.BusinessLogic.ViewModels.Requests;
using RandomUserGeneratorTestApp.BusinessLogic.ViewModels.Responses;

namespace RandomUserGeneratorTestApp.BusinessLogic.Services.Interfaces
{
    public interface IStatisticsService
    {
        string[] GetStatsSummaryText(UserRequest[] users);
        
        StatsResponse GetStats(UserRequest[] users);
    }
}
