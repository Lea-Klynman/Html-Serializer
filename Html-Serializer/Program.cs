
using System.Text.RegularExpressions;
using Html_Serializer;

async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}


HtmlElement Serialize(IEnumerable<string> htmllines)
{
    HtmlElement root = new HtmlElement();
    HtmlElement current = root;
    foreach (var line in htmllines)
    {
        string[] words = line.Split(' ');
        if (!words[0].Equals("/html"))
        {
            if (words[0].StartsWith('/'))//תוית סוגרת           
                current = current.Parent;
            else if (HtmlHelper.Instance.HtmlTags.Contains(words[0]))
            {
                HtmlElement ne = new HtmlElement();
                ne.Parent = current;
                ne.Name = words[0];
                current.Children.Add(ne);
                if(line.IndexOf(' ') > 0)
                {
                    var attributes = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(line.Substring(line.IndexOf(' '))).ToList();
                    foreach (var item in attributes)
                    {
                        string[] arr = item.ToString().Split("=");
                        if (arr[0].Contains("id"))
                            ne.Id = words[1];
                       else if (arr[0].Contains("class"))
                        {
                            ne.Classes = arr[1].Split(" ").ToList();
                        }
                        else
                        {
                            ne.Attributes[arr[0]] = arr[1];
                        }
                    }
                }
                
                if (!HtmlHelper.Instance.HtmlVoidTags.Contains(words[0]))
                    current = ne;
            }
            else
            {
                current.InnerHtml = line;
            }
            
        }
       
    }
    root.Parent = null;
    return root;


}

void PrintTree(HtmlElement root)
{
    if (root == null)
        return;
    Console.WriteLine(root.ToString());
    for (int i = 0; i < root.Children.Count; i++) { PrintTree(root.Children[i]); }
}

void Check(string s,HtmlElement dom)
{
    Selector selector = Selector.Convert(s);
    var result = dom.Query(selector);
    result.ToList().ForEach(element => { Console.WriteLine(element); });
}
var html = await Load("https://hebrewbooks.org/");//loading html from website

html = new Regex("[\\r\\n\\t]").Replace(new Regex("\\s{2,}").Replace(html, ""), "");
var htmlLines = new Regex("<(.*?)>").Split(html).Where(s => s.Length > 0);

HtmlElement dom = Serialize(htmlLines);
//Console.WriteLine("print tree-------");
 //PrintTree(dom);
Console.WriteLine("-----------------");
Check("div img",dom);

 
Console.WriteLine();

