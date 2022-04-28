<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConfirmPage.aspx.cs" Inherits="DynamicQuestionnaire.FrontEnd.ConfirmPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>前台-確認頁</title>
    <link href="../CSS/bootstrap.css" rel="stylesheet" />
    <script src="../JS/bootstrap.js"></script>
    <script src="../JS/jquery.js"></script>
    <style>
        * {
            background-color: #181717;
            color: rgba(255, 248, 240, 0.93);
        }

        .Question span {
            display: block;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div runat="server" id="all" class="row col-sm-11 col-md-11 col-lg-11">
            <div class="State col-sm-3 col-md-3 col-lg-3">
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
                <asp:Literal runat="server" Text="姓名："></asp:Literal><asp:Literal runat="server" ID="ltlName"></asp:Literal><br />
                <asp:Literal runat="server" Text="手機："></asp:Literal><asp:Literal runat="server" ID="ltlPhone"></asp:Literal><br />
                <asp:Literal runat="server" Text="Email："></asp:Literal><asp:Literal runat="server" ID="ltlEmail"></asp:Literal><br />
                <asp:Literal runat="server" Text="年齡："></asp:Literal><asp:Literal runat="server" ID="ltlAge"></asp:Literal><br />
            </div>
            <div class="Question col-sm-8 col-md-8 col-lg-8">
                <placeholder runat="server" id="plh"></placeholder>
            </div>
            <button type="button" class="btn-dark" data-bs-toggle="modal" data-bs-target="#hondouniedit">修改</button>
            <button type="button" class="btn-dark" data-bs-toggle="modal" data-bs-target="#hondounigogo">送出</button>
        </div>
        <div class="modal fade" id="hondouniedit" tabindex="-1" data-bs-backdrop="static" data-bs-keyboard="false" aria-labelledby="staticBackdropLabel" aria-modal="true" role="dialog">
            <div class="modal-dialog" style="background-color: black;">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">
                            <asp:Literal runat="server" ID="ltlModalTitle">你</asp:Literal>
                        </h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <p>
                            <asp:Literal runat="server" ID="ltlModalContent">確定要修改？</asp:Literal>
                        </p>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn-dark" data-bs-dismiss="modal">關閉</button>
                        <asp:Button runat="server" ID="edit" CssClass="btn-dark" Text="修改" OnClick="edit_Click" />
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade" id="hondounigogo" tabindex="-1" data-bs-backdrop="static" data-bs-keyboard="false" aria-labelledby="staticBackdropLabel" aria-modal="true" role="dialog">
            <div class="modal-dialog" style="background-color: black;">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">
                            <asp:Literal runat="server" ID="Literal1">你</asp:Literal>
                        </h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <p>
                            <asp:Literal runat="server" ID="Literal2">確定要送出？</asp:Literal>
                        </p>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn-dark" data-bs-dismiss="modal">關閉</button>
                        <asp:Button runat="server" ID="gogo" CssClass="btn-dark" Text="送出" OnClick="gogo_Click" />
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
