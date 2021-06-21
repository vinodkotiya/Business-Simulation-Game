<%@ Page Language="VB" AutoEventWireup="false" CodeFile="admin.aspx.vb" Inherits="admin" %>
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
    <%-- <asp:PostBackTrigger ControlID="LinkButton1" />
      <asp:PostBackTrigger ControlID="btnGo" />
     
      <asp:PostBackTrigger ControlID="btnSMS" /> --%>
     
     <asp:PostBackTrigger ControlID="btnProduct" />
     <asp:PostBackTrigger ControlID="btnLogin" />
           <asp:PostBackTrigger ControlID="gvLogistics" />
     <asp:PostBackTrigger ControlID="gvFixedCost" />
     <asp:PostBackTrigger ControlID="gvElasticity" />
      <asp:PostBackTrigger ControlID="gvRawRatio" />
      <asp:PostBackTrigger ControlID="gvCashCollection" />
     <asp:PostBackTrigger ControlID="gvLiveTenders" />
      <asp:AsyncPostBackTrigger ControlID="rblBuiseness" EventName="SelectedIndexChanged" />
    </Triggers>
              <ContentTemplate>
		<!-- top-nav -->
		<nav class="top-nav">
			<div class="shell">
				<a href="#" class="nav-btn">HOMEPAGE<span></span></a>
				<span class="top-nav-shadow"></span>
				<ul>
					<li class="active"><span><a href="admin.aspx">Admin</a></span></li>
					<li><span><a href="dashboard.aspx">Dashboard</a></span></li>
					<li><span><a href="decisions.aspx?mode=production">Decisions</a></span></li>
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
                            <table border="0">
                                <tr><td style="width:82%">
                                    <asp:Panel ID="pnlLogin" runat="server">
                                <table style="float: none; text-align: center;">
                                    <tr><td>Login ID:</td><td><asp:TextBox ID="txtLogin" runat="server" Text="admin" /></td></tr>
                                <tr><td>Password:</td><td><asp:TextBox ID="txtPwd" runat="server" Text="nimda" TextMode="Password" /></td></tr>
                                   <tr><td><asp:Button ID="btnLogin" runat="server" Text="Login" Enabled="true" /></td><td></td></tr>
                              </table>
                              </asp:Panel>
                            <asp:Panel ID="pnlGame" runat="server">
						<table style="float: none; text-align: left;" border="1">
                            <tr><td colspan="3" ><center><asp:ImageButton ID="btnJoinGame" runat="server" ImageUrl="~/css/images/joingame.png" ImageAlign="Middle" AlternateText="Join Existing Game" BorderWidth="2" BorderStyle="Outset" />
                                <<-- Join Existing Game or Create a New Game.
                                                 </center> </td></tr>
                            <tr><td colspan="2">
                                <asp:RadioButtonList ID="rblBuiseness" runat="server" DataTextField="d" DataValueField="v"  RepeatDirection="Horizontal" Width="550px" AutoPostBack="True" RepeatColumns="3">
                             
                                </asp:RadioButtonList></td>
                                <td>
                                    <asp:TextBox ID="txtNewProductA" runat="server" /> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                                    <asp:TextBox ID="txtNewProductB" runat="server" /> <br />
                                    <asp:TextBox ID="txtNewProductC" runat="server" /> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                                    <asp:TextBox ID="txtNewProductD" runat="server" /> <br />
                                    <asp:Button ID="btnProduct" runat="server" Text="Update Products" />

                                </td>
                            </tr>
                            <tr>
                                <td>Total Teams</td><td><asp:TextBox ID="txtTeam" runat="server" Width="50px" Text="5"></asp:TextBox></td>
                                <td>Members in Each Team</td><td><asp:TextBox ID="txtMember" Width="50px" runat="server" Text="4" /></td>
                            </tr>
                            <tr>
                                <td>Initial Wealth of Each Team (Currency)</td><td><asp:TextBox ID="txtWealth" Width="50px" runat="server" Text="100000"></asp:TextBox></td>
                                <td>Plant Capacity for each Team (Units):</td><td><asp:TextBox ID="txtPlantCapacity" Width="50px" runat="server" Text="10000"></asp:TextBox></td>
                            </tr>
                           
                            <tr>
                          <td colspan="2"> CostCenters  

                              <asp:ListBox ID="lbCostCenters" runat="server"   DataTextField="d" DataValueField="v" SelectionMode="Multiple" Rows="8"></asp:ListBox>
                          </td> 
                             
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnNewGame" runat="server" Text="Create Game" />
                                    <br />
                                
                                </td>
                            </tr>
						</table>
                                
                                </asp:Panel>
                            <asp:Panel ID="pnlTeam" runat="server">
                               <asp:GridView ID="gvTeams" runat="server" AutoGenerateEditButton="True" CellPadding="4" ForeColor="#333333" CssClass="padder" GridLines="Vertical" >
                                   <AlternatingRowStyle BackColor="White" />
                                   <EditRowStyle BackColor="#2461BF" />
                                   <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                   <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                   <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                   <RowStyle BackColor="#EFF3FB" />
                                   <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                   <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                   <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                   <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                   <SortedDescendingHeaderStyle BackColor="#4870BE" />
                                </asp:GridView>
                                <asp:Button ID="btnConditions" runat="server" Text="Next:Set Forecast Conditions >>" />
                            </asp:Panel>
                            <asp:Panel ID="pnlConditions" runat="server">
                             <table><tr><td>   <p>Set Elasticity</p>
                                 <asp:GridView ID="gvElasticity" runat="server" AutoGenerateEditButton="True" CellSpacing="30" GridLines="Both" BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CssClass="padder">
                                   <AlternatingRowStyle BackColor="#F7F7F7" />
                                   <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7"  HorizontalAlign="Right" />
                                    <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" />
                                 </asp:GridView> 
                                 </td><td> 
                                 </td></tr></table>
