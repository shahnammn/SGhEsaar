using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PersianCalendar pc1 = new PersianCalendar();
            DateTime thisDate1 = DateTime.Now;
            string datetime = pc1.GetYear(thisDate1) + "/" + pc1.GetMonth(thisDate1) + "/" +
                              pc1.GetDayOfMonth(thisDate1); //+ "  " + pc1.GetHour(thisDate1) + ":" + pc1.GetMinute(thisDate1) + ":" + pc1.GetSecond(thisDate1);
            lblDateTime.Text = datetime;

            if (Session["UserName"] != null)
            {
                HeadLoginView.Visible= false;
                LogOut.Visible = true;
                lblwelcome.Text = Session["ClientFirstName"] + @" " + Session["ClientLastName"] + @" به سایت خوش آمدید";
                ProfileImage.ImageUrl = @"~/images/avatar/" + Session["avatarName"];
                ProfileImage.Visible = true;
            }
            else
            {
                NavigationMenu.Items.RemoveAt(0);
            }
     
        }

        protected void logOut_Click(object sender, EventArgs e)
        {
            NavigationMenu.Items.RemoveAt(0);
            Session.RemoveAll();
            Response.Redirect("~/");
        }
    }
}
