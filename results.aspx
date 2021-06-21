<%@ Page Language="VB" AutoEventWireup="false" CodeFile="results.aspx.vb" Inherits="results" %>
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
					<li ><span><a href="dashboard.aspx">Dashboard</a></span></li>
					<li ><span><a href="decisions.aspx">Decisions</a></span></li>
					<li class="active"><span><a href="#">Results</a></span></li>
					<li ><span><a href="messages.aspx">Messages</a></span></li>
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
								<section class="blog">
							<!-- content -->
							<div class="content">
                                         <asp:Panel ID="pnlGenerate" runat="server">
                         <div id ="divQuarter" runat="server" />
                   
                                  <h3>Step1:</h3><p>Check All Teams have entered Data:</p>
                                 <asp:RadioButtonList ID="rbTeams" DataTextField="d" DataValueField="v"  runat="server" RepeatDirection="Horizontal" RepeatColumns="5" ></asp:RadioButtonList>
                                 <asp:GridView ID="gvDecisionProduction" Caption="A" CaptionAlign="Left" CssClass="padder" runat="server" CellPadding="4" EmptyDataText="No Teams has entered data for current Quarter" ForeColor="#333333" GridLines="Vertical" >
                                   <AlternatingRowStyle BackColor="White" />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                   <RowStyle BackColor="#EFF3FB" />
                                   </asp:GridView>
                                             (Give a button here to reSchedule/decrease Production if Raw Material given is not enough to meet. It will change production decision of all teams.)
                             <br />
                                         <h3>Step2:</h3>   
                                              <table border="0" ><tr><td style="width:70%">
                                              <p>Forecasted Demand(Qty) :</p>
                            <asp:GridView ID="gvForecastQuarter" CssClass="padder" Caption="B" CaptionAlign="Left" runat="server" CellPadding="4" EmptyDataText="No Data Available" ForeColor="#333333" GridLines="Vertical" >
                                   <AlternatingRowStyle BackColor="White" />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                   <RowStyle BackColor="#EFF3FB" />
                                   </asp:GridView> 
                           
                                                                    </td><td>
                                             <p>Average Industry unit cost (Which is just Team Avg):</p>
                             <asp:GridView ID="gvAvgPrice" runat="server" Caption="C" CaptionAlign="Left" CssClass="padder" CellPadding="4" EmptyDataText="No Data Available" ForeColor="#333333" GridLines="Vertical" >
                                   <AlternatingRowStyle BackColor="White" />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                   <RowStyle BackColor="#EFF3FB" />
                                   </asp:GridView> 
                                </td></tr>
                                <tr><td> <p>Total Productwise Production by All Teams:</p>
                                     <asp:GridView ID="gvTotalProduction" Caption="" CaptionAlign="Left" runat="server" CssClass="padder" EmptyDataText="No Data Available"  CellPadding="4" ForeColor="#333333" GridLines="Vertical" >
                                   <AlternatingRowStyle BackColor="White"  />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                   <RowStyle BackColor="#EFF3FB" />
                                   </asp:GridView> 

                                    </td><td>Elasticity: 
                                         <asp:GridView ID="gvElasticity" runat="server" CssClass="padder" EmptyDataText="No Data Available"  CellPadding="4" ForeColor="#333333" GridLines="Vertical" >
                                   <AlternatingRowStyle BackColor="White"  />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                   <RowStyle BackColor="#EFF3FB" />
                                   </asp:GridView> </td></tr>
                                <tr><td> <p>Manpower Cost:</p>
                                     <asp:GridView ID="gvManpowerCost" Caption="" CaptionAlign="Left" runat="server" CssClass="padder" EmptyDataText="No Data Available"  CellPadding="4" ForeColor="#333333" GridLines="Vertical" >
                                   <AlternatingRowStyle BackColor="White"  />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                   <RowStyle BackColor="#EFF3FB" />
                                   </asp:GridView> 

                                    </td><td><p><div title="Can be customised more with staggered delevry condn in future quarters as well as multiple tender in same quarter.
                                  " >Sum of Tender Results(Supply in this Qtr from inventory/outsource)</div></p>
                                      <asp:GridView ID="gvTenderResults" runat="server" Caption="H" CaptionAlign="Left" CssClass="padder" EmptyDataText="No Data Available"  CellPadding="4" ForeColor="#333333" GridLines="Vertical" >
                                   <AlternatingRowStyle BackColor="White"  />
                                    <HeaderStyle BackColor="#00CC66" Font-Bold="True" ForeColor="White" />
                                   <RowStyle BackColor="#EFF3FB" />
                                   </asp:GridView>  </td></tr>
                                <tr><td> 
                                    <p>Raw Material Consumption on Production decision:</p>
                                       <asp:GridView ID="gvRawMaterial" Caption="D" CaptionAlign="Left" runat="server" CssClass="padder" EmptyDataText="No Data Available"  CellPadding="4" ForeColor="#333333" GridLines="Vertical" >
                                   <AlternatingRowStyle BackColor="White"  />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                   <RowStyle BackColor="#EFF3FB" />
                                   </asp:GridView>
                                    </td><td> <p>Cash Collection (%):</p>
                                     <asp:GridView ID="gvCashCollection" runat="server" CssClass="padder" EmptyDataText="No Data Available"  CellPadding="4" ForeColor="#333333" GridLines="Vertical" >
                                   <AlternatingRowStyle BackColor="White"  />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                   <RowStyle BackColor="#EFF3FB" />
                                   </asp:GridView> </td></tr>
                                <tr><td>  <p>Inventory Data from Prev Quarter</p>
                                        <asp:GridView ID="gvInventoryPrevQtr" Caption="E" CaptionAlign="Left" runat="server" CssClass="padder" EmptyDataText="No Data Available"  CellPadding="4" ForeColor="#333333" GridLines="Vertical" >
                                   <AlternatingRowStyle BackColor="White"  />
                                    <HeaderStyle BackColor="#00CC66" Font-Bold="True" ForeColor="White" />
                                   <RowStyle BackColor="#EFF3FB" />
                                   </asp:GridView>
                                    </td>
                                    <td>  <p>Stock Available for Sales:</p>
                                        (Production + Inventory)
                                        <asp:GridView ID="gvStockforSale" Caption="G" CaptionAlign="Left" runat="server" CssClass="padder" EmptyDataText="No Data Available"  CellPadding="4" ForeColor="#333333" GridLines="Vertical" >
                                   <AlternatingRowStyle BackColor="White"  />
                                    <HeaderStyle BackColor="#00CC66" Font-Bold="True" ForeColor="White" />
                                   <RowStyle BackColor="#EFF3FB" />
                                   </asp:GridView> 
                                    </td>
                                </tr>
                                                  <tr><td colspan="2"> 
                                                      <asp:GridView ID="gvMarketCost" Caption="Spot Rates/Empanneled Vendor Rates" CaptionAlign="Left" runat="server" CssClass="padder" EmptyDataText="No Data Available"  CellPadding="4" ForeColor="#333333" GridLines="Vertical" >
                                   <AlternatingRowStyle BackColor="White"  />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                   <RowStyle BackColor="#EFF3FB" />
                                   </asp:GridView> </td></tr>
                                <tr><td colspan="2"> <p>Cost of Raw Material:</p>
                                    <div id="divRaw" runat="server" />
                                    <div class="view" title="ConsumX is consumption of Raw material for production, OpeningX is closing inventory from last quarter, decisionX is
