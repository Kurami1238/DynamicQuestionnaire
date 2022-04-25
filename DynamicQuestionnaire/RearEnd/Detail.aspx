<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="DynamicQuestionnaire.RearEnd.Detail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
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
            <div class="hidari col-sm-3 col-md-3 col-lg-3">
            </div>
            <div class="migi col-sm-8 col-md-8 col-lg-8">
                    <input type="hidden" id="changetab" class="changetab" runat="server"/>
                <nav>
                    <div class="nav nav-tabs" id="nav-tab" role="tablist">
                        <button class="nav-link active" id="nav-question-tab" data-bs-toggle="tab" data-bs-target="#nav-question" type="button" role="tab" aria-controls="nav-question" aria-selected="true">問卷</button>
                        <button class="nav-link" id="nav-mondai-tab" data-bs-toggle="tab" data-bs-target="#nav-mondai" type="button" role="tab" aria-controls="nav-mondai" aria-selected="false">問題</button>
                        <button class="nav-link" id="nav-siryou-tab" data-bs-toggle="tab" data-bs-target="#nav-siryou" type="button" role="tab" aria-controls="nav-siryou" aria-selected="false">填寫資料</button>
                        <button class="nav-link" id="nav-static-tab" data-bs-toggle="tab" data-bs-target="#nav-static" type="button" role="tab" aria-controls="nav-static" aria-selected="false">統計</button>
                    </div>
                </nav>
                <div class="tab-content" id="nav-tabContent">
                    <div class="tab-pane fade show active" id="nav-question" role="tabpanel" aria-labelledby="nav-question-tab">
                        <asp:Literal runat="server" Text="問卷名稱"></asp:Literal><asp:TextBox runat="server" ID="txbQname"></asp:TextBox><br />
                        <asp:Literal runat="server" Text="描述內容"></asp:Literal><asp:TextBox runat="server" ID="txbQsetume" TextMode="MultiLine"></asp:TextBox><br />
                        <asp:Literal runat="server" Text="開始時間"></asp:Literal><asp:TextBox runat="server" ID="txbS" TextMode="DateTime" ></asp:TextBox><br />
                        <asp:Literal runat="server" Text="結束時間"></asp:Literal><asp:TextBox runat="server" ID="txbE" TextMode="DateTime"></asp:TextBox><br />
                        <asp:CheckBox runat="server" ID="ckbState" Checked="true" Text="已啟用" /><br />
                        <asp:Literal runat="server" ID="ltlquestionmsg"></asp:Literal><br />
                        <asp:Button runat="server" ID="cancer" Text="取消" OnClick="cancer_Click" />
                        <asp:Button runat="server" ID="gogogo" Text="送出" OnClick="gogogo_Click" />
                    </div>
                    <div class="tab-pane fade" id="nav-mondai" role="tabpanel" aria-labelledby="nav-mondai-tab">
                        <asp:Literal runat="server" Text="種類"></asp:Literal>
                        <asp:DropDownList runat="server" ID="ddlTitle">
                            <asp:ListItem Text="自訂問題" Value="1"></asp:ListItem>
                        </asp:DropDownList><br />
                        <asp:Literal runat="server" Text="問題"></asp:Literal><asp:TextBox runat="server" ID="txbquestion"></asp:TextBox>
                        <asp:DropDownList runat="server" ID="ddlType">
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
                        <asp:Button runat="server" ID="btnCreateMondai" Text="加入" OnClick="btnCreateMondai_Click" /><br />
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
                                <asp:TemplateField HeaderText="#">
                                    <ItemTemplate>
                                        <asp:Label ID="ltlZyunban" runat="server" Text='<%# Eval("Zyunban") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="問題">
                                    <ItemTemplate>
                                            <asp:Literal runat="server" ID="ltlTitle" Text='<%# Eval("Title") %>'></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="種類">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="ltlType" Text='<%# Eval("Type").ToString() == "1" ? "單選方塊" : Eval("Type").ToString() == "2" ? "多選方塊" : Eval("Type").ToString() == "3" ? "文字" : Eval("Type").ToString() == "4" ? "數字" : Eval("Type").ToString() == "5" ? "Email" : "日期" %>'></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="必填">
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="ckbZettai" Checked='<%# Eval("Zettai").ToString() == "1" ? true : false %>'/>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Button runat="server" ID="btnEdit" Text="編輯" CommandName="btnEdit" CommandArgument='<%# Eval("NaiyoListID") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:Literal runat="server" ID="ltlmondaimsg" ></asp:Literal>
                        <asp:Button runat="server" ID="btnCancerMondai" Text="取消" OnClick="btnCancerMondai_Click" />
                        <asp:Button runat="server" ID="btnMondaigogogo" Text="送出" OnClick="btnMondaigogogo_Click" />
                    </div>
                    <div class="tab-pane fade" id="nav-siryou" role="tabpanel" aria-labelledby="nav-siryou-tab">
                    </div>
                    <div class="tab-pane fade" id="nav-static" role="tabpanel" aria-labelledby="nav-static-tab">
                    </div>
                </div>

            </div>
        </div>
    </form>
    <script>
        $(document).ready(function () {
            var name = $('.changetab').val;
            console.log(name);
            if (name != undefined && name != null && name != "") {
                var triggerTabList = [].slice.call(document.querySelectorAll(`#${name} a`))
                triggerTabList.forEach(function (triggerEl) {
                    var tabTrigger = new bootstrap.Tab(triggerEl)
                    triggerEl.addEventListener('click', function (event) {
                        event.preventDefault()
                        tabTrigger.show()
                    })
                })
                //var triggerEl = document.querySelector(`#${name} a[href="#${name}"]`);
                //bootstrap.Tab.getInstance(triggerEl).show()
            };
        });
        //var name = $('.changetab').val;
       /* var tabName = $("[id*=TabName]").val() != "" ? $("[id=TabName]").val() : "nav-mondai";*/
        //$('#Tabs a[href="#' + name + '"]').tab('show');
        //if ($(".changetab").val() == "" || tabName == "nav-mondai") {
        //    $("#nav-mondai").addClass('active').addClass('show');
        //}

        //$("#Tabs a").click(function () {
        //    $(".changetab").val($(this).attr("href").replace("#", ""));
        //});
        //$('.changetab').change(function () {
        //    var name = $('.changetab').val;
        //    console.log(name);
        //    var triggerEl = document.querySelector(`#${name} a[href="#${name}"]`);
        //    bootstrap.Tab.getInstance(triggerEl).show()
        //});
    </script>
</body>
</html>
