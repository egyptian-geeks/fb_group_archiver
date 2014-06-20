using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Facebook;

namespace GroupArchiver
{
   public class FB
    {
       public static FacebookClient client = null;
       public static dynamic me;
       public static FacebookClient GetClient()
       {
          
           if (client!=null)
           {
               return client;
           }


           try
           {
               client = new FacebookClient(Data.Get("token"));
              
               me = client.Get("me/");
              
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
       


     
      
    }
}