<p>Forecast Condition for All Quarter</p>
                               <asp:GridView ID="gvForecast" runat="server" AutoGenerateEditButton="True" CellPadding="3" GridLines="Both" BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CssClass="padder">
                                   <AlternatingRowStyle BackColor="#F7F7F7" />
                                   <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
                                   <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Right" />
                                   <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" />
                                   
                                 </asp:GridView> <br/>

                                <fieldset>
                                    <legend>Auto Reconsile</legend>
                                    Demand Factor <asp:TextBox ID="txtDemandFactor" runat="server" Text="0.95" Width="50px" /> of Total Production Capacity. <br />
                                    Current Quarter  <asp:TextBox ID="txtCurrentQuarter" runat="server" Text="0" Width="50px"/> <br />
                                <asp:Button ID="btnForecastReconsile" runat="server" Text="Auto Reconsile based on Last Quarter Result" />
                                </fieldset>
                            </asp:Panel>
                                     <asp:Panel ID="pnlLogistics" runat="server">
                                <p>There Are Two type of Raw Material namely X and Y. Fixed unit cost of these Raw Materials are Given Here. A certain Ratio of X and Y is Required to make Product A/B/C/D.</p>
                                <h3>Fixed Cost:</h3>          <p>Fixed Unit Cost of Raw Material:</p>
                               <asp:GridView ID="gvFixedCost" Caption="VendorID 0 is Market, from where teams without empanneled vendor will purchase." runat="server" AutoGenerateEditButton="True" CellPadding="4" ForeColor="#333333" GridLines="Vertical" CssClass="padder" >
                                   <AlternatingRowStyle BackColor="White" />
                                   <EditRowStyle BackColor="#2461BF" />
                                   <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                   <RowStyle BackColor="#EFF3FB" />
                                   </asp:GridView>
                                         <p>Ratio (Qty) of Raw Material Required for Each Product:</p>
                               <asp:GridView ID="gvRawRatio" runat="server" AutoGenerateEditButton="True" CellPadding="4" ForeColor="#333333" GridLines="Vertical" CssClass="padder" >
                                   <AlternatingRowStyle BackColor="White" />
                                   <EditRowStyle BackColor="#2461BF" />
                                   <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                   <RowStyle BackColor="#EFF3FB" />
                                   </asp:GridView>
                                     <h3>Variable cost</h3>    <p>Variable Cost Conditions:</p>
                               <asp:GridView ID="gvLogistics" runat="server" AutoGenerateEditButton="True" CellPadding="4" ForeColor="#333333" GridLines="Vertical" CssClass="padder" >
                                   <AlternatingRowStyle BackColor="White" />
                                   <EditRowStyle BackColor="#2461BF" />
                                   <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                   <RowStyle BackColor="#EFF3FB" />
                                   </asp:GridView>
                                         <p>Set Cash Collection(%)</p>
                                     <asp:GridView ID="gvCashCollection" runat="server" AutoGenerateEditButton="True" CellSpacing="30" GridLines="Both" BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CssClass="padder">
                                   <AlternatingRowStyle BackColor="#F7F7F7" />
                                   <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7"  HorizontalAlign="Right" />
                                   <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" />
                                 </asp:GridView> 
                                        </asp:Panel>     
                                     <asp:Panel ID="pnlL" runat="server">
                               
                              
                            </asp:Panel>
                                    <asp:Panel ID="pnlTenders" runat="server">
                                    <h3>Tender Management</h3>    <p>Create New Tender</p>
                                        <table style="float: none; text-align: left;">
                                    <tr><td></td><td><asp:Label ID="lblA" runat="server" Text="Product A" /></td><td><asp:Label ID="lblB" runat="server" Text="Product B" /></td><td><asp:Label ID="lblC" runat="server" Text="Product C" /></td><td><asp:Label ID="lblD" runat="server" Text="Product D" /></td></tr>

                                    <tr><td>Tender Quantity</td> <td> <asp:TextBox ID="txtProdA" runat="server" onkeypress="return onlyNumbers();"/></td><td> <asp:TextBox ID="txtProdB" runat="server" onkeypress="return onlyNumbers();"/></td><td> <asp:TextBox ID="txtProdC" runat="server" onkeypress="return onlyNumbers();"/></td><td> <asp:TextBox ID="txtProdD" runat="server" onkeypress="return onlyNumbers();"/></td></tr>
                                      <tr><td>Supply Quarter</td><td><asp:TextBox ID="txtSupplyQuarter" runat="server" onkeypress="return onlyNumbers();"/></td><td colspan="2">Enter Tender Conditions: <div title ="To be developed for tender decision engine." style="display: inline">Details</div></td></tr> 
                                   
                                         <tr><td></td><td><asp:Button ID="btnCreateTender" runat="server" Text="Create New Tender" Enabled="true" /></td></tr>
                                     </table>
                                        <p>Live Tenders: </p>
                                         <div style="width:1000px; height:100%; overflow:auto;">
                                         <asp:GridView ID="gvLiveTenders" runat="server" AutoGenerateEditButton="True" CellSpacing="30" GridLines="Both" BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CssClass="padder">
                                   <AlternatingRowStyle BackColor="#F7F7F7" />
                                   <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7"  HorizontalAlign="Right" />
                                   <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" />
                                 </asp:GridView> </div>
                                         <p>Quotes Recieved: </p>
                                         <asp:GridView ID="gvTenderQuotes" runat="server" AutoGenerateEditButton="True" CellSpacing="30" GridLines="Both" BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CssClass="padder">
                                   <AlternatingRowStyle BackColor="#F7F7F7" />
                                   <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7"  HorizontalAlign="Right" />
                                   <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" />
                                 </asp:GridView> 
                                         <p>Finalize & Close Live Tenders</p>
                                    <asp:RadioButtonList ID="rbLiveTenders"  DataTextField="d" DataValueField="v"  runat="server" RepeatDirection="Horizontal" RepeatColumns="5" AutoPostBack="True" CssClass="RadioButtonList"></asp:RadioButtonList>
                               <asp:Button ID="btnCloseTender" runat="server" Text="Finalize and Close Selected Tender" Enabled="true" />
                                    <p>Tender Results affecting this quarter: Do manual Intervention in Results from Here.</p>
                                     <asp:GridView ID="gvTenderResults" runat="server" AutoGenerateEditButton="True" CellSpacing="30" GridLines="Both" BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CssClass="padder">
                                   <AlternatingRowStyle BackColor="#F7F7F7" />
                                   <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7"  HorizontalAlign="Right" />
                                   <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" />
                                 </asp:GridView> 
                                         </asp:Panel>
                                   
                            <div id ="divInfo" runat="server" style="background-color: #FFFFFF; color: #FF0000" />
                                </td>
                                <td>
                                    <asp:Panel ID="pnlDashboard" runat="server">
                              <div id="divAdminDashboard" runat="server" style="border-left-style: outset; border-left-width: medium; border-left-color: #00FF00" />
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
