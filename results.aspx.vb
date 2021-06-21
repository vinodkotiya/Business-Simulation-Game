Imports common
Imports dbOperation
Imports decisionEngine
Partial Class results
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("gameID") Is Nothing Then
            Response.Redirect("admin.aspx?source=results")
        End If
        If Not Page.IsPostBack Then
            Dim submenu = 0


            Session("mode") = Request.Params("mode")
            If Request.Params("mode") Is Nothing Then
                Session("mode") = "production"
            End If
            pnlGenerate.Visible = False
            pnlViewResult.Visible = False

            Dim productA, productB, productC, productD As String
            Dim myquery = "select productA, productB, productC, productD  from productMaster where business = '" & Session("BusinessType") & "'"

            Dim dtProduct = getDBTable(myquery)

            productA = dtProduct.Rows(0)(0)
            productB = dtProduct.Rows(0)(1)
            productC = dtProduct.Rows(0)(2)
            productD = dtProduct.Rows(0)(3)
            myquery = "select teamid as v, teamname as d from teamMaster where gameid=" & Session("gameID")

            If Not Session("user") = "admin" Then
                Session("mode") = "view"
            End If

            If Session("mode") = "view" Then
                submenu = 1
                pnlViewResult.Visible = True
                rbTeamsView.DataSource = getDBTable(myquery)
                rbTeamsView.DataBind()
                colorTeamsView(Session("mode"))
            Else
                pnlGenerate.Visible = True

                divQuarter.InnerHtml = "Generate Results for Quarter:" & Session("currentQuarter")
                ''  Try
                rbTeams.DataSource = getDBTable(myquery)
                rbTeams.DataBind()
                '' Step 1


                Dim teamsEnteredData = colorTeams("production") 'Teamid's who entered data
                Dim totalTeamsEnteredData = teamsEnteredData.Count
                Dim totalTeams = rbTeams.Items.Count
                '' If totalTeamsEnteredData = 0 Then Exit Try ''No Teams have entered data 
                myquery = "select teamid, productA as " & productA & ", productB as " & productB & ", productC  as " & productC & ", productD  as " & productD & ", priceA as price" & productA & ", priceB as price" & productB & ", priceC as price" & productC & ", priceD as price" & productD & ", rawX, rawY from decisionProduction where gameID= " & Session("gameID") & " and quarter = " & Session("currentQuarter")
                Dim dtDecisionProduction = getDBTable(myquery)
                gvDecisionProduction.DataSource = dtDecisionProduction
                gvDecisionProduction.DataBind()
                DisplayMessage("Team Decision Status Shown Successfully.")
                ''Step2
                myquery = "select demandA as demand" & productA & ", demandB as " & productB & ", demandC as " & productC & ", demandD as " & productD & " from forecastQuarter where gameid=" & Session("gameID") & " and quarter=" & Session("currentQuarter")
                gvForecastQuarter.DataSource = getDBTable(myquery)
                gvForecastQuarter.DataBind()
                ' 
                myquery = "select sum(productA) as Total_" & productA & ", sum(productB) as " & productB & ", sum(productC) as " & productC & ", sum(productD) as " & productD & " from decisionProduction where gameid = " & Session("gameID") & " and quarter=" & Session("currentQuarter")
                gvTotalProduction.DataSource = getDBTable(myquery)
                gvTotalProduction.DataBind()
                DisplayMessage("Productwise Demand & Elasticity Shown")

                myquery = "select avg(priceA) as Avg_" & productA & ", avg(priceB) as " & productB & ", avg(priceC) as " & productC & ", avg(priceD) as " & productD & " from decisionProduction where gameid = " & Session("gameID") & " and quarter=" & Session("currentQuarter")
                gvAvgPrice.DataSource = getDBTable(myquery)
                gvAvgPrice.DataBind()
                DisplayMessage("Avg Market Price Shown")
                ''' Elasticity
                ''' 
                myquery = "select elasticityA as " & productA & ", elasticityB as " & productB & ", elasticityC as " & productC & ", elasticityD as " & productD & " from productMaster where business='" & Session("BusinessType") & "'"
                gvElasticity.DataSource = getDBTable(myquery)
                gvElasticity.DataBind()




                ''Manpower Cost
                DisplayMessage("Manpower Cost Calculated")
                myquery = "select manpowerSlab1, manpowerSlab2, manpowerSlab3, manpowerSlab4, manpowerQtySlab1, manpowerQtySlab2, manpowerQtySlab3, manpowerQtySlab4 from productMaster where business='" & Session("BusinessType") & "'"
                Dim dtManpower = getDBTable(myquery)
                ViewState("dtManpower") = getManpowerSlabBasedCost(dtDecisionProduction.DefaultView.ToTable(False, "teamID", productA, productB, productC, productD), dtManpower)
                gvManpowerCost.DataSource = ViewState("dtManpower")
                gvManpowerCost.DataBind()

                ''Cash Collectiomn
                myquery = "select CashCollectionA as " & productA & ", CashCollectionB as " & productB & ", CashCollectionC as " & productC & ", CashCollectionD as " & productD & " from productMaster where business='" & Session("BusinessType") & "'"
                gvCashCollection.DataSource = getDBTable(myquery)
                gvCashCollection.DataBind()
                ' txtElasticity.Text = getDBsingle("select elasticity from gameMaster where uid=" & Session("gameID"))
                DisplayMessage("Productwise Demand & Elasticity Shown")
                '''###########################33
                ''' 
                '' If dont have empanneled vendor then buy spot rate 
                myquery = "select teamid, t.myvendor, costX, costY, costA, costB, costC, costD, spotcostX, spotcostY, spotcostA, spotcostB, spotcostC, spotcostD, lastQtrPercent from vendorMarket v, teammaster t where t.myvendor = v.myvendor and  v.business='" & Session("BusinessType") & "' and t.gameID=" & Session("gameID")
                ''Caution: myvendor returns only single row if used without tablename t.
                Dim dtMarketCost = getDBTable(myquery)
                'dtMarketCost.Merge(getDBTable(myquery), False, Data.MissingSchemaAction.Add)
                gvMarketCost.DataSource = dtMarketCost
                gvMarketCost.DataBind()
                ViewState("dtMarketCost") = dtMarketCost

                ''gvRawMaterialCost calculation
                '
                '  Exit Sub
                myquery = "select D.TEAMID, d.productA * p.qtyRawXforA as Ax, d.productA * p.qtyRawYforA as Ay, d.productB * p.qtyRawXforB as Bx, d.productB * p.qtyRawYforB as 'By',d.productC * p.qtyRawXforC as Cx, d.productC * p.qtyRawYforC as Cy,d.productD * p.qtyRawXforD as Dx, d.productD * p.qtyRawYforD as Dy, d.RawX as decisionX, d.RawY as decisionY  from productMaster p, decisionProduction d where p.business='" & Session("BusinessType") & "' and d.quarter = " & Session("currentQuarter")
                Dim dtRawMaterial = getDBTable(myquery)
                '  dtRawMaterial.Columns.Add("TotalRaw")
                'sumObject = table.Compute("Sum(Amount)", "");
                Dim dtCol1 As New System.Data.DataColumn("ConsumX", System.Type.GetType("System.Decimal"))
                dtCol1.Expression = "Ax + Bx + Cx +  Dx "
                dtRawMaterial.Columns.Add(dtCol1)
                Dim dtCol2 As New System.Data.DataColumn("ConsumY", System.Type.GetType("System.Decimal"))
                dtCol2.Expression = "Ay + By +  Cy + Dy"
                dtRawMaterial.Columns.Add(dtCol2)

                '"{1} + {2} + {3} + {4} + {5} + {6} + {7} "
                ' dtRawMaterial.Columns.Add(dtCol)
                gvRawMaterial.DataSource = dtRawMaterial
                gvRawMaterial.DataBind()
                DisplayMessage("Raw Material Required calculated based on Production Decision")

                ''Last Quarter Inventory
                myquery = "select teamID, productA, productB, productC, productD, closingX as openingX, closingY as openingY, permissibleX, permissibleY, wtdAvgConX ,wtdAvgConY from resultInventory where  gameid=" & Session("gameid") & " and quarter=" & Session("currentQuarter") & "-1 "
                Dim dtInventoryLastQtr = getDBTable(myquery)
                gvInventoryPrevQtr.DataSource = dtInventoryLastQtr
                gvInventoryPrevQtr.DataBind()
                DisplayMessage("Last quarter inventory fetched.")
                '' Get stock available for Sales= production + last qtr inventory
                DisplayMessage("Get stock available for Sales= production + last qtr inventory G")
                ''select d.teamid, d.productA + r.productA as  productA , d.productB + r.productB as  productB , d.productC + r.productC as  productC , d.productD + r.productD as  productD  from decisionProduction d , resultInventory r on d.gameID = r.gameid and d.teamid=r.teamid and d.quarter - 1 = r.quarter where  d.quarter = 4 and d.gameID=12
                Dim dtStockBeforeSales = getDBTable("select d.teamid, d.productA + r.productA as " & productA & ", d.productB + r.productB as " & productB & ", d.productC + r.productC as " & productC & ", d.productD + r.productD as " & productD & " from decisionProduction d , resultInventory r on d.gameID = r.gameid and d.teamid=r.teamid and d.quarter - 1 = r.quarter where  d.quarter = " & Session("CurrentQuarter") & " and d.gameID=" & Session("gameID"))
                ViewState("StockBeforeSales") = dtStockBeforeSales
                gvStockforSale.DataSource = ViewState("StockBeforeSales")
                gvStockforSale.DataBind()

                ''
                '' Show Tender Results
                DisplayMessage("Show Tender Results")
                ViewState("tenderResults") = getDBTable("select teamid, sum(productA) as " & productA & ", sum(productB) as " & productB & ", sum(productC) as " & productC & ", sum(productD) as " & productD & " , count(tenderID) as totalTenders from resultTender where  supplyQuarter = " & Session("CurrentQuarter") & " and gameID=" & Session("gameID") & " group by teamID")
                gvTenderResults.DataSource = ViewState("tenderResults")
                gvTenderResults.DataBind()
                DisplayMessage("Show Tender Sales")
                gvTenderSales.DataSource = getDBTable("select tenderID, productA as " & productA & ", productB as " & productB & ", productC  as " & productC & ", productD  as " & productD & ", priceA as price" & productA & ", priceB as price" & productB & ", priceC as price" & productC & ", priceD as price" & productD & ", (productA * priceA) + (productB * priceB) + (productC * priceC) + (productD * priceD) as TenderSales from resultTender where gameID= " & Session("gameID") & " and supplyquarter = " & Session("currentQuarter"))
                gvTenderSales.DataBind()

                ''Show tender quantities with production priority set to 1, multiply logic 
                DisplayMessage("Show tender quantities with production priority set to 1, multiply logic G1")
                myquery = "select teamID, sum(t.productA * t.prodPriorityA)  as " & productA & ",  sum(t.productB * t.prodPriorityB) as " & productB & ",  sum(t.productC * t.prodPriorityC) as " & productC & ", sum(t.productD * t.prodPriorityD) as " & productD & " from resultTender t where gameID= " & Session("gameID") & " and supplyquarter = " & Session("currentQuarter") & " group by teamid"
                Dim dtTenderPriority = getDBTable(myquery)
                gvTenderPriority.DataSource = dtTenderPriority
                gvTenderPriority.DataBind()

                ''get final products available for market allocation 
                DisplayMessage("get final products available for market allocation G2=G-G1")
                Dim dtTenderPriorityView = dtTenderPriority.DefaultView
                For Each dr In dtStockBeforeSales.Rows
                    dtTenderPriorityView.RowFilter = "teamID = " & dr(0)
                    If dtTenderPriorityView.Count > 0 Then
                        ''subtract the priority quantities of tender for each products. If production is less than tender to be supplied then market allocation for that product shall be zero.
                        If dr(1) < dtTenderPriorityView(0)(1) Then dr(1) = 0 Else dr(1) = dr(1) - dtTenderPriorityView(0)(1)
                        If dr(2) < dtTenderPriorityView(0)(2) Then dr(2) = 0 Else dr(2) = dr(2) - dtTenderPriorityView(0)(2)
                        If dr(3) < dtTenderPriorityView(0)(3) Then dr(3) = 0 Else dr(3) = dr(3) - dtTenderPriorityView(0)(3)
                        If dr(4) < dtTenderPriorityView(0)(4) Then dr(4) = 0 Else dr(4) = dr(4) - dtTenderPriorityView(0)(4)
                    End If
                Next

                gvStockforAllocation.DataSource = dtStockBeforeSales  ''Will go for allocation
                'ViewState("stocksForAllocation") = dtStockBeforeSales
                gvStockforAllocation.DataBind()
                '' show Tender Sales


                '' Merge raw inventory with production to get cost of purchase
                dtRawMaterial.Merge(dtInventoryLastQtr, False, Data.MissingSchemaAction.AddWithKey)
                Dim dtrawMaterialCost = dtRawMaterial.DefaultView.ToTable(False, "teamID", "ConsumX", "ConsumY", "openingX", "openingY", "decisionX", "decisionY")
                dtrawMaterialCost.Columns.Add("lastWtdAvgConX", System.Type.GetType("System.Decimal"))
                dtrawMaterialCost.Columns.Add("lastWtdAvgConY", System.Type.GetType("System.Decimal"))
                dtrawMaterialCost.Columns.Add("costX", System.Type.GetType("System.Decimal"))
                dtrawMaterialCost.Columns.Add("costY", System.Type.GetType("System.Decimal"))
                dtrawMaterialCost.Columns.Add("spotcostX", System.Type.GetType("System.Decimal"))
                dtrawMaterialCost.Columns.Add("spotcostY", System.Type.GetType("System.Decimal"))
                dtrawMaterialCost.Columns.Add("lastQtrPercent", System.Type.GetType("System.Decimal"))
                dtrawMaterialCost.Columns.Add("lastpermissibleX", System.Type.GetType("System.Decimal"))
                dtrawMaterialCost.Columns.Add("lastpermissibleY", System.Type.GetType("System.Decimal"))
                Dim dtCol3 As New System.Data.DataColumn("freshPurX", System.Type.GetType("System.Decimal"))
                dtCol3.Expression = "ConsumX - openingX"
                dtCol3.DefaultValue = 0
                dtrawMaterialCost.Columns.Add(dtCol3)
                Dim dtCol4 As New System.Data.DataColumn("freshPurY", System.Type.GetType("System.Decimal"))
                dtCol4.DefaultValue = 0
                dtCol4.Expression = "ConsumY - openingY"
                dtrawMaterialCost.Columns.Add(dtCol4)
                Dim dtCol61 As New System.Data.DataColumn("permissibleX", System.Type.GetType("System.Decimal"))
                ''   dtCol61.Expression = "IIf(lastpermissibleX > 0, freshPurX - decisionX, 0)"
                dtCol61.DefaultValue = 0
                dtrawMaterialCost.Columns.Add(dtCol61)
                Dim dtCol62 As New System.Data.DataColumn("permissibleY", System.Type.GetType("System.Decimal"))
                dtCol62.DefaultValue = 0
                '' dtCol62.Expression = "ConsumY - openingY"
                dtrawMaterialCost.Columns.Add(dtCol62)
                Dim dtCol31 As New System.Data.DataColumn("spotX", System.Type.GetType("System.Decimal"))
                ''  dtCol31.Expression = "IIf(freshPurX > decisionX, freshPurX - decisionX, 0)"
                dtrawMaterialCost.Columns.Add(dtCol31)
                Dim dtCol41 As New System.Data.DataColumn("spotY", System.Type.GetType("System.Decimal"))
                dtCol41.DefaultValue = 0
                '' dtCol41.Expression = "IIf(freshPurY > decisionY, freshPurY - decisionY, 0)"
                dtrawMaterialCost.Columns.Add(dtCol41)
                '' loop through market rate and get raw material cost
                Dim dtMarketCostView = dtMarketCost.DefaultView
                Dim dtInventoryLastQtrView = dtInventoryLastQtr.DefaultView

                For Each dr In dtrawMaterialCost.Rows
                    dtMarketCostView.RowFilter = "teamID = " & dr(0)
                    dtInventoryLastQtrView.RowFilter = "teamID = " & dr(0)
                    If dtMarketCostView.Count > 0 Then
                        dr("costX") = dtMarketCostView(0)("costX")
                        dr("costY") = dtMarketCostView(0)("costY")
                        dr("spotcostX") = dtMarketCostView(0)("spotcostX")
                        dr("spotcostY") = dtMarketCostView(0)("spotcostY")
                        dr("lastQtrPercent") = dtMarketCostView(0)("lastQtrPercent")
                        ''Else part not implementing because no decision given.
                    End If
                    If dtInventoryLastQtrView.Count > 0 Then
                        dr("lastpermissibleX") = dtInventoryLastQtrView(0)("permissibleX")
                        dr("lastpermissibleY") = dtInventoryLastQtrView(0)("permissibleY")
                        dr("lastWtdAvgConX") = If(String.IsNullOrEmpty(dtInventoryLastQtrView(0)("wtdAvgConX").ToString), dr("costX"), dtInventoryLastQtrView(0)("wtdAvgConX"))
                        dr("lastWtdAvgConY") = If(String.IsNullOrEmpty(dtInventoryLastQtrView(0)("wtdAvgConY").ToString), dr("costY"), dtInventoryLastQtrView(0)("wtdAvgConY"))
                    Else
                        dr("lastpermissibleX") = 0
                        dr("lastpermissibleY") = 0
                        dr("lastWtdAvgConX") = dr("costX")
                        dr("lastWtdAvgConY") = dr("costY")
                    End If
                    '' Now calculate how much from market and how much from spot purchase using freshx and lastpermx
                    Dim maxPerX, minPerX As Integer
                    If dr("lastpermissibleX").ToString = "0" Or String.IsNullOrEmpty(dr("lastpermissibleX").ToString) Then
                        '' if 0 or Empty then allow all fresh purchase from market not on spot
                        maxPerX = dr("freshPurX")
                        minPerX = dr("freshPurX")
                    ElseIf Not String.IsNullOrEmpty(dr("lastpermissibleX").ToString) Then
                        maxPerX = dr("lastpermissibleX") * (dr("lastQtrPercent") + 100) / 100
                        minPerX = dr("lastpermissibleX") * dr("lastQtrPercent") / 100

                    End If

                    If dr("freshPurX") >= minPerX And dr("freshPurX") <= maxPerX Then
                        dr("spotX") = 0
                        dr("permissibleX") = dr("freshPurX")
                    ElseIf dr("freshPurX") > maxPerX Then
                        dr("spotX") = dr("freshPurX") - maxPerX
                        dr("permissibleX") = maxPerX
                    ElseIf dr("freshPurX") < minPerX Then
                        dr("spotX") = dr("freshPurX")   '' all will go to stock purchase
                        dr("permissibleX") = 0
                    End If

                    Dim maxPerY, minPerY As Integer
                    If dr("lastpermissibleY").ToString = "0" Or String.IsNullOrEmpty(dr("lastpermissibleY").ToString) Then
                        '' if 0 or Empty then allow all fresh purchase from market not on spot
                        maxPerY = dr("freshPurY")
                        minPerY = dr("freshPurY")
                    ElseIf Not String.IsNullOrEmpty(dr("lastpermissibleY").ToString) Then
                        maxPerY = dr("lastpermissibleY") * (dr("lastQtrPercent") + 100) / 100
                        minPerY = dr("lastpermissibleY") * dr("lastQtrPercent") / 100

                    End If
                    If dr("freshPurY") >= minPerY And dr("freshPurY") <= maxPerY Then
                        dr("spotY") = 0
                        dr("permissibleY") = dr("freshPurY")
                    ElseIf dr("freshPurY") > maxPerY Then
                        dr("spotY") = dr("freshPurY") - maxPerY
                        dr("permissibleY") = maxPerY
                    ElseIf dr("freshPurY") < minPerY Then
                        dr("spotY") = dr("freshPurY")   '' all will go to stock purchase
                        dr("permissibleY") = 0
                    End If
                Next

                DisplayMessage("Unit Cost appended succesfully for Raw Material. Prcedeing for inventory cost")
                DisplayMessage("Get cost if bought from market/vendor")
                dtrawMaterialCost.Columns.Add("Xvalue", System.Type.GetType("System.Decimal"), "permissibleX * costX")
                dtrawMaterialCost.Columns.Add("Yvalue", System.Type.GetType("System.Decimal"), "permissibleY * costY")
                DisplayMessage("Get cost if bought from market/vendor")
                dtrawMaterialCost.Columns.Add("spotXvalue", System.Type.GetType("System.Decimal"), "spotX * spotcostX")
                dtrawMaterialCost.Columns.Add("spotYvalue", System.Type.GetType("System.Decimal"), "spotY * spotcostY")
                dtrawMaterialCost.Columns.Add("freshXvalue", System.Type.GetType("System.Decimal"), "Xvalue + spotXvalue")
                dtrawMaterialCost.Columns.Add("freshYvalue", System.Type.GetType("System.Decimal"), "Yvalue + spotYvalue")
                dtrawMaterialCost.Columns.Add("wtdAvgPurX", System.Type.GetType("System.Decimal"), "CONVERT(freshXvalue / freshPurX * 100, System.Int64 ) / 100")
                dtrawMaterialCost.Columns.Add("wtdAvgPurY", System.Type.GetType("System.Decimal"), "CONVERT(freshYvalue / freshPurY * 100, System.Int64 ) / 100")
                dtrawMaterialCost.Columns.Add("wtdAvgConX", System.Type.GetType("System.Decimal"), "CONVERT(((freshXvalue + openingX * lastWtdAvgConX)/ConsumX) * 100, System.Int64 ) / 100")
                dtrawMaterialCost.Columns.Add("wtdAvgConY", System.Type.GetType("System.Decimal"), "CONVERT(((freshYvalue + openingY * lastWtdAvgConY)/ConsumY) * 100, System.Int64 ) / 100")
                Dim dtCol7 As New System.Data.DataColumn("freshTotalvalue", System.Type.GetType("System.Decimal"))
                dtCol7.DefaultValue = 0
                dtCol7.Expression = "freshXvalue + freshYvalue"
                dtrawMaterialCost.Columns.Add(dtCol7)
                dtrawMaterialCost.Columns.Add("closingX", System.Type.GetType("System.Decimal"), "IIf(ConsumX < freshPurX, freshPurX - ConsumX, 0)")
                dtrawMaterialCost.Columns.Add("closingY", System.Type.GetType("System.Decimal"), "IIf(ConsumY < freshPurY, freshPurY - ConsumY, 0)")

                ''   Dim dtRaw = getDBTable("select teamid, rawX, rawY from decisionProduction where gameID= " & Session("gameID") & " and quarter = " & Session("currentQuarter"))
                ''  dtrawMaterialCost.Merge(dtRaw)
                ''F
                ViewState("dtrawMaterialCost") = dtrawMaterialCost
                gvRawMaterialCost.DataSource = ViewState("dtrawMaterialCost")
                gvRawMaterialCost.DataBind()
                DisplayMessage("Merge raw inventory with production to get cost of purchase")


                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXx
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXxx
                ''Allocating Demand
                'Fixed Cost
                'myquery = "select FixedCostRawX * QtyRawXforA + FixedCostRawY * QtyRawYforA as CostA, FixedCostRawX * QtyRawXforB + FixedCostRawY * QtyRawYforB as CostB, FixedCostRawX *QtyRawXforC + FixedCostRawY * QtyRawYforC as CostC , FixedCostRawX * QtyRawXforD + FixedCostRawY * QtyRawYforD as CostD from productMaster where business='" & Session("BusinessType") & "'"
                'Dim dtFixedCost = getDBTable(myquery)
                'Dim dtAllocation As New System.Data.DataTable
                'dtAllocation.Columns.Add("Products")
                'dtAllocation.Columns.Add("FixedCost")
                'dtAllocation.Columns.Add("VarCost")
                '' Dim teams(totalTeams) As Integer
                'For Each i As ListItem In rbTeams.Items
                '    'Product, Team1, Team2, Team3, Team4, Team5
                '    dtAllocation.Columns.Add(i.Text)
                'Next
                'Dim productIndex = 0
                'Dim teamIndex = 0
                'For Each product In dtProduct.Rows(0).ItemArray
                '    teamIndex = 0 '' will forward grid production rows
                '    Dim tmprow = dtAllocation.NewRow
                '    tmprow(0) = product ' "Laptop"
                '    tmprow(1) = dtFixedCost.Rows(0)(productIndex)
                '    tmprow(2) = 0
                '    For teamID = 1 To totalTeams
                '        If Array.IndexOf(teamsEnteredData, teamID) < 0 Then
                '            tmprow(teamID + 2) = "0"
                '            DisplayMessage("Product: " & product & " Team" & teamID & " NA")
                '        Else
                '            Dim forecastedDemand = If(IsNumeric(gvForecastQuarter.Rows(0).Cells(productIndex).Text), gvForecastQuarter.Rows(0).Cells(productIndex).Text, 0)
                '            Dim myUnitCost = If(IsNumeric(gvDecisionProduction.Rows(teamIndex).Cells(productIndex + 5).Text), gvDecisionProduction.Rows(teamIndex).Cells(productIndex + 5).Text, 0)
                '            Dim myProduction = If(IsNumeric(gvDecisionProduction.Rows(teamIndex).Cells(productIndex + 1).Text), gvDecisionProduction.Rows(teamIndex).Cells(productIndex + 1).Text, 0)
                '            Dim avgMarketCost = If(IsNumeric(gvAvgPrice.Rows(0).Cells(productIndex).Text), gvAvgPrice.Rows(0).Cells(productIndex).Text, 0)
                '            Dim elasticity = If(IsNumeric(gvElasticity.Rows(0).Cells(productIndex).Text), gvElasticity.Rows(0).Cells(productIndex).Text, 6)
                '            ''+2 for fixed cost, varCost
                '            tmprow(teamID + 2) = allocateProductAsPerDemand(forecastedDemand, totalTeamsEnteredData, myUnitCost, avgMarketCost, elasticity, myProduction)
                '            ''''' Additional Logic 

                '            DisplayMessage("Product: " & product & " Team" & teamID & "-" & forecastedDemand & " " & totalTeams & " " & myUnitCost & " " & avgMarketCost & " " & elasticity)
                '            teamIndex = teamIndex + 1 ''get next row of grid
                '        End If

                '    Next
                '    dtAllocation.Rows.Add(tmprow)
                '    productIndex = productIndex + 1 ''for product index only
                'Next

                ''tmprow(0) = "Laptop"
                ''tmprow(1) = allocateProductAsPerDemand(gvForecastQuarter.Rows(0).Cells(1).Text, totalTeams, gvDecisionProduction.Rows(0).Cells(5).Text, gvAvgPrice.Rows(0).Cells(0).Text, txtElasticity.Text)
                ''tmprow(1) = allocateProductAsPerDemand(3000, 6, 271, 271, 7)
                'gvAllocation.DataSource = dtAllocation
                'gvAllocation.DataBind()

                ''##############3
                ''Net Sales

                'Dim dtSales = dtAllocation.Copy
                '' dtSales.Columns.Add("Leader")
                '' Dim LeaderA, LeaderB, LeaderC, LeaderD As Integer
                'teamIndex = 0
                'For Each i As ListItem In rbTeams.Items
                '    dtSales.Columns.Add(i.Text & "-Sales")
                '    productIndex = 0
                '    For Each dr In dtSales.Rows
                '        If Array.IndexOf(teamsEnteredData, i.Value) < 0 Then
                '            Dim myUnitCost = If(IsNumeric(gvDecisionProduction.Rows(teamIndex).Cells(productIndex + 5).Text), gvDecisionProduction.Rows(teamIndex).Cells(productIndex + 5).Text, 0)
                '            Dim res = Convert.ToDouble(myUnitCost) * Convert.ToDouble(dr(i.Text))
                '            dr(i.Text & "-Sales") = res.ToString
                '            productIndex = productIndex + 1
                '        Else
                '            dr(i.Text & "-Sales") = i.Value
                '        End If

                '    Next
                '    dtSales.Columns.Remove(i.Text)
                '    If Array.IndexOf(teamsEnteredData, i.Value) > 0 Then teamIndex = teamIndex + 1
                'Next

                'gvNetSales.DataSource = dtSales
                'gvNetSales.DataBind()

                ' ''#####################
                ' '''''' Sells data and leader

                'Dim dtSells = dtAllocation.Copy
                'dtSells.Columns.Add("Leader")
                'Dim LeaderA, LeaderB, LeaderC, LeaderD As Integer
                'For Each i As ListItem In rbTeams.Items
                '    dtSells.Columns.Add(i.Text & "-sells")
                '    Dim rowindex = 0
                '    For Each dr In dtSells.Rows
                '        Dim res = (Convert.ToDouble(dr(1)) + Convert.ToDouble(dr(2))) * Convert.ToDouble(dr(i.Text))
                '        dr(i.Text & "-sells") = res.ToString
                '        If rowindex = 0 And res > LeaderA Then
                '            LeaderA = res
                '            dr("Leader") = i.Text
                '        End If

                '        If rowindex = 1 And res > LeaderB Then
                '            LeaderB = res
                '            dr("Leader") = i.Text
                '        End If

                '        If rowindex = 2 And res > LeaderC Then
                '            LeaderC = res
                '            dr("Leader") = i.Text
                '        End If
                '        If rowindex = 3 And res > LeaderD Then
                '            LeaderD = res
                '            dr("Leader") = i.Text
                '        End If
                '        rowindex = rowindex + 1
                '    Next
                '    dtSells.Columns.Remove(i.Text)
                'Next

                'gvSales.DataSource = dtSells
                'gvSales.DataBind()
                ''#####################
                '''''' Sells data and leader#########


                'Catch ex As Exception
                '    DisplayMessage("Error:" & ex.Message & myquery)
                'End Try
                ' txtQuarter.Text = Session("CurrentQuarter")
            End If

            div_submenu.InnerHtml = getsubMenu("results", submenu)

            'showDashboard(True)
        End If
        Dim wealth = "NA"
        If Not Session("teamID") Is Nothing Then wealth = getDBsingle("select wealth from teamMaster where teamID=" & Session("teamID") & " and gameID=" & Session("gameID"))
        myinfo.InnerHtml = "Wealth: $ " & wealth & " " & Session("user") & "<span>|</span><a href=admin.aspx?mode=kill>Logout</a>"

    End Sub

    Function colorTeams(ByVal mode As String) As Integer()
        '' Color green for Teams who have entered Data
        Dim myquery = ""
        Try
            If mode = "production" Then
                myquery = "select teamid from decisionProduction where gameid = " & Session("gameID") & " and quarter = " & Session("CurrentQuarter")
         
            End If
            Dim dt = getDBTable(myquery)
            Dim dv = dt.DefaultView
            dv.Sort = "teamid"
            Dim countYellow = 0
            Dim teamsEnteredData(rbTeams.Items.Count) As Integer
            Dim j = 0
            For Each i As ListItem In rbTeams.Items
                If dv.Find(i.Value) = -1 Then
                    i.Attributes("style") = "background-color:yellow;"
                    countYellow = countYellow + 1
                Else
                    i.Attributes("style") = "background-color:lime;"
                    teamsEnteredData(j) = i.Value
                    j = j + 1
                End If
                '' unlock for admin and disable for users
                i.Enabled = False
                'If Session("user") = i.Value Then
                '    i.Enabled = True
                'ElseIf Session("user") = "admin" Then
                '    i.Enabled = True
                'Else
                '    i.Enabled = False
                'End If
            Next
            If Not countYellow = 0 Then
                divDashboard.InnerHtml = "<p><div class=warning>Production Decision Pending <br/>for " & countYellow & " Teams </div></p>"
            Else
                divDashboard.InnerHtml = "<p><div class=success>Production Decision done by<br/> all Teams </div></p>"

            End If
            ''''''''''''''''
            '  Return rbTeams.Items.Count - countYellow ''Return no of teams entered data
            Return teamsEnteredData  'return array of team id entered data
        Catch ex As Exception
            DisplayMessage("Error Coloring Teams: " & myquery)
        End Try

    End Function
    Function colorTeamsView(ByVal mode As String) As Integer()
        '' Color green for Teams who have entered Data
        Dim myquery = ""
        Try
            If mode = "view" Then
                myquery = "select teamid from decisionProduction where gameid = " & Session("gameID") & " and quarter = " & Session("CurrentQuarter")

            End If
            Dim dt = getDBTable(myquery)
            Dim dv = dt.DefaultView
            dv.Sort = "teamid"
            Dim countYellow = 0
            Dim teamsEnteredData(rbTeamsView.Items.Count) As Integer
            Dim j = 0
            For Each i As ListItem In rbTeamsView.Items
                If dv.Find(i.Value) = -1 Then
                    i.Attributes("style") = "background-color:yellow;"
                    countYellow = countYellow + 1
                Else
                    i.Attributes("style") = "background-color:lime;"
                    teamsEnteredData(j) = i.Value
                    j = j + 1
                End If
                '' unlock for admin and disable for users
                i.Enabled = False
                If Session("user") = i.Value Then
                    i.Enabled = True
                ElseIf Session("user") = "admin" Then
                    i.Enabled = True
                Else
                    i.Enabled = False
                End If
            Next
            If Not countYellow = 0 Then
                divDashboard.InnerHtml = "<p><div class=warning>Production Decision Pending <br/>for " & countYellow & " Teams </div></p>"
            Else
                divDashboard.InnerHtml = "<p><div class=success>Production Decision done by<br/> all Teams </div></p>"

            End If
            ''''''''''''''''
            '  Return rbTeams.Items.Count - countYellow ''Return no of teams entered data
            Return teamsEnteredData  'return array of team id entered data
        Catch ex As Exception
            DisplayMessage("Error Coloring Teams: " & myquery)
        End Try

    End Function

    Function showDashboard(ByVal isAdmin As Boolean) As Boolean
        '<div class=info>Info message</div>
        '<div class="success">Successful operation message</div>
        '<div class=warning>Warning message</div>
        '<div class="error">Error message</div>
        Dim str = "<p>"
        If isAdmin Then
            divDashboard.InnerHtml = "<p><div class=warning>Production Decision Pending </div></p>"
            divDashboard1.InnerHtml = "<div class=warning>Leadship tagging Pending </div>"
            divDashboard2.InnerHtml = "<div class=info>Inventory Updation Pending </div>"
        Else

        End If
        str = str & "<div class='info'>" & _
            getDBsingle("select 'DemandA-' || demandA || '(' || minA || '/' || maxA || ') ' || '<br/>DemandB-' || demandB || '(' || minB || '/' || maxB || ') ' || '<br/>DemandC-' || demandC || '(' || minC || '/' || maxC || ') ' || '<br/>DemandD-' || demandD || '(' || minD || '/' || maxD || ') ' as v from forecastQuarter where gameid=12 and quarter = " & Session("currentQuarter")) & _
            "</div>"
        '   divDashboard.InnerHtml = "<h3>Decision Dashboard </h3><br/>" & str
    End Function
    Public Sub DisplayMessage(ByVal text As String)
        'divMsg.InnerHtml = text & "<br/>"
        txtConsole.Text = CType(ViewState("displayMessage"), String) & Now.ToString("%H:mm:ss") & ">>" & text & vbCrLf
        divInfo.InnerHtml = Now.ToString("%H:mm:ss") & "> " & text
        ViewState("displayMessage") = txtConsole.Text
    End Sub



    Protected Sub btnGenerateResults_Click(sender As Object, e As EventArgs) Handles btnGenerateResults.Click
        Dim myquery = ""
        Dim teamsEnteredData = colorTeams("production") 'Teamid's who entered data
        Dim totalTeamsEnteredData = teamsEnteredData.Count
        Dim totalTeams = rbTeams.Items.Count
        If totalTeamsEnteredData = 0 Then
            DisplayMessage("No teams have entered data to generate results.")
            Exit Sub ''No Teams have
        End If

        ''' Have a datatable to book all cost in resultCost
        ''' ########################

        Dim dtResultCost As New System.Data.DataTable
        Dim dtCol44 As New System.Data.DataColumn("gameID", System.Type.GetType("System.Decimal"))
        dtCol44.DefaultValue = Session("gameID")
        dtResultCost.Columns.Add(dtCol44)
        Dim dtCol45 As New System.Data.DataColumn("quarter", System.Type.GetType("System.Decimal"))
        dtCol45.DefaultValue = Session("CurrentQuarter")
        dtResultCost.Columns.Add(dtCol45)
        dtResultCost.Columns.Add("teamID", System.Type.GetType("System.Decimal"))
        dtResultCost.Columns.Add("costCenter", System.Type.GetType("System.String"))
        dtResultCost.Columns.Add("cost", System.Type.GetType("System.Decimal"))
        ''' Have a datatable to book all income in resultIncome
        ''' ########################
        Dim dtResultIncome As New System.Data.DataTable
        Dim dtCol46 As New System.Data.DataColumn("gameID", System.Type.GetType("System.Decimal"))
        dtCol46.DefaultValue = Session("gameID")
        dtResultIncome.Columns.Add(dtCol46)
        Dim dtCol47 As New System.Data.DataColumn("quarter", System.Type.GetType("System.Decimal"))
        dtCol47.DefaultValue = Session("CurrentQuarter")
        dtResultIncome.Columns.Add(dtCol47)
        dtResultIncome.Columns.Add("teamID", System.Type.GetType("System.Decimal"))
        dtResultIncome.Columns.Add("profitCenter", System.Type.GetType("System.String"))
        dtResultIncome.Columns.Add("income", System.Type.GetType("System.Decimal"))


        Dim dtAllocation As New System.Data.DataTable
        dtAllocation.Columns.Add("gameID")
        dtAllocation.Columns.Add("quarter")
        dtAllocation.Columns.Add("teamID")
        dtAllocation.Columns.Add("allotA", System.Type.GetType("System.Decimal"))
        dtAllocation.Columns.Add("allotB", System.Type.GetType("System.Decimal"))
        dtAllocation.Columns.Add("allotC", System.Type.GetType("System.Decimal"))
        dtAllocation.Columns.Add("allotD", System.Type.GetType("System.Decimal"))
        dtAllocation.Columns.Add("salesA", System.Type.GetType("System.Decimal"))
        dtAllocation.Columns.Add("salesB", System.Type.GetType("System.Decimal"))
        dtAllocation.Columns.Add("salesC", System.Type.GetType("System.Decimal"))
        dtAllocation.Columns.Add("salesD", System.Type.GetType("System.Decimal"))
        dtAllocation.Columns.Add("totalSales", System.Type.GetType("System.Decimal"))
        dtAllocation.Columns.Add("CashCollectionA", System.Type.GetType("System.Decimal"))
        dtAllocation.Columns.Add("CashCollectionB", System.Type.GetType("System.Decimal"))
        dtAllocation.Columns.Add("CashCollectionC", System.Type.GetType("System.Decimal"))
        dtAllocation.Columns.Add("CashCollectionD", System.Type.GetType("System.Decimal"))
        dtAllocation.Columns.Add("totalCashCollection", System.Type.GetType("System.Decimal"))
        ''computed column not entering in data base
        '  dtAllocation.Columns.Add("cashRecievable", System.Type.GetType("System.Decimal"), "totalSales - totalCashCollection")
        dtAllocation.Columns.Add("cashRecievable", System.Type.GetType("System.Decimal"))


        ' Dim teams(totalTeams) As Integer
        Dim teamindex = 0 ''for row of decisionProduction
        For teamID = 1 To totalTeams
            Dim tmprow = dtAllocation.NewRow
            ''book income
            Dim inRow = dtResultIncome.NewRow
            tmprow(0) = Session("gameID")
            tmprow(1) = Session("currentQuarter")
            tmprow(2) = teamID
            inRow("teamID") = teamID
            inRow("profitCenter") = "ProductSales"
            
            Dim totalSales = 0  'reset for each team
            Dim totalCashCollection = 0  'reset for each team
            For productIndex = 0 To 3
                If Array.IndexOf(teamsEnteredData, teamID) < 0 Then
                    tmprow(3 + productIndex) = 0
                    tmprow(7 + productIndex) = 0
                    tmprow(11) = totalSales
                    tmprow(12 + productIndex) = 0
                    tmprow(16) = totalCashCollection
                    inRow("income") = totalCashCollection
                    tmprow(17) = totalSales - totalCashCollection  '''Dont know recievable in which future quarter
                    DisplayMessage(" Team" & teamID & " NA")
                Else
                    Dim forecastedDemand = If(IsNumeric(gvForecastQuarter.Rows(0).Cells(productIndex).Text), gvForecastQuarter.Rows(0).Cells(productIndex).Text, 0)
                    Dim myUnitCost = If(IsNumeric(gvDecisionProduction.Rows(teamindex).Cells(productIndex + 5).Text), gvDecisionProduction.Rows(teamindex).Cells(productIndex + 5).Text, 0)
                    Dim myProduction = If(IsNumeric(gvStockforAllocation.Rows(teamindex).Cells(productIndex + 1).Text), gvStockforAllocation.Rows(teamindex).Cells(productIndex + 1).Text, 0)
                    Dim avgMarketCost = If(IsNumeric(gvAvgPrice.Rows(0).Cells(productIndex).Text), gvAvgPrice.Rows(0).Cells(productIndex).Text, 0)
                    Dim elasticity = If(IsNumeric(gvElasticity.Rows(0).Cells(productIndex).Text), gvElasticity.Rows(0).Cells(productIndex).Text, 6)
                    Dim cashCollection = If(IsNumeric(gvCashCollection.Rows(0).Cells(productIndex).Text), gvCashCollection.Rows(0).Cells(productIndex).Text, 6)

                    Dim myAllotment = allocateProductAsPerDemand(forecastedDemand, totalTeamsEnteredData, myUnitCost, avgMarketCost, elasticity, myProduction)
                    ''+3 for gameID, quarter, teamid
                    tmprow(3 + productIndex) = myAllotment
                    '' +3+4 for allotA/B/C/D
                    Dim mySales = myAllotment * myUnitCost
                    tmprow(7 + productIndex) = mySales
                    totalSales = totalSales + mySales
                    tmprow(11) = totalSales
                    '' +3+4 for allotA/B/C/D +1 totalsales +4 forcashA/B/C/D
                    Dim myCashCollection = mySales * cashCollection * 0.01
                    tmprow(12 + productIndex) = myCashCollection
                    totalCashCollection = totalCashCollection + myCashCollection
                    tmprow(16) = totalCashCollection
                    inRow("income") = totalCashCollection
                    tmprow(17) = totalSales - totalCashCollection

                    DisplayMessage("Team" & teamID & "-" & forecastedDemand & " " & totalTeams & " " & myUnitCost & " " & avgMarketCost & " " & elasticity & " =>" & myAllotment)
                End If

            Next
            dtAllocation.Rows.Add(tmprow)
            dtResultIncome.Rows.Add(inRow)
         
            If Not Array.IndexOf(teamsEnteredData, teamID) < 0 Then teamindex = teamindex + 1 ''get next row of grid decisionProduction

        Next

        
        '' I have to work upon it.. updation of records
        Dim res = updateTableinDBwithDeleteOld(dtAllocation, "resultDemand", "delete from resultDemand where gameID=" & Session("gameID") & " and quarter=" & Session("CurrentQuarter"))
        If res.Contains("ok") Then DisplayMessage("resultDemand allocation data updated followed by old delete.") Else DisplayMessage("resultDemand allocation data not updated followed by old delete." & res)

      
        gvAllocation.DataSource = dtAllocation
        gvAllocation.DataBind()

        ''' Inventoryaftersales
        ''' 
        ' Dim dtInventoryAfterSales = dtAllocation.DefaultView.ToTable(False, "teamID", "allotA", "allotB", "allotC", "allotD")
        ' Dim primaryKey(0) As System.Data.DataColumn
        ' primaryKey(0) = dtInventoryAfterSales.Columns(0)
        '   dtInventoryAfterSales.PrimaryKey = New System.Data.DataColumn() {dtInventoryAfterSales.Columns("teamID")}
        ' dtStockBeforeSales.PrimaryKey = New System.Data.DataColumn() {dtStockBeforeSales.Columns("teamID")}
        ' dtStockBeforeSales.PrimaryKey = primaryKey
        ''dtInventoryAfterSales.PrimaryKey = CType(dtInventoryAfterSales.Columns(2), System.Data.DataColumn)
        ' dtInventoryAfterSales.Merge(dtStockBeforeSales, True, Data.MissingSchemaAction.Ignore)
        Dim dtInventoryAfterSales = CType(ViewState("StockBeforeSales"), System.Data.DataTable).Clone
        Dim dtStockBeforeSales = CType(ViewState("StockBeforeSales"), System.Data.DataTable).DefaultView
        ''Cost calculation
        Dim dtManpower = CType(ViewState("dtManpower"), System.Data.DataTable).DefaultView

        '' Following loop covers all teams so use this for dataview filters to port on other databases
        For Each dr In dtAllocation.Rows
            Dim tmpRow = dtInventoryAfterSales.NewRow
            dtStockBeforeSales.RowFilter = "teamID = " & dr("teamID")
            ''cost booking
            dtManpower.RowFilter = "teamID = " & dr("teamID")
            Dim mpRow = dtResultCost.NewRow
            mpRow("teamID") = dr("teamID")

            tmpRow(0) = dr("teamID")
            '  tmpRow(1) = dr("allotA")
            If dtStockBeforeSales.Count > 0 Then
                tmpRow(1) = dtStockBeforeSales(0)(1) - dr("allotA")
                tmpRow(2) = dtStockBeforeSales(0)(2) - dr("allotB")
                tmpRow(3) = dtStockBeforeSales(0)(3) - dr("allotC")
                tmpRow(4) = dtStockBeforeSales(0)(4) - dr("allotD")
            Else
                tmpRow(1) = 0
                tmpRow(2) = 0
                tmpRow(3) = 0
                tmpRow(4) = 0
            End If
            If dtManpower.Count > 0 Then
                mpRow("costCenter") = "Manpower"
                mpRow("cost") = dtManpower(0)("TotalCost")
            Else
                mpRow("costCenter") = "Manpower"
                mpRow("cost") = 0
            End If

            dtInventoryAfterSales.Rows.Add(tmpRow)
            dtResultCost.Rows.Add(mpRow)
        Next
        gvStockAfterSales.DataSource = dtInventoryAfterSales
        gvStockAfterSales.DataBind()

        ''Tendering Item to be outsourced
        Dim dtOutsourced = dtInventoryAfterSales.Clone
        Dim dtInventoryAfterTender = dtInventoryAfterSales.Clone
        Dim dtTenderResults = CType(ViewState("tenderResults"), System.Data.DataTable).DefaultView
        Dim dtrawMaterialCost = CType(ViewState("dtrawMaterialCost"), System.Data.DataTable).DefaultView
        'adding or manipulating columns of  dtRawMaterialCost at time of creation may affect wrong results in all types of inventory. So be cautius.
        dtOutsourced.Columns.Add("permissibleX")
        dtOutsourced.Columns.Add("permissibleY")
        dtOutsourced.Columns.Add("spotX")
        dtOutsourced.Columns.Add("spotY")
        ''cost and profit to be developed
        dtOutsourced.Columns.Add("CostA", System.Type.GetType("System.Decimal"))
        dtOutsourced.Columns.Add("CostB", System.Type.GetType("System.Decimal"))
        dtOutsourced.Columns.Add("CostC", System.Type.GetType("System.Decimal"))
        dtOutsourced.Columns.Add("CostD", System.Type.GetType("System.Decimal"))
       
       
        '' If dont have empanneled vendor then buy spot rate 
        Dim dtMarketCostView = CType(ViewState("dtMarketCost"), System.Data.DataTable).DefaultView

        'Raw materialcost already calculated in F
        ' dtOutsourced.Columns.Add("CostX")
        ' dtOutsourced.Columns.Add("CostY")
        ' dtOutsourced.Columns.Add("CostSpotX")
        ' dtOutsourced.Columns.Add("CostSpotY")
        '' collection will depend on each tender cost
        dtOutsourced.Columns.Add("totalFinishedProductCost")
        

        dtInventoryAfterTender.Columns.Add("closingX")
        dtInventoryAfterTender.Columns.Add("closingY")


        For Each dr In dtInventoryAfterSales.Rows
            Dim tmpRow = dtOutsourced.NewRow
            Dim tmRowClose = dtInventoryAfterTender.NewRow
            Dim rawcostRow = dtResultCost.NewRow   ''book raw material cost of each team
            Dim finishedproductcostRow = dtResultCost.NewRow   ''book raw material cost of each team
            dtTenderResults.RowFilter = "teamID = " & dr("teamID")
            dtrawMaterialCost.RowFilter = "teamID = " & dr("teamID")
            dtMarketCostView.RowFilter = "teamID = " & dr("teamID") 'teamID 0 for spot rates
            rawcostRow("teamID") = dr("teamID")
            rawcostRow("costCenter") = "RawMaterial"
            finishedproductcostRow("teamID") = dr("teamID")
            finishedproductcostRow("costCenter") = "FinishedProducts"
            tmpRow(0) = dr("teamID")
            tmRowClose(0) = dr("teamID")
            '  tmpRow(1) = dr("allotA")
            If dtTenderResults.Count > 0 Then  ''this team id has some data..
                If dtTenderResults(0)(1) > dr(1) Then
                    tmpRow(1) = dtTenderResults(0)(1) - dr(1) ''items to be outsourced
                    tmRowClose(1) = dr(1)
                Else
                    tmpRow(1) = 0
                    tmRowClose(1) = dr(1) - dtTenderResults(0)(1)
                End If
                If dtTenderResults(0)(2) > dr(2) Then
                    tmpRow(2) = dtTenderResults(0)(2) - dr(2)
                    tmRowClose(2) = dr(2)
                Else
                    tmpRow(2) = 0
                    tmRowClose(2) = dr(2) - dtTenderResults(0)(2)
                End If

                If dtTenderResults(0)(3) > dr(3) Then
                    tmpRow(3) = dtTenderResults(0)(3) - dr(3)
                    tmRowClose(3) = dr(3)
                Else
                    tmpRow(3) = 0
                    tmRowClose(3) = dr(3) - dtTenderResults(0)(3)
                End If
                If dtTenderResults(0)(4) > dr(4) Then
                    tmpRow(4) = dtTenderResults(0)(4) - dr(4)
                    tmRowClose(4) = dr(4)
                Else
                    tmpRow(4) = 0
                    tmRowClose(4) = dr(4) - dtTenderResults(0)(4)
                End If

            Else
                tmpRow(1) = 0
                tmpRow(2) = 0
                tmpRow(3) = 0
                tmpRow(4) = 0
                ''No consumption in tendering so late inventory intact
                tmRowClose(1) = dr(1)
                tmRowClose(2) = dr(2)
                tmRowClose(3) = dr(3)
                tmRowClose(4) = dr(4)

            End If
            '' Closing inventory of X & Y
            If dtrawMaterialCost.Count > 0 Then
                tmRowClose(5) = dtrawMaterialCost(0)("closingX")
                tmRowClose(6) = dtrawMaterialCost(0)("closingY")
                '' spot raw material purchase from market
                tmpRow(5) = dtrawMaterialCost(0)("permissibleX")
                tmpRow(6) = dtrawMaterialCost(0)("permissibleY")
                tmpRow("spotX") = dtrawMaterialCost(0)("spotX")
                tmpRow("spotY") = dtrawMaterialCost(0)("spotY")
                rawcostRow("cost") = dtrawMaterialCost(0)("freshTotalvalue")
            Else
                tmRowClose(5) = 0
                tmRowClose(6) = 0
                tmpRow(5) = 0
                tmpRow(6) = 0
                tmpRow(7) = 0
                tmpRow(8) = 0
                rawcostRow("cost") = 0
            End If

            '' Get cost from market / Empanneled Vendors --  Spot rate logic not developed (For Future)
            If dtMarketCostView.Count > 0 Then
                tmpRow("CostA") = dtMarketCostView(0)("CostA") * tmpRow(1)
                tmpRow("CostB") = dtMarketCostView(0)("CostB") * tmpRow(2)
                tmpRow("CostC") = dtMarketCostView(0)("CostC") * tmpRow(3)
                tmpRow("CostD") = dtMarketCostView(0)("CostD") * tmpRow(4)
                tmpRow("totalFinishedProductCost") = tmpRow("CostA") + tmpRow("CostB") + tmpRow("CostC") + tmpRow("CostD")
                finishedproductcostRow("Cost") = tmpRow("totalFinishedProductCost")
            End If
            dtOutsourced.Rows.Add(tmpRow)
            dtInventoryAfterTender.Rows.Add(tmRowClose)
            dtResultCost.Rows.Add(rawcostRow)
            dtResultCost.Rows.Add(finishedproductcostRow)
        Next
        gvOutsourced.DataSource = dtOutsourced
        gvOutsourced.DataBind()
        Dim dtWarehousing = getDBTable("select warehouseA, warehouseB, warehouseC, warehouseD, warehouseX, warehouseY from productMaster where business='" & Session("BusinessType") & "'")
        Dim dtStockAfterSalesTender = getWareHousingCost(dtInventoryAfterTender, dtWarehousing).DefaultView
        gvStockAfterSalesTender.DataSource = dtStockAfterSalesTender
        gvStockAfterSalesTender.DataBind()
        ''do cost booking
        For Each item As ListItem In rbTeams.Items
            Dim warehouseRow = dtResultCost.NewRow   ''book raw material cost of each team
            Dim subscriptionRow = dtResultCost.NewRow   ''book subscription cost of each team

            dtStockAfterSalesTender.RowFilter = "teamID = " & item.Value
            warehouseRow("teamID") = item.Value
            warehouseRow("costCenter") = "Warehouse"

            subscriptionRow("teamID") = item.Value
            subscriptionRow("costCenter") = "Subscription"
            Dim subscriptionCost = getDBsingle("select sum(cost) from subscriptionMaster, subscriptionDetail where subscriptionid = uid and teamID=" & item.Value & " and gameID=" & Session("gameID"))
            If subscriptionCost.Contains("Error") Or String.IsNullOrEmpty(subscriptionCost) Then subscriptionCost = 0
            subscriptionRow("cost") = subscriptionCost
            If dtStockAfterSalesTender.Count > 0 Then
                warehouseRow("cost") = dtStockAfterSalesTender(0)("WarehousingCost")
            Else
                warehouseRow("cost") = 0
            End If
            dtResultCost.Rows.Add(warehouseRow)
            dtResultCost.Rows.Add(subscriptionRow)
        Next

        Dim dtCol21 As New System.Data.DataColumn("gameID", System.Type.GetType("System.Decimal"))
        dtCol21.DefaultValue = Session("gameID")
        dtInventoryAfterTender.Columns.Add(dtCol21)
        Dim dtCol22 As New System.Data.DataColumn("quarter", System.Type.GetType("System.Decimal"))
        dtCol22.DefaultValue = Session("CurrentQuarter")
        dtInventoryAfterTender.Columns.Add(dtCol22)
        ''changing column name for sake of data insertion
        DisplayMessage("changing column name for sake of data insertion in  resultInventory")
        dtInventoryAfterTender.Columns(1).ColumnName = "productA"
        dtInventoryAfterTender.Columns(2).ColumnName = "productB"
        dtInventoryAfterTender.Columns(3).ColumnName = "productC"
        dtInventoryAfterTender.Columns(4).ColumnName = "productD"
        dtInventoryAfterTender.Columns(5).ColumnName = "closingX"
        dtInventoryAfterTender.Columns(6).ColumnName = "closingY"
        res = updateTableinDBwithDeleteOld(dtInventoryAfterTender, "resultInventory", "delete from resultInventory where gameID=" & Session("gameID") & " and quarter=" & Session("CurrentQuarter"))
        If res.Contains("ok") Then DisplayMessage("resultInventory allocation data updated followed by old delete.") Else DisplayMessage("resultInventory allocation data not updated followed by old delete." & res)


        ''''Same thing for dtOutsource to enter data for market purchase
        Dim dtCol23 As New System.Data.DataColumn("gameID", System.Type.GetType("System.Decimal"))
        dtCol23.DefaultValue = Session("gameID")
        dtOutsourced.Columns.Add(dtCol23)
        Dim dtCol24 As New System.Data.DataColumn("quarter", System.Type.GetType("System.Decimal"))
        dtCol24.DefaultValue = Session("CurrentQuarter")
        dtOutsourced.Columns.Add(dtCol24)
        ''changing column name for sake of data insertion
        DisplayMessage("changing column name for sake of data insertion in  resultInventory")
        dtOutsourced.Columns(1).ColumnName = "productA"
        dtOutsourced.Columns(2).ColumnName = "productB"
        dtOutsourced.Columns(3).ColumnName = "productC"
        dtOutsourced.Columns(4).ColumnName = "productD"
        'res = updateTableinDBwithDeleteOld(dtOutsourced, "resultMarket", "delete from resultMarket where gameID=" & Session("gameID") & " and quarter=" & Session("CurrentQuarter"))
        If res.Contains("ok") Then DisplayMessage("resultMarket allocation data updated followed by old delete.") Else DisplayMessage("resultMarket allocation data not updated followed by old delete." & res)

        ''cost booking this quarter
        ''#########################33
        ''Get more cost for this quarter Here (also Tender Cost / bank loan/Subscription

        gvResultQuarter.DataSource = dtResultCost
        gvResultQuarter.DataBind()
        ' res = updateTableinDBwithDeleteOld(dtResultCost, "resultCost", "delete from resultCost where gameID=" & Session("gameID") & " and quarter=" & Session("CurrentQuarter"))
        If res.Contains("ok") Then DisplayMessage("resultCost allocation data updated followed by old delete.") Else DisplayMessage("resultCost allocation data not updated followed by old delete." & res)

        ''' income booking this quarter
        ''' ########################
        ''' Get more income for this quarter here (also tender Income/Bank interest

        gvResultIncome.DataSource = dtResultIncome
        gvResultIncome.DataBind()
        res = updateTableinDBwithDeleteOld(dtResultIncome, "resultIncome", "delete from resultIncome where gameID=" & Session("gameID") & " and quarter=" & Session("CurrentQuarter"))
        If res.Contains("ok") Then DisplayMessage("resultIncome allocation data updated followed by old delete.") Else DisplayMessage("resultIncome allocation data not updated followed by old delete." & res)

    End Sub

    Protected Sub rbTeamsView_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rbTeamsView.SelectedIndexChanged
        Dim myquery = ""
        Try

            Dim teamid = rbTeamsView.SelectedItem.Value
            Dim productA, productB, productC, productD As String
            myquery = "select productA, productB, productC, productD  from productMaster where business = '" & Session("BusinessType") & "'"

            Dim dtProduct = getDBTable(myquery)

            productA = dtProduct.Rows(0)(0)
            productB = dtProduct.Rows(0)(1)
            productC = dtProduct.Rows(0)(2)
            productD = dtProduct.Rows(0)(3)

            Session("teamID") = teamid
            ''Finished Products
            myquery = "  select 1 as SN,  'Opening Stock' as Desc,  productA as " & productA & ", productB as " & productB & ", productC  as " & productC & ", productD  as " & productD & " from resultInventory where teamID=" & Session("teamID") & " and gameID=" & Session("gameID") & " and quarter=" & Session("CurrentQuarter") & "-1" & _
                " union select 2 as SN, 'Production' as Desc, productA as " & productA & ", productB as " & productB & ", productC  as " & productC & ", productD  as " & productD & " from decisionProduction where teamID=" & Session("teamID") & " and gameID=" & Session("gameID") & " and quarter=" & Session("CurrentQuarter") & _
                 "  union select 3 as SN, 'Market Sales' as Desc,  allotA as " & productA & ", allotB as " & productB & ", allotC as " & productC & ", allotD as " & productD & " from resultDemand where teamID=" & Session("teamID") & " and gameID=" & Session("gameID") & " and quarter=" & Session("CurrentQuarter") & _
                " union select 4 as SN, 'Tender Sales' as Desc, sum(productA) as " & productA & ", sum(productB) as " & productB & ", sum(productC)  as " & productC & ", sum(productD)  as " & productD & " from resultTender where teamID=" & Session("teamID") & " and gameID=" & Session("gameID") & " and supplyquarter=" & Session("CurrentQuarter") & " group by teamid" & _
                 " union select 5 as SN,  'Closing Stock' as Desc, productA as " & productA & ", productB as " & productB & ", productC  as " & productC & ", productD  as " & productD & " from resultInventory where teamID=" & Session("teamID") & " and gameID=" & Session("gameID") & " and quarter=" & Session("CurrentQuarter") & _
                   " union select 6 as SN, 'Selling Price' as Desc, priceA as " & productA & ", priceB as " & productB & ", priceC as " & productC & ", priceD as " & productD & " from decisionProduction where teamID=" & Session("teamID") & " and gameID=" & Session("gameID") & " and quarter=" & Session("CurrentQuarter") & _
                    " union select 7 as SN, 'Market Price' as Desc,  avg(priceA) as " & productA & ", avg(priceB) as " & productB & ", avg(priceC) as " & productC & ", avg(priceD) as " & productD & " from decisionProduction where gameid = " & Session("gameID") & " and quarter=" & Session("currentQuarter")

            Dim dtTeamInventoryData = getDBTable(myquery)
            dtTeamInventoryData.Rows.InsertAt(dtTeamInventoryData.NewRow, 5)
            gvTeamInventoryData.DataSource = dtTeamInventoryData
            gvTeamInventoryData.DataBind()
            gvTeamInventoryData.Caption = rbTeamsView.SelectedItem.Text
            DisplayMessage(rbTeamsView.SelectedItem.Text & " Shown Finished Products")

            ''Show raw material
            myquery = "  select 1 as SN,  'Opening Stock' as Desc,  openingX, openingY from resultInventory where teamID=" & Session("teamID") & " and gameID=" & Session("gameID") & " and quarter=" & Session("CurrentQuarter") & "" & _
                     " union  select 2 as SN,  'Scheduled Purchase' as Desc,  rawX, rawY from decisionProduction where teamID=" & Session("teamID") & " and gameID=" & Session("gameID") & " and quarter=" & Session("CurrentQuarter") & _
                     " union select 3 as SN, 'Required Material' as desc, (d.productA * p.qtyRawXforA) + (d.productB * p.qtyRawXforB) + (d.productC * p.qtyRawXforC) + (d.productD * p.qtyRawXforD) as rawX, (d.productA * p.qtyRawYforA) + (d.productB * p.qtyRawYforB) + (d.productC * p.qtyRawYforC) + (d.productD * p.qtyRawYforD) as rawY  from productMaster p, decisionProduction d where p.business='" & Session("BusinessType") & "' and d.quarter = " & Session("currentQuarter") & " and d.teamID=" & Session("teamID") & _
                     " union  select 4 as SN,  'Market Purchase' as Desc,  rawX, rawY from resultMarket where teamID=" & Session("teamID") & " and gameID=" & Session("gameID") & " and quarter=" & Session("CurrentQuarter") & _
                      " union  select 5 as SN,  'Spot Purchase' as Desc,  spotRawX as rawX, spotRawY as rawY from resultMarket where teamID=" & Session("teamID") & " and gameID=" & Session("gameID") & " and quarter=" & Session("CurrentQuarter") & _
                        " union select 6 as SN,  'Closing Stock' as Desc, closingX, closingY from resultInventory where teamID=" & Session("teamID") & " and gameID=" & Session("gameID") & " and quarter=" & Session("CurrentQuarter")
            gvTeamRawMaterial.DataSource = getDBTable(myquery)
            gvTeamRawMaterial.DataBind()
            DisplayMessage(rbTeamsView.SelectedItem.Text & " Shown raw material")

            myquery = "select costCenter, cost from resultCost where teamID=" & Session("teamID") & " and gameID=" & Session("gameID") & " and quarter=" & Session("CurrentQuarter")
            gvIncomeStatement.DataSource = getDBTable(myquery)
            gvIncomeStatement.DataBind()

            myquery = "select profitCenter, income from resultIncome where teamID=" & Session("teamID") & " and gameID=" & Session("gameID") & " and quarter=" & Session("CurrentQuarter")
            gvIncome.DataSource = getDBTable(myquery)
            gvIncome.DataBind()
            colorTeamsView(Session("mode"))

        Catch ex As Exception
            DisplayMessage("Error in rbTeamsView & " & ex.Message & myquery)
        End Try
    End Sub
End Class
