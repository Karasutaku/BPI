namespace BPIBR.Models.MainModel.Stream
{
    public class FileStream
    {
        public string type { get; set; } = string.Empty;
        public string fileName { get; set; } = string.Empty;
        public string fileType { get; set; } = string.Empty;
        public int fileSize { get; set; } = 0;
        public byte[] content { get; set; } = new byte[0];
    }
}
