<%@ Page Language="C#" Debug="true" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SoapDemo.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
     <div>
        Response: <br />
        <textarea id="resp" cols="40" rows="25" style="width:100%" runat="server"></textarea><br />
    </div>
    <form id="form1" runat="server">
        <asp:Button ID="acquireTicket" runat="server" Text="Refresh Ticket" OnClick="acquireTicket_Click" />
    </form>
</body>
</html>
