using BPIWebApplication.Shared.DbModel;

namespace BPIWebApplication.Shared.MainModel.Standarizations
{
    public class StandarizationStream
    {
        public QueryModel<Standarizations> standarizationDetails { get; set; } = new();
        public List<BPIWebApplication.Shared.MainModel.Stream.FileStream> files { get; set; } = new List<BPIWebApplication.Shared.MainModel.Stream.FileStream>();
    }
}
