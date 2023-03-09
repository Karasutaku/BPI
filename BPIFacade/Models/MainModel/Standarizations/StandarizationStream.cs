using BPIFacade.Models.DbModel;

namespace BPIFacade.Models.MainModel.Standarizations
{
    public class StandarizationStream
    {
        public QueryModel<Standarizations> standarizationDetails { get; set; } = new();
        public List<BPIFacade.Models.MainModel.Stream.FileStream> files { get; set; } = new List<BPIFacade.Models.MainModel.Stream.FileStream>();
    }
}
