using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SupremeCourtDocketApp.Models
{
    public class SupremeCourtDocketSimple
    {
        public int ID { get; set; }
        public string DocketNumber { get; set; }
        public string WebAddress { get; set; }
        public string WebPage { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateRetrieved { get; set; }
        public string CaseTitle { get; set; }
    }
}