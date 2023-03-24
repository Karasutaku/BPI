
using BPIWebApplication.Shared.DbModel;
using BPIWebApplication.Shared.MainModel.Procedure;

namespace BPIWebApplication.Shared.FileUploadModel
{
    public class ProcedureStream
    {
        public QueryModel<Procedure> procedureDetails { get; set; } = new QueryModel<Procedure>();
        public List<BPIWebApplication.Shared.MainModel.Stream.FileStream> files { get; set; } = new List<BPIWebApplication.Shared.MainModel.Stream.FileStream>();
    }
}
