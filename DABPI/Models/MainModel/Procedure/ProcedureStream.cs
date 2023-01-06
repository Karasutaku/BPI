using BPIDA.Models.DbModel;

namespace BPIDA.Models.MainModel.Procedure
{
    public class ProcedureStream
    {
        public QueryModel<Procedure> procedureDetails { get; set; } = new QueryModel<Procedure>();
        public List<BPIDA.Models.MainModel.Stream.FileStream> files { get; set; } = new List<BPIDA.Models.MainModel.Stream.FileStream>();
    }
}
