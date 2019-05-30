using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupremeCourtDocketApp.Models.Utilities
{
    public class ProceedingsRow
    {
        private readonly HtmlAgilityPack.HtmlNodeCollection _nodes;
        private readonly HtmlNodeCollection _links_nodes;
        private readonly int _columnCount;
        public DateTime DateOfProceeding { get; set; }
        public string TextOfProceeding { get; set; }
        public List<TextAndLink> Links { get; set; }
        public ProceedingsRow(HtmlAgilityPack.HtmlNodeCollection nodes, HtmlAgilityPack.HtmlNodeCollection links_nodes)
        {
            _nodes = nodes;
            _links_nodes = links_nodes;
            _columnCount = _nodes.Count;
            // all ColumnCount should be 2
            if (_columnCount != 2) { throw new Exception("ProceedingsRow -- ColumnCount != 2"); }
            // else
            if (DateTime.TryParse(_nodes.ElementAt(0).InnerText, out DateTime d))
            {
                DateOfProceeding = d;
            }
            TextOfProceeding = _nodes.ElementAt(1).InnerText;

            if (null != links_nodes)
            {
                if (!links_nodes.ElementAt(0).HasClass("borderbttm"))
                {
                    throw new Exception("borderbttm not found in ProceedingsRow");
                }

                // else
                var lnks = links_nodes.ElementAt(1).Descendants("a");

                if (lnks.Count() > 0)
                {
                    Links = new List<TextAndLink>();
                    foreach (var lnk in lnks)
                    {
                        var tandl = new TextAndLink();
                        tandl.Text = lnk.InnerText;
                        tandl.Link = lnk.Attributes["href"].Value;
                        Links.Add(tandl);
                    }
                }

            }
        }
    }
}
