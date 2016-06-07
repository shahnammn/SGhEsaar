
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Web;

namespace DBLayare
{
    public class Dl
    {
        public string dbPathm = HttpRuntime.AppDomainAppPath + ("DB\\Database11.accdb");
        public struct PaymentStruct
        {
            public Decimal accountId;
            public Decimal amount;
            public Decimal balanceAmount;
            public Boolean paymentFlag;
            public DateTime paymentDate;
            public String description;
        }

        public struct Parameter
        {
            public int id;
            public String code;
            public String value;
            public String description;
        }

        public bool vocherRepayment(int loanId, DateTime repaymentDate, int installmentNumber, decimal amountRepayment, string name)
        {
            #region Connection
            OleDbConnection conn;
            try
            {
                conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.10.0;Data Source=" + dbPathm);
                conn.Open();
            }
            catch
            {
                try
                {
                    conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dbPathm);
                    conn.Open();
                }
                catch
                {
                    try
                    {
                        conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.14.0;Data Source=" + dbPathm);
                        conn.Open();
                    }
                    catch
                    {
                        conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.15.0;Data Source=" + dbPathm);
                        conn.Open();
                    }
                }
            }
            #endregion
            var transaction = conn.BeginTransaction();
            try
            {
                var clientSGHInfo = fillClientByClientNumber("000");
                var balanceAmount = amountRepayment + Convert.ToDecimal(clientSGHInfo.Rows[0]["Balance"].ToString());
                string description = "پرداخت اقساط مشتری : " + name + " برای " + installmentNumber + " تعداد قسط ";
                string strQuery =
                    "INSERT INTO payment ( Account_ID, Amount, [date], deposit, Bl_Amount, Description ) VALUES (" +
                    4 + "," + amountRepayment + ",'" + repaymentDate + "'," + true + "," +
                    balanceAmount + ",'" + description + "')";
                //connectAndInsertAccess(strQuery, dbPathm);
                OleDbDataAdapter oledbAdapterpaymentTable = new OleDbDataAdapter();
                oledbAdapterpaymentTable.InsertCommand = new OleDbCommand(strQuery, conn, transaction);
                oledbAdapterpaymentTable.InsertCommand.ExecuteNonQuery();

                const string strQuery2 = "SELECT Max(id)+1 AS mId FROM payment";

                DataTable paymentTable = connectAndQueryAccess(strQuery2, dbPathm);
                int paymentId = (int)paymentTable.Rows[0]["mid"];

                String strQuery1 = "SELECT TOP " + installmentNumber + "  id FROM InstallmentTable WHERE (((InstallmentTable.Loan_ID)=" + loanId + ") AND ((InstallmentTable.Installment_Status)<>2 )) ORDER BY InstallmentTable.id";
                DataTable installmentTable = connectAndQueryAccess(strQuery1, dbPathm);

                string strUpdateAccountQuery = "UPDATE Account SET  Balance= " + balanceAmount + " where ID = 4";
                OleDbDataAdapter oledbAdapterAccountTable = new OleDbDataAdapter();
                oledbAdapterAccountTable.InsertCommand = new OleDbCommand(strUpdateAccountQuery, conn, transaction);
                oledbAdapterAccountTable.InsertCommand.ExecuteNonQuery();


                for (int i = 0; i < installmentNumber; i++)
                {
                    int installmentid = (int) installmentTable.Rows[i]["id"];
                    String strInsertQuery = "INSERT INTO Rep_inst (pay_id,inst_id) VALUES (" + paymentId + "," + installmentid + ")";
                    OleDbDataAdapter oledbAdapterRepnstTable = new OleDbDataAdapter();
                    oledbAdapterRepnstTable.InsertCommand = new OleDbCommand(strInsertQuery, conn, transaction);
                    oledbAdapterRepnstTable.InsertCommand.ExecuteNonQuery();
                    string strUpdateInstallmentQuery = "UPDATE InstallmentTable SET  Installment_Status= " + (int)Loan.InstallmentStatus.Pass + ",Repayment_date='" + repaymentDate + "'   WHERE ID=" +
                               installmentid;
                    OleDbDataAdapter oledbAdapterInstallmentTable = new OleDbDataAdapter();
                    oledbAdapterInstallmentTable.InsertCommand = new OleDbCommand(strUpdateInstallmentQuery, conn, transaction);
                    oledbAdapterInstallmentTable.InsertCommand.ExecuteNonQuery();

                }
                transaction.Commit();
                conn.Close();
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                conn.Close();
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loan"></param>
        /// <returns></returns>
        public int insertLoan(Loan.LoanStruct loan)
        {

            OleDbConnection conn;
            try
            {
                conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.10.0;Data Source=" + dbPathm);
                conn.Open();
            }
            catch
            {
                try
                {
                    conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dbPathm);
                    conn.Open();
                }
                catch
                {
                    try
                    {
                        conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.14.0;Data Source=" + dbPathm);
                        conn.Open();
                    }
                    catch
                    {
                        conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.15.0;Data Source=" + dbPathm);
                        conn.Open();
                    }
                }
            }

            #region sample


            //                DataSet dataSetLoan = new Database11DataSet();
            //                DataRow dataRowLoan = dataSetLoan.Tables["Loan"].NewRow();
            //                dataRowLoan["Client_id"] = loan.clientId;
            //                dataRowLoan["Grant_Date"] = loan.grantDate;
            //                dataRowLoan["Installment_Count"] = loan.longTimeLoan;
            //                dataRowLoan["Installment_Amount"] = loan.installmentAmount;
            //                dataRowLoan["Exp_Date"] = loan.expDate;
            //                dataRowLoan["First_installment_date"] = loan.installmentFirstDate;
            //                dataRowLoan["Loan_amount"] = loan.amount;
            //                dataRowLoan["Loan_status"] = int.Parse("0");
            //                /*
            //                using (myTrans = myConnection.BeginTransaction(
            //                    IsolationLevel.RepeatableRead, "SampleTransaction"))
            //                {
            //
            //                }*/
            //
            //
            //
            //                dataSetLoan.Tables["Loan"].Rows.Add(dataRowLoan);
            //
            //
            //                OleDbDataAdapter dAdapterLoan = new
            //                  OleDbDataAdapter(new OleDbCommand("SELECT * FROM Loan", conn));
            //                OleDbCommandBuilder oOrdersCmdBuilder = new OleDbCommandBuilder(dAdapterLoan);



            //                dAdapterLoan.Update(dataSetLoan, "Loan");



            //for (int i = 0; i < loan.longTimeLoan; i++)
            //{
            //    DataRow dataRowInstallmentTable = dataSetLoan.Tables["InstallmentTable"].NewRow();
            //    dataRowInstallmentTable["Loan_ID"] = loanId;
            //    dataRowInstallmentTable["Installment_Number"] = loan.installmentTable[i].installmentNumber;
            //    dataRowInstallmentTable["DUE_DATE"] = loan.installmentTable[i].dueDate;
            //    dataRowInstallmentTable["Installment_Amount"] = loan.installmentTable[i].installmentAmount;
            //    dataRowInstallmentTable["Installment_Status"] = loan.installmentTable[i].installmentStatus;
            //    dataSetLoan.Tables["InstallmentTable"].Rows.Add(dataRowInstallmentTable);
            //}

            //OleDbDataAdapter dAdapterInstallmentTable = new
            // OleDbDataAdapter(new OleDbCommand("SELECT * FROM InstallmentTable", conn));
            //OleDbCommandBuilder oOrdersCmdBuilderInstallmentTable = new OleDbCommandBuilder(dAdapterInstallmentTable);
            //dAdapterInstallmentTable.Update(dataSetLoan, "InstallmentTable");
            //
            //
            //            DataSet oDS = fillSchema("select * from InstallmentTable");
            //            //DataSet oDS = new Database11DataSet();
            //            
            //
            //            DataTable pTable = oDS.Tables["Table"];
            //            pTable.TableName = "InstallmentTable";
            //
            //             
            //            for (int i = 0; i < loan.longTimeLoan; i++)
            //            {
            //                DataRow dataRowInstallmentTable = oDS.Tables["InstallmentTable"].NewRow();
            //                dataRowInstallmentTable["Loan_ID"] = loanId;
            //                dataRowInstallmentTable["Installment_Number"] = loan.installmentTable[i].installmentNumber;
            //                dataRowInstallmentTable["DUE_DATE"] = loan.installmentTable[i].dueDate;
            //                dataRowInstallmentTable["Installment_Amount"] = loan.installmentTable[i].installmentAmount;
            //                dataRowInstallmentTable["Installment_Status"] = loan.installmentTable[i].installmentStatus;
            //                oDS.Tables["InstallmentTable"].Rows.Add(dataRowInstallmentTable);
            //            }
            //            
            //
            //            updateTable(oDS, "select * from InstallmentTable");
            //
            #endregion

            string strQuery =
                "INSERT INTO Loan ( Client_id, Grant_Date, Installment_Count, Installment_Amount, Exp_Date, First_installment_date, Loan_amount, Loan_status,calc_date ) VALUES (" +
                loan.clientId + ",'" + loan.grantDate + "'," + loan.longTimeLoan + "," + loan.installmentAmount +
                ",'" +
                loan.expDate + "','" + loan.installmentFirstDate + "'," + loan.amount + " , 0, '" + loan.grantDate + "')";

            connectAndInsertAccess(strQuery, dbPathm);

            string strQuery2 = "select Top 1 * from loan  " + " where (((loan.Client_id)= " + loan.clientId + ")) ORDER BY loan.ID DESC";
            DataTable loanDt = connectAndQueryAccess(strQuery2, dbPathm);

            int loanId = (int)loanDt.Rows[0]["id"];

            var transaction = conn.BeginTransaction();
            try
            {
                for (int i = 0; i < loan.longTimeLoan; i++)
                {
                    string strQuery1 =
                        "INSERT INTO InstallmentTable  ( Loan_ID, Installment_Number, DUE_DATE, Installment_Amount, Installment_Status ) VALUES (" +
                        loanId + "," + loan.installmentTable[i].installmentNumber + ",'" +
                        loan.installmentTable[i].dueDate + "'," +
                        loan.installmentTable[i].installmentAmount + "," + loan.installmentTable[i].installmentStatus +
                        ")";
                    OleDbDataAdapter oledbAdapterInstallmentTable = new OleDbDataAdapter();
                    oledbAdapterInstallmentTable.InsertCommand = new OleDbCommand(strQuery1, conn, transaction);
                    oledbAdapterInstallmentTable.InsertCommand.ExecuteNonQuery();
                }

                transaction.Commit();
                conn.Close();

                PaymentStruct dtLoan = new PaymentStruct();
                dtLoan.accountId = 4;
                dtLoan.amount = (-1) * loan.amount;
                dtLoan.paymentFlag = false;
                dtLoan.description = "اعطای تسهیلات  :" + loanId;
                dtLoan.paymentDate = loan.grantDate;
                //مشتری صندوق
                var clientSGHInfo = fillClientByClientNumber("000");
                dtLoan.balanceAmount = dtLoan.amount + Convert.ToDecimal(clientSGHInfo.Rows[0]["Balance"].ToString());
                vocher(dtLoan);

                PaymentStruct dtcom = new PaymentStruct();
                //مشتری کارمزد
                var comInfo = fillClientByClientNumber("002");
                dtcom.amount = loan.amount * (decimal)0.01;
                dtcom.balanceAmount = dtcom.amount + Convert.ToDecimal(comInfo.Rows[0]["Balance"].ToString());
                dtcom.description = "کارمزد تسهیلات :" + loanId;
                dtcom.accountId = 80;
                dtcom.paymentDate = loan.grantDate;
                dtcom.paymentFlag = true;
                vocher(dtcom);
                return loanId;
            }
            catch (Exception e)
            {
                transaction.Rollback();
                throw;
            }
        }


