<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Form.aspx.cs" Inherits="DynamicQuestionnaire.FrontEnd.Form" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>前台-內頁</title>
    <style>
        * {
            background-color: #181717;
            color: rgba(255, 248, 240, 0.93);
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div runat="server" id="all">
            <asp:Panel runat="server" ID="pnl">
                <div class="State">
                    <asp:Literal runat="server" ID="State"></asp:Literal><br />
                    <asp:Literal runat="server" ID="Date"></asp:Literal>
                </div>
                <div class="Qname">
                    <asp:Label runat="server" ID="lblQname"></asp:Label>
                </div>
                <div class="QSetume">
                    <asp:Label runat="server" ID="lblQSetume"></asp:Label>
                </div>
                <div class="Kiroku">
                    <asp:Literal runat="server" Text="姓名"></asp:Literal><asp:TextBox runat="server" ID="txbName"></asp:TextBox><br />
                    <asp:Literal runat="server" Text="手機"></asp:Literal><asp:TextBox runat="server" ID="txbPhone" TextMode="Phone"></asp:TextBox><br />
                    <asp:Literal runat="server" Text="Email"></asp:Literal><asp:TextBox runat="server" ID="txbEmail" TextMode="Email"></asp:TextBox><br />
                    <asp:Literal runat="server" Text="年齡"></asp:Literal><asp:TextBox runat="server" ID="txbAge" TextMode="Number"></asp:TextBox><br />
                </div>
                <div class="Question">
                    <placeholder runat="server" id="plh"></placeholder>
                </div>
                <asp:Label runat="server" ID="errormsg"></asp:Label>
                <asp:Literal runat="server" ID="Total"></asp:Literal>
            </asp:Panel>
        </div>
        <asp:Button runat="server" ID="cancer" Text="取消" OnClick="cancer_Click" />
        <asp:Button runat="server" ID="gogogo" Text="送出" OnClick="gogogo_Click" />
    </form>
</body>
</html>
