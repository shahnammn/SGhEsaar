<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Main.aspx.cs" Inherits="WebApplication1.Main" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3>
        <asp:Label ID="lblName" runat="server" Text="Label"></asp:Label>
    </h3>
    <div style="text-align: center;" >
        <asp:Label ID="lblNote" runat="server" Visible="False" Font-Size="Large"></asp:Label></div>
    <table width="100%">
        <tr>
            <td width="50%">
                <asp:Panel ID="Panel1" CssClass="menu" runat="server" GroupingText="مدیریت کاربر ">
                    <div>
                        <ul>
                            <li>
                                <asp:LinkButton ID="LinkButton2" PostBackUrl="Account/ChangePassword.aspx" runat="server">تغییر رمز ورود</asp:LinkButton></li>
                            <li>
                                <asp:LinkButton ID="LinkButton3" PostBackUrl="ClintInfoEdit.aspx" runat="server">تغییر مشخصات کاربر</asp:LinkButton></li>
                        </ul>
                    </div>
                </asp:Panel>
            </td>
            <td width="50%">
                <asp:Panel ID="PnlAdmin" CssClass="menu" runat="server" GroupingText="مدیریت سیستم صندوق">
                    <div>
                        <ul>
                            <li><asp:LinkButton ID="LinkButton1" PostBackUrl="AccountPayment.aspx" runat="server">واریز مبلغ به حساب</asp:LinkButton></li>
                            <li><asp:LinkButton ID="LinkButton4" PostBackUrl="Grant.aspx" runat="server">اعطای تسهیلات</asp:LinkButton></li>
                            <li><asp:Button ID="btnOverNighte" runat="server" Text="اجرای شبانه" OnClick="btnOverNighte_OnClick"/></li>
                        </ul>
                    </div>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td width="50%">
                <asp:Panel ID="Panel2" CssClass="menu" runat="server" GroupingText="حساب مشتری ">
                    <div>
                        <asp:Label ID="lblBalance" runat="server" Text=" مانده حساب: "></asp:Label><br />
                        <asp:Label ID="lblSignAmount" runat="server" Text=" مبلغ ماهانه عضویت در صندوق : "></asp:Label><br />
                        <asp:Panel ID="Panel5" CssClass="menu" runat="server" ScrollBars="Vertical" Style="height: 300px">
                            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                DataSourceID="AccessDataSource1" ForeColor="#333333" GridLines="None" Width="100%"
                                ScrollBars="Auto" PageSize="5">
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <Columns>
                                    <asp:BoundField DataField="date" HeaderText="تاریخ" SortExpression="date" />
                                    <asp:BoundField DataField="Amount" HeaderText="مبلغ" SortExpression="Amount" />
                                    <asp:BoundField DataField="Description" HeaderText="توضیحات" SortExpression="Description" />
                                </Columns>
                                <EditRowStyle BackColor="#999999" />
                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                            </asp:GridView>
                        </asp:Panel>
                        <asp:AccessDataSource ID="AccessDataSource1" runat="server" DataFile="~/db/Database11.accdb">
                        </asp:AccessDataSource>
                    </div>
                </asp:Panel>
            </td>
            <td width="50%">
                <asp:Panel ID="Panel3" CssClass="menu" runat="server" GroupingText="تسهیلات مشتری">
                    <div>
                        <asp:Label ID="lblNoteLoan" runat="server" Text="" Visible="False"></asp:Label><br />
                        <asp:Label ID="lblLoanNumber" runat="server" Text="" ></asp:Label><br />
                        <asp:Label ID="lblGrantDate" runat="server" Text="" ></asp:Label><br />
                        <asp:Label ID="lblLoanAmount" runat="server" Text="" ></asp:Label><br />
                        
                        <asp:Panel ID="Panel4" CssClass="menu" runat="server" ScrollBars="Vertical" Style="height: 300px">
                            <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                Caption="جدول اقساط تسهیلات" DataSourceID="loanDS" ForeColor="#333333" GridLines="None"
                                Width="100%" ScrollBars="Auto" PageSize="5">
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <Columns>
                                    <asp:BoundField DataField="Installment_Number" HeaderText="شماره قسط" SortExpression="Installment_Number" />
                                    <asp:BoundField DataField="DUE_DATE" HeaderText="تاریخ سر رسید " SortExpression="DUE_DATE" />
                                    <asp:BoundField DataField="Installment_Amount" HeaderText="مبلغ " SortExpression="Installment_Amount" />
                                    <asp:BoundField DataField="Installment_Status" HeaderText="وضعیت قسط" SortExpression="Installment_Status" />
                                </Columns>
                                <EditRowStyle BackColor="#999999" />
                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                            </asp:GridView>
                            <asp:AccessDataSource ID="loanDS" runat="server" DataFile="~/db/Database11.accdb">
                            </asp:AccessDataSource>
                        </asp:Panel>
                        <div style="text-align: left">
                            <asp:LinkButton ID="LinkButton5" PostBackUrl="installmentDetail.aspx" runat="server">مشاهده جدول اقساط...</asp:LinkButton>
                        </div>
                    </div>
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Content>
