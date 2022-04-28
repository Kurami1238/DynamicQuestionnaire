<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Stastic.aspx.cs" Inherits="DynamicQuestionnaire.FrontEnd.Stastic" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../CSS/bootstrap.css" rel="stylesheet" />
    <script src="../JS/bootstrap.js"></script>
    <%--<script src="../JS/jquery.min.js"></script>--%>
    <%--<script src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script src="http://static.pureexample.com/js/flot/excanvas.min.js"></script>
    <script src="http://static.pureexample.com/js/flot/jquery.flot.min.js"></script>
    <script src="http://static.pureexample.com/js/flot/jquery.flot.pie.min.js"></script>--%>
    <script src="../JS/jquery.js"></script>
    <%--<script src="../JS/moment.min.js"></script>
    <script src="../JS/Chart.min.js"></script>--%>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/3.7.1/chart.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/chartjs-plugin-annotation/1.4.0/chartjs-plugin-annotation.js"></script>
    <%--<script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.9.3/Chart.min.js%22%3E"></script>--%>
    <%--<script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.4.0/Chart.min.js%22%3E"></script>--%>
    <%--<script src="https://cdn.jsdelivr.net/npm/chart.js"></script>--%>
    <style>
        * {
            background-color: #181717;
            color: rgba(255, 248, 240, 0.93);
        }
        .chartgroup{
            height :300px;
            width:300px;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
        <div class="row col-sm-11 col-md-11 col-lg-11">
        <div class="col-sm-11 col-md-11 col-lg-11">
            <asp:HiddenField runat="server" ID="QID" />
            <asp:Literal runat="server" ID="ltlQName"></asp:Literal>
            <asp:Literal runat="server" ID="ltlmsg"></asp:Literal>
            <%--<input type="button" onclick="Javascript:window.history.go(-1);" value="返回" />--%>
            <asp:Button runat="server" ID="btnback" OnClick="btnback_Click" Text="返回列表" CssClass="btn-dark"/>
        </div>
        <div runat="server" class="col-sm-11 col-md-11 col-lg-11">
            <div class="col-sm-8 col-md-8 col-lg-8" id="Question"></div>
            <canvas id="myChart" width="400" height="400"></canvas>
            <asp:PlaceHolder runat="server" ID="phl"></asp:PlaceHolder>
           
        </div>
         </div>
    </form>
    
    <script>
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
        function ramdomrgb(length) {
            var rgb = '';
            var rgb2 = '';
            function rr() {
                var rr = Math.floor(Math.random() * 256);
                return rr
            };
            function gg() {
                var gg = Math.floor(Math.random() * 256);
                return gg
            };
            function bb() {
                var bb = Math.floor(Math.random() * 256);
                return bb
            };
            for (var j = 0; j < length; j++) {
                var r = rr();
                var g = gg();
                var b = bb();
                if (j != length - 1) {
                    rgb += `rgba(${r},${g},${b},1)/`;
                    rgb2 += `rgba(${r},${g},${b},0.4)/`;
                }
                else {
                    rgb += `rgba(${r},${g},${b},1)`;
                    rgb2 += `rgba(${r},${g},${b},0.4)`;
                }
            }
            var stasticrgb = rgb.split("/");
            var stasticrgb2 = rgb2.split("/");
            return [stasticrgb, stasticrgb2]
        }
        function CreateChart(Type, title, labels, data, color, colorb, ctxID) {
            Chart.defaults.font.size = 24;
            //var ctx = document.getElementById(ctxID).getContext('2d');
            var ctx = $(`#Question #${ctxID}`);
            var myChart = new Chart(ctx, {
                type: Type,
                data: {
                    labels: labels,
                    datasets: [{
                        data: data,
                        backgroundColor: color,
                        borderColor: colorb,
                        borderWidth: 1
                    }]
                },
                options: {
                    if(Type = "bar"){
                        indexAxis: 'y'
                    },
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true,
                                responsive: true,
                                maintainAspectRatio: false
                            }
                        }]
                    },
                    plugins: {
                        legend: {
                            labels: {
                                font: {
                                    size: 20
                                }
                            },
                            maxHeight: 300,
                            maxWidth: 300
                        },
                        title: {
                            display: true,
                            text: title,
                            padding: {
                                top: 10
                            }
                        }
                    }
                }
            });
        };
        function Stastic(henzi) {
            var list = henzi.SourceList;
            console.log(list);
            for (var i = 0; i < list.length; i++) {
                var Zyouken = '';
                var kazu = '';
                for (var j = 0; j < list[i].KazuandKiroku.length; j++) {
                    if (j != list[i].KazuandKiroku.length - 1) {
                        Zyouken += `${list[i].KazuandKiroku[j].Naiyo},`;
                    }
                    else {
                        Zyouken += `${list[i].KazuandKiroku[j].Naiyo}`;
                    }
                };
                var stasticlabel = Zyouken.split(",");
                for (var j = 0; j < list[i].KazuandKiroku.length; j++) {
                    if (j != list[i].KazuandKiroku.length - 1) {
                        kazu += `${list[i].KazuandKiroku[j].Kazu},`;
                    }
                    else {
                        kazu += `${list[i].KazuandKiroku[j].Kazu}`;
                    }
                };
                var stasticdata = kazu.split(",");
                var rgba = ramdomrgb(list[i].KazuandKiroku.length);
                console.log(stasticlabel);
                console.log(stasticdata);
                console.log(rgba);
                switch (list[i].Type) {
                    case 1:
                        $('#Question').append(`<canvas id="chart${i}" class="chartgroup"></canvas>`);
                        CreateChart('doughnut', `${i + 1}. ${list[i].Title}`, stasticlabel, stasticdata, rgba[1], rgba[0], `chart${i}`);
                        break;
                    case 2:
                        $('#Question').append(`<canvas id="chart${i}" class="chartgroup"></canvas>`);
                        CreateChart('bar', `${i + 1}. ${list[i].Title}`, stasticlabel, stasticdata, rgba[1], rgba[0], `chart${i}`);
                        break;
                    default:
                        $('#Question').append(`<span>${i + 1}.${list[i].Title}<br />  -</span>`);
                        break;
                }
            }
        }
        
    </script>
</body>
</html>
