namespace BPIDA.Models.MainModel.Company
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
