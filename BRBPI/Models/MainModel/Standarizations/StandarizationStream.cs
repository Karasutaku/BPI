using BPIBR.Models.DbModel;

namespace BPIBR.Models.MainModel.Standarizations
{
    public class StandarizationStream
    {
        public QueryModel<Standarizations> standarizationDetails { get; set; } = new();
        public List<BPIBR.Models.MainModel.Stream.FileStream> files { get; set; } = new List<BPIBR.Models.MainModel.Stream.FileStream>();
    }
}
