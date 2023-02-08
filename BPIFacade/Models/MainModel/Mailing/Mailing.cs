namespace BPIFacade.Models.MainModel.Mailing
{
    public class Mailing
    {
        public string ModuleName { get; set; } = string.Empty;
        public string ActionName { get; set; } = string.Empty;
        public string Receiver { get; set; } = string.Empty;
        public string LocationID { get; set; } = string.Empty;
        public string MailSubject { get; set; } = string.Empty;
        public string MailBeginningBody { get; set; } = string.Empty;
        public string MailMainBody { get; set; } = string.Empty;
        public string MailFooter { get; set; } = string.Empty;
        public string MailNote { get; set; } = string.Empty;
    }
}
