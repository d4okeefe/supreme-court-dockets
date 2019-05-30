using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SupremeCourtDocketApp.Models
{
    public class DocketContacts
    {
        public int ID { get; set; }
        public int SupremeCourtDocketID { get; set; }
        public string PartyHeader { get; set; }
        public string PhoneNumber { get; set; }
        public string PartyFooter { get; set; }

        public string PartyDescription { get; set; }//Petitioner, Respondent, Other
        public string PartyName { get; set; }
        public string NameBlock { get; internal set; }
        public string AddressBlock { get; set; }
        public bool IsCounselOfRecord { get; set; }
        public string AttorneyFullName { get; set; }
        public string AttorneySurname { get; set; }
        public string AttorneyEmail { get; set; }
        public string AttorneyCityStateZip { get; set; }
        public bool IsCityStateZipValid { get; set; }
        public string AttorneyCity { get; set; }
        public string AttorneyState { get; set; }
        public string AttorneyZip { get; set; }
        public string AttorneyOffice { get; set; }
        public string AttorneyStreetAddress { get; set; }
        public string PhoneNumberTenDigit { get; private set; }

        public void SetExtendedProperties()
        {
            if (!string.IsNullOrEmpty(NameBlock))
            {
                ParseNameBlock();
            }

            if (!string.IsNullOrEmpty(AddressBlock))
            {
                ParseAddressBlock();
            }
            if (!string.IsNullOrEmpty(PartyHeader))
            {
                if (PartyHeader.StartsWith("Attorneys for"))
                {
                    PartyDescription = PartyHeader.Substring("Attorneys for".Length).Trim();
                }
                else if (PartyHeader.StartsWith("Other"))
                {
                    PartyDescription = "Other";
                }
                else if (string.IsNullOrEmpty(PartyHeader))
                {
                    Debug.WriteLine("Problem extracting PartyDescription");
                }
                else
                {
                    Debug.WriteLine("Problem extracting PartyDescription");
                }
            }
            if (!string.IsNullOrEmpty(PartyFooter))
            {
                if (PartyFooter.StartsWith("Party name:"))
                {
                    PartyName = PartyFooter.Substring("Party name:".Length).Trim();
                }
                else
                {
                    Debug.WriteLine("Problem extracting PartyName");
                }
            }
            if (!string.IsNullOrEmpty(PhoneNumber))
            {
                PhoneNumberTenDigit = string.Empty;
                foreach (var c in PhoneNumber.ToCharArray())
                {
                    if (char.IsDigit(c))
                    {
                        PhoneNumberTenDigit += c;
                    }
                }
                if (PhoneNumberTenDigit.Length != 10)
                {
                    PhoneNumberTenDigit = string.Empty;
                }
            }
        }

        private void ParseAddressBlock()
        {
            try
            {
                var OFFICE = string.Empty;
                var ADDRESS = string.Empty;
                var CITY = string.Empty;
                //var EMAIL = string.Empty;

                var split_by_break = AddressBlock.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                var email_slot = -1;
                var city_slot = -1;
                var office_slot_last = -1;


                var ptrn_zip = @"\b\d{5}(?:-\d{4})?\b";
                var ptrn_cty = @"(?:[A-Z][a-z.-]+[ ]?)+";
                var ptrn_ste = @"Alabama|Alaska|Arizona|Arkansas|California|Colorado|Connecticut|Delaware|Florida|Georgia|Hawaii|Idaho|Illinois|Indiana|Iowa|Kansas|Kentucky|Louisiana|Maine|Maryland|Massachusetts|Michigan|Minnesota|Mississippi|Missouri|Montana|Nebraska|Nevada|New[ ]Hampshire|New[ ]Jersey|New[ ]Mexico|New[ ]York|North[ ]Carolina|North[ ]Dakota|Ohio|Oklahoma|Oregon|Pennsylvania|Rhode[ ]Island|South[ ]Carolina|South[ ]Dakota|Tennessee|Texas|Utah|Vermont|Virginia|Washington|West[ ]Virginia|Wisconsin|Wyoming";
                var ptrn_abv = @"AL|AK|AS|AZ|AR|CA|CO|CT|DE|DC|FM|FL|GA|GU|HI|ID|IL|IN|IA|KS|KY|LA|ME|MH|MD|MA|MI|MN|MS|MO|MT|NE|NV|NH|NJ|NM|NY|NC|ND|MP|OH|OK|OR|PW|PA|PR|RI|SC|SD|TN|TX|UT|VT|VI|VA|WA|WV|WI|WY";
                var ptrn_city_state_zip = $"({ptrn_cty}),[ ](?:({ptrn_ste}|{ptrn_abv}))[ ]({ptrn_zip})";
                var re_city_state_zip = new System.Text.RegularExpressions.Regex(ptrn_city_state_zip);

                // get email
                if (split_by_break.Last().Contains('@'))
                {
                    AttorneyEmail = split_by_break.Last();
                    email_slot = split_by_break.Length - 1;
                }

                // get city, state zip
                if (string.IsNullOrEmpty(AttorneyEmail))
                {
                    AttorneyCityStateZip = split_by_break[split_by_break.Length - 1];
                    city_slot = split_by_break.Length - 1;
                }
                else
                {
                    AttorneyCityStateZip = split_by_break[split_by_break.Length - 2];
                    city_slot = split_by_break.Length - 2;
                }
                //VALIDATE city, state zip
                var m_city_state_zip = re_city_state_zip.Match(AttorneyCityStateZip);
                if (m_city_state_zip.Success)
                {
                    IsCityStateZipValid = true;
                    AttorneyCity = m_city_state_zip.Groups[1].Value;
                    AttorneyState = m_city_state_zip.Groups[2].Value;
                    AttorneyZip = m_city_state_zip.Groups[3].Value;
                }


                for (var i = 0; i < split_by_break.Length - 1; i++)
                {
                    if (split_by_break[i].ToCharArray().Any(x => char.IsDigit(x)))
                    {
                        office_slot_last = i - 1;
                        if (office_slot_last > -1)
                        {
                            Console.WriteLine(office_slot_last + ": " + split_by_break[office_slot_last]);
                        }
                        break;
                    }
                }
                if (office_slot_last == -1)
                {
                    for (var i = 0; i <= office_slot_last; i++)
                    {
                        AttorneyOffice += i == office_slot_last ? split_by_break[i] : split_by_break[i] + '\n';
                    }
                }
                if (office_slot_last == -1)
                {
                    for (var i = office_slot_last + 1; i < city_slot; i++)
                    {
                        AttorneyStreetAddress += i == city_slot - 1 ? split_by_break[i] : split_by_break[i] + '\n';
                    }
                }
                else
                {
                    for (var i = 0; i < city_slot; i++)
                    {
                        AttorneyStreetAddress += i == city_slot - 1 ? split_by_break[i] : split_by_break[i] + '\n';
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in ParseAddressBlock()");
                Console.WriteLine(ex.Message);
            }
        }

        private void ParseNameBlock()
        {
            try
            {
                IsCounselOfRecord = NameBlock.ToLower().Contains("Counsel of Record".ToLower());
                if (IsCounselOfRecord)
                {
                    var index_of_counselofrecord = NameBlock.IndexOf("Counsel of Record");
                    AttorneyFullName = NameBlock.Remove(index_of_counselofrecord);
                }
                else
                {
                    AttorneyFullName = NameBlock;
                }
                while (AttorneyFullName.EndsWith("&nbsp;"))
                {
                    AttorneyFullName = AttorneyFullName.Substring(0, AttorneyFullName.Length - "&nbsp;".Length);
                }

                // get last name
                var ptrn = @"^(.+?) ([^\s,]+)(,? (?:[JS]r\.?|II?|III?|IV))?$";
                var re = new System.Text.RegularExpressions.Regex(ptrn);
                var m = re.Match(AttorneyFullName);
                if (m.Success)
                {
                    System.Text.RegularExpressions.Group lastName = m.Groups[2];
                    AttorneySurname = lastName.Value;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in ParseNameBlock()");
                Console.WriteLine(ex.Message);
            }

        }
    }
}
