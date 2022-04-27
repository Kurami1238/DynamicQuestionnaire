<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Stastic.aspx.cs" Inherits="DynamicQuestionnaire.FrontEnd.Stastic" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="../JS/bootstrap.js"></script>
    <%--<script src="../JS/jquery.min.js"></script>--%>
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script src="http://static.pureexample.com/js/flot/excanvas.min.js"></script>
    <script src="http://static.pureexample.com/js/flot/jquery.flot.min.js"></script>
    <script src="http://static.pureexample.com/js/flot/jquery.flot.pie.min.js"></script>
    <%--<script src="../JS/jquery.js"></script>--%>
    <%--<script src="../JS/moment.min.js"></script>
    <script src="../JS/Chart.min.js"></script>--%>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/3.7.1/chart.min.js"></script>
    <%--<script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.9.3/Chart.min.js%22%3E"></script>--%>
    <%--<script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.4.0/Chart.min.js%22%3E"></script>--%>
    <%--<script src="https://cdn.jsdelivr.net/npm/chart.js"></script>--%>
    <style>
        * {
            background-color: #181717;
            color: rgba(255, 248, 240, 0.93);
        }

        #flotcontainer {
            width: 500px;
            height: 400px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:HiddenField runat="server" ID="QID" />
            <asp:Literal runat="server" ID="ltlQName"></asp:Literal>
        </div>
        <div runat="server">
            <canvas id="Question"></canvas>
            <canvas id="myChart" width="800" height="600"></canvas>
            <asp:PlaceHolder runat="server" ID="phl"></asp:PlaceHolder>
           
        </div>
         <div id="legendPlaceholder"></div>
            <div id="flotcontainer"></div>
    </form>
    <script>
        $(function () {
            var data = [
                { label: "影像合成", data: 10 },
                { label: "商品拍攝", data: 20 },
                { label: "打光技巧", data: 30 },
                { label: "實拍案例", data: 40 }
            ];
            var options = {
                series: {
                    pie: { show: true }
                },
                legend: {
                    show: false
                }
            };
            $.plot($("#flotcontainer"), data, options);
        });
        $(document).ready(function () {
            var hf = $("#QID").val();
            var postData = {
                "QID": hf,
            }

            $.ajax({
                url: "/API/StasticHandler.ashx?Action=Stastic",
                method: "POST",
                data: postData,
                dataType: "json",
                success: Stastic,
                error: function (henzi) {
                    console.log(henzi);
                    alert("通訊失敗，請聯絡管理員。")
                }
            });
        });
        function Stastic(henzi) {
            var ctx = $('#Question');
            console.log(ctx);
            var list = henzi.SourceList;
            console.log(list);
            for (var i = 0; i < list.length; i++) {

                switch (list[i].Type) {
                    case 1:
                        var Zyouken = '';
                        var kazu = '';
                        var rgb = '';
                        for (var j = 0; j < list[i].KazuandKiroku.length; j++) {
                            Zyouken += ` '${list[i].KazuandKiroku[j].Naiyo}', `;
                        };
                        var stasticlabel = `[${Zyouken}]`
                        for (var j = 0; j < list[i].KazuandKiroku.length; j++) {
                            kazu += `${list[i].KazuandKiroku[j].Kazu}, `;
                        };
                        var stasticdata = `[${kazu}]`;
                        function randomColor() {
                            var r = Math.floor(Math.random() * 256);
                            var g = Math.floor(Math.random() * 256);
                            var b = Math.floor(Math.random() * 256);
                            return "rgb(" + r + "," + g + "," + b + ")"
                        };
                        for (var j = 0; j < list[i].KazuandKiroku.length; j++) {
                            rgb += `['${randomColor()}', ]`
                        }
                        var stasticrgb = rgb;
                        var mychart = new Chart(ctx, {
                            type: 'pie',
                            data: {
                                // 標題
                                labels: stasticlabel,
                                datasets: [{
                                    label: '# test',
                                    data: stasticdata,
                                    backgroundColor: stasticrgb,
                                    borderColor: stasticrgb,
                                    borderWidth: 1
                                }]
                            },
                            //options: {
                            //    scales: {
                            //        yAxes: [{
                            //            ticks: {
                            //                beginAtZero: true,
                            //                responsive: true //符合響應式
                            //            }
                            //        }]
                            //    }
                            //}
                        })
                        break;
                    case 2:
                        break;

                    default:
                        break;
                }
            }



        }
    </script>
</body>
</html>
