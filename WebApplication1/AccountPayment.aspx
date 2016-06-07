<%@ Page Title="واریز به حساب" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="AccountPayment.aspx.cs" Inherits="WebApplication1.WebForm1" %>

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
            width: 384px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="failureNotification" align="center">
        <asp:Label ID="lblnote" runat="server"></asp:Label>
        <asp:ValidationSummary ID="LoginUserValidationSummary" runat="server" CssClass="failureNotification"
            ValidationGroup="LoginUserValidationGroup" />
            <asp:ValidationSummary ID="_PayDateValidationSummary" runat="server" CssClass="failureNotification"
            ValidationGroup="_PayDateValidationGroup" />
    </div>
    <div>
        <asp:Label ID="Label1" runat="server" Text="شماره مشتری: "></asp:Label>
        <asp:TextBox ID="txtClientNum" runat="server"></asp:TextBox>
        <asp:Button ID="btnSearch" runat="server" Text="جستجو" OnClick="btnSearch_Click" />
        <asp:Panel ID="pnlClientDetail" GroupingText="اطلاعات مشتری" runat="server">
            <table width="100%">
                <tr>
                    <td width="50%">
                        <asp:Label ID="Label2" runat="server" Text="نام: "></asp:Label>
                    </td>
                    <td width="50%">
                        <asp:Label ID="Label3" runat="server" Text="  نام خانوادگی: "></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td width="50%">
                        <asp:Label ID="Label4" runat="server" Text="مانده حساب:  "></asp:Label>
                    </td>
                    <td width="50%">
                        <asp:Label ID="Label5" runat="server" Text="   "></asp:Label>
                    </td>
                </tr>
                 <tr>
                    <td width="50%">
                        <asp:Label ID="Label11" runat="server" Text=""></asp:Label>
                    </td>
                    <td width="50%">
                        <asp:Label ID="Label12" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                 <tr>
                    <td width="50%">
                        <asp:Label ID="Label13" runat="server" Text=""></asp:Label>
                    </td>
                    <td width="50%">
                        <asp:Label ID="Label14" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    <table width="100%">
        <tr>
            <td width="50%">
                <asp:Panel ID="pnlPayment" CssClass="login" GroupingText="اطلاعات واریز به حساب"
                    runat="server">
                    <%--                    <asp:Label ID="lblCurentDate" runat="server"></asp:Label>--%>
                    <rhp:DatePicker ID="_grantDate" runat="server" LabelText="تاریخ واریز / پرداخت:"
                        BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" ShowWeekNumbers="True"
                        ShowOthers="True" OnClose="onClose" OnUpdate="onUpdate" DatePersian=""></rhp:DatePicker>
                    <asp:RequiredFieldValidator ID="_grantDateRequired" runat="server" ControlToValidate="_grantDate"
                        CssClass="failureNotification" ErrorMessage="تاریخ واریز / پرداخت اجباری می باشد"
                        ToolTip="Grant Date is required." ValidationGroup="LoginUserValidationGroup">*</asp:RequiredFieldValidator>
                    <br />
                    <asp:CheckBox ID="chbPayment" Text="واریز" Checked="True" runat="server" /><br />
                    <asp:Label ID="lblAmount" runat="server" Text="مبلغ: "></asp:Label>
                    <asp:TextBox ID="txtAmount" TextMode="SingleLine" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="AmountRequired" runat="server" ControlToValidate="txtAmount"
                        CssClass="failureNotification" ErrorMessage="مبلغ اجباری می باشد" ToolTip="Amount is required."
                        ValidationGroup="LoginUserValidationGroup">*</asp:RequiredFieldValidator>
                    <asp:Label ID="Label6" runat="server" Text=" ريال "></asp:Label><br />
                    <asp:Label ID="Label7" runat="server" Text="توضیحات: "></asp:Label>
                    <asp:TextBox ID="txtDesc" runat="server" Width="100%"></asp:TextBox>
                    <div align="center">
                        <asp:HiddenField ID="hfAccountID" runat="server" />
                        <asp:HiddenField ID="hfBalanceAmount" runat="server" />
                        <asp:HiddenField ID="hfClientName" runat="server" />
                        <asp:Button ID="btnok" runat="server" Text="واریز به حساب" ValidationGroup="LoginUserValidationGroup"
                            OnClick="btnok_Click" />
                        <asp:Button ID="btnCancel" runat="server" Text="انصراف" OnClick="btnCancel_Click" />
                    </div>
                </asp:Panel>
            </td>
            <td width="50%">
                <asp:Panel ID="loanPanel" GroupingText="اطلاعات واریز اقساط" runat="server">
                    <rhp:DatePicker ID="_PayDate" runat="server" LabelText=" تاریخ پرداخت قسط:" BorderColor="#CCCCCC"
                        BorderStyle="Solid" BorderWidth="1px" ShowWeekNumbers="True" ShowOthers="True"
                        OnClose="onClose" OnUpdate="onUpdate" DatePersian=""></rhp:DatePicker>
                    <asp:RequiredFieldValidator ID="_PayDateRequiredFieldValidator" runat="server" ControlToValidate="_PayDate"
                        CssClass="failureNotification" ErrorMessage="تاریخ پرداخت قسط اجباری می باشد"
                        ToolTip="PayMent Date is required." ValidationGroup="_PayDateValidationGroup">*</asp:RequiredFieldValidator>
                    <br />
                    <asp:Label ID="Label15" runat="server" Text="تعداد اقساط پرداخت نشده: "></asp:Label>
                    <asp:Label ID="Label9" runat="server" Text=""></asp:Label><br />
                    <asp:Label ID="Label8" runat="server" Text="تعداد قسط پرداختی : "></asp:Label>
                    <asp:TextBox ID="_txtInstallmentNumber" TextMode="SingleLine" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="installmentAmountValidator" runat="server" ControlToValidate="_txtInstallmentNumber"
                        CssClass="failureNotification" ErrorMessage="تعداد قسط پرداختی اجباری می باشد"
                        ToolTip="Grant Date is required." ValidationGroup="_PayDateValidationGroup">*</asp:RequiredFieldValidator>
                    <br />
                    <asp:Button ID="clcAmountPay" runat="server" Text="محاسبه مبلغ قابل پرداخت" OnClick="clcAmountPay_OnClick" ValidationGroup="_PayDateValidationGroup"/><br />
                    <asp:Label ID="Label10" runat="server" Text="مبلغ قابل پرداخت : "></asp:Label>
                    <div align="center">
                        <asp:HiddenField ID="hfInstallmentAmount" runat="server" />
                        <asp:HiddenField ID="hfLoanId" runat="server" />
                        <asp:Button ID="_payInstallment" runat="server" Text="پرداخت قسط" ValidationGroup="_PayDateValidationGroup"
                            OnClick="_payInstallment_OnClick" />
                        <asp:Button ID="Button2" runat="server" Text="انصراف" OnClick="btnCancel_Click" />
                    </div>
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Content>
