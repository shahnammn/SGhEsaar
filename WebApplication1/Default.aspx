<%@ Page Title="صندوق قرض الحسنه ایثار و تعاون" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication1._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2 align="center">
        به سایت صندوق قرض الحسنه ایثار و تعاون خوش آمدید</h2>
    <p>
        <%-- <h1 align="center" dir="rtl">
            جلسه صندوق مورخ 1395/03/07 ساعت 17 تا 19 در منزل آقای محمد شهرآیینی برگزار خواهد
            شد.</h1>--%>
    </p>
    <table width="100%">
        <tr>
            <td width="80%">
                <img src="images/367460_1smPuWot.jpg" />
            </td>
            <td width="20%">
                <div title="اوقات شرعی">
                    <font size="2">
                        <asp:Panel ID="Panel1" HorizontalAlign="Right" runat="server" GroupingText="اوقات شرعی">
                            ﻿<script type='text/javascript' src='Scripts/azan.js'></script>
                            <script language="javascript">
                                function pz() { }; init(); document.getElementById('cities').selectedIndex = 12; coord(); main();
                            </script>
                        </asp:Panel>
                    </font>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
