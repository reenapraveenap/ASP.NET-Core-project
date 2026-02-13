<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProtectionPDF.aspx.cs" Inherits="FileProtection.View.ProtectionPDF" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:GridView ID="grdStudents" runat="server" AutoGenerateColumns="true"></asp:GridView>
            <%--            <asp:Button runat="server" ID="btnGetDetails" Text="btnGetDetails" OnClick="btnGetDetails_Click" />--%>
            <br />
           <asp:Button ID="btnExport" runat="server" Text="Export To PDF" OnClick = "ExportToPDF" />
        </div>
    </form>
</body>
</html>
