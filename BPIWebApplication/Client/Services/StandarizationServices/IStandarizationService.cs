using BPIWebApplication.Shared.DbModel;
using BPIWebApplication.Shared.MainModel;
using BPIWebApplication.Shared.MainModel.Standarizations;

namespace BPIWebApplication.Client.Services.StandarizationServices
{
    public interface IStandarizationService
    {
        List<StandarizationType> standarizationTypes { get; set; }
        List<Standarizations> standarizations { get; set; }
        List<BPIWebApplication.Shared.MainModel.Stream.FileStream> fileStreams { get; set; }

        Task<ResultModel<List<StandarizationType>>> getStandarizationTypes();
        Task<ResultModel<List<Standarizations>>> getStandarizations(string param);
        Task<ResultModel<List<BPIWebApplication.Shared.MainModel.Stream.FileStream>>> getFileStream(string param);
        Task<ResultModel<QueryModel<Standarizations>>> createStandarizations(StandarizationStream data);
        Task<ResultModel<QueryModel<Standarizations>>> updateStandarizations(StandarizationStream data);
        Task<ResultModel<QueryModel<string>>> deleteStandarizations(QueryModel<string> data);

    }
}
