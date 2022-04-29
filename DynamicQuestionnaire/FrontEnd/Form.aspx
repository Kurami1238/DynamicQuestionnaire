<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Form.aspx.cs" Inherits="DynamicQuestionnaire.FrontEnd.Form" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>前台-內頁</title>
    <link href="../CSS/bootstrap.css" rel="stylesheet" />
    <script src="../JS/bootstrap.js"></script>
    <script src="../JS/jquery.js"></script>
    <style>
        * {
            background-color: #181717;
            color: rgba(255, 248, 240, 0.93);
        }
        .Question textarea{
            display: block;
        }
        .Kiroku{
            padding: 4px 10px 2px 2px;
        }
        div{
            padding-bottom : 5px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div runat="server" id="all" class="row col-sm-11 col-md-11 col-lg-11">
            <asp:Panel runat="server" ID="pnl">
                <div class="State col-sm-10 col-md-10 col-lg-10">
                    <asp:Literal runat="server" ID="State"></asp:Literal><br />
                    <asp:Literal runat="server" ID="Date"></asp:Literal>
                </div>
                <div class="Qname col-sm-8 col-md-8 col-lg-8">
                    <asp:Label runat="server" ID="lblQname"></asp:Label>
                </div>
                <div class="QSetume col-sm-8 col-md-8 col-lg-8">
                    <asp:Label runat="server" ID="lblQSetume"></asp:Label>
                </div>
                <div class="Kiroku col-sm-8 col-md-8 col-lg-8">
                    <asp:Literal runat="server"  Text="姓名 *"></asp:Literal><asp:TextBox runat="server" ID="txbName" ></asp:TextBox><br />
                    <asp:Literal runat="server" Text="手機 *"></asp:Literal><asp:TextBox runat="server" ID="txbPhone" TextMode="Phone" ></asp:TextBox><br />      
                    <asp:Literal runat="server" Text="Email *"></asp:Literal><asp:TextBox runat="server" ID="txbEmail" TextMode="Email"></asp:TextBox><br />      
                    <asp:Literal runat="server" Text="年齡 *"></asp:Literal><asp:TextBox runat="server" ID="txbAge" TextMode="Number" ></asp:TextBox><br />
                </div>
                <div class="Question col-sm-8 col-md-8 col-lg-8" >
                    <placeholder runat="server" id="plh"></placeholder>
                </div>
                <asp:Label runat="server" ID="errormsg"></asp:Label>
                <asp:Literal runat="server" ID="Total"></asp:Literal>
            </asp:Panel>
        <asp:Button runat="server" ID="cancer" Text="取消" CssClass="btn-dark" OnClick="cancer_Click" />
        <asp:Button runat="server" ID="gogogo" Text="送出" CssClass="btn-dark" OnClick="gogogo_Click" />
        </div>

    </form>
</body>
</html>
