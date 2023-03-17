using BPIBR.Models.DbModel;

namespace BPIBR.Models.MainModel.PettyCash
{
    public class ReimburseStream
    {
        public QueryModel<Reimburse> reimburseDetails { get; set; } = new QueryModel<Reimburse>();
        public List<BPIBR.Models.MainModel.Stream.FileStream> files { get; set; } = new();
    }
}
