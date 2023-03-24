using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPIWebApplication.Shared.PagesModel.Dashboard
{
    public class DashboardFilter
    {
        public string locationId { get; set; } = string.Empty;
        public int pageNo { get; set; } = 0;
        public int rowPerPage { get; set; } = 0;
        public string filterNo { get; set; } = string.Empty;
        public string filterName { get; set; } = string.Empty;
        public string filterDept { get; set; } = string.Empty;
        public string filterBU { get; set; } = string.Empty;
    }
}
