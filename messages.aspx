<%@ Page Language="VB" AutoEventWireup="false" CodeFile="messages.aspx.vb" Inherits="messages" %>
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
     
      <asp:AsyncPostBackTrigger ControlID="gvInbox" EventName="SelectedIndexChanged" />
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
					<li ><span><a href="decisions.aspx">Decisions</a></span></li>
					<li><span><a href="#">Results</a></span></li>
					<li class="active"><span><a href="messages.aspx">Messages</a></span></li>
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
                            <asp:Panel ID="pnlInbox" runat="server">
                                 <asp:GridView ID="gvInbox" runat="server"  AutoGenerateSelectButton ="True" CellPadding="4" ForeColor="#333333" GridLines="Vertical" >
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
                              </asp:Panel>
                           <asp:Panel ID="pnlCompose" runat="server">
                               
                              </asp:Panel> 
                                <div id ="divInfo" runat="server" style="background-color: #FFFFFF; color: #FF0000" />
                           </> <td>
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
