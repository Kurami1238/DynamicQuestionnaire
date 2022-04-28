<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="DynamicQuestionnaire.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>首頁</title>
    <style>
        * {
            background-color: #181717;
            color: rgba(255, 248, 240, 0.93);
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>DynamicQuestionnaire</h1>
            <a href="FrontEnd/List.aspx" title="前往所有問卷列表">
                <h2>問卷列表</h2>
            </a><br />
            <a href="RearEnd/List.aspx" title="前往問卷管理系統">
                <h2>問卷管理系統</h2>
            </a>
        </div>
    </form>
</body>
</html>