decision sheet value of raw material, lastwtdavgConX is last wtd avg price of total consumption(For qtr 0 it may be actual market rate as qtr -1 be blank).
CostX and spotCostX is either open market cost or empanneled VendorCost, lastpermissibleX is permissibleX ie fresh purchases made on market/vendor (and not spot) in last qtr.
lastQtrPercent is to be applied on lastpermissibleX  to get permissibleX of this quarter to purchase from market/vendor (and not spot). spotX is to be purchased on spot rates thus 
freshPurX is sum of  permissibleX  and spotX. Xvalue is cost of permissibleX and spotXvalue is cost of spotX.
freshXvalue is sum of Xvalue and spotXvalue . wtdAvgPurX is freshXvalue / freshPurX .
while wtdAvgConX is (openingXvalue+freshXvalue)/(OpeningQty+FreshQty) and this will go to get unit cost of product." >...</div>
                                    <div style="width:1000px; height:100%; overflow:auto;">
                                    <asp:GridView ID="gvRawMaterialCost" Caption="F" CaptionAlign="Left" runat="server" CssClass="padder" EmptyDataText="No Data Available"  CellPadding="4" ForeColor="#333333" GridLines="Vertical" >
                                   <AlternatingRowStyle BackColor="White"  />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                   <RowStyle BackColor="#EFF3FB" />
                                   </asp:GridView> 
                                        </div>
                                    </td>
                                    <td>
                                 </tr>
                                <tr><td>
                                    <p> <div title="collection will depend on each tender cost. Low cost tender to be supplied from own production line preferebaly before outsourced.(Logic to be developed)" >
                                              Tender Sales</div> </p>
                                     <asp:GridView ID="gvTenderSales" Caption="TenderSales" CaptionAlign="Left" runat="server" CssClass="padder" EmptyDataText="No Data Available"  CellPadding="4" ForeColor="#333333" GridLines="Vertical" >
                                   <AlternatingRowStyle BackColor="White"  />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                   <RowStyle BackColor="#EFF3FB" />
                                   </asp:GridView> 
                                    </td>
                                     <td>
                                         
                                     </td>
                                </tr>
                                                  <tr><td>
                                    <p> <div title="By default System will try to supply Tender qty from inventory. If not able to meet then outsource it to resultMarket.
