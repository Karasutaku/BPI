using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BPIWebApplication.Shared.MainModel.Procedure
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
