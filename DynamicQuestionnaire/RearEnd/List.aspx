<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="DynamicQuestionnaire.RearEnd.List" %>

<%@ Register Src="~/ShareControls/Pager.ascx" TagPrefix="uc1" TagName="Pager" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>後台-問卷管理</title>
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
        <asp:HiddenField runat="server" ID="hdf" />
        <input type="hidden" id="msgmsg" class="msgmsg" runat="server" />
        <div class="row">
            <div class="hidari col-sm-3 col-md-3 col-lg-3">
                <a>
                    <asp:Literal runat="server" Text="問卷管理"></asp:Literal>
                </a><br />
                <a>
                    <asp:Literal runat="server" Text="常用問題管理"></asp:Literal>
                </a>
            </div>
            <div class="migi col-sm-8 col-md-8 col-lg-8">
                <div>
                    <asp:Literal runat="server" ID="ltl1" Text="問卷標題"></asp:Literal>
                    <asp:TextBox runat="server" ID="txbT"></asp:TextBox><br />
                    <asp:Literal runat="server" ID="ltl2" Text="開始／結束"></asp:Literal>
                    <asp:TextBox runat="server" ID="txbS"></asp:TextBox><asp:TextBox runat="server" ID="txbE"></asp:TextBox>
                    <asp:Button runat="server" ID="btnS" Text="搜尋" OnClick="btnS_Click" /><br />
                    <asp:Literal runat="server" ID="ltlmsg"></asp:Literal>
                </div>
                <div>
                    <asp:ImageButton class="imgbtn" ID="btnDelete" runat="server" ImageUrl="../CSS/1.png" Height="50px" Width="50px" OnClick="btnDelete_Click" />
                    <asp:ImageButton class="imgbtn" ID="btnCreate" runat="server" ImageUrl="../CSS/2.png" Height="50px" Width="50px" OnClick="btnCreate_Click" />
                    <br />

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
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="ckb" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="#">
                                <ItemTemplate>
                                    <asp:Label ID="ltlZyunban" runat="server" Text='<%# Eval("Zyunban") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="問卷">
                                <ItemTemplate>
                                    <a runat="server" href='<%# "Detail.aspx?ID="+Eval("QuestionID") %>' title='<%# "前往："+Eval("QName")+"問卷管理" %>'>
                                        <asp:Literal runat="server" ID="ltlQuestion" Text='<%# Eval("QName") %>'></asp:Literal>
                                    </a>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="狀態">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="ltlState" Text='<%# Eval("State").ToString() == "1" ? "開啟" : "已關閉" %>'></asp:Literal>
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
                                    <a runat="server" href='<%# "Detail.aspx?ID="+Eval("QuestionID") %>' title='<%# "前往："+Eval("QName")+"問卷統計" %>'>
                                        <asp:Literal runat="server" ID="ltlsougou" Text="前往"></asp:Literal>
                                    </a>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:Literal runat="server" ID="ltldlmsg"></asp:Literal><br />
                    <asp:Button runat="server" ID="btndlcancer" CssClass="btn btn-dark" Visible="false" Text="取消" OnClick="btndlcancer_Click" />
                    <asp:Button runat="server" ID="btndl" CssClass="btn btn-dark" Visible="false" Text="確定刪除" OnClick="btndl_Click" />
                </div>
                <uc1:Pager runat="server" ID="Pager" />
            </div>
            <div class="modal fade" id="hondounidelete" tabindex="-1" data-bs-backdrop="static" data-bs-keyboard="false" aria-labelledby="staticBackdropLabel" aria-modal="true" role="dialog">
                <div class="modal-dialog" style="background-color: black;">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title">
                                <asp:Literal runat="server" ID="ltlModalTitle">刪除</asp:Literal>
                            </h5>
                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                        </div>
                        <div class="modal-body">
                            <p>
                                <asp:Literal runat="server" ID="ltlDeleteModalContent"></asp:Literal>
                            </p>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-dark" data-bs-dismiss="modal">關閉</button>
                            <asp:Button runat="server" ID="kakuzitudelete" CssClass="btn btn-dark" Text="刪除" OnClick="kakuzitudelete_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
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
