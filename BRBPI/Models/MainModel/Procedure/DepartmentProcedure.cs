using BPIBR.Models.MainModel.Company;

namespace BPIBR.Models.MainModel.Procedure
{
    public class DepartmentProcedure
    {
        public Procedure Procedure { get; set; } = new Procedure();

//      [Required(AllowEmptyStrings = false, ErrorMessage = "Required.")]
        public string ProcedureNo { get; set; } = string.Empty;
        public Department Department { get; set; } = new Department();

 //     [Required(AllowEmptyStrings = false, ErrorMessage = "Required.")]
        public string DepartmentID { get; set; } = string.Empty;
    }
}
