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
      
       public static void GetAllPosts(string groupId,string path,DateTime? since=null)
       {
           FacebookClient fb = FB.GetClient();
           string sincestr = "";
           if (since != null)
           {
               long epoch = since.Value.ToEpoch();
               sincestr = "&since=" + epoch.ToString();
           }
           dynamic data = fb.Get("/" + groupId + "/feed?limit=1000" + sincestr);
           Data.SaveGroup(groupId, DateTime.Now.ToEpoch(), path);
           var utf8WithoutBom = new System.Text.UTF8Encoding(false);
           while (data.data.Count > 0)
           {
               foreach (var item in data.data)
               {
                   File.WriteAllText(path + "\\" + item.id + ".json", item.ToString(), utf8WithoutBom);
                  
               }
               string next = Archiver.ExtractNextPageData(data);
               data = fb.Get("/"+groupId+"/feed?limit=1000&" + next);
           }
           
        }
    }
}
