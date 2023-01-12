using BPIBR.Models.DbModel;

namespace BPIBR.Models.MainModel.Procedure
{
    public class ProcedureStream
    {
        public QueryModel<Procedure> procedureDetails { get; set; } = new QueryModel<Procedure>();
        public List<BPIBR.Models.MainModel.Stream.FileStream> files { get; set; } = new List<BPIBR.Models.MainModel.Stream.FileStream>();
    }
}
