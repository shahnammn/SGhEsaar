using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBLayare;

namespace WebApplication1.Account
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //RegisterHyperLink.NavigateUrl = "Register.aspx?ReturnUrl=" + HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
        }

        protected void loginButton_Click(object sender, EventArgs e)
        {
            DBLayare.Dl dataLayerClass = new Dl();
            var dataTable = dataLayerClass.systemLogin(UserName.Text, Password.Text);
            if (dataTable != null &&  dataTable.Rows.Count !=0)
                {
                    Session.Add("UserName", UserName.Text);
                    Session.Add("AccountId", Convert.ToDecimal(dataTable.Rows[0][3].ToString()));
                    Session.Add("ClientFirstName", dataTable.Rows[0][0].ToString());
                    Session.Add("ClientLastName", dataTable.Rows[0][1].ToString());
                    Session.Add("avatarName", dataTable.Rows[0]["avatarName"].ToString());
                    Response.Redirect("~/main.aspx");
                }
                else
                {
                    Session.RemoveAll();
                    FailureText.Text = "شناسه کاربری و یا رمز اشتباه می باشد";
                }
        }
    }
}
