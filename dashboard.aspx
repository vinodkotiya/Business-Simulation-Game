<%@ Page Language="VB" AutoEventWireup="false" CodeFile="dashboard.aspx.vb" Inherits="dashboard" %>
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
      <asp:AsyncPostBackTrigger ControlID="rbTeamsView" EventName="SelectedIndexChanged" />
    
    <%-- <asp:PostBackTrigger ControlID="LinkButton1" />
          <asp:AsyncPostBackTrigger ControlID="gvInbox" EventName="SelectedIndexChanged" />
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
					<li class="active"><span><a href="dashboard.aspx">Dashboard</a></span></li>
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
				<div class="shell">
                    
					<div class="container">
                     <!-- cols -->
						<section class="cols">
                            <table border="0"> <tr>
						<td>	<div class="col">
								<h3>Production Capacity</h3>
								<img src="css/images/cols-img.png" alt="" class="alignleft"/>
								<div class="col-cnt">
								<ul>
										<%--<li><a href="#">Initial Capacity: </a></li>
										<li><a href="#">Additional Capacity: </a></li>--%>
                                    <div id="divCapacity" runat="server" />
							 	</ul>

								</div>
								<%--<a href="#" class="view">View More</a>--%>
							</div>
						</td><td>	<div class="col">
								<h3>Opening Inventory</h3>
								<img src="css/images/cols-img3.png" alt="" class="alignleft"/>
								<div class="col-cnt">
									<ul>
										  <div id="divInventory" runat="server" />
							 	</ul>
								</div>
									</div>
						</td><td>	<div class="col">
								<h3>My Subscriptions</h3>
								<img src="css/images/cols-img2.png" alt="" class="alignleft"/>
								<div class="col-cnt">
									<ul>
										<div id="divSubscription" runat="server" />
									</ul>

								</div>
								<a href="decisions.aspx?mode=subscriptions" class="view">Change</a>
							</div>
						</td></tr></table>	<div class="cl">&nbsp;</div>
                                
						</section>
						<!-- end of cols -->
						<!-- testimonial -->
								<section class="blog">
                                    	
							<!-- content -->
							<div class="content">
								<img src="css/images/post-img.png" alt=""  class="alignleft"/>
							<table><tr><td><div id ="divTeamDetail" runat="server" /></td>
                                <td> <asp:RadioButtonList ID="rbTeamsView"  DataTextField="d" DataValueField="v"  runat="server" RepeatDirection="Horizontal" RepeatColumns="5" AutoPostBack="True" CssClass="RadioButtonList"></asp:RadioButtonList>
                               </td>
							       </tr></table>
                                    
                                          
                                    <table border="0" >
                                        <tr>
                                        <td colspan="2">                
                                 <h3>Fixed/Spot Cost:</h3>          <p>Fixed Unit Cost of Raw Material:</p>
                               <asp:GridView ID="gvFixedCost" runat="server" AutoGenerateEditButton="false" CellPadding="4" ForeColor="#333333" GridLines="Vertical" CssClass="padder" >
                                   <AlternatingRowStyle BackColor="White" /> <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                   <RowStyle BackColor="#EFF3FB" />  </asp:GridView>

                                        </td>
                                      
                                        <td>  
                                            </td>
                                            <td>

                                            </td>
                                        </tr>
                                        <tr><td colspan="2"> <h3>Variable cost</h3>   </td></tr>
                                        <tr>
                                            <td>
                                                 <p>Raw Material Ratio(Qty):</p>
                               <asp:GridView ID="gvRawRatio" runat="server" AutoGenerateEditButton="false" CellPadding="4" ForeColor="#333333" GridLines="Vertical" CssClass="padder" >
                                   <AlternatingRowStyle BackColor="White" />  <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                   <RowStyle BackColor="#EFF3FB" />  </asp:GridView>
                                            </td>
                                            <td>
                                                <p>Manpower Cost Each Product:</p>
                               <asp:GridView ID="gvLogistics" runat="server" AutoGenerateEditButton="false" CellPadding="4" ForeColor="#333333" GridLines="Vertical" CssClass="padder" >
                                   <AlternatingRowStyle BackColor="White" />   <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                   <RowStyle BackColor="#EFF3FB" />  </asp:GridView>
                                            </td>
                                            <td>
                                                      <p>Demand and Price Range this Quarter</p>
                                                 <asp:GridView ID="gvDemand" runat="server" AutoGenerateEditButton="false" CellPadding="4" ForeColor="#333333" GridLines="Vertical" CssClass="padder" >
                                   <AlternatingRowStyle BackColor="White" />   <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                   <RowStyle BackColor="#EFF3FB" />  </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr><td>
                                            <p>Warehousing Cost:</p>
                               <asp:GridView ID="gvWarehousing" runat="server" AutoGenerateEditButton="false" CellPadding="4" ForeColor="#333333" GridLines="Vertical" CssClass="padder" >
                                   <AlternatingRowStyle BackColor="White" />
                                   <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                   <RowStyle BackColor="#EFF3FB" />
                                   </asp:GridView>
                                            </td>
                                            <td>
                                          <p>Tender Results(Supply in this Qtr from inventory/outsource)</p>
                                      <asp:GridView ID="gvTenderResults" runat="server" Caption="H" CaptionAlign="Left" CssClass="padder" EmptyDataText="No Data Available"  CellPadding="4" ForeColor="#333333" GridLines="Vertical" >
                                   <AlternatingRowStyle BackColor="White"  />
                                    <HeaderStyle BackColor="#00CC66" Font-Bold="True" ForeColor="White" />
                                   <RowStyle BackColor="#EFF3FB" />
                                   </asp:GridView> 
                                            </td>
                                            <td>

                                            </td>
                                        </tr>
                                    </table>
									
	<div class="cnt">
                          </div>
                                <div id ="divInfo" runat="server" style="background-color: #FFFFFF; color: #FF0000" />
                          
							</div>
							<!-- end of content -->

							<!-- sidebar -->
							<aside class="sidebar">
								<!-- widget -->
								<div class="widget">
									<h3>CheckList</h3>
									
										    <div id="divDashboard" runat="server" style="border-left-style: outset; border-left-width: medium; border-left-color: #00FF00" />
                                    	
											  <div id="divDashboard1" runat="server" style="border-left-style: outset; border-left-width: medium; border-left-color: #00FF00" />
                                    	
										             <div id="divDashboard2" runat="server" style="border-left-style: outset; border-left-width: medium; border-left-color: #00FF00" />
                                    
									<div class="cl">&nbsp;</div>
								</div>
									<a href="#" class="view">View All</a>
								<!-- end of widget -->
							</aside>
							<!-- end of sidebar -->
							<div class="cl">&nbsp;</div>
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
