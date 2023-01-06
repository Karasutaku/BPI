using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPIWebApplication.Shared.FileUploadModel
{
    public class FileReadyUpload
    {
        public string type { get; set; } = string.Empty;
        public string fileName { get; set; } = string.Empty;
        public string fileType { get; set; } = string.Empty;
        public int fileSize { get; set; } = 0;
        public byte[] content { get; set; } = new byte[0];
    }
}
