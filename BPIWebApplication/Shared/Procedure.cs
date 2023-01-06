using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BPIWebApplication.Shared
{
    public class Procedure
    {
//      [Required(AllowEmptyStrings = false, ErrorMessage = "Required.")]
        public string ProcedureNo { get; set; } = string.Empty;

//      [Required(AllowEmptyStrings = false, ErrorMessage = "Required.")]
        public string ProcedureName { get; set; } = string.Empty;

//      [Required(AllowEmptyStrings = false, ErrorMessage = "Required.")]
        public DateTime ProcedureDate { get; set; } = DateTime.Today;
        public string ProcedureWi { get; set; } = string.Empty;
        public string ProcedureSop { get; set; } = string.Empty;
    }
}
