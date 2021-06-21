<%@ Page Language="VB" AutoEventWireup="false" CodeFile="temp.aspx.vb" Inherits="temp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:GridView runat="server" ID="gvTemp"  AutoGenerateEditButton="true" >
        </asp:GridView>
        <br />
        <asp:GridView runat="server" ID="GridView1"  AutoGenerateEditButton="true" OnRowUpdating="gvTemp_RowUpdating" OnRowEditing ="gvTemp_RowEditing" OnRowUpdated="gvTemp_RowUpdated"  >
        </asp:GridView>
        <br />
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
        
        <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
    </div>
    </form>
</body>
</html>
