using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPIWebApplication.Shared.DbModel
{
    public class QueryModel<T>
    {
        public T? Data { get; set; }
        public string userEmail { get; set; } = string.Empty;
        public string userAction { get; set; } = string.Empty;
        public DateTime userActionDate { get; set; } = DateTime.Now;
    }
}
