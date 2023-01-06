using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPIWebApplication.Shared.PagesModel.AddEditUser
{
    public class UserAdmin
    {
        public string UserID { get; set; } = string.Empty; // user (first) actual name
        public string UserEmail { get; set; } = string.Empty;
        public string UserRole { get; set; } = string.Empty;
    }
}
