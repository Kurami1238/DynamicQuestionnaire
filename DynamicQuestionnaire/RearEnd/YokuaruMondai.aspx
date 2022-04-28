<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="YokuaruMondai.aspx.cs" Inherits="DynamicQuestionnaire.RearEnd.YokuaruMondai" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>常見問題管理</title>
    <link href="../CSS/bootstrap.css" rel="stylesheet" />
    <script src="../JS/bootstrap.js"></script>
    <script src="../JS/jquery.js"></script>
    <style>
        * {
            background-color: #181717;
            color: rgba(255, 248, 240, 0.93);
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="row">
             <a href="../Index.aspx">
                    <h2>DynamicQuestionnaire</h2>
                </a>
            <div class="hidari col-sm-3 col-md-3 col-lg-3">
                <a href="List.aspx">
                    <asp:Literal runat="server" Text="問卷管理"></asp:Literal>
                </a>
                <br />
                <a href="YokuaruMondai.aspx">
                    <asp:Literal runat="server" Text="常用問題管理"></asp:Literal>
                </a>
            </div>
            <div class="migi col-sm-8 col-md-8 col-lg-8">
                <input type="hidden" id="msgmsg" class="msgmsg" runat="server" />
                <asp:Literal runat="server" Text="問題"></asp:Literal><asp:TextBox runat="server" ID="txbquestion"></asp:TextBox>
                <asp:DropDownList runat="server" ID="ddlType" OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                    <asp:ListItem Text="單選方塊" Value="1"></asp:ListItem>
                    <asp:ListItem Text="多選方塊" Value="2"></asp:ListItem>
                    <asp:ListItem Text="文字" Value="3"></asp:ListItem>
                    <asp:ListItem Text="數字" Value="4"></asp:ListItem>
                    <asp:ListItem Text="Email" Value="5"></asp:ListItem>
                    <asp:ListItem Text="日期" Value="6"></asp:ListItem>
                </asp:DropDownList>
                <asp:CheckBox runat="server" ID="ckbhituyou" Checked="true" /><asp:Literal runat="server" Text="必填"></asp:Literal><br />
                <asp:PlaceHolder runat="server" ID="ph" Visible='<%# this.ddlType.SelectedValue == "1" ? true : this.ddlType.SelectedValue == "2" ? true : false %>'>
                    <asp:Literal runat="server" Text="回答"></asp:Literal><asp:TextBox runat="server" ID="txbNaiyo"></asp:TextBox><asp:Literal runat="server" Text="多個答案以;分隔"></asp:Literal>
                </asp:PlaceHolder>
                <asp:Button runat="server" ID="btnCreateMondai" Text="加入" OnClick="btnCreateMondai_Click" CssClass="btn-dark" /><br />
                <asp:ImageButton class="imgbtn" ID="btnDeleteMondai" runat="server" ImageUrl="../CSS/1.png" Height="30px" Width="30px" OnClick="btnDeleteMondai_Click" /><br />
                <asp:GridView runat="server" ID="gv" CellPadding="4" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" OnRowCommand="gv_RowCommand">
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
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="ckb" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="問題">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblTitle" Text='<%# Eval("Title") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="種類">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="ltlType" Text='<%# Eval("Type").ToString() == "1" ? "單選方塊" : Eval("Type").ToString() == "2" ? "多選方塊" : Eval("Type").ToString() == "3" ? "文字" : Eval("Type").ToString() == "4" ? "數字" : Eval("Type").ToString() == "5" ? "Email" : "日期" %>'></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="必填">
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="ckbZettai" Checked='<%# Eval("Zettai").ToString() == "1" ? true : false %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button runat="server" ID="btnEdit" Text="編輯" CommandName="btnEdit" CommandArgument='<%# Eval("MondaiID") %>' CssClass="btn-dark" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:Literal runat="server" ID="ltlmondaimsg"></asp:Literal>
                <asp:Button runat="server" ID="btnCancerMondai" Text="取消" OnClick="btnCancerMondai_Click" CssClass="btn-dark" />
                <asp:Button runat="server" ID="btnMondaigogogo" Text="送出" OnClick="btnMondaigogogo_Click" CssClass="btn-dark" />
            </div>
        </div>
    </form>
</body>
</html>
