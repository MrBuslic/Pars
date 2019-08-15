using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Xml;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Reflection;
using System.Text.RegularExpressions;


namespace Pars
{
    class Program
    {
        static void Main(string[] args)
        {
            string main_url = "http://demo.yambr.ru/API/Help/Types";
            //List<string> prop = new List<string>();
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(main_url);
            XmlDocument docXml = new XmlDocument();
            List<string> hrefTags = new List<string>();
            HtmlNodeCollection c = doc.DocumentNode.SelectNodes("//a[@href]");
            StreamWriter f = new StreamWriter("test.txt");
            StreamWriter fprop = new StreamWriter("fprop.txt");
            StreamWriter graf = new StreamWriter("graf.mup");
            List<string> Labels = new List<string>();
            int iter = 0;
            int k = 0;
            string text;
            //List<My> json_struct = new List<My>();
            dynamic json_struct;
            List<List<string>> prop_line = new List<List<string>>();
            List<string> FromTo = new List<string>();
            List<string> LabelArrow = new List<string>();
            var temp = new StringBuilder();
            int num_from = 1;
            int num_to = 1;
            bool trig = false;
            int times = 0;
            int col = 50;

            graf.WriteLine("{\"formatVersion\": 3, \"id\": \"root\", \"ideas\": {");

            if (c != null)
            {
                foreach(HtmlNode n in c)
                {
                    if(n.Attributes["href"] != null)
                    {
                        iter++;
                        string u = "http://demo.yambr.ru" + n.Attributes["href"].Value;
                        HtmlWeb web_url = new HtmlWeb();
                        HtmlDocument doc_url = web_url.Load(u);
                        HtmlNodeCollection json_code = doc_url.DocumentNode.SelectNodes("/ html / body / div[2] / text()");
                        HtmlNodeCollection Name_nodes = doc_url.DocumentNode.SelectNodes("/ html / body / div[1] / text()");
                        StringBuilder sb = new StringBuilder();                        
                        foreach (HtmlAgilityPack.HtmlTextNode node in json_code)
                        {
                            sb.AppendLine(node.Text);
                            //json_struct = JsonConvert.DeserializeObject<dynamic>(WebUtility.HtmlDecode(sb.ToString()).Replace("[{", " ").Replace("}]", " "));
                            //foreach (var i in json_struct)
                            //{
                            //    Console.WriteLine(i);
                            //}
                        }
                        
                        foreach (HtmlAgilityPack.HtmlTextNode node in Name_nodes)
                        {
                            Labels.Add(new Regex("\r\n    (.*)\r\n    ").Match(node.Text).Groups[1].Value);
                            //Labels.Add(node.Text);
                        }

                        text = WebUtility.HtmlDecode(sb.ToString()).Replace("[{", " ").Replace("}]"," ").Replace(" \"Все\"", " \\\"Все\\\"");
                                                
                        if (iter == 32)
                        {
                            text = System.IO.File.ReadAllText(@"error1.txt");
                        }
                        if (iter == 225)
                        {
                            text = System.IO.File.ReadAllText(@"error2.txt");
                        }
                        if (iter == 287)
                        {
                            text = System.IO.File.ReadAllText(@"error3.txt");
                        }
                        if (iter == 337)
                        {
                            text = System.IO.File.ReadAllText(@"error4.txt");
                        }
                        if (iter == 381)
                        {
                            text = System.IO.File.ReadAllText(@"error5.txt");
                        }
                        if (iter == 406)
                        {
                            text = System.IO.File.ReadAllText(@"error6.txt");
                        }
                        if (iter == 419)
                        {
                            text = System.IO.File.ReadAllText(@"error7.txt");
                        }
                        if (iter == 429)
                        {
                            text = System.IO.File.ReadAllText(@"error8.txt");
                        }

                        json_struct = (JsonConvert.DeserializeObject<dynamic>(text));


                        //foreach (var i in json_struct)
                        //{
                        string a;
                        times++;
                        
                        graf.WriteLine("\""+ (k+1) + "\": {\"title\": \"" + Labels[k] + "\", \"id\": " + (k + 1) + ", \"attr\": { \"style\": {\"width\": 88}}}");                    

                        if (times > col)
                        {
                            break;
                            break;
                        }
                        graf.WriteLine(",");
                        k++;
                        //}
                        //json_struct.Add(JsonConvert.DeserializeObject<My>(text));
                        //json_struct = new My(text);
                        List<string> prop = new List<string>();
                        foreach (var i in json_struct)
                        {
                            prop.Add(i.Name);
                        }
                        prop_line.Add(prop);
                        
                        f.WriteLine(text);                        
                    }
                }
            }
            int skip = 0;
            times = 0;
            
            foreach (var i in prop_line)
            {
                times++;
                if (times > col)
                {
                    break;
                }
                //num_from = 0;
                skip++;
                num_to = skip + 1;
                int times1 = 0;
                foreach (var x in prop_line.Skip(skip)) 
                {
                    times1++;
                    if (times1 > col)
                    {
                        break;
                    }
                    trig = false;
                    foreach (var j in i)
                    {
                        
                        foreach (var y in x)
                        {
                            if (j == y)
                            {
                                trig = true;                                
                                temp.Append(y + " ");                               
                            }
                        }
                    }
                    if (trig)
                    {
                        FromTo.Add("{\"ideaIdFrom\": " + num_from + " ,\n\"ideaIdTo\": " + num_to + ",\"attr\": {\"style\": {\"arrow\": \"both\", \"label\": \"" + temp + "\"}}}");
                        temp.Clear();
                    }
                    num_to++;
                }
                num_from++;
            }
            f.Close();
            graf.WriteLine("}, \n\"attr\": {\"theme\": \"default\"},\"title\": \"Отсутствие пользователя\",\"links\": [");
            graf.WriteLine(FromTo[0]);
            foreach (var i in FromTo.Skip(1))
            {
                graf.WriteLine(",");
                graf.WriteLine(i);                
            }
            graf.WriteLine("]}");
            graf.Close();
            /*fprop.WriteLine("{");
            foreach (var i in prop)
            {
                fprop.WriteLine(i + ",");
                //fprop.WriteLine(",");
                Console.WriteLine(i);
            }
            fprop.WriteLine("}");
            fprop.Close();*/
            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}
