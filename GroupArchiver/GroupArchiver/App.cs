using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GroupArchiver
{
    class App:ApplicationContext
    {
        public App()
        {
            if (FB.GetClient()==null)
            {
                Browser b = new Browser(UrlType.Login,new Groups());
                b.Show();
            }
            else
            {
                Groups g = new Groups();
                g.Show();
            }
        }
    }
}
