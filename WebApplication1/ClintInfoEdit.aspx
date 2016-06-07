<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ClintInfoEdit.aspx.cs" Inherits="WebApplication1.ClintInfoEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="failureNotification" align="center">
        <asp:Label ID="lblnote" runat="server"></asp:Label>
        <asp:ValidationSummary ID="LoginUserValidationSummary" runat="server" CssClass="failureNotification"
            ValidationGroup="LoginUserValidationGroup" />
    </div>
    <table width="100%">
        <tr>
            <td width="50%">
                نام:
                <asp:Label ID="lblFName" runat="server" Text=""></asp:Label>
            </td>
            <td width="50%">
                نام خانوادگی:
                <asp:Label ID="lblLName" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td width="50%">
                تعداد اعضای خانواده:
                <asp:Label ID="lblNum" runat="server" Text=""></asp:Label>
            </td>
            <td width="50%">
            </td>
        </tr>
        <tr>
            <td width="50%">
                شماره تلفن:
                <asp:TextBox ID="txtTel" runat="server"></asp:TextBox>
            </td>
            <td width="50%">
                شماره موبایل:
                <asp:TextBox ID="txtMobile" runat="server" Text=""></asp:TextBox>
                <asp:RequiredFieldValidator ID="AmountRequired" runat="server" ControlToValidate="txtMobile"
                    CssClass="failureNotification" ErrorMessage="موبایل اجباری می باشد" ToolTip="موبایل اجباری می باشد"
                    ValidationGroup="LoginUserValidationGroup">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td width="100%" colspan="2" align="center" valign="middle">
                آدرس:
                <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine" Width="860px"></asp:TextBox>
            </td>
        </tr>
    </table>
    <div align="center">
        <asp:Button ID="btnok" runat="server" Text="تایید" ValidationGroup="LoginUserValidationGroup"
            OnClick="btnok_Click" />
        <asp:Button ID="btnCancel" runat="server" Text="انصراف" OnClick="btnCancel_Click" />
    </div>
</asp:Content>
