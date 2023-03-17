using BPIWebApplication.Shared.DbModel;

namespace BPIWebApplication.Shared.MainModel.PettyCash
{
    public class ReimburseStream
    {
        public QueryModel<Reimburse> reimburseDetails { get; set; } = new QueryModel<Reimburse>();
        public List<BPIWebApplication.Shared.MainModel.Stream.FileStream> files { get; set; } = new();
    }
}
