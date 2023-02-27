using BPIBR.Models.MainModel.Company;

namespace BPIBR.Models.MainModel.PettyCash
{
    public class Advance
    {
        public string AdvanceID { get; set; } = string.Empty;
        public DateTime AdvanceDate { get; set; } = DateTime.Now;
        public string AdvanceStatus { get; set; } = string.Empty;
        public string AdvanceNIK { get; set; } = string.Empty;
        public string AdvanceNote { get; set; } = string.Empty;
        public string AdvanceType { get; set; } = string.Empty;
        public string TypeAccount { get; set; } = string.Empty;

        public string DepartmentID { get; set; } = string.Empty;
        public Department Department { get; set; } = new();

        public string LocationID { get; set; } = string.Empty;
        public string Approver { get; set; } = string.Empty;
        public string Applicant { get; set; } = string.Empty; // from audit

        public List<AdvanceLine> lines { get; set; } = new();
        public AdvanceDocumentStatus statusDetails { get; set; } = new();
    }
}
