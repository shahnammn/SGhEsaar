using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;

namespace DBLayare
{
    public class OverNight
    {

        public Boolean startOverNight(DateTime nowDate)
        {
            Dl dataClass = new Dl();
            DataTable loanList = dataClass.fillLoanListForOverNighte(nowDate);
          
            try
            {
                foreach (DataRow loanRow in loanList.Rows)
                {
                    #region connection
                    OleDbConnection conn;
                    try
                    {
                        conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.10.0;Data Source=" + dataClass.dbPathm);
                        conn.Open();
                    }
                    catch
                    {
                        try
                        {
                            conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dataClass.dbPathm);
                            conn.Open();
                        }
                        catch
                        {
                            try
                            {
                                conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.14.0;Data Source=" + dataClass.dbPathm);
                                conn.Open();
                            }
                            catch
                            {
                                conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.15.0;Data Source=" + dataClass.dbPathm);
                                conn.Open();
                            }
                        }
                    }
                    #endregion
                    var transaction = conn.BeginTransaction();
                    try
                    {
                        DataTable installmenList =
                      dataClass.fillInstallmentForOverNighte(int.Parse(loanRow["id"].ToString()), nowDate);
                        foreach (DataRow installmenRow in installmenList.Rows)
                        {
                            int installmentStatus;
                            //Todo: add Too parametter Table
                            int longTimeDueDate = 1;
                            if ((DateTime.Parse(installmenRow["DUE_DATE"].ToString()).AddMonths(longTimeDueDate)) > nowDate)
                            {
                                installmentStatus = (int) Loan.InstallmentStatus.DueDate;
                            }
                            else
                            {
                                installmentStatus = (int) Loan.InstallmentStatus.PassDue;
                            }
                            string strSqlInstallmen =
                                "UPDATE InstallmentTable SET InstallmentTable.Installment_Status = " + installmentStatus  + " WHERE (((InstallmentTable.id)=" +
                                int.Parse(installmenRow["id"].ToString()) + "))";
                            OleDbDataAdapter oledbAdapterInstallmentTable = new OleDbDataAdapter();
                            oledbAdapterInstallmentTable.InsertCommand = new OleDbCommand(strSqlInstallmen, conn, transaction);
                            oledbAdapterInstallmentTable.InsertCommand.ExecuteNonQuery();
                        }
                        string strSqlLoan;
                        if (installmenList.Rows.Count != 0)
                        {
                            strSqlLoan = "UPDATE Loan SET Loan.Loan_status =" + (int) Loan.LoanStatus.PassDue +
                                                ", Loan.calc_date = '" + nowDate + "' WHERE (((Loan.ID)=" +
                                                loanRow["id"] + " ))";
                        }
                        else
                        {
                            strSqlLoan = "UPDATE Loan SET Loan.calc_date = '" + nowDate + "' WHERE (((Loan.ID)=" +
                                                loanRow["id"] + " ))";
                        }
                        OleDbDataAdapter oledbAdapterLoanTable = new OleDbDataAdapter();
                        oledbAdapterLoanTable.InsertCommand = new OleDbCommand(strSqlLoan, conn, transaction);
                        oledbAdapterLoanTable.InsertCommand.ExecuteNonQuery();
                        transaction.Commit();
                        conn.Close();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        conn.Close();
                    }

                }
                return true;
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}