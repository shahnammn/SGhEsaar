using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBLayare;

namespace WebApplication1
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        private DBLayare.Dl dataLayerClass = new Dl();
        public Int32 numberOfinstallment;
        
        public decimal accountId;
        public Decimal balanceAmount;
        protected void Page_Load(object sender, EventArgs e)
        {
            PersianCalendar pc1 = new PersianCalendar();
            DateTime thisDate1 = DateTime.Now;
            string datetime = pc1.GetDayOfMonth(thisDate1) + "/" + pc1.GetMonth(thisDate1) + "/" +
                              pc1.GetYear(thisDate1);
            //            lblCurentDate.Text = "تاریخ واریز: " + datetime;
            

            if (!IsPostBack)
            {
                _PayDate.Date = _grantDate.Date = thisDate1;
                txtDesc.Text = (string)System.Web.HttpContext.GetLocalResourceObject("~/Grant.aspx", "WebForm1_Page_Load_تاریخ_واریز__") + _grantDate.DatePersian.ToString();
                txtDesc.ForeColor = System.Drawing.Color.Gray;
            }
        }

        /// <summary>
        /// جستجوی مشتری
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Label2.Text = "نام: ";
            Label3.Text = "نام خانوادگی: ";
            Label4.Text = "مانده حساب:  ";

            var dataTable = dataLayerClass.fillClientByClientNumber(txtClientNum.Text);
            DBLayare.Loan loanInfoClass = new Loan();
            Loan.LoanStruct? loanInfoByClientNumber = loanInfoClass.getLoanInfoByClientNumber(txtClientNum.Text);
            if (loanInfoByClientNumber != null)
            {
                DBLayare.Loan.LoanStruct  loanInfo = (Loan.LoanStruct)loanInfoByClientNumber;
                Label11.Text = "شماره تسهیلات: " + loanInfo.loanNumber;
                hfLoanId.Value = loanInfo.loanNumber.ToString();

                hfInstallmentAmount.Value = loanInfo.installmentAmount.ToString();
                Label12.Text = "مبلغ قسط تسهیلات: " + loanInfo.installmentAmount.ToString("0,0") + " ريال";
                Label13.Text = "وضعیت تسهیلات: ";
                loanPanel.Visible = true;

                switch (loanInfo.loanStatus)
                {
                    case 0:
                        {
                            Label13.Text = Label13.Text  +  "جاری";
                            break;
                        }
                    case 1:
                        {
                            Label13.Text = Label13.Text  +  "سررسید گذشته";
                            Label13.ForeColor = System.Drawing.Color.Red;
                            break;
                        }
                    case 2:
                        {
                            Label13.Text = Label13.Text  +  "مشکوک الوصول";
                            Label13.ForeColor = System.Drawing.Color.Red;
                            break;
                        }
                    case 3:
                        {
                            Label13.Text = Label13.Text  +  "تسویه شده";
                            loanPanel.Visible = false;
                            break;
                        }
                }
                 numberOfinstallment = dataLayerClass.getNumberOfinstallment((int) loanInfo.loanNumber);
                Label9.Text = numberOfinstallment.ToString();
            }
            else
            {
                loanPanel.Visible = false;
                Label11.Text = "";
                Label12.Text = "";
                Label13.Text = "";

            }
            hfClientName.Value = dataTable.Rows[0]["FirstName"].ToString() + " " + dataTable.Rows[0]["LastName"].ToString();
            Label2.Text = " " + Label2.Text + dataTable.Rows[0]["FirstName"].ToString();
            Label3.Text = " " + Label3.Text + dataTable.Rows[0]["LastName"].ToString();

            var param = dataLayerClass.getParameterById(1);
            decimal aa = 1;
            aa = Convert.ToDecimal(dataTable.Rows[0]["FamilyCount"].ToString());
            decimal signAmount = (aa * (Convert.ToDecimal((param.value.ToString()))));
            string txtsignAmount = signAmount.ToString("0,0");
            Label5.Text = "مبلغ عضویت: " + txtsignAmount + " ريال ";

            if (dataTable.Rows[0]["Balance"].ToString() != "")
            {
                balanceAmount = (decimal)Convert.ToDouble(dataTable.Rows[0]["Balance"].ToString());
                string balanceString = balanceAmount.ToString("0,0");
                Label4.Text = " " + Label4.Text + balanceString + " ریال ";
            }
            accountId = Convert.ToDecimal(dataTable.Rows[0]["ID"].ToString());
            hfAccountID.Value = dataTable.Rows[0]["ID"].ToString();
            hfBalanceAmount.Value = dataTable.Rows[0]["Balance"].ToString();

            btnok.Enabled = true;
            lblnote.Text = "";
            txtAmount.Text = "";
        }


        /// <summary>
        /// انصراف
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/AccountPayment.aspx");
        }

        /// <summary>
        /// ثبت سند
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnok_Click(object sender, EventArgs e)
        {
            DBLayare.Dl.PaymentStruct payForm = new DBLayare.Dl.PaymentStruct();
            payForm.amount = Convert.ToDecimal(txtAmount.Text);
            payForm.description = txtDesc.Text;
            payForm.paymentDate = DateTime.Now;
            payForm.paymentFlag = chbPayment.Checked;
            if (!payForm.paymentFlag)
            {
                payForm.amount = payForm.amount * (-1);
            }
            if (hfBalanceAmount.Value != "")
            {
                payForm.accountId = decimal.Parse(hfAccountID.Value);
                payForm.balanceAmount = decimal.Parse(hfBalanceAmount.Value) + payForm.amount;
            }
            else
                payForm.balanceAmount = payForm.amount;
            var result = dataLayerClass.vocherPosting(payForm, hfClientName.Value);
            if (result)
            {
                lblnote.Text = (string)System.Web.HttpContext.GetLocalResourceObject("~/Grant.aspx", "Sucess");
                lblnote.ForeColor = System.Drawing.Color.DarkCyan;
            }
            else
            {
                lblnote.Text = (string)System.Web.HttpContext.GetLocalResourceObject("~/Grant.aspx", "notSucess");
                lblnote.ForeColor = System.Drawing.Color.Red;
            }
        }


        protected void clcAmountPay_OnClick(object sender, EventArgs e)
        {
            Label10.Text = "مبلغ قابل پرداخت :";
            if (int.Parse(Label9.Text) >= int.Parse(_txtInstallmentNumber.Text.ToString()))
                {
                    decimal amount = int.Parse(hfInstallmentAmount.Value) * int.Parse(_txtInstallmentNumber.Text.ToString());
                    Label10.Text = Label10.Text + " " + amount.ToString("0,0")  + " ريال";
                }
                else
                {
                    lblnote.Text = "تعداد اقساط پرداختی نمی تواند از تعداد اقساط پرداخت نشده بزرگتر باشد";
                    lblnote.ForeColor = System.Drawing.Color.Red;
                }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _payInstallment_OnClick(object sender, EventArgs e)
        {
            if (dataLayerClass.vocherRepayment(Convert.ToInt32(hfLoanId.Value), (DateTime)_PayDate.Date, int.Parse(_txtInstallmentNumber.Text), int.Parse(hfInstallmentAmount.Value) * int.Parse(_txtInstallmentNumber.Text.ToString()), hfClientName.Value))
            {
                lblnote.Text = (string)System.Web.HttpContext.GetLocalResourceObject("~/Grant.aspx", "Sucess");
                lblnote.ForeColor = System.Drawing.Color.DarkCyan;

            }
            else
            {
                lblnote.Text = (string)System.Web.HttpContext.GetLocalResourceObject("~/Grant.aspx", "notSucess");
                lblnote.ForeColor = System.Drawing.Color.Red;

            }
        }
    }

}