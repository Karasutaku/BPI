namespace BPIWebApplication.Shared.MainModel.Company
{
    public class Shift
    {
        public int ShiftID { get; set; } = 0;
        public string ShiftDesc { get; set; } = string.Empty;
        public bool isActive { get; set; } = false;
        //public string ModuleName { get; set; } = string.Empty;
    }
}
