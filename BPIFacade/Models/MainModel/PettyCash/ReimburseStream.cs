using BPIFacade.Models.DbModel;

namespace BPIFacade.Models.MainModel.PettyCash
{
    public class ReimburseStream
    {
        public QueryModel<Reimburse> reimburseDetails { get; set; } = new QueryModel<Reimburse>();
        public List<BPIFacade.Models.MainModel.Stream.FileStream> files { get; set; } = new();
    }
}
