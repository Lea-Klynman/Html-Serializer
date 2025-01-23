using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Html_Serializer
{
    internal class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; }

        public Selector Parent { get; set; }
        public Selector Child { get; set; }
        public static Selector Convert(string query)
        {
            Selector root = null;
            Selector current = null;
            string[] levels = query.Split(' ');

            foreach (string level in levels)
            {
                Selector selector = new Selector();
                string remaining = level;

                int tagEndIndex = remaining.IndexOfAny(new[] { '#', '.' });
                if (tagEndIndex == -1)
                {

                    selector.TagName = remaining;
                    remaining = string.Empty;
                }
                else if (tagEndIndex > 0)
                {
                    selector.TagName = remaining.Substring(0, tagEndIndex);
                    remaining = remaining.Substring(tagEndIndex);
                }


                int idIndex = remaining.IndexOf('#');
                if (idIndex != -1)
                {
                    int idEndIndex = remaining.IndexOf('.', idIndex);
                    if (idEndIndex == -1) idEndIndex = remaining.Length;

                    selector.Id = remaining.Substring(idIndex + 1, idEndIndex - idIndex - 1);
                    remaining = remaining.Remove(idIndex, idEndIndex - idIndex);
                }

                if (!string.IsNullOrEmpty(remaining))
                {
                    selector.Classes = remaining.Split('.')
                                                .Where(c => !string.IsNullOrEmpty(c))
                                                .ToList();
                }

                if (root == null)
                {
                    root = selector;
                }
                else
                {
                    current.Child = selector;
                    selector.Parent = current;
                }

                current = selector;
            }

            return root;
        }
    }
}
