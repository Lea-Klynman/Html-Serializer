
using System.Text.RegularExpressions;

async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}


var html = await Load("https://hebrewbooks.org/");//loading html from website

var cleanhtml = new Regex("\\s").Replace(html,"");
var htmlLines = new Regex("<(.*?)>").Split(cleanhtml).Where(s=>s.Length>0);
Console.WriteLine();

