<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="DynamicQuestionnaire.FrontEnd.List" %>

<%@ Register Src="~/ShareControls/Pager.ascx" TagPrefix="uc1" TagName="Pager" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>前台-列表頁</title>
    <link href="../CSS/bootstrap.css" rel="stylesheet" />
    <script src="../JS/bootstrap.js"></script>
    <script src="../JS/jquery.min.js"></script>
    <style>
        * {
            background-color: #181717;
            color: rgba(255, 248, 240, 0.93);
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField runat="server" ID="hdf" />
        <input type="hidden" id="msgmsg" class="msgmsg" runat="server" />
        <div>
            <asp:Literal runat="server" ID="ltl1" Text="問卷標題"></asp:Literal>
            <asp:TextBox runat="server" ID="txbT"></asp:TextBox><br />
            <asp:Literal runat="server" ID="ltl2" Text="開始／結束"></asp:Literal>
            <asp:TextBox runat="server" ID="txbS"></asp:TextBox><asp:TextBox runat="server" ID="txbE"></asp:TextBox>
            <asp:Button runat="server" ID="btnS" Text="搜尋" OnClick="btnS_Click" /><br />
            <asp:Literal runat="server" ID="ltlmsg"></asp:Literal>
        </div>
        <div>
            <asp:GridView runat="server" ID="gv" CellPadding="4" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False">
                <AlternatingRowStyle BackColor="White" />
                <EditRowStyle BackColor="#7C6F57" />
                <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#E3EAEB" />
                <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#F8FAFA" />
                <SortedAscendingHeaderStyle BackColor="#246B61" />
                <SortedDescendingCellStyle BackColor="#D4DFE1" />
                <SortedDescendingHeaderStyle BackColor="#15524A" />
                <Columns>
                    <asp:TemplateField HeaderText="#">
                        <ItemTemplate>
                            <asp:Literal ID="ltlZyunban" runat="server" Text='<%# Eval("Zyunban") %>'></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="問卷">
                        <ItemTemplate>
                            <a runat="server" href='<%# "Form.aspx?ID="+Eval("QuestionID") %>' title='<%# "前往："+Eval("QName")+"問卷" %>'>
                                <asp:Literal runat="server" ID="ltlQuestion" Text='<%# Eval("QName") %>'></asp:Literal>
                            </a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="狀態">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="ltlState" Text='<%# DateTime.Compare((DateTime)Eval("DateEnd"),(DateTime.Now)) > 0 ? "投票中" : "已完結" %>'></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="開始時間">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="ltlds" Text='<%# Eval("DateStart") %>'></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="結束時間">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="ltlde" Text='<%# Eval("DateEnd") %>'></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="觀看統計">
                        <ItemTemplate>
                            <a runat="server">
                                <asp:Literal runat="server" ID="ltlsougou" Text="前往"></asp:Literal>
                            </a>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <uc1:Pager runat="server" ID="Pager" />
    </form>
    <script>
        $(document).ready(function () {
            var msg = $(".msgmsg").val();
            if (msg != undefined && msg != null && msg != "")
                alert(msg);
        });
    </script>
</body>
</html>