        private DataSet fillSchema(string strSql)
        {
            OleDbConnection conn;
            try
            {
                conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.10.0;Data Source=" + dbPathm);
                conn.Open();
            }
            catch
            {
                try
                {
                    conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dbPathm);
                    conn.Open();
                }
                catch
                {
                    try
                    {
                        conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.14.0;Data Source=" + dbPathm);
                        conn.Open();
                    }
                    catch
                    {
                        conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.15.0;Data Source=" + dbPathm);
                        conn.Open();
                    }
                }
            }

            DataSet oDs1 = new DataSet();
            OleDbDataAdapter oOrdersDataAdapter = new
                OleDbDataAdapter(new OleDbCommand(strSql, conn));
            OleDbCommandBuilder oOrdersCmdBuilder = new OleDbCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.FillSchema(oDs1, SchemaType.Source);
            return oDs1;
        }

        private void updateTable(DataSet aa, string strSql)
        {
            OleDbConnection conn;
            try
            {
                conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.10.0;Data Source=" + dbPathm);
                conn.Open();
            }
            catch
            {
                try
                {
                    conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dbPathm);
                    conn.Open();
                }
                catch
                {
                    try
                    {
                        conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.14.0;Data Source=" + dbPathm);
                        conn.Open();
                    }
                    catch
                    {
                        conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.15.0;Data Source=" + dbPathm);
                        conn.Open();
                    }
                }
            }
            try
            {

                OleDbDataAdapter oOrdersDataAdapter = new
                    OleDbDataAdapter(new OleDbCommand(strSql, conn));
                OleDbCommandBuilder oOrdersCmdBuilder = new OleDbCommandBuilder(oOrdersDataAdapter);
                oOrdersDataAdapter.FillSchema(aa, SchemaType.Source);

                DataTable pTable = aa.Tables["Table"];
                pTable.TableName = "InstallmentTable";


                //            new OleDbCommandBuilder(oOrdersDataAdapter).GetInsertCommand();
                //
                //            foreach (DataRow row in aa.Tables["InstallmentTable"].Rows)
                //            {
                //                row.SetAdded();
                //                
                //            }
                oOrdersDataAdapter.Update(aa, "InstallmentTable");
                conn.Close();
                //OleDbCommandBuilder oOrderDetailsCmdBuilder = new 
                //    OleDbCommandBuilder(oOrderDetailsDataAdapter);
            }
            catch (Exception e)
            {
                e.Data.Add("shahnam", strSql);
                throw;
            }
        }

        private void test()
        {
            // Create the DataSet object
            DataSet oDS = new DataSet();
            OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; Data Source=orders.mdb");
            conn.Open();

            // Create the DataTable "Orders" in the Dataset and the OrdersDataAdapter
            OleDbDataAdapter oOrdersDataAdapter = new
                OleDbDataAdapter(new OleDbCommand("SELECT * FROM Orders", conn));
            OleDbCommandBuilder oOrdersCmdBuilder = new OleDbCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source);

            DataTable pTable = oDS.Tables["Table"];
            pTable.TableName = "Orders";

            // Create the DataTable "OrderDetails" in the Dataset 
            //and the OrderDetailsDataAdapter
            OleDbDataAdapter oOrderDetailsDataAdapter = new
                OleDbDataAdapter(new OleDbCommand("SELECT * FROM OrderDetails", conn));
            OleDbCommandBuilder oOrderDetailsCmdBuilder = new OleDbCommandBuilder(oOrderDetailsDataAdapter);
            oOrderDetailsDataAdapter.FillSchema(oDS, SchemaType.Source);

            pTable = oDS.Tables["Table"];
            pTable.TableName = "OrderDetails";


            // Insert the Data
            DataRow oOrderRow = oDS.Tables["Orders"].NewRow();
            oOrderRow["CustomerName"] = "Customer ABC";
            oOrderRow["ShippingAddress"] = "ABC street, 12345";
            oDS.Tables["Orders"].Rows.Add(oOrderRow);

            DataRow oDetailsRow = oDS.Tables["OrderDetails"].NewRow();
            oDetailsRow["ProductId"] = 1;
            oDetailsRow["ProductName"] = "Product 1";
            oDetailsRow["UnitPrice"] = 1;
            oDetailsRow["Quantity"] = 2;

            oDetailsRow.SetParentRow(oOrderRow);
            oDS.Tables["OrderDetails"].Rows.Add(oDetailsRow);


            oOrdersDataAdapter.Update(oDS, "Orders");
            oOrderDetailsDataAdapter.Update(oDS, "OrderDetails");

            conn.Close();
        }

        public int getNumberOfinstallment(int loanId)
        {
            String strQuery = "SELECT count(*) FROM InstallmentTable WHERE (((InstallmentTable.Loan_ID)=" + loanId + ") AND ((InstallmentTable.Installment_Status)<>2 ))";
            DataTable a = connectAndQueryAccess(strQuery, dbPathm);
            return Convert.ToInt32(a.Rows[0][0].ToString());
        }

        public DataTable fillLoanInfoByClientNum(string clientNum)
        {

            String strQuery =
                "SELECT TOP 1 * FROM Client INNER JOIN Loan ON Client.ID = Loan.Client_id WHERE (((Client.Client_number)='" +
                clientNum + "') " +
                " AND ((Loan.Loan_status)<>3)) ORDER BY Loan.ID DESC";
            return connectAndQueryAccess(strQuery, dbPathm);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="now">تاریخ محاسبه تسهیلات</param>
        /// <returns></returns>
        public DataTable fillLoanListForOverNighte(DateTime now)
        {

            String strQuery = "SELECT * FROM Loan WHERE  (Loan.Loan_status)<> 3 AND (((Loan.calc_date) < #" + now.ToString("yyy/MM/dd") + "#))";
            return connectAndQueryAccess(strQuery, dbPathm);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loanId">شماره تسهیلات</param>
        /// <param name="now">تاریخ سررسید اقساط</param>
        /// <returns></returns>
        public DataTable fillInstallmentForOverNighte(int loanId, DateTime now)
        {

            String strQuery = "SELECT * FROM InstallmentTable WHERE (((InstallmentTable.Loan_ID)=" + loanId + ") AND ((InstallmentTable.DUE_DATE)<=#" + now.ToString("yyy/MM/dd") + "#) AND ((InstallmentTable.Installment_Status)=0))";
            return connectAndQueryAccess(strQuery, dbPathm);
        }

        /// <summary>
        /// گرفتن اطلاعات یک مشتری
        /// </summary>
        /// <param name="clientNumber"></param>
        /// <returns></returns>
        public DataTable fillClientByClientNumber(string clientNumber)
        {
            //            db.Database11DataSetTableAdapters.ClientTableAdapter clad = new ClientTableAdapter();
            //            DataTable dataTableClient = new DataTable("Client");
            //            clad.Fill(dataTableClient);


            String strQuery = "SELECT Client.FirstName, Client.LastName, Account.Balance, Account.ID, Client.FamilyCount,Client.Tel,Client.Mobile,Client.Address, Client.ID as clientID " +
                              "FROM Client LEFT JOIN Account ON Client.ID = Account.client_ID " +
                              "WHERE (((Client.Client_number)='" + clientNumber + "'))";
            return connectAndQueryAccess(strQuery, dbPathm);
        }

        /// <summary>
        /// پارامترهای سیستم
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Parameter getParameterById(int id)
        {
            String strQuery = "SELECT Parameter.id, Parameter.code, Parameter.description, Parameter.value " +
                              "FROM Parameter WHERE Parameter.id = " + id;
            DataTable dt = connectAndQueryAccess(strQuery, dbPathm);
            Parameter param;
            param.id = (int)dt.Rows[0]["id"];
            param.code = dt.Rows[0]["code"].ToString();
            param.description = dt.Rows[0]["description"].ToString();
            param.value = dt.Rows[0]["value"].ToString();
            return param;
        }

        /// <summary>
        /// ثبت سند 
        /// </summary>
        /// <param name="dtMain"></param>
        /// <param name="clintName"></param>
        /// <returns></returns>
        public Boolean vocherPosting(PaymentStruct dtMain, string clintName)
        {

            vocher(dtMain);
            PaymentStruct dtCash = dtMain;
            dtCash.description = dtMain.description + " مربوط به مشتری " + clintName;
            var clientSGHInfo = fillClientByClientNumber("000");
            dtCash.balanceAmount = dtMain.amount + Convert.ToDecimal(clientSGHInfo.Rows[0]["Balance"].ToString());
            dtCash.accountId = 4;
            vocher(dtCash);
            return true;
        }


        private OleDbConnection connection()
        {
            OleDbConnection conn;
            try
            {
                conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.10.0;Data Source=" + dbPathm);
                conn.Open();
            }
            catch
            {
                try
                {
                    conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dbPathm);
                    conn.Open();
                }
                catch
                {
                    try
                    {
                        conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.14.0;Data Source=" + dbPathm);
                        conn.Open();
                    }
                    catch
                    {
                        conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.15.0;Data Source=" + dbPathm);
                        conn.Open();
                    }
                }
            }
            return conn;
        }

        private void vocher(PaymentStruct dtMain)
        {
            OleDbConnection conn = connection();
           var transaction = conn.BeginTransaction();
            try
            {
                string strQuery =
                    "INSERT INTO payment ( Account_ID, Amount, [date], deposit, Bl_Amount, Description ) VALUES (" +
                    dtMain.accountId + "," + dtMain.amount + ",'" + dtMain.paymentDate + "'," + dtMain.paymentFlag + "," +
                    dtMain.balanceAmount + ",'" + dtMain.description + "')";
                //            connectAndInsertAccess(strQuery, dbPathm);
                OleDbDataAdapter oledbAdapter = new OleDbDataAdapter();
                oledbAdapter.InsertCommand = new OleDbCommand(strQuery, conn,transaction);
                oledbAdapter.InsertCommand.ExecuteNonQuery();

                string strQuery1 = "UPDATE Account SET  Account.Balance= " + dtMain.balanceAmount +
                                   "   WHERE Account.ID=" +
                                   dtMain.accountId;
                //connectAndInsertAccess(strQuery1, dbPathm);
                OleDbDataAdapter oledbAdapter1 = new OleDbDataAdapter();
                oledbAdapter1.InsertCommand = new OleDbCommand(strQuery1, conn, transaction);
                oledbAdapter1.InsertCommand.ExecuteNonQuery();

                transaction.Commit();
                conn.Close();

            }   
            catch (Exception e)
            {
                transaction.Rollback();
                throw;
            }
        }

        /// <summary>
        /// بروز رسانی اطلاعات مشتری توسط مشتری
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="tel"></param>
        /// <param name="mobie"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public bool clientInfoUpdate(string userName, string tel, string mobie, string address)
        {
            try
            {
                string strUpdate = "UPDATE Client SET  Client.Tel= '" + tel + "', Client.Mobile= '" + mobie + "', Client.Address='" + address + "' WHERE Client.Client_number ='" + userName + "'";
                if (connectAndInsertAccess(strUpdate, dbPathm))
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName">شناسه کاربر</param>
        /// <param name="oldPass">رمز عبور قدیمی</param>
        /// <param name="newPass">رمز عبور جدید</param>
        public bool changePass(string userName, string oldPass, string newPass)
        {
            try
            {
                String strQuery = "SELECT count(*) as co " +
                              "FROM Client " +
                              "WHERE Client.Client_number ='" + userName + "' and Client.pass='" + oldPass + "'";
                DataTable dataTable = connectAndQueryAccess(strQuery, dbPathm);
                if (dataTable.Rows[0]["co"].ToString() == "0")
                {
                    // old pass False
                    return false;
                }
                string strUpdate = "UPDATE Client SET  Client.pass= '" + newPass + "' WHERE Client.Client_number ='" + userName + "'";
                connectAndInsertAccess(strUpdate, dbPathm);
                //Change Sucsess
                return true;
            }
            catch (Exception)
            {
                //err
                return false;
            }
        }

        /// <summary>
        /// بروز رسانی صندوق
        /// </summary>
        /// <param name="newAmount"></param>
        /// <returns></returns>
        private bool updateSGHAccount(decimal newAmount)
        {
            var clientSGHInfo = fillClientByClientNumber("000");
            decimal aa = newAmount + Convert.ToDecimal(clientSGHInfo.Rows[0][2].ToString());
            string strQuery = "UPDATE Account SET  Account.Balance= " + aa + " WHERE Account.ID= " + clientSGHInfo.Rows[0][3];
            if (connectAndInsertAccess(strQuery, dbPathm))
                return true;
            else
                return false;
        }

        public DataTable systemLogin(string userName, string pass)
        {
            try
            {
                String strQuery = "SELECT Client.FirstName, Client.LastName, Account.Balance, Account.ID " +
                  "FROM Client LEFT JOIN Account ON Client.ID = Account.client_ID " +
                  "WHERE Client.Client_number='" + userName + "' and Client.pass ='" + pass + "'";
                return connectAndQueryAccess(strQuery, dbPathm);
            }
            catch (Exception)
            {

                return null;
            }


        }

        /// <summary>
        /// ذخیره اطلاعات در جداول اکسس
        /// </summary>
        /// <param name="strQuery"></param>
        /// <param name="dbPath"></param>
        /// <returns></returns>
        private static Boolean connectAndInsertAccess(string strQuery, string dbPath)
        {
            try
            {


                OleDbConnection conn;
                // connect to DB
                try
                {
                    conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.10.0;Data Source=" + dbPath);
                    conn.Open();
                }
                catch
                {
                    try
                    {
                        conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dbPath);
                        conn.Open();
                    }
                    catch
                    {
                        try
                        {
                            conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.14.0;Data Source=" + dbPath);
                            conn.Open();
                        }
                        catch
                        {
                            conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.15.0;Data Source=" + dbPath);
                            conn.Open();
                        }
                    }
                }

                using (conn)
                {
                    try
                    {
                        OleDbDataAdapter oledbAdapter = new OleDbDataAdapter();
                        oledbAdapter.InsertCommand = new OleDbCommand(strQuery, conn);
                        oledbAdapter.InsertCommand.ExecuteNonQuery();
                        conn.Close();

                        //                   oleadapter.InsertCommand= 
                        //                    foreach (DataRow row in dtMain.Rows)
                        //                    {
                        //                        row.SetAdded();
                        //                       //                        strLNo = row["loan_number"].ToString();
                        //                        if ((rownum++) % 100 == 0)
                        //                            oleadapter.Update(dtMain);
                        //                    }
                        //                    oleadapter.Update(dtMain);
                    }
                    catch (Exception e)
                    {
                        //MessageBox.Show(strLNo + " : " + rownum + " : " + e.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        throw;
                        return false;
                    }
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }



        /// <summary>
        /// کوری از جداول اکسس
        /// </summary>
        /// <param name="strQuery"></param>
        /// <param name="dbPath"></param>
        /// <returns></returns>
        private static DataTable connectAndQueryAccess(string strQuery, string dbPath)
        {
            OleDbConnection conn;
            // connect to DB
            try
            {
                conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.10.0;Data Source=" + dbPath);
                conn.Open();
            }
            catch
            {
                try
                {
                    conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dbPath);
                    conn.Open();
                }
                catch
                {
                    try
                    {
                        conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.14.0;Data Source=" + dbPath);
                        conn.Open();
                    }
                    catch
                    {
                        conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.15.0;Data Source=" + dbPath);
                        conn.Open();
                    }
                }
            }
            using (conn)
            {
                OleDbDataAdapter oracladapter = new OleDbDataAdapter(strQuery, conn);
                new OleDbCommandBuilder(oracladapter);
                DataTable dtMain = new DataTable();
                oracladapter.Fill(dtMain);
                conn.Close();
                return dtMain;
            }
        }
    }
}
