<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Mock.Client.WebThinClient.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Mock thin client</title>
    <link href="content/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="content/font-awesome/css/font-awesome.min.css" rel="stylesheet" />
    <link href="content/main.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server" class="container">
        <h1>Long-executing service mock ThinClient</h1>
        <div class="input-group">
            <span class="input-group-addon"><i class="fa fa-internet-explorer"></i></span>
            <input type="url" id="tbUrlProxy" name="tbUrlProxy" value="http://localhost:11111/simulation" class="form-control" />
        </div>
        <div class="actions">
            <button type="button" id="btnSubmit" class="btn btn-success">Sproži simulacijo</button>
            <button type="button" id="btnClean" class="btn btn-danger">Počisti izpis</button>
        </div>
        <div id="divResult">
        </div>
    </form>
    <script type="text/javascript" src="content/jquery-3.2.0.min.js"></script>
    <script type="text/javascript" src="content/bootstrap/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="content/main.js"></script>
</body>
</html>
