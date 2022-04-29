<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="DynamicQuestionnaire.RearEnd.Detail" %>

<%@ Register Src="~/ShareControls/Pager.ascx" TagPrefix="uc1" TagName="Pager" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <%--<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0/dist/js/bootstrap.bundle.min.js" integrity="sha384-p34f1UUtsS3wqzfto5wAAmdvj+osOnFyQFpp4Ua3gs/ZVWx6oOypYoCJhGGScy+8" crossorigin="anonymous"></script>--%>
    <link href="../CSS/bootstrap.css" rel="stylesheet" />
    <script src="../JS/bootstrap.js"></script>
    <script src="https://code.jquery.com/jquery-3.6.0.js" integrity="sha256-H+K7U5CnXl1h5ywQfKtSj8PCmoN9aaq30gDh27Xc0jk=" crossorigin="anonymous"></script>
    <%--<script src="../JS/jquery.min.js"></script>--%>
    <style>
        * {
            background-color: #181717;
            color: rgba(255, 248, 240, 0.93);
        }

        #nav-static span {
            display: block;
        }

    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="row">
            <div class="hidari col-sm-3 col-md-3 col-lg-3">
            </div>
            <div class="migi col-sm-8 col-md-8 col-lg-8">
                <input type="hidden" id="changetab" class="changetab" runat="server" />
                <%--<nav>
                    <div class="nav nav-tabs" id="nav-tab" role="tablist">
                        <button class="nav-link active" id="nav-question-tab" data-bs-toggle="tab" data-bs-target="#nav-question" type="button" role="tab" aria-controls="nav-question" aria-selected="true">問卷</button>
                        <button class="nav-link" id="nav-mondai-tab" data-bs-toggle="tab" data-bs-target="#nav-mondai" type="button" role="tab" aria-controls="nav-mondai" aria-selected="false" >問題</button>
                        <button class="nav-link" id="nav-siryou-tab" data-bs-toggle="tab" data-bs-target="#nav-siryou" type="button" role="tab" aria-controls="nav-siryou" aria-selected="false" >填寫資料</button>
                        <button class="nav-link" id="nav-static-tab" data-bs-toggle="tab" data-bs-target="#nav-static" type="button" role="tab" aria-controls="nav-static" aria-selected="false" >統計</button>
                    </div>
                </nav>--%>
                <ul class="nav nav-tabs" id="myTab">
                    <li class="nav-item">
                        <a class="nav-link active" aria-current="page" href="#nav-question">問卷</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link"  href="#nav-mondai">問題</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link"  href="#nav-siryou">填寫資料</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link"  href="#nav-static">統計</a>
                    </li>
                </ul>
                <div class="tab-content" id="nav-tabContent">
                    <div class="tab-pane fade show active" id="nav-question" role="tabpanel" aria-labelledby="nav-question-tab">
                        <asp:Literal runat="server" Text="問卷名稱"></asp:Literal><asp:TextBox runat="server" ID="txbQname"></asp:TextBox><br />
                        <asp:Literal runat="server" Text="描述內容"></asp:Literal><asp:TextBox runat="server" ID="txbQsetume" TextMode="MultiLine"></asp:TextBox><br />
                        <asp:Literal runat="server" Text="開始時間"></asp:Literal><asp:TextBox runat="server" ID="txbS" TextMode="DateTime"></asp:TextBox><br />
                        <asp:Literal runat="server" Text="結束時間"></asp:Literal><asp:TextBox runat="server" ID="txbE" TextMode="DateTime"></asp:TextBox><br />
                        <asp:CheckBox runat="server" ID="ckbState" Checked="true" Text="已啟用" /><br />
                        <asp:Literal runat="server" ID="ltlquestionmsg"></asp:Literal><br />
                        <asp:Button runat="server" ID="cancer" Text="取消" OnClick="cancer_Click" CssClass="btn-dark" />
                        <asp:Button runat="server" ID="gogogo" Text="送出" OnClick="gogogo_Click" CssClass="btn-dark" />
                    </div>
                    <div class="tab-pane fade" id="nav-mondai" role="tabpanel" aria-labelledby="nav-mondai-tab">
                        <asp:Literal runat="server" Text="種類"></asp:Literal>
                        <asp:DropDownList runat="server" ID="ddlTitle" OnSelectedIndexChanged="ddlTitle_SelectedIndexChanged" AppendDataBoundItems="true" AutoPostBack="true">
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
                                        <asp:CheckBox runat="server" ID="ckbZettai" Checked='<%# Eval("Zettai").ToString() == "1" ? true : false %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Button runat="server" ID="btnEdit" Text="編輯" CommandName="btnEdit" CommandArgument='<%# Eval("NaiyoListID") %>' CssClass="btn-dark" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:Literal runat="server" ID="ltlmondaimsg"></asp:Literal>
                        <asp:Button runat="server" ID="btnCancerMondai" Text="取消" OnClick="btnCancerMondai_Click" CssClass="btn-dark"/>
                        <asp:Button runat="server" ID="btnMondaigogogo" Text="送出" OnClick="btnMondaigogogo_Click" CssClass="btn-dark" />
                    </div>
                    <div class="tab-pane fade" id="nav-siryou" role="tabpanel" aria-labelledby="nav-siryou-tab">
                        <asp:Panel runat="server" ID="pnlsiryou1">
                            <asp:Button runat="server" ID="btnTocsv" Text="匯出" OnClick="btnTocsv_Click" CssClass="btn-dark" /><asp:Literal runat="server" ID="ltlsiryoumsg" Text="預設路徑為C:\temp\"></asp:Literal><br />
                            <asp:GridView runat="server" ID="gvKiroku" CellPadding="4" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" OnRowCommand="gvKiroku_RowCommand">
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
                                            <asp:Label ID="ltlZyunban" runat="server" Text='<%# Eval("Zyunban") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="姓名">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblname" Text='<%# Eval("Name") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="填寫時間">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lbldate" Text='<%# Eval("Date") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="觀看細節">
                                        <ItemTemplate>
                                            <asp:Button runat="server" ID="btnsiryoAll" Text="前往" CommandName="siryou" CommandArgument='<%# Eval("KirokuID") %>' CssClass="btn-dark" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <uc1:Pager runat="server" ID="Pager" />
                        </asp:Panel>
                        <asp:Panel runat="server" ID="pnlsiryou2" Visible="false">
                            <asp:Panel runat="server" ID="pnlsiryou2fuuin">
                                <asp:Literal runat="server" Text="姓名" /><asp:TextBox runat="server" ID="txbkirokuname"></asp:TextBox><asp:Literal runat="server" Text="手機" /><asp:TextBox runat="server" ID="txbkirokuphone"></asp:TextBox><br />
                                <asp:Literal runat="server" Text="Email" /><asp:TextBox runat="server" ID="txbkirokuemail"></asp:TextBox><asp:Literal runat="server" Text="年齡" /><asp:TextBox runat="server" ID="txbkirokuage"></asp:TextBox><br />
                                <asp:Literal runat="server" ID="ltlkirokudate"></asp:Literal>
                                <br />
                                <asp:PlaceHolder runat="server" ID="plhkiroku"></asp:PlaceHolder>
                            </asp:Panel>
                            <asp:Button runat="server" ID="btnkirokucancer" Text="返回" OnClick="btnkirokucancer_Click" />
                        </asp:Panel>
                    </div>
                    <div class="tab-pane fade" id="nav-static" role="tabpanel" aria-labelledby="nav-static-tab">
                        <asp:PlaceHolder runat="server" ID="plhstatic"></asp:PlaceHolder>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <script>
        $(document).ready(function () {
            var currentTabIndex = "0";
            //if ($('.changetab').val != undefined && $('.changetab').val != null && $('.changetab').val != "") {
            //    $('#myTab a[href="#nav-mondai"]').tab('show');
            //    //console.log($('.changetab').val);
            //}
            $('#myTab a').click(function (e) {
                e.preventDefault()
                $(this).tab('show')
            });

            // store the currently selected tab in the hash value
            $("ul.nav-tabs > li > a").on("shown.bs.tab", function (e) {
                var id = $(e.target).attr("href");
                var id2 = $(e.relatedTarget).attr("href");
                console.log(id);
                console.log(id2);
                sessionStorage.setItem('tab-name',id)
                window.location.hash = id;
            });
            // on load of the page: switch to the currently selected tab
            var hash = window.location.hash;
            if (sessionStorage.getItem('tab-name') != null) {
                hash = sessionStorage.getItem('tab-name');
            }
            console.log(hash);
            $('#myTab a[href="' + hash + '"]').tab('show');

            //var currentTabIndex = "0";
            //var tabname;
            //$tab = $(`#${tabname}`).tabs({
            //    activate: function (e, ui) {
            //        currentTabIndex = ui.newTab.index().toString();
            //        sessionStorage.setItem('tab-index', currentTabIndex);
            //    }
            //});

            //if (sessionStorage.getItem('tab-index') != null) {
            //    currentTabIndex = sessionStorage.getItem('tab-index');
            //    console.log(currentTabIndex);
            //    $tab.tabs('option', 'active', currentTabIndex);
            //}
            //$('#nav-tab').on('click', function () {
            //    sessionStorage.setItem("tab-index", currentTabIndex);
            //    tabname = this.id;
            //    //window.location = "/Home/Index/";
            //});
        });

        //$(function () {
        //    $("#tabs").tabs({
        //        active: 2,
        //        activate: function (event, ui) {
        //            var active = $(".selector").tabs("option", "active");
        //            console.log(active);
        //            $('#active_tab').val(active.context.activeElement.id);
        //        }
        //    });

        //});
        //$(document).ready(function () {
        //    //var name = $('.changetab').val;
        //    //console.log(name);
        //    //if (name != undefined && name != null && name != "") {
        //    //    var someTabTriggerEl = document.querySelector(`#nav a[href="${name}"]`)
        //    //    var tab = new bootstrap.Tab(someTabTriggerEl)
        //    //    tab.show()
        //    //}
        //    //    var triggerTabList = [].slice.call(document.querySelectorAll(`#${name} a`))
        //    //    triggerTabList.forEach(function (triggerEl) {
        //    //        var tabTrigger = new bootstrap.Tab(triggerEl)
        //    //        triggerEl.addEventListener('click', function (event) {
        //    //            event.preventDefault()
        //    //            tabTrigger.show()
        //    //        })
        //    //    })
        //    //var triggerEl = document.querySelector(`#${name} a[href="#${name}"]`);
        //    //bootstrap.Tab.getInstance(triggerEl).show()
        //    //};
        //    //$(function kk() {
        //    //    var tabName = $("[id*=nav]").val() != "" ? $("[id*=nav]").val() : "tab01";
        //    //    $('#tabs a[href="#' + tabName + '"]').tab('show');
        //    //    if ($("[id*=nav]").val() == "" || tabName == "tab01") {
        //    //        $("#tab01").addClass('active');
        //    //    }

        //    //    $("#tab a").click(function () {
        //    //        $("[id*=nav]").val($(this).attr("href").replace("#", ""));
        //    //    });
        //    //})
        //});

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
