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
    public partial class Grant : System.Web.UI.Page
    {
        private DBLayare.Dl dataLayerClass = new Dl();
        public decimal accountId;
        public Decimal balanceAmount;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                _grantDate.Date = DateTime.Now;
            }
        }

        /// <summary>
        /// جستجوی مشتری
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Label2.Text = (string)GetLocalResourceObject("Grant_btnSearch_Click_name");
            Label3.Text = (string)GetLocalResourceObject("LName");
            Label4.Text = (string)GetLocalResourceObject("balance");

            var dataTable = dataLayerClass.fillClientByClientNumber(txtClientNum.Text);
            hfClientName.Value = dataTable.Rows[0]["FirstName"].ToString() + " " + dataTable.Rows[0]["LastName"].ToString();
            Label2.Text = " " + Label2.Text + dataTable.Rows[0]["FirstName"].ToString();
            Label3.Text = " " + Label3.Text + dataTable.Rows[0]["LastName"].ToString();

            var accAmount = dataLayerClass.getParameterById(1);
            decimal familyCount = 1;
            familyCount = Convert.ToDecimal(dataTable.Rows[0]["FamilyCount"].ToString());
            decimal signAmount = (familyCount * (Convert.ToDecimal((accAmount.value.ToString()))));
            string txtsignAmount = signAmount.ToString("0,0");
            Label5.Text = (string)GetLocalResourceObject("AccAmount") + txtsignAmount + (string)GetLocalResourceObject("Rial");

            if (dataTable.Rows[0]["Balance"].ToString() != "")
            {
                balanceAmount = (decimal)Convert.ToDouble(dataTable.Rows[0]["Balance"].ToString());
                string balanceString = balanceAmount.ToString("0,0");
                Label4.Text = " " + Label4.Text + balanceString + (string)GetLocalResourceObject("Rial");
            }
            accountId = Convert.ToDecimal(dataTable.Rows[0]["ID"].ToString());
            hfAccountID.Value = dataTable.Rows[0]["ID"].ToString();
            hfBalanceAmount.Value = dataTable.Rows[0]["Balance"].ToString();
            hfClientid.Value = dataTable.Rows[0]["Clientid"].ToString();
            decimal aa = (decimal.Parse(hfBalanceAmount.Value.ToString())) * (decimal)2.5;
            _txtAmountLoan.Text = aa.ToString("0,0");

            plnLoan.Visible = pnlClientDetail.Visible = true;
            btnok.Enabled = true;
            lblnote.Text = "";
            txtClientNum.Enabled = btnSearch.Enabled = false;
            
        }
        /// <summary>
        /// تایید
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnok_Click(object sender, EventArgs e)
        {
            try
            {
                DBLayare.Loan loanClass = new Loan();
                Loan.LoanStruct result = new Loan.LoanStruct();
                if (_grantDate.Date != null)
                    result = loanClass.grantLoan(txtClientNum.Text, (DateTime)_grantDate.Date, Convert.ToInt32(_ddlTimeLoan.SelectedValue.ToString()), Convert.ToDecimal(_txtAmountLoan.Text), Convert.ToInt32(hfClientid.Value), false);
                lblnote.Text = @"عملیات با موفقیت انجام شد. شماره تسهیلات :" + result.loanNumber;
                lblnote.ForeColor = System.Drawing.Color.DarkCyan;
                btnok.Enabled = btnSearch.Enabled = false;
            }
            catch (Exception a)
            {
                lblnote.Text = (string) System.Web.HttpContext.GetLocalResourceObject("~/Grant.aspx", "notSucess") +
                               "<br>" + a.Message;
                lblnote.ForeColor = System.Drawing.Color.Red;
            }
        }

        /// <summary>
        /// انصراف
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// محاسبه
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _btnCalc_OnClick(object sender, EventArgs e)
        {
            DBLayare.Loan loanClass = new Loan();
            Loan.LoanStruct result = new Loan.LoanStruct();
            if (_grantDate.Date != null)
            {
                result = loanClass.grantLoan(txtClientNum.Text, (DateTime)_grantDate.Date, Convert.ToInt32(_ddlTimeLoan.SelectedValue.ToString()), Convert.ToDecimal(_txtAmountLoan.Text), Convert.ToInt32(hfClientid.Value), true);
            }
            DateTime endLoanTime = result.expDate.Date;
            PersianCalendar pc1 = new PersianCalendar();
            _lblEndDate.Text = pc1.GetDayOfMonth(endLoanTime) + "/" + pc1.GetMonth(endLoanTime) + "/" +
                               pc1.GetYear(endLoanTime);
            lblinstallmentFirstDate.Text = pc1.GetDayOfMonth(result.installmentFirstDate) + "/" + pc1.GetMonth(result.installmentFirstDate) + "/" +
                                   pc1.GetYear(result.installmentFirstDate);
            _lblinstallmentAmount.Text = result.installmentAmount.ToString("0,0");
            _lblFirstInstallmentAmount.Text = result.installmentFirstAmount.ToString("0,0");
            _lblGrantAmount.Text = (result.amount - (result.amount * (decimal)0.01)).ToString("0,0");
            _lblcomm.Text = (result.amount*(decimal) 0.01).ToString("0,0");
        }
    }
}