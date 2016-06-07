<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="installmentDetail.aspx.cs" Inherits="WebApplication1.installmentDetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
         <asp:Label ID="lblNoteLoan" runat="server" Text="" Visible="False"></asp:Label><br />
                        <asp:Panel ID="Panel4" CssClass="menu" runat="server">
                            <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                Caption="جدول اقساط تسهیلات" DataSourceID="loanDS" ForeColor="#333333" GridLines="None"
                                Width="100%" ScrollBars="Auto" PageSize="5" style="text-align: center">
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
                       
    

</asp:Content>
