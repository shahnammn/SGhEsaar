using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBLayare;

namespace WebApplication1
{
    public partial class installmentDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DBLayare.Loan loanInfo = new Loan();
            Loan.LoanStruct? loanInfoByClientNumber = loanInfo.getLoanInfoByClientNumber((string)Session["UserName"]);
            if (loanInfoByClientNumber != null)
            {
                lblNoteLoan.Visible = false;
                DBLayare.Loan.LoanStruct loanInfoResult = (Loan.LoanStruct)loanInfoByClientNumber;

                loanDS.SelectCommand =
                    "SELECT InstallmentTable.Installment_Number, InstallmentTable.DUE_DATE, InstallmentTable.Installment_Amount, InstallmentTable.Installment_Status" +
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
               
                    string a = GridView2.Rows[i].Cells[1].Text;
                    PersianCalendar pc1 = new PersianCalendar();
                    string datetime = pc1.GetYear(Convert.ToDateTime(a)) + "/" + pc1.GetMonth(Convert.ToDateTime(a)) +
                                      "/" +
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
                                GridView2.Rows[i].BackColor = System.Drawing.Color.Aquamarine;
                                break;
                            }
                        case "1":
                            {
                                GridView2.Rows[i].Cells[3].Text = "سررسید گذشته";
                                GridView2.Rows[i].BackColor = System.Drawing.Color.LightPink;
                                break;
                            }
                    }
            }
        }
    }
}