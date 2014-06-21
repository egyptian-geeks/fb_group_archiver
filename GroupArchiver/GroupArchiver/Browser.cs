using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Facebook;
using System.Dynamic;

namespace GroupArchiver
{
    public partial class Browser : Form
    {
        UrlType type;
        Form next;
        public Browser(UrlType type,Form next )
        {
            InitializeComponent();
            this.type = type;
            this.next = next;
        }
        private Uri GenerateLoginUrl()
        {

            // for .net 3.5
            // var parameters = new Dictionary<string,object>
            // parameters["client_id"] = appId;
            dynamic parameters = new ExpandoObject();
            parameters.client_id = Data.Get("id") ;
            parameters.redirect_uri = "https://www.facebook.com/connect/login_success.html";

            // The requested response: an access token (token), an authorization code (code), or both (code token).
            parameters.response_type = "token";

            // list of additional display modes can be found at http://developers.facebook.com/docs/reference/dialogs/#display
            parameters.display = "popup";

            // add the 'scope' parameter only if we have extendedPermissions.

            parameters.scope = "public_profile, basic_info, publish_checkins, status_update, photo_upload, video_upload, create_note, share_item, publish_stream, manage_notifications, publish_actions, user_friends,user_groups";

            // generate the login url

            var fb = new FacebookClient();
            return fb.GetLoginUrl(parameters);
        }

        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            var fb = new FacebookClient();
            FacebookOAuthResult oauthResult;
            if (fb.TryParseOAuthCallbackUrl(e.Url, out oauthResult))
            {
                // The url is the result of OAuth 2.0 authentication
                if (oauthResult.IsSuccess)
                {
                    var accesstoken = oauthResult.AccessToken;
                    Data.Set("token", FB.ExtendToken(accesstoken));
                    next.Show();
                    this.Close();
                }
                else
                {
                    var errorDescription = oauthResult.ErrorDescription;
                    var errorReason = oauthResult.ErrorReason;
                }
            }
           
        }

        private void Browser_Leave(object sender, EventArgs e)
        {
            next.Show();
        }

        private void Browser_Load(object sender, EventArgs e)
        {
            switch (type)
            {
                case UrlType.Login:

                    webBrowser1.Navigate(GenerateLoginUrl());

                    break;
                case UrlType.Logout:
                    break;
                default:
                    break;
            }   
        }

      
    }
}
