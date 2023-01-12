namespace BPIBR.Models.MainModel.Procedure
{
    public class HistoryAccess
    {
        public string ProcedureNo { get; set; } = string.Empty;
        public string ProcedureName { get; set; } = string.Empty;
        public string UserEmail { get; set;} = string.Empty;
        public DateTime HistoryAccessDate { get; set; } = DateTime.Now;
    }
}
