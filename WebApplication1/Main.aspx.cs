using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBLayare;

namespace WebApplication1
{
    public partial class Main : System.Web.UI.Page
    {
        private DBLayare.Dl _dataLayerClass = new Dl();
        protected void Page_Load(object sender, EventArgs e)
        {
            
            lblName.Text = " صفحه شخصی " + Session["ClientFirstName"] + " " + Session["ClientLastName"];
            this.Page.Title = lblName.Text;
            profileImage.ImageUrl = @"~/images/avatar/" + Session["avatarName"];
            PnlAdmin.Visible = (string)Session["UserName"] == "000";
            AccessDataSource1.SelectCommand =
                "SELECT [Amount], [date], [Description] FROM [payment] where Account_ID=" + Session["AccountId"] +
                " ORDER BY [date] DESC";
            GridView1.Caption = "ریز پرداختهای مشتری";
            GridView1.CaptionAlign = TableCaptionAlign.Left;
            int count = GridView1.Rows.Count;
            for (int i = 0; i < count; i++)
            {
                try
                {
                    string a = GridView1.Rows[i].Cells[0].Text;
                    PersianCalendar pc1 = new PersianCalendar();
                    string datetime = pc1.GetYear(Convert.ToDateTime(a)) + "/" + pc1.GetMonth(Convert.ToDateTime(a)) + "/" +
                                      pc1.GetDayOfMonth(Convert.ToDateTime(a));
                    GridView1.Rows[i].Cells[0].Text = datetime;
                    decimal balanceString = Convert.ToDecimal(GridView1.Rows[i].Cells[1].Text);
                    GridView1.Rows[i].Cells[1].Text = balanceString.ToString("0,0") + " ريال ";
                }
                catch (Exception err)
                {
                    lblNoteLoan.Text = "مشکل لود ریز پرداختها" + err.Message;
                }
            }
            var dataTable = _dataLayerClass.fillClientByClientNumber(Session["UserName"].ToString());
            var balanceAmount = (decimal)Convert.ToDouble(dataTable.Rows[0][2].ToString());
            string balanceString1 = balanceAmount.ToString("0,0");
            lblBalance.Text = " " + lblBalance.Text + balanceString1 + " ریال ";
            var param = _dataLayerClass.getParameterById(1);
            decimal aa = 1;
            aa = Convert.ToDecimal(dataTable.Rows[0]["FamilyCount"].ToString());
            decimal signAmount = (aa * (Convert.ToDecimal((param.value.ToString()))));
            string txtsignAmount = " مبلغ عضویت: " + signAmount.ToString("0,0");
            lblSignAmount.Text = lblSignAmount.Text + txtsignAmount;

            
            DBLayare.Loan loanInfo = new Loan();
            Loan.LoanStruct? loanInfoByClientNumber = loanInfo.getLoanInfoByClientNumber((string) Session["UserName"]);
            if (loanInfoByClientNumber != null)
            {
                lblNoteLoan.Visible = false;
                DBLayare.Loan.LoanStruct loanInfoResult = (Loan.LoanStruct) loanInfoByClientNumber;
                lblLoanNumber.Text ="شماره تسهیلات: " + loanInfoResult.loanNumber.ToString();
                lblLoanAmount.Text = "مبلغ تسهیلات: " +loanInfoResult.amount.ToString("0,0");

                PersianCalendar pc1 = new PersianCalendar();
                string datetime = pc1.GetYear(Convert.ToDateTime(loanInfoResult.grantDate)) + "/" + pc1.GetMonth(Convert.ToDateTime(loanInfoResult.grantDate)) + "/" +
                                  pc1.GetDayOfMonth(Convert.ToDateTime(loanInfoResult.grantDate));
                lblGrantDate.Text = "تاریخ اعطای تسهیلات: " + datetime.ToString();
                loanDS.SelectCommand = "SELECT InstallmentTable.Installment_Number, InstallmentTable.DUE_DATE, InstallmentTable.Installment_Amount, InstallmentTable.Installment_Status" +
                                       " FROM InstallmentTable WHERE (((InstallmentTable.Loan_ID)=" + loanInfoResult.loanNumber + "))";

            }
            else
            {
                lblNoteLoan.Font.Size = 12;
                lblNoteLoan.Text = "در حال حاضر تسهیلات اعطا شده ای وجود ندارد";
                lblNoteLoan.Visible = true;
            }

            for (int i = 0; i < GridView2.Rows.Count; i++)
            {
                try
                {
                    string a = GridView2.Rows[i].Cells[1].Text;
                    PersianCalendar pc1 = new PersianCalendar();
                    string datetime = pc1.GetYear(Convert.ToDateTime(a)) + "/" + pc1.GetMonth(Convert.ToDateTime(a)) + "/" +
                                      pc1.GetDayOfMonth(Convert.ToDateTime(a));
                    GridView2.Rows[i].Cells[1].Text = datetime;
                    decimal balanceString = Convert.ToDecimal(GridView2.Rows[i].Cells[2].Text);
                    GridView2.Rows[i].Cells[2].Text = balanceString.ToString("0,0") + " ريال ";
                    switch (GridView2.Rows[i].Cells[3].Text)
                    {
                        case "0":
                            {
                                GridView2.Rows[i].Cells[3].Text = "پرداخت نشده";
                                break;
                            }
                        case "2":
                            {
                                GridView2.Rows[i].Cells[3].Text = "پرداخت شده";
                                GridView2.Rows[i].BackColor = System.Drawing.Color.LightCyan;
                                break;
                            }
                        case "1":
                            {
                                GridView2.Rows[i].Cells[3].Text = "سررسید گذشته";
                                GridView2.Rows[i].BackColor = System.Drawing.Color.LightPink;
                                break;
                            }
                        case "3":
                            {
                                GridView2.Rows[i].Cells[3].Text = "سررسید شده";
                                GridView2.Rows[i].BackColor = System.Drawing.Color.Beige;
                                break;
                            }
                    }
                }
                catch (Exception)
                {

                }
            }


        }

        protected void btnOverNighte_OnClick(object sender, EventArgs e)
        {
            try
            {
                DBLayare.OverNight overNight = new OverNight();
                if (overNight.startOverNight(DateTime.Now.Date))
                {
                    lblNote.Text = "عملیات شبانه با موفقیت انجام گردید";
                    lblNote.Visible = true;
                    lblNote.ForeColor = System.Drawing.Color.DarkGreen;
                }
                
            }
            catch (Exception err)
            {
                lblNote.Text = "مشکل شبانه : " + err.Message;
                lblNote.Visible = true;
                lblNote.ForeColor = System.Drawing.Color.Red;
            }
        }

        
    }
}