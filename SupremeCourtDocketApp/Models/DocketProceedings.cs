using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SupremeCourtDocketApp.Models
{
    public enum SubTypeOfProceeding
    {
        PetitionForProhibitionWithMotion,
        PetitionForCert,
        PetitionForCertWithMotion,
        PetitionForCertBeforeJudgment,
        PetitionForCertBeforeJudgmentWithMotion,
        PetitionForMandamus,
        PetitionForMandamusWithMotion,
        PetitionForMandamusOrProhibition,
        PetitionForMandamusOrProhibitionWithMotion,
        PetitionForHabeasCorpusWithMotion,
        PetitionForHabeasCorpus,
        StatementOfJurisdiction
    }
    public enum TypeOfProceeding
    {
        Petition,

        PetitionForProhibitionWithMotion,
        PetitionForCert,
        PetitionForCertWithMotion,
        PetitionForCertBeforeJudgment,
        PetitionForCertBeforeJudgmentWithMotion,
        PetitionForMandamus,
        PetitionForMandamusWithMotion,
        PetitionForMandamusOrProhibition,
        PetitionForMandamusOrProhibitionWithMotion,
        PetitionForHabeasCorpusWithMotion,
        PetitionForHabeasCorpus,

        StatementOfJurisdiction,


        PetitionLevelPetition,
        PetitionLevelResponse,
        PetitionLevelReply,

        BlankConsent,
        AmicusBrief,



        DistributedForConference,
        PetitionDenied,


        Unidentified
    }

    public class DocketProceedings
    {
        private List<string> Patterns
        {
            get
            {
                var patterns = new List<string>();
                patterns.AddRange(new List<string>
                {
                    // --to extend the time
                    // --granted
                    // Application no.
                    "Application",
                    //
                    "Corrected petition for a writ of certiorari filed",
                    // Response due date
                    "Petition for a writ of certiorari filed",
                    // by which party
                    "Blanket Consent",
                    // name of amici
                    "Brief amici curiae",
                    "Brief amicus curiae",
                    // name of respondent
                    "Waiver of right of respondent",
                    // multiple times?
                    "DISTRIBUTED for Conference",
                    "Rescheduled",
                    "Response Requested",
                    "Motion to extend the time to file",
                    "Brief of respondent",
                    "Reply of petitioner",
                    "Petition DENIED",
                    "Petition GRANTED",
                    "Petition for Rehearing filed",
                    "Rehearing DENIED",

                    "Joint appendix filed",
                    // party
                    "Brief of petitioner",
                    // date
                    "SET FOR ARGUMENT",
                    "CIRCULATED",
                    "Record requested",
                    "The record received",
                    //problem: "Brief of respondent" == petition stage
                    //problem: "Reply of petitioner" == petition stage
                    "Argued"
                });
                return patterns;
            }
        }

        public void SetTypeOfProceeding()
        {
            if (string.IsNullOrEmpty(ProceedingDescription))
            {
                TypeOfProceeding = TypeOfProceeding.Unidentified;
            }
            foreach (var p in Patterns)
            {
                var pattern = new Regex(p);
            }
        }

        public int ID { get; set; }
        public int SupremeCourtDocketID { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:M/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ProceedingDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:M/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime SecondaryDate { get; set; }
        //public DateTime DateOfResponseToPetitionDue { get; set; }
        public string ProceedingDescription { get; set; }
        public TypeOfProceeding TypeOfProceeding { get; set; }
        public virtual ICollection<ProceedingLink> ProceedingLinks { get; set; }

    }
}