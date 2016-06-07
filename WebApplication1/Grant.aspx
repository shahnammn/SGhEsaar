<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Grant.aspx.cs" Inherits="WebApplication1.Grant" Culture="auto" meta:resourcekey="PageResource1"
    UICulture="auto" %>

<%@ Register TagPrefix="rhp" Namespace="Heidarpour.WebControlUI" Assembly="Heidarpour.WebControlUI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="Scripts/jquery-1.4.1-vsdoc.js" type="text/javascript"></script>
    <script type="text/javascript">

        function onSelect(calendar, date) {
            // Beware that this function is called even if the end-user only
            // changed the month/year. In order to determine if a date was
            // clicked you can use the dateClicked property of the calendar:
            if (calendar.dateClicked) {
                var msg =
                        "<br/>Persian: Year: " + calendar.date.getJalaliFullYear() +
                        ", Month: " + (calendar.date.getJalaliMonth() + 1) +
                        ", Day: " + calendar.date.getJalaliDate() +
                        "<br/>Gregorian: Year: " + calendar.date.getFullYear() +
                        ", Month: " + calendar.date.getMonth() +
                        ", Day: " + calendar.date.getDate();

                $("#<%= _grantDate.ClientID %>").val(date);
                logEvent("onSelect Event: <br> Selected Date: " + date + msg);
                calendar.hide();
                //calendar.callCloseHandler(); // this calls "onClose"
            }
        };

        function onUpdate(calendar) {
            var msg =
                    "<br/>Persian: Year: " + calendar.date.getJalaliFullYear() +
                    ", Month: " + (calendar.date.getJalaliMonth() + 1) +
                    ", Day: " + calendar.date.getJalaliDate() +
                    "<br/>Gregorian: Year: " + calendar.date.getFullYear() +
                    ", Month: " + calendar.date.getMonth() +
                    ", Day: " + calendar.date.getDate();

            logEvent("onUpdate Event: <br> Selected Date: " + calendar.date.print('%Y/%m/%d', 'jalali') + msg);
        };

        function onClose(calendar) {
            logEvent("onClose Event");
            calendar.hide();
        };

        function logEvent(str) {
            $("#log").append("<li>" + str + "</li>");
        }
    </script>
    <style type="text/css">
        .style1
        {
            font-size: 8pt;
            color: red;
            direction: ltr;
            text-align: left;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="failureNotification" align="center">
        <asp:Label ID="lblnote" runat="server" meta:resourcekey="lblnoteResource1"></asp:Label>
        <asp:ValidationSummary ID="LoginUserValidationSummary" runat="server" CssClass="failureNotification"
            ValidationGroup="LoginUserValidationGroup" meta:resourcekey="LoginUserValidationSummaryResource1" />
    </div>
    <div>
        <asp:Label ID="Label1" runat="server" Text="شماره مشتری: " meta:resourcekey="Label1Resource1"></asp:Label>
        <asp:TextBox ID="txtClientNum" runat="server" meta:resourcekey="txtClientNumResource1"></asp:TextBox>
        <asp:Button ID="btnSearch" runat="server" Text="جستجو" OnClick="btnSearch_Click"
            meta:resourcekey="btnSearchResource1" />
        <asp:Panel ID="pnlClientDetail" GroupingText="اطلاعات مشتری" runat="server" Visible="False"
            meta:resourcekey="pnlClientDetailResource1">
            <table width="100%">
                <tr>
                    <td width="50%">
                        <asp:Label ID="Label2" runat="server" Text="نام: " meta:resourcekey="Label2Resource1"></asp:Label>
                    </td>
                    <td width="50%">
                        <asp:Label ID="Label3" runat="server" Text="  نام خانوادگی: " meta:resourcekey="Label3Resource1"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td width="50%">
                        <asp:Label ID="Label4" runat="server" Text="مانده حساب:  " meta:resourcekey="Label4Resource1"></asp:Label>
                    </td>
                    <td width="50%">
                        <asp:Label ID="Label5" runat="server" Text="   " meta:resourcekey="Label5Resource1"></asp:Label>
                        <asp:HiddenField ID="hfAccountID" runat="server" />
                        <asp:HiddenField ID="hfBalanceAmount" runat="server" />
                        <asp:HiddenField ID="hfClientName" runat="server" />
                        <asp:HiddenField ID="hfClientid" runat="server" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    <div>
        <asp:Panel ID="plnLoan" GroupingText="اطلاعات اعطا تسهیلات" runat="server" Visible="False"
            meta:resourcekey="Panel1Resource1">
            <table width="100%">
                <tr>
                    <td width="50%">
                        <rhp:DatePicker ID="_grantDate" runat="server" LabelText="تاریخ اعطا:" BorderColor="#CCCCCC"
                            BorderStyle="Solid" BorderWidth="1px" ShowWeekNumbers="True" ShowOthers="True"
                            OnClose="onClose" OnUpdate="onUpdate" DatePersian="" meta:resourcekey="_grantDateResource1"></rhp:DatePicker>
                        <asp:RequiredFieldValidator ID="_grantDateRequired" runat="server" ControlToValidate="_grantDate"
                            CssClass="failureNotification" ErrorMessage="تاریخ اعطا اجباری می باشد" ToolTip="Grant Date is required."
                            ValidationGroup="LoginUserValidationGroup">*</asp:RequiredFieldValidator>
                    </td>
                    <td width="50%">
                        اولویت وام:
                        <asp:Label ID="Label7" runat="server" meta:resourcekey="Label7Resource1"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td width="50%">
                        <asp:Label ID="Label8" runat="server" Text="مبلغ تسهیلات:" meta:resourcekey="Label8Resource1"></asp:Label>
                        <asp:TextBox ID="_txtAmountLoan" runat="server" meta:resourcekey="TextBox1Resource1"></asp:TextBox>ريال
                        <asp:RequiredFieldValidator ID="_amountRequiredFieldValidator" runat="server" ControlToValidate="_txtAmountLoan"
                            CssClass="failureNotification" ErrorMessage="مبلغ تسهیلات اجباری می باشد" ToolTip="َئخعدف »خشد is required."
                            ValidationGroup="LoginUserValidationGroup">*</asp:RequiredFieldValidator>
                        <span class="style1">مانده حساب * 5/2 </span>
                    </td>
                    <td width="50%">
                        مدت تسهیلات:
                        <asp:DropDownList ID="_ddlTimeLoan" runat="server" meta:resourcekey="_ddlTimeLoanResource1">
                            <asp:ListItem Selected="True" Value="20" meta:resourcekey="ListItemResource1">20 ماه</asp:ListItem>
                            <asp:ListItem Value="25" meta:resourcekey="ListItemResource2">25 ماه</asp:ListItem>
                            <asp:ListItem Value="30" meta:resourcekey="ListItemResource3">30 ماه</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="_btnCalc" runat="server" Text="محاسبه" OnClick="_btnCalc_OnClick"
                            ValidationGroup="LoginUserValidationGroup" meta:resourcekey="_btnCalcResource1" />
                    </td>
                </tr>
                <tr>
                    <td width="50%">
                        مبلغ قابل پرداخت پس از کسر کارمزد:
                        <asp:Label ID="_lblGrantAmount" runat="server" meta:resourcekey="Label6Resource1"></asp:Label>
                        &nbsp;ريال
                    </td>
                    <td width="50%">
                        مبلغ کارمزد:
                        <asp:Label ID="_lblcomm" runat="server" meta:resourcekey="Label6Resource1"></asp:Label>
                        &nbsp;ريال
                    
                    </td>
                </tr>
                <tr>
                    <td width="50%">
                        مبلغ هر قسط:
                        <asp:Label ID="_lblinstallmentAmount" runat="server" meta:resourcekey="Label6Resource1"></asp:Label>
                        &nbsp;ريال
                    </td>
                    <td width="50%">
                        مبلغ اولین قسط:
                        <asp:Label ID="_lblFirstInstallmentAmount" runat="server" meta:resourcekey="Label6Resource1"></asp:Label>
                        &nbsp;ريال
                    </td>
                </tr>
                <tr>
                    <td width="50%">
                        تاریخ اولین قسط تسهیلات:
                        <asp:Label ID="lblinstallmentFirstDate" runat="server" meta:resourcekey="Label9Resource1"></asp:Label>
                    </td>
                    <td width="50%">
                        تاریخ خاتمه تسهیلات:
                        <asp:Label ID="_lblEndDate" runat="server" meta:resourcekey="Label9Resource1"></asp:Label>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    <div align="center">
        <asp:Button ID="btnok" runat="server" Enabled="False" Text="تایید" ValidationGroup="LoginUserValidationGroup"
            OnClick="btnok_Click" meta:resourcekey="btnokResource1" />
        <asp:Button ID="btnCancel" runat="server" Text="انصراف" OnClick="btnCancel_Click"
            meta:resourcekey="btnCancelResource1" />
    </div>
</asp:Content>