If team has given production priority in resultTenders then reduce it from total production to get available stock for sales.
set 1 fro production priority and 0 for not so that multipliucation will give a zero.Reduction in total production will result
in market share allocation. subtract the priority quantities of tender for each products. If production is less than tender to be supplied then market allocation for that product shall be zero." >
                                              Tender with Priority Production</div> </p>
                                     <asp:GridView ID="gvTenderPriority" Caption="G1" CaptionAlign="Left" runat="server" CssClass="padder" EmptyDataText="No Data Available"  CellPadding="4" ForeColor="#333333" GridLines="Vertical" >
                                   <AlternatingRowStyle BackColor="White"  />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                   <RowStyle BackColor="#EFF3FB" />
                                   </asp:GridView> 
                                    </td>
                                     <td>
                                         <p>Stock Available for Market Allocation:</p>
                                        (Production + Inventory-Tenders committed with production in priority)
                                        <asp:GridView ID="gvStockforAllocation" Caption="G2 = G-G1" CaptionAlign="Left" runat="server" CssClass="padder" EmptyDataText="No Data Available"  CellPadding="4" ForeColor="#333333" GridLines="Vertical" >
                                   <AlternatingRowStyle BackColor="White"  />
                                    <HeaderStyle BackColor="#00CC66" Font-Bold="True" ForeColor="White" />
                                   <RowStyle BackColor="#EFF3FB" />
                                   </asp:GridView> 
                                     </td>
                                </tr>
                                </table>


                                <table border="0">
                                    <tr><td >
                                        <h3>Step:3</h3><p>Demand Allocation for This Quarter:</p>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnGenerateResults" runat="server" Text="Initiate Sales/ Book Cost" Height="30px"  />
                                  
                                        </td>
                                    </tr>
                                <tr>
                                    <td colspan="2"> 
                                      <asp:GridView ID="gvAllocation" runat="server" Caption="I - Market Sales" CaptionAlign="Left" CssClass="padder" EmptyDataText="No Data Available"  CellPadding="4" ForeColor="#333333" GridLines="Vertical" >
                                   <AlternatingRowStyle BackColor="White"  />
                                    <HeaderStyle BackColor="#00CC66" Font-Bold="True" ForeColor="White" />
                                   <RowStyle BackColor="#EFF3FB" />
                                   </asp:GridView> 
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr><td>
                                    <p> Inventory After Sales:</p>
                                     <asp:GridView ID="gvStockAfterSales" Caption="J=G-I" CaptionAlign="Left" runat="server" CssClass="padder" EmptyDataText="No Data Available"  CellPadding="4" ForeColor="#333333" GridLines="Vertical" >
                                   <AlternatingRowStyle BackColor="White"  />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                   <RowStyle BackColor="#EFF3FB" />
                                   </asp:GridView> 
                                   
                                    </td><td>
                                          <p> <div title=" This will be closing Inventory. Tender Decision Engine will buy on spot price for 0 inventory otherwise get from inventory on production price.
                                        Let it process seperately for each tender and as per delivery quarter condition. [output>update inventory and get tender profit/loss]">
                                       Closing Inventory After Tendering:</div></p>
                                      
                                     <asp:GridView ID="gvStockAfterSalesTender" Caption="L=J~H" CaptionAlign="Left" runat="server" CssClass="padder" EmptyDataText="No Data Available"  CellPadding="4" ForeColor="#333333" GridLines="Vertical" >
                                   <AlternatingRowStyle BackColor="White"  />
                                    <HeaderStyle BackColor="#00CC66" Font-Bold="True" ForeColor="White" />
                                   <RowStyle BackColor="#EFF3FB" />
                                   </asp:GridView> 
                                         </td>

                                </tr>
                                    
                                    <tr><td colspan="2">
                                   <p> <div title="collection will depend on each tender cost. Low cost tender to be supplied from own production line preferebaly before outsourced.(Logic to be developed)" >
                                              Tendering Item to be outsourced</div> </p>
                                     <asp:GridView ID="gvOutsourced" Caption="K=H~j" CaptionAlign="Left" runat="server" CssClass="padder" EmptyDataText="No Data Available"  CellPadding="4" ForeColor="#333333" GridLines="Vertical" >
                                   <AlternatingRowStyle BackColor="White"  />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                   <RowStyle BackColor="#EFF3FB" />
                                   </asp:GridView> 
                                    </td><td>
                                    
                                         </td>

                                </tr>
                                <tr><td>
                                    <p> Cost Booking this Quarter:</p>
                                    <div style="width:100%; height:120px; overflow:auto;">
                                     <asp:GridView ID="gvResultQuarter" runat="server" CssClass="padder" EmptyDataText="No Data Available"  CellPadding="4" ForeColor="#333333" GridLines="Vertical" >
                                   <AlternatingRowStyle BackColor="White"  /> <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                   <RowStyle BackColor="#EFF3FB" />    </asp:GridView> 
                                        </div>
                                    </td>
                                    <td>
                                        <p> Income this Quarter:</p>
                                     <asp:GridView ID="gvResultIncome" runat="server" CssClass="padder" EmptyDataText="No Data Available"  CellPadding="4" ForeColor="#333333" GridLines="Vertical" >
                                   <AlternatingRowStyle BackColor="White"  /> <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                   <RowStyle BackColor="#EFF3FB" />    </asp:GridView> 
                                    </td>
                                </tr>
                                    <tr>
                                        <td>
                                             <h3>Step:4</h3><p>Finalize Result and Close this Quarter:</p>
                                            <div class="Ino">This can not be undone. Teams which have not entered data for this quarter will have trouble.</div>
                                           
                                        </td>
                                        <td> <asp:Button ID="btnFreez" runat="server" Text="Freez This Quarter" Height="30px"  />

                                        </td>
                                    </tr>
                                </table>
                                  <div id ="divInfo" runat="server" style="background-color: #FFFFFF; color: #FF0000" />
                   </asp:Panel>





                                <asp:Panel ID="pnlViewResult" runat="server">
                                          <asp:RadioButtonList ID="rbTeamsView"  DataTextField="d" DataValueField="v"  runat="server" RepeatDirection="Horizontal" RepeatColumns="5" AutoPostBack="True"></asp:RadioButtonList>
                                   <table border="0">
                                        <tr><td>
                                            <h3>Inventory Data</h3>
                                            <p>Finished Products</p>
                                             <asp:GridView ID="gvTeamInventoryData" Caption="" CaptionAlign="Left" runat="server" CssClass="padder" EmptyDataText="No Data Available"  CellPadding="4" ForeColor="#333333" GridLines="Vertical" >
                                   <AlternatingRowStyle BackColor="White"  /> <HeaderStyle BackColor="#00CC66" Font-Bold="True" ForeColor="White" />
                                   <RowStyle BackColor="#EFF3FB" /></asp:GridView>
                                            </td>
                                            <td><p>Raw Material</p>
                                                <asp:GridView ID="gvTeamRawMaterial" Caption="" CaptionAlign="Left" runat="server" CssClass="padder" EmptyDataText="No Data Available"  CellPadding="4" ForeColor="#333333" GridLines="Vertical" >
                                   <AlternatingRowStyle BackColor="White"  /> <HeaderStyle BackColor="#00CC66" Font-Bold="True" ForeColor="White" />
                                   <RowStyle BackColor="#EFF3FB" /></asp:GridView>
                                            </td>
                                        </tr>
                                       <tr><td>
                                            <h3>Income Statement</h3>
                                            <p>Expanses</p>
                                             <asp:GridView ID="gvIncomeStatement" Caption="" CaptionAlign="Left" runat="server" CssClass="padder" EmptyDataText="No Data Available"  CellPadding="4" ForeColor="#333333" GridLines="Vertical" >
                                   <AlternatingRowStyle BackColor="White"  /> <HeaderStyle BackColor="#00CC66" Font-Bold="True" ForeColor="White" />
                                   <RowStyle BackColor="#EFF3FB" /></asp:GridView>
                                            </td>
                                            <td><p>Income</p>
                                                <asp:GridView ID="gvIncome" Caption="" CaptionAlign="Left" runat="server" CssClass="padder" EmptyDataText="No Data Available"  CellPadding="4" ForeColor="#333333" GridLines="Vertical" >
                                   <AlternatingRowStyle BackColor="White"  /> <HeaderStyle BackColor="#00CC66" Font-Bold="True" ForeColor="White" />
                                   <RowStyle BackColor="#EFF3FB" /></asp:GridView>
                                            </td>
                                        </tr>
                                         <tr><td>
                                            <h3>Balance Sheet</h3>
                                            <p>Expanses</p>
                                             <asp:GridView ID="GridView1" Caption="" CaptionAlign="Left" runat="server" CssClass="padder" EmptyDataText="No Data Available"  CellPadding="4" ForeColor="#333333" GridLines="Vertical" >
                                   <AlternatingRowStyle BackColor="White"  /> <HeaderStyle BackColor="#00CC66" Font-Bold="True" ForeColor="White" />
                                   <RowStyle BackColor="#EFF3FB" /></asp:GridView>
                                            </td>
                                            <td><p>Raw Material</p>
                                                <asp:GridView ID="GridView3" Caption="" CaptionAlign="Left" runat="server" CssClass="padder" EmptyDataText="No Data Available"  CellPadding="4" ForeColor="#333333" GridLines="Vertical" >
                                   <AlternatingRowStyle BackColor="White"  /> <HeaderStyle BackColor="#00CC66" Font-Bold="True" ForeColor="White" />
                                   <RowStyle BackColor="#EFF3FB" /></asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                          </div>
                            <!-- end of content -->
							</div>
                          
							<!-- end of contenor -->

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
						
						<!-- cols -->
					<!-- end of cols -->
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
<asp:TextBox ID="txtConsole" runat="server" Width="100%" Height="100px"  AutoPostBack="true"  TextMode="MultiLine" Enabled="False" ></asp:TextBox>
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
