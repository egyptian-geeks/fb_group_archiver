using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Facebook;
using System.IO;

namespace GroupArchiver
{
   public class Archiver
    {
       
       public static string ExtractNextPageData(dynamic data)
       {
           string next = data.paging.next;
           var par=next.Split('?')[1].Split('&');
           string pageToken = par.Where(c => c.Contains("paging_token")).First();
           string until = par.Where(c => c.Contains("until")).First();
           return pageToken+"&"+until;
       }
      
       public static void GetAllPosts(string groupId,string path,string since=null)
       {
           List<string> files = new List<string>();
           FacebookClient fb = FB.GetClient();
           
           string sincestr = "";
           if (since != null)
           {
               sincestr = "&since=" + since.ToString();
           }
           dynamic data = fb.Get("/" + groupId + "/feed?limit=1000" + sincestr);
           Data.SaveGroup(groupId, FB.GetServerTime(), path);
           string postPath = path+"\\post";
           Directory.CreateDirectory(postPath);
           var utf8WithoutBom = new System.Text.UTF8Encoding(false);
           while (data.data.Count > 0)
           {
               foreach (var item in data.data)
               {
                   string filePath = postPath + "\\" + item.id + ".json";
                   File.WriteAllText(filePath, item.ToString(),utf8WithoutBom);
                   files.Add(filePath);
               }
               string next = Archiver.ExtractNextPageData(data);
               data = fb.Get("/"+groupId+"/feed?limit=1000&" + next+sincestr);
           }
           if (since==null)
           {
               Summerizer.ResetSummaryCount(path);
           }
           Summerizer.Summerize(files,path);
           
        }
    }
}
