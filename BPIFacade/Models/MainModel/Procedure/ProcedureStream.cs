using BPIFacade.Models.DbModel;

namespace BPIFacade.Models.MainModel.Procedure
{
    public class ProcedureStream
    {
        public QueryModel<Procedure> procedureDetails { get; set; } = new QueryModel<Procedure>();
        public List<BPIFacade.Models.MainModel.Stream.FileStream> files { get; set; } = new List<BPIFacade.Models.MainModel.Stream.FileStream>();
    }
}
