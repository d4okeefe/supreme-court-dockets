namespace SupremeCourtDocketApp.Models
{
    public class ProceedingLink
    {
        public int ID { get; set; }
        public int DocketProceedingsID { get; set; }
        public string Link { get; set; }
        public string LinkDescription { get; set; }
    }
}