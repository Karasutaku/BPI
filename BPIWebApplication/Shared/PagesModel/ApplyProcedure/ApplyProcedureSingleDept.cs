using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPIWebApplication.Shared.PagesModel.ApplyProcedure
{
    public class ApplyProcedureSingleDept
    {
        public string ProcedureNo { get; set; } = string.Empty;
        public DeptSelected listDepartment { get; set; } = new DeptSelected();
    }
}
