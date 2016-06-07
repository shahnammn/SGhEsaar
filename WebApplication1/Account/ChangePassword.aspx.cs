using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBLayare;

namespace WebApplication1.Account
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void changePasswordPushButton_Click(object sender, EventArgs e)
        {
            DBLayare.Dl dataLayer = new Dl();
            bool result= dataLayer.changePass((string)Session["UserName"], CurrentPassword.Text, NewPassword.Text);
            if (!result)
            {
                FailureText.Text = "رمز عبور تغییر نکرد";
            }
            else
            {
                FailureText.Text = "عملیات با موفقیت انجام شد.";
            }

        }
    }
}
