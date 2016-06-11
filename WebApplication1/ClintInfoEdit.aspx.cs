using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBLayare;

namespace WebApplication1
{
    public partial class ClintInfoEdit : System.Web.UI.Page
    {
        private DBLayare.Dl dataLayer = new Dl();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            DataTable dataTable = dataLayer.fillClientByClientNumber(Session["UserName"].ToString());
            lblFName.Text = dataTable.Rows[0]["FirstName"].ToString();
            lblLName.Text = dataTable.Rows[0]["LastName"].ToString();
            lblNum.Text = dataTable.Rows[0]["FamilyCount"].ToString();
            txtTel.Text = dataTable.Rows[0]["Tel"].ToString();
            txtMobile.Text = dataTable.Rows[0]["Mobile"].ToString();
            txtAddress.Text = dataTable.Rows[0]["Address"].ToString();
            profileImage.ImageUrl = @"~/images/avatar/" + Session["avatarName"];
            }
            

        }

        protected void btnok_Click(object sender, EventArgs e)
        {
          
                //Some code to insert values in DataBase

                if (dataLayer.clientInfoUpdate(Session["UserName"].ToString(), txtTel.Text, txtMobile.Text,
                                               txtAddress.Text, Session["avatarName"].ToString()))
                {
                    lblnote.Text = @"عملیات با موفقیت انجام شد";
                    lblnote.ForeColor = System.Drawing.Color.DarkCyan;
                }
                else
                {
                    lblnote.Text = @"عملیات با موفقیت انجام نشد";
                    lblnote.ForeColor = System.Drawing.Color.Red;

            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Main.aspx");
        }



        protected void SaveDetails(object sender, EventArgs e)
        {
            if (this.fuDemo.HasFile)
            {
                
                fuDemo.SaveAs(Server.MapPath("~/images/avatar/" + this.fuDemo.FileName));
                string fileName = Path.GetFileName(this.fuDemo.PostedFile.FileName);
                FileInfo f1 = new FileInfo(Server.MapPath("~/images/avatar/") + fileName);
                string srtFileName = Server.MapPath("~/images/avatar/") + Session["UserName"] + f1.Extension;

                FileInfo f2 = new FileInfo(srtFileName);
                if (f2.Exists)
                {
                    f2.Delete();
                }
                f1.CopyTo(srtFileName);
                f1.Delete();
                profileImage.ImageUrl = "~/images/avatar/" + Session["UserName"] + f1.Extension;
                Session["avatarName"] =  Session["UserName"] + f1.Extension;


            }
        }
    }
}