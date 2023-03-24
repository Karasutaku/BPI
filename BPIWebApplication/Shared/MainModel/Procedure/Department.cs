using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BPIWebApplication.Shared.MainModel.Procedure
{
    public class Department
    {
        public string DepartmentID { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
        public string DepartmentLabel { get; set; } = string.Empty;
        public BisnisUnit BisnisUnit { get; set; } = new BisnisUnit();
        public string BisnisUnitID { get; set; } = string.Empty;

    }
}
