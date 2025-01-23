using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Html_Serializer
{
    internal class HtmlElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Dictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();
        public List<string> Classes { get; set; }=new List<string>();
        public string InnerHtml { get; set; }
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; }=new List<HtmlElement>();

        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> q = new Queue<HtmlElement>();
            q.Enqueue(this);
            while (q.Count > 0)
            {
                HtmlElement el = q.Dequeue();
                yield return el;
                foreach (HtmlElement el2 in el.Children)
                { q.Enqueue(el2); }
            }
        }


        public IEnumerable<HtmlElement> Ancestors()
        {
            HtmlElement h = this;
            while (h != null)
            {
                yield return h;
                h = h.Parent;
            }
        }

        public IEnumerable<HtmlElement> Query(Selector query)
        {
            HashSet<HtmlElement> res = new HashSet<HtmlElement>();
            findSelectore(res, query, this);
            return res;
        }
        public void findSelectore(HashSet<HtmlElement> hash, Selector s, HtmlElement element)
        {
            if (s == null || element == null) return;

            var dece = element.Descendants();
            foreach (HtmlElement child in dece)
            {
                if(child.CheckS(s))
                {
                    if(s.Child==null)
                        hash.Add(child);
                    else
                        findSelectore(hash,s.Child,child);
                }
            }
        }

        private bool CheckS(Selector selector)
        {
            if (selector == null) return false;
            if (!string.IsNullOrEmpty(selector.Id) && selector.Id != this.Id) return false;
            if (!string.IsNullOrEmpty(selector.TagName) && selector.TagName != this.Name) return false;
            if (selector.Classes != null && !selector.Classes.All(c => this.Classes.Contains(c))) return false;
            return true;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"<{Name} > id={Id} classes: ");
            if (Classes == null) sb.Append("null");
            else
                foreach (var item in Classes)
            {
                sb.Append(item.ToString()+" ");
            }
            sb.Append("Attributes:");
            if(Attributes==null) sb.Append("null");
            else foreach (var item in Attributes)
                {
                    sb.Append(item.ToString() + " ");
                }
            sb.AppendLine(".");
            sb.AppendLine($"InnerHtml: {InnerHtml}");
            sb.AppendLine($"Parent: {Parent?.Id ?? Parent?.Name ?? "null"}"); // Parent might be null
            sb.AppendLine("Children:");
            foreach (var child in Children)
            {
                sb.AppendLine($"   {child.Name ?? child.Id}");
            }


            return sb.ToString();
        }
    }
}
