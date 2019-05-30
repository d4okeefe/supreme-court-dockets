using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace SupremeCourtDocketApp.Models
{
    public class SupremeCourtDocket
    {
        #region EXTERNAL DATA -- Proceedings & Contacts
        public virtual ICollection<DocketProceedings> Proceedings { get; set; }
        public virtual ICollection<DocketContacts> Contacts { get; set; }
        #endregion

        #region BASIC PROPERTIES -- Directly set from method
        public int ID { get; set; }
        [Display(Name = "Docket Number")]
        public string DocketNumber { get; set; }
        public string WebAddress { get; set; }
        public string WebPage { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:M/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date Retrieved")]
        public DateTime DateRetrieved { get; set; }
        #endregion

        #region EXTENDED PROPERTIES
        public int? DocketYear { get; set; }
        public int? DocketNoYear { get; set; }
        [Display(Name = "Case")]
        public string CaseTitle { get; set; }
        [Display(Name = "Lower Court")]
        public string LowerCourt { get; set; }
        public string LowerCourtCaseNumbers { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:M/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date Docketed")]
        public DateTime? DateDocketed { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:M/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date of Decision")]
        public DateTime? DateOfDecision { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:M/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date of Rehearing Denied")]
        public DateTime? DateOfRehearingDenied { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:M/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date of Discretionary Court Decision")]
        public DateTime? DateOfDiscretionaryCourtDecision { get; set; }
        public string Analyst { get; set; }
        public List<DocketInfoLink> DocketInfoLinks { get; set; }
        #endregion

        #region PUBLIC SETTER
        public void SetExtendedProperties()
        {
            if (string.IsNullOrEmpty(WebPage)) return;

            SetDocketYear();
            SetDocketNoYear();
            try
            {
                var html = new HtmlAgilityPack.HtmlDocument();
                html.LoadHtml(WebPage);
                SetCaseTitle(html);
                SetLowerCourt(html);
                SetLowerCourtCaseNumbers(html);
                SetDateDocketed(html);
                SetDateOfDecision(html);
                SetDateOfRehearingDenied(html);
                SetDateOfDiscretionaryCourtDecision(html);
                SetAnalyst(html);
                SetDocketInfoLinks(html);
                SetProceedings(html);
                SetContacts(html);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
        #endregion

        #region PRIVATE SETTERS -- Contacts & Proceedings
        private void SetDocketInfoLinks(HtmlDocument html)
        {
            try
            {
                var outer_node = html.GetElementbyId("docketinfo");
                var a_node = outer_node.Descendants("a");
                var tandl = new List<DocketInfoLink>();
                DocketInfoLink tl = null;
                foreach (var a in a_node)
                {
                    if (!string.IsNullOrEmpty(a.InnerText)
                        && !string.IsNullOrEmpty(a.Attributes["href"].Value))
                    {

                        tl = new DocketInfoLink();
                        tl.Text = a.InnerText;
                        tl.Link = a.Attributes["href"].Value;
                    }
                    if (!string.IsNullOrEmpty(tl.Link))
                    {
                        tandl.Add(tl);
                    }
                    else
                    {
                        continue;
                    }
                }
                DocketInfoLinks = tandl;
            }
            catch
            {
                DocketInfoLinks = null;
            }
        }
        private void SetContacts(HtmlDocument html)
        {
            var outer_node = html.GetElementbyId("Contacts");
            var cells = outer_node.Descendants("td");

            /*
             * classes:
             * ContactSubHeader    : Attorneys for Petitioner
             * ContactData2        : Name & Address
             * ContactData         : Phone number
             * ContactParty spacer : Party name: C. G.
             * */
            var contact_found = false;

            var contacts = new List<DocketContacts>();
            DocketContacts contact = null;

            var ContactSubHeader_nodes = cells.Where(x => x.HasClass("ContactSubHeader"));


            foreach (var c in cells)
            {
                // do the work in between
                var node_class = c.Attributes
                    .Where(x => x.Name == "class")
                    .Select(x => x.Value).FirstOrDefault();

                if (node_class == "ContactSubHeader")
                {
                    contact_found = true;
                    contact = new DocketContacts();
                }
                if (contact_found)
                {
                    if (node_class == "ContactSubHeader")
                    {
                        contact.PartyHeader = c.InnerText;
                    }
                    if (node_class == "ContactData2")
                    {
                        contact.NameBlock = c.InnerText;
                    }
                    if (string.IsNullOrEmpty(node_class)) // this is the address
                    {
                        var sb = new System.Text.StringBuilder();
                        foreach (var x in c.ChildNodes)
                        {
                            if (x.NodeType == HtmlNodeType.Text)
                                sb.Append(x.InnerText);

                            if (x.NodeType == HtmlNodeType.Element && x.Name == "br")
                                sb.AppendLine();
                        }
                        contact.AddressBlock = sb.ToString();
                    }
                    if (node_class == "ContactData")
                    {
                        contact.PhoneNumber = c.InnerText;
                    }
                    if (node_class == "ContactParty spacer")
                    {
                        contact.PartyFooter = c.InnerText;
                    }
                }

                if (node_class == "ContactParty spacer")
                {
                    // signals end of contact
                    contact_found = false;
                    contact.SetExtendedProperties();
                    contacts.Add(contact);
                }
            }
            Contacts = contacts;
        }
        private void SetProceedings(HtmlDocument html)
        {
            var outer_node = html.GetElementbyId("proceedings");
            var rows = outer_node.Descendants("tr");

            var proceedings = new List<DocketProceedings>();
            // start at row 1 (not 0) -- skip the headers
            // count by 2 because proceedings rows come in pairs:
            // Odd numbered row is Date/Proceeding; Even are the links
            for (var i = 1; i < rows.Count(); i += 2)
            {
                var nodes = rows.ElementAt(i).SelectNodes("td");
                var links_node = rows.ElementAt(i + 1).SelectNodes("td");
                var proceedingsRow = new Utilities.ProceedingsRow(nodes, links_node);

                var proceeding = new DocketProceedings()
                {
                    SupremeCourtDocketID = ID,
                    ProceedingDate = proceedingsRow.DateOfProceeding,
                    ProceedingDescription = proceedingsRow.TextOfProceeding
                };

                if (null != proceedingsRow.Links)
                {
                    List<ProceedingLink> proceedingLinks = new List<ProceedingLink>();
                    foreach (var p in proceedingsRow.Links)
                    {
                        var proceedingLink = new ProceedingLink()
                        {
                            Link = p.Link,
                            LinkDescription = p.Text
                        };
                        proceedingLinks.Add(proceedingLink);
                    }
                    proceeding.ProceedingLinks = proceedingLinks;
                }

                proceedings.Add(proceeding);
            }
            Proceedings = proceedings;
        }
        private void SetCaseTitle(HtmlDocument html)
        {
            try
            {
                CaseTitle = html.DocumentNode.SelectNodes(
                    "//span[@class='title']").FirstOrDefault()
                    .InnerText;
            }
            catch
            {
                CaseTitle = string.Empty;
            }
        }
        private void SetDateOfDecision(HtmlDocument html)
        {
            try
            {
                var outer_node = html.GetElementbyId("docketinfo");

                var spans = outer_node.SelectNodes("//span");

                var txt = string.Empty;
                for (int i = 0; i < spans.Count; i++)
                {
                    if (spans[i].InnerText.EndsWith("Decision Date:"))
                    {
                        txt = spans[i + 1].InnerText;
                        break;
                    }
                }

                DateTime out_date;
                if (DateTime.TryParse(txt, out out_date))
                {
                    DateOfDecision = out_date;
                }
                else
                {
                    DateOfDecision = null;
                }
            }
            catch
            {
                DateOfDecision = null;
            }
        }
        private void SetDateDocketed(HtmlDocument html)
        {
            try
            {
                var outer_node = html.GetElementbyId("docketinfo");

                var spans = outer_node.SelectNodes("//span");

                var txt = string.Empty;
                for (int i = 0; i < spans.Count; i++)
                {
                    if (spans[i].InnerText == "Docketed:")
                    {
                        txt = spans[i + 1].InnerText;
                        break;
                    }
                }

                DateTime out_date;
                if (DateTime.TryParse(txt, out out_date))
                {
                    DateDocketed = out_date;
                }
                else
                {
                    DateDocketed = null;
                }
            }
            catch
            {
                DateDocketed = null;
            }
        }
        private void SetLowerCourtCaseNumbers(HtmlDocument html)
        {
            try
            {
                var outer_node = html.GetElementbyId("docketinfo");

                var spans = outer_node.SelectNodes("//span");

                for (int i = 0; i < spans.Count; i++)
                {
                    if (spans[i].InnerText.EndsWith("Case Numbers:"))
                    {
                        LowerCourtCaseNumbers = spans[i + 1].InnerText;
                        return;
                    }
                }
                LowerCourtCaseNumbers = string.Empty;
            }
            catch
            {
                LowerCourtCaseNumbers = string.Empty;
            }
        }
        private void SetLowerCourt(HtmlDocument html)
        {
            try
            {
                var outer_node = html.GetElementbyId("docketinfo");

                var spans = outer_node.SelectNodes("//span");

                for (int i = 0; i < spans.Count; i++)
                {
                    if (spans[i].InnerText == "Lower Ct:")
                    {
                        LowerCourt = spans[i + 1].InnerText;
                        return;
                    }
                }
                LowerCourt = string.Empty;
            }
            catch
            {
                LowerCourt = string.Empty;
            }
        }
        private void SetAnalyst(HtmlDocument html)
        {
            try
            {
                var outer_node = html.GetElementbyId("docketinfo");

                var spans = outer_node.SelectNodes("//span");

                for (int i = 0; i < spans.Count; i++)
                {
                    if (spans[i].InnerText.EndsWith("Analyst:"))
                    {
                        Analyst = spans[i + 1].InnerText;
                        return;
                    }
                }
                Analyst = string.Empty;
            }
            catch
            {
                Analyst = string.Empty;
            }
        }
        private void SetDateOfDiscretionaryCourtDecision(HtmlDocument html)
        {
            try
            {
                var outer_node = html.GetElementbyId("docketinfo");

                var spans = outer_node.SelectNodes("//span");

                var txt = string.Empty;
                for (int i = 0; i < spans.Count; i++)
                {
                    if (spans[i].InnerText.EndsWith("Discretionary Court Decision Date:"))
                    {
                        txt = spans[i + 1].InnerText;
                        break;
                    }
                }

                DateTime out_date;
                if (DateTime.TryParse(txt, out out_date))
                {
                    DateOfDiscretionaryCourtDecision = out_date;
                }
                else
                {
                    DateOfDiscretionaryCourtDecision = null;
                }
            }
            catch
            {
                DateOfDiscretionaryCourtDecision = null;
            }
        }
        private void SetDateOfRehearingDenied(HtmlDocument html)
        {
            try
            {
                var outer_node = html.GetElementbyId("docketinfo");

                var spans = outer_node.SelectNodes("//span");

                var txt = string.Empty;
                for (int i = 0; i < spans.Count; i++)
                {
                    if (spans[i].InnerText.EndsWith("Rehearing Denied:"))
                    {
                        txt = spans[i + 1].InnerText;
                        break;
                    }
                }

                DateTime out_date;
                if (DateTime.TryParse(txt, out out_date))
                {
                    DateOfRehearingDenied = out_date;
                }
                else
                {
                    DateOfRehearingDenied = null;
                }
            }
            catch
            {
                DateOfRehearingDenied = null;
            }
        }
        private void SetDocketYear()
        {
            try
            {
                if (string.IsNullOrEmpty(DocketNumber))
                {
                    DocketYear = null;
                }
                else
                {
                    if (int.TryParse(DocketNumber.Substring(0, 2), out int i))
                    {
                        DocketYear = i;
                    }
                    else
                    {
                        DocketYear = null;
                    }
                }
            }
            catch
            {
                DocketYear = null;
            }
        }
        private void SetDocketNoYear()
        {
            try
            {
                if (string.IsNullOrEmpty(DocketNumber))
                {
                    DocketNoYear = null;
                }
                var tmp_txt = string.Empty;
                var txt_after_hyphen = DocketNumber.Substring(DocketNumber.IndexOf('-') + 1);
                if (int.TryParse(txt_after_hyphen, out int i))
                {
                    DocketNoYear = i;
                }
                else
                {
                    for (i = 0; i < txt_after_hyphen.Length; i++)
                    {
                        if (char.IsNumber(txt_after_hyphen[i]))
                        {
                            tmp_txt += txt_after_hyphen[i];
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (tmp_txt.Length > 0)
                    {
                        DocketNoYear = int.Parse(tmp_txt);
                    }
                    else
                    {
                        DocketNoYear = null;
                    }
                }
            }
            catch
            {
                DocketNoYear = null;
            }
        }
        #endregion
    }
}
