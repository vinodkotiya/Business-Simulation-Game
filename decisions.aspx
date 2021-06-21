<%@ Page Language="VB" AutoEventWireup="false" CodeFile="decisions.aspx.vb" Inherits="decisions" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
    <title>Project X</title>
	<meta charset="utf-8" />
	<meta name="viewport" content="width=device-width, initial-scale=1, minimum-scale=1, maximum-scale=1, user-scalable=0" />
	
	<link rel="shortcut icon" type="image/x-icon" href="css/images/favicon.ico" />
	<link rel="stylesheet" href="css/style.css" type="text/css" media="all" />
	<link rel="stylesheet" href="css/flexslider.css" type="text/css" media="all" />
	<link href='http://fonts.googleapis.com/css?family=Ubuntu:400,500,700' rel='stylesheet' type='text/css' />
	
	<script src="js/jquery-1.8.0.min.js" type="text/javascript"></script>
	<!--[if lt IE 9]>
		<script src="js/modernizr.custom.js"></script>
	<![endif]-->
	<script src="js/jquery.flexslider-min.js" type="text/javascript"></script>
	<script src="js/functions.js" type="text/javascript"></script>
    <script type="text/javascript">
function onlyNumbers(evt)
{
    var e = event || evt; // for trans-browser compatibility
    var charCode = e.which || e.keyCode;

    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;

    return true;

}
</script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
</asp:ToolkitScriptManager>
    	<div id="wrapper">
		<asp:UpdatePanel ID="UpdatePanel1" runat="server">
 <Triggers>
     
      <asp:AsyncPostBackTrigger ControlID="rbTeams" EventName="SelectedIndexChanged" />
    <%-- <asp:PostBackTrigger ControlID="LinkButton1" />
      <asp:PostBackTrigger ControlID="btnGo" />
      <asp:PostBackTrigger ControlID="btnSMS" />--%>
    </Triggers>
              <ContentTemplate>
		<!-- top-nav -->
		<nav class="top-nav">
			<div class="shell">
				<a href="#" class="nav-btn">HOMEPAGE<span></span></a>
				<span class="top-nav-shadow"></span>
				<ul>
					<li ><span><a href="admin.aspx">Admin</a></span></li>
						<li><span><a href="dashboard.aspx">Dashboard</a></span></li>
					<li class="active"><span><a href="decisions.aspx">Decisions</a></span></li>
					<li><span><a href="results.aspx">Results</a></span></li>
					<li><span><a href="messages.aspx">Messages</a></span></li>
					<li><span><a href="#">Readings</a></span></li>
					
				</ul>
			</div>
		</nav>
		<!-- end of top-nav -->
			 	<div class="footer-bottom">
			<div class="shell">
				<nav class="footer-nav">
				<div  id="div_submenu" runat="server"  />	
					<div class="cl">&nbsp;</div>
				</nav>
				<p class="copy"><div id="myinfo" runat="server" style="display: inline; float: right;"/></p>
			</div>
		</div>
			<!-- main -->
			<div class="main">
               
				<span class="shadow-top"></span>
				<!-- shell -->
				<div class="shell1">
					<div class="container">
						<!-- testimonial -->
						<section class="testimonial">
                            <table border="0" > <tr><td style="width:82%">
                          Enter decisions for Quarter  <asp:TextBox ID="txtQuarter" runat="server" Width="20px"></asp:TextBox>  
                           <asp:RadioButtonList ID="rbTeams" DataTextField="d" DataValueField="v"  runat="server" RepeatDirection="Horizontal" RepeatColumns="5" AutoPostBack="True"></asp:RadioButtonList>
                            <asp:Panel ID="pnlProduction" runat="server">
                                <table style="float: none; text-align: left;">
                                    <tr><td></td><td><asp:Label ID="lblA" runat="server" Text="Product A" /></td><td><asp:Label ID="lblB" runat="server" Text="Product B" /></td><td><asp:Label ID="lblC" runat="server" Text="Product C" /></td><td><asp:Label ID="lblD" runat="server" Text="Product D" /></td></tr>

                                    <tr><td>Total Production</td> <td> <asp:TextBox ID="txtProdA" runat="server" onkeypress="return onlyNumbers();"/></td><td> <asp:TextBox ID="txtProdB" runat="server" onkeypress="return onlyNumbers();"/></td><td> <asp:TextBox ID="txtProdC" runat="server" onkeypress="return onlyNumbers();"/></td><td> <asp:TextBox ID="txtProdD" runat="server" onkeypress="return onlyNumbers();"/></td></tr>
                                      <tr><td>Unit Price </td> <td> <asp:TextBox ID="txtPriceA" runat="server" onkeypress="return onlyNumbers();"/></td><td> <asp:TextBox ID="txtPriceB" runat="server" onkeypress="return onlyNumbers();"/></td><td> <asp:TextBox ID="txtPriceC" runat="server" onkeypress="return onlyNumbers();"/></td><td> <asp:TextBox ID="txtPriceD" runat="server" onkeypress="return onlyNumbers();"/></td></tr>
                                    <tr><td colspan="4">Enter Raw Material: <div title ="If less then engine will purchase from spot rate or your production will be reduced. Also raw material limit shall be +-50% of you last order." style="display: inline">Details</div></td></tr> 
                                      <tr><td>Raw Material X </td> <td> <asp:TextBox ID="txtRawX" runat="server" onkeypress="return onlyNumbers();"/></td><td> Y </td><td> <asp:TextBox ID="txtRawY" runat="server" onkeypress="return onlyNumbers();"/></td><td> </td></tr>
                             
                                         <tr><td></td><td><asp:Button ID="btndecisionProduction" runat="server" Text="Check & Submit" Enabled="true" /></td></tr>

                                </table>
                                <p>Note: Only enter Production for This Quarter because you may have finished product in your inventory from Last Quarter. <a href="decisions.aspx?mode=capacity">Check Here</a></p>
                              </asp:Panel>
                           <asp:Panel ID="pnlCapacity" runat="server">
                               Inventory from Last Quarter:
                                        <asp:GridView ID="gvInventoryPrevQtr" runat="server" CssClass="padder" EmptyDataText="No Data Available"  CellPadding="4" ForeColor="#333333" GridLines="Vertical" >
                                   <AlternatingRowStyle BackColor="White"  />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                   <RowStyle BackColor="#EFF3FB" />
                                   </asp:GridView> 
                                <table style="float: none; text-align: left;">
                                    
                                    <tr><td>Current Capacity:</td><td><asp:Label ID="lblCapacity" runat="server" Text="0" /></td></tr>
                                <tr><td>Capacity Augmentation Cost per Unit:</td><td><asp:Label ID="lblCapacityCost" runat="server" Text="0" /></td></tr>
                                <tr><td>Add Capacity(Units):</td><td><asp:TextBox ID="txtAddCapacity" runat="server" onkeypress="return onlyNumbers();" Text="0" /></td></tr>
                                  <tr><td>Note: Capacity Addition TurnAround Time is one Quarter. Added Capacity shall be Reflected in next quater.</td><td></td></tr>
                                    <tr><td><asp:Button ID="btnDecisionCapacity" runat="server" Text="Check & Submit" Enabled="true" /></td><td></td></tr>
                              </table>
                              </asp:Panel> 
                                <asp:Panel ID="pnlFinances" runat="server">
                                <table style="float: none; text-align: left;">
                                   <tr><td>Investment in Bank FD(@9%)- (Currency)</td><td><asp:TextBox ID="txtFD" runat="server" onkeypress="return onlyNumbers();" Text="0" /></td></tr>
                                    <tr><td>DisInvestment from Bank FD- (Currency)</td><td><asp:TextBox ID="txtFDback" runat="server" onkeypress="return onlyNumbers();" Text="0" /></td></tr>
                                 <tr><td>Take Bank Loan(@14%)- (Currency)</td><td><asp:TextBox ID="txtLoan" runat="server" onkeypress="return onlyNumbers();" Text="0" /></td></tr>
                                    <tr><td>Repay Bank Loan- (Currency)</td><td><asp:TextBox ID="txtLoanRepay" runat="server" onkeypress="return onlyNumbers();" Text="0" /></td></tr>
                                 <tr><td>Note: This shall be Reflected in current quater results.</td><td></td></tr>
                                    <tr><td><asp:Button ID="btnDecisionFinances" runat="server" Text="Check & Submit" Enabled="true" /></td><td></td></tr>
                              </table>
                              </asp:Panel>
                                 <asp:Panel ID="pnlSubscriptions" runat="server">
                                     <asp:CheckBoxList ID="cbSubscription" runat="server" DataTextField="d" DataValueField="v"  RepeatDirection="Horizontal" RepeatColumns="3" />
                                   <asp:Button ID="btnSubscription" runat="server" Text="Subscribe" Enabled="true" />
                              
                              </asp:Panel> 
                                <div id ="divInfo" runat="server" style="background-color: #FFFFFF; color: #FF0000" />
                           </td> <td>
                                    <asp:Panel ID="pnlDashboard" runat="server">
                              <div id="divDashboard" runat="server" style="border-left-style: outset; border-left-width: medium; border-left-color: #00FF00" />
                             <div id="divDashboard1" runat="server" style="border-left-style: outset; border-left-width: medium; border-left-color: #00FF00" />
                                         <div id="divDashboard2" runat="server" style="border-left-style: outset; border-left-width: medium; border-left-color: #00FF00" />
                                     <div id="divDashboard3" runat="server" style="border-left-style: outset; border-left-width: medium; border-left-color: #00FF00" />
                           
                            </asp:Panel>
                                </td></tr></table>
						</section>
						<!-- testimonial -->

					</div>
				<!-- end of shell -->
				</div>
				<!-- end of container -->
			</div>
			<!-- end of main -->
          </ContentTemplate>
    </asp:UpdatePanel>
		</div>	
	<!-- end of wrapper -->
		<div id="footer-push"></div>
		<!-- end of footer-push -->
	
	
	<!-- footer -->
		<div  id="footer">
		<span class="shadow-bottom"></span>
		<!-- footer-cols -->
       
		<div class="footer-cols">
			<!-- shell -->
             <div class="shell">
                 <asp:UpdatePanel runat="server"><ContentTemplate>
<asp:TextBox ID="txtConsole" runat="server" Width="100%" Height="50px"  AutoPostBack="true"  TextMode="MultiLine" Enabled="False" ></asp:TextBox>
           </ContentTemplate></asp:UpdatePanel>
            
        </div>
			<div class="shell">
				<div class="col">
					<h3><a href="#">Simulation</a></h3>
					
					
				</div>
				<div class="col">
					<h3><a href="#">Clients</a></h3>
					
				</div>
				<div class="col">
					<h3><a href="#">About Us</a></h3>
					
				</div>
				<div class="col">
					<h3><a href="#">Contact Us</a></h3>
					
				</div>
				<div class="cl">&nbsp;</div>
			</div>
			<!-- end of shell -->
		</div>
		<!-- end of footer-cols -->
		<div class="footer-bottom">
			<div class="shell">
				<nav class="footer-nav">
				
					<div class="cl">&nbsp;</div>
				</nav>
				</div>
		</div>
	</div>
	<!-- end of footer -->
    </form>
</body>
</html>
