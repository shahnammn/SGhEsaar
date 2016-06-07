using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DBLayare
{
    public class Loan
    {
        public struct LoanStruct
        {
            /// <summary>
            /// شماره مشتری
            /// </summary>
            public string clientNumber;
            /// <summary>
            /// شناسه مشتری
            /// </summary>
            public int clientId;
            /// <summary>
            /// مبلغ وام
            /// </summary>
            public Decimal amount;
            /// <summary>
            /// مبلغ قسط
            /// </summary>
            public Decimal installmentAmount;
            /// <summary>
            /// مبلغ اولین قسط
            /// </summary>
            public Decimal installmentFirstAmount;
            /// <summary>
            /// تاریخ اعطا
            /// </summary>
            public DateTime grantDate;
            /// <summary>
            /// تاریخ پیش بینی تسویه
            /// </summary>
            public DateTime expDate;
            /// <summary>
            /// تاریخ اولین قسط
            /// </summary>
            public DateTime installmentFirstDate;
            /// <summary>
            /// تعداد اقساط
            /// </summary>
            public int longTimeLoan;
            /// <summary>
            /// وضعیت وام
            /// </summary>
            public int loanStatus;
            /// <summary>
            /// جدول اقساط
            /// </summary>
            public List<InstallmentTableStruct> installmentTable;
            /// <summary>
            /// شماره تسهیلات
            /// </summary>
            public int? loanNumber;
            /// <summary>
            /// تاریخ محاسبه
            /// </summary>
            public DateTime calcDate;

        }
        /// <summary>
        /// جدول اقساط
        /// </summary>
        public struct InstallmentTableStruct
        {
            /// <summary>
            /// شماره قسط
            /// </summary>
            public int installmentNumber;
            /// <summary>
            /// تاریخ سررسید
            /// </summary>
            public DateTime dueDate;
            /// <summary>
            /// مبلغ قسط
            /// </summary>
            public decimal installmentAmount;
            /// <summary>
            /// وضعیت قسط
            /// </summary>
            public int installmentStatus;
        }

        public enum LoanStatus
        {
            /// <summary>
            /// جاری
            /// </summary>
             Current = 0,
            /// <summary>
            /// سررسید گذشته
            /// </summary>
            PassDue = 1,
            /// <summary>
            /// مشکوک الوصول
            /// </summary>
            Mashkok = 2,
            /// <summary>
            /// تسویه شده
            /// </summary>
            Settlment = 3
            
        }

        public enum InstallmentStatus
        {
            /// <summary>
            /// پرداخت نشده
            /// </summary>
            Current = 0,
            /// <summary>
            /// سر رسید شده
            /// </summary>
            DueDate=3,
            /// <summary>
            /// سررسید گذشته
            /// </summary>
            PassDue = 1,
            /// <summary>
            /// پرداخت شده
            /// </summary>
            Pass = 2
        }
        public LoanStruct loanResualt;

        /// <summary>
        /// اعطای وام
        /// </summary>
        /// <param name="clientNumber">شماره مشتری</param>
        /// <param name="grantDate">تاریخ اعطای تسهیلات</param>
        /// <param name="longTime">زمان تسهیلات</param>
        /// <param name="loanAmount">مبلغ تسهیلات</param>
        /// <param name="clientid"></param>
        /// <param name="calcFlag"></param>
        /// <returns></returns>
        public LoanStruct grantLoan(string clientNumber, DateTime grantDate, int longTime, Decimal loanAmount, int clientid, bool calcFlag)
        {
            loanResualt.clientNumber = clientNumber;
            loanResualt.clientId = clientid;
            loanResualt.grantDate = grantDate;
            loanResualt.amount = loanAmount;
            loanResualt.longTimeLoan = longTime;
            loanResualt.expDate = loanResualt.grantDate.AddMonths(loanResualt.longTimeLoan);
            calcInstallmentAmount();
            loanResualt.installmentFirstDate = loanResualt.grantDate.AddMonths(1);

            //            decimal installmentAmount = amountLoan / timeLoan;
            //            _lblinstallmentAmount.Text = installmentAmount.ToString("0,0.00");
            InstallmentTableStruct installmentRow = new InstallmentTableStruct();
            installmentRow.installmentNumber = 1;
            installmentRow.installmentAmount = loanResualt.installmentFirstAmount;
            installmentRow.dueDate = loanResualt.installmentFirstDate;
            installmentRow.installmentStatus = 0;
            loanResualt.installmentTable = new List<InstallmentTableStruct>();
            loanResualt.installmentTable.Add(installmentRow);
            for (int i = 1; i < longTime; i++)
            {
                InstallmentTableStruct installmentRow1 = new InstallmentTableStruct();
                installmentRow1.installmentNumber = i + 1;
                installmentRow1.installmentAmount = loanResualt.installmentAmount;
                installmentRow1.dueDate = loanResualt.installmentFirstDate.AddMonths(i);
                installmentRow1.installmentStatus = 0;
                loanResualt.installmentTable.Add(installmentRow1);
            }
            if (!calcFlag)
            {
                Dl dataLayer = new Dl();
                loanResualt.loanNumber = dataLayer.insertLoan(loanResualt);
            }
            return loanResualt;
        }
        /// <summary>
        /// 
        /// </summary>
        private void calcInstallmentAmount()
        {
            decimal installmentAmount = loanResualt.amount / loanResualt.longTimeLoan;
            Decimal fraction = 0;
            loanResualt.installmentAmount = decimal.Truncate(installmentAmount);
            if ((loanResualt.amount % loanResualt.longTimeLoan) != 0)
            {
                fraction = ((Decimal)((installmentAmount - loanResualt.installmentAmount))) * loanResualt.longTimeLoan;
            }
            loanResualt.installmentFirstAmount = Math.Round(fraction) + loanResualt.installmentAmount;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientNumber"></param>
        /// <returns></returns>
        public LoanStruct? getLoanInfoByClientNumber(string clientNumber)
        {
            try
            {
                Dl dataLayer = new Dl();
                DataTable loanTable = dataLayer.fillLoanInfoByClientNum(clientNumber);
                LoanStruct result = new LoanStruct();
                if (loanTable.Rows.Count != 0)
                {
                    result.loanNumber = (int?)loanTable.Rows[0]["Loan.ID"];
                    result.installmentAmount = (decimal) loanTable.Rows[0]["Installment_Amount"];
                    result.loanStatus = int.Parse(loanTable.Rows[0]["Loan_status"].ToString());
                    result.grantDate = (DateTime) loanTable.Rows[0]["Grant_Date"];
                    result.amount = (decimal) loanTable.Rows[0]["Loan_amount"];
                    return result;
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
