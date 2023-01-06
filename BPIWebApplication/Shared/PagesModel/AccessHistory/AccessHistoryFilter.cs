using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPIWebApplication.Shared.PagesModel.AccessHistory
{
    public class AccessHistoryFilter
    {
        public int pageNo { get; set; } = 0;
        public int rowPerPage { get; set; } = 0;
        public string filterType { get; set; } = string.Empty;
        public string filterDetails { get; set; } = string.Empty;
    }
}
