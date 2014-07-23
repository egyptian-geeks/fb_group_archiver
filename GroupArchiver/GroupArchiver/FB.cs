using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Facebook;
using System.Net;
using System.IO;
using System.Xml.Linq;

namespace GroupArchiver
{
   public class FB
    {
       public static FacebookClient client = null;
       public static dynamic me;
       public static FacebookClient GetClient(bool reload=false)
       {
           if (reload)
               client = null;
           if (client!=null)
           {
               return client;
           }


           try
           {
               client = new FacebookClient(Data.Get("token"));

               me = client.Get("me/");

           }
           catch (WebExceptionWrapper)
           {
               throw new NoConnection();
           }
           catch (Exception)
           {

               return null;
           }
              
           return client;
       }
       public static void GenerateToken()
       {
           //client = null;
           //Program.ongoingAuth = true;
           //Browser b = new Browser(UrlType.Login);
           //b.Show();
           
       }
       public static string ExtendToken(string oldToken)
       {
           var fb = new FacebookClient();
           dynamic result = fb.Get("oauth/access_token", new
           {
               client_id = Data.Get("id"),
               client_secret = Data.Get("secret"),
               grant_type = "fb_exchange_token",
               fb_exchange_token = oldToken
           });
           return result.access_token;
       }

       public static string GetServerTime()
       {
           WebRequest req = HttpWebRequest.Create("https://api.facebook.com/method/fql.query?query=SELECT+now%28%29+FROM+link_stat+WHERE+url+%3D+%271.2%27&format=xml");
           WebResponse res = req.GetResponse();
           var stream = res.GetResponseStream();
           var streamreader = new StreamReader(stream);
           XDocument doc = XDocument.Parse(streamreader.ReadToEnd());
           return doc.Root.Value;
       }

     
      
    }
}
