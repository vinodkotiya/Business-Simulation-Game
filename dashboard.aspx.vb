Imports common
Imports dbOperation
Partial Class dashboard
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("gameID") Is Nothing Then
            Response.Redirect("admin.aspx?source=dashboard")
        End If
        If Not Page.IsPostBack Then

            Dim submenu = 0


            divTeamDetail.InnerHtml = "<h3>Team ID: " & Session("user") & "</h3>" & _
                "<p>Business: " & Session("BusinessType") & "<br/>" & _
                "Game ID :" & Session("GameID") & "<br/> </p>"

            div_submenu.InnerHtml = getsubMenu("dashboard", submenu)
            myinfo.InnerHtml = "Wealth: $xxx " & Session("user") & "<span>|</span><a href=admin.aspx?mode=kill>Logout</a>"
            Dim myquery = ""
            If Session("user") = "admin" Then
                showDashboard(True)
                myquery = "select teamid as v, teamname as d from teamMaster where gameid=" & Session("gameID")

                rbTeamsView.DataSource = getDBTable(myquery)
                rbTeamsView.DataBind()
            Else
                showDashboard(False)
                
                End If
            showGameConditions()

            Dim wealth = "NA"
            If Not Session("teamID") Is Nothing Then wealth = getDBsingle("select wealth from teamMaster where teamID=" & Session("teamID") & " and gameID=" & Session("gameID"))
            myinfo.InnerHtml = "Wealth: $ " & wealth & " " & Session("user") & "<span>|</span><a href=admin.aspx?mode=kill>Logout</a>"

        End If
    End Sub
    Function showGameConditions()
        Dim productA, productB, productC, productD As String
        Dim myquery = "select productA, productB, productC, productD  from productMaster where business = '" & Session("BusinessType") & "'"

        Dim dtProduct = getDBTable(myquery)

        productA = dtProduct.Rows(0)(0)
        productB = dtProduct.Rows(0)(1)
        productC = dtProduct.Rows(0)(2)
        productD = dtProduct.Rows(0)(3)
        'ViewState("logistics") = getDBTable("select manpowerslab1 from productMaster where business='" & Session("BusinessType") & "'")
        Dim dtLogistics = getDBTable("select 1 as SN, 'up to ' || manpowerQtySlab1 as Production, manpowerSlab1 as Cost from productMaster where business = '" & Session("BusinessType") & "' union select 2 as SN,'between ' || manpowerQtySlab1 || '-' || manpowerQtySlab2 as Production, manpowerSlab2  as Cost from productMaster where business = '" & Session("BusinessType") & "' union select  3 as SN, 'between ' || manpowerQtySlab2 || '-' || manpowerQtySlab3 as Production, manpowerSlab3  as Cost from productMaster where business = '" & Session("BusinessType") & "' union select 4 as SN, 'more than ' || manpowerQtySlab3 as Production, manpowerSlab4  as Cost  from productMaster where business='" & Session("BusinessType") & "'")
        dtLogistics.Columns.RemoveAt(0)
        gvLogistics.DataSource = dtLogistics
        gvLogistics.DataBind()

        gvFixedCost.DataSource = getDBTable("select fixedCostRawX, fixedCostRawY, spotCostRawX, spotCostRawY from productMaster where business='" & Session("BusinessType") & "'")
        gvFixedCost.DataBind()

        Dim dtRatio = getDBTable("select 1 as SN, '" & productA & "' as Product, QtyRawXforA as rawX, QtyRawYforA as rawY from productMaster where business = '" & Session("BusinessType") & "' union select 2 as SN, '" & productB & "' as Product, QtyRawXforB as rawX, QtyRawYforB as rawY from productMaster where business = '" & Session("BusinessType") & "' union select 3 as SN, '" & productC & "' as Product, QtyRawXforC as rawX, QtyRawYforC as rawY from productMaster where business = '" & Session("BusinessType") & "' union select 4 as SN, '" & productD & "' as Product, QtyRawXforD as rawX, QtyRawYforD as rawY from productMaster where business='" & Session("BusinessType") & "'")
        dtRatio.Columns.RemoveAt(0)
        gvRawRatio.DataSource = dtRatio
        gvRawRatio.DataBind()
        ''Warehousing
        Dim dtWarehousing = getDBTable("select 1 as SN, '" & productA & "' as Product,  warehouseA as Cost from productMaster where business = '" & Session("BusinessType") & "' union select 2 as SN, '" & productB & "' as Product, warehouseB as Cost from productMaster where business = '" & Session("BusinessType") & "' union select 3 as SN, '" & productC & "' as Product, warehouseC as Cost from productMaster where business = '" & Session("BusinessType") & "' union select 4 as SN, '" & productD & "' as Product, warehouseD as Cost from productMaster where business='" & Session("BusinessType") & "' union select 5 as SN, 'RawX' as Product, warehouseX as Cost from productMaster where business='" & Session("BusinessType") & "' union select 6 as SN, 'RawY' as Product, warehouseY as Cost from productMaster where business='" & Session("BusinessType") & "'")
        dtWarehousing.Columns.RemoveAt(0)
        gvWarehousing.DataSource = dtWarehousing
        gvWarehousing.DataBind()
        ''Demand
        Dim dtDemand = getDBTable("select 1 as SN, '" & productA & "' as Product , demandA as Demand,  minA as Min, maxA as Max from forecastQuarter where gameid=" & Session("gameID") & " and quarter=" & Session("CurrentQuarter") & " union select 2 as SN, '" & productB & "' as Product , demandB as Demand,  minB as Min, maxB as Max from forecastQuarter where gameid=" & Session("gameID") & " and quarter=" & Session("CurrentQuarter") & " union select 3 as SN, '" & productC & "' as Product , demandC as Demand,  minC as Min, maxC as Max from forecastQuarter where gameid=" & Session("gameID") & " and quarter=" & Session("CurrentQuarter") & " union select 4 as SN, '" & productD & "'  as Product , demandD as Demand,  minD as Min, maxD as Max from forecastQuarter where gameid=" & Session("gameID") & " and quarter=" & Session("CurrentQuarter"))
        dtDemand.Columns.RemoveAt(0)
        gvDemand.DataSource = dtDemand
        gvDemand.DataBind()
        ''Tender to fullfill
        Dim dtTender = getDBTable("select tenderID, productA as " & productA & ", productB as " & productB & ", productC as " & productC & ", productD as " & productD & "  from resultTender where  supplyQuarter = " & Session("CurrentQuarter") & " and gameID=" & Session("gameID") & " and teamID = " & Session("teamID"))
        gvTenderResults.DataSource = decisionEngine.pivot2colTable(dtTender)
        gvTenderResults.DataBind()
        myquery = "select plantcapacity  from teamMaster where gameid=" & Session("gameID") & " and teamid=" & Session("teamID")
        If Session("user") = "admin" Then myquery = "select plantcapacity  from teamMaster where gameid=" & Session("gameID") & " and teamid=1"

        Dim plantcapacity = getDBsingle(myquery)
        If plantcapacity.Contains("Error") Then plantcapacity = "NA"
        myquery = "select  addcapacity from teamMaster where gameid=" & Session("gameID") & " and teamid=" & Session("teamID")
        Dim addCap = getDBsingle(myquery)
        If addCap.Contains("Error") Then addCap = "0"
        myquery = "select CapacityCost from productMaster where business='" & Session("BusinessType") & "'"
        Dim capCost = getDBsingle(myquery)
        divCapacity.InnerHtml = "<li><a>Capacity:</a> " & plantcapacity & "</li> <li><a>Addnl Cap:</a>" & addCap & "</li>" & _
            "<li>Augmentation Cost:" & capCost & "</li>"

        myquery = " select  '<li>" & productA & ": ' || productA  || '</li><li>" & productB & ": ' || productB || '</li><li>" & productC & ": ' || productD  || '</li><li>" & productD & ": ' || productD || '</li><li>rawX: ' || rawX || '</li>rawY: ' || rawY || '</li>' as v from resultInventory where teamID=" & Session("teamID") & " and gameID=" & Session("gameID") & " and quarter=" & Session("CurrentQuarter") & "-1"
        Dim inv = getDBsingle(myquery)
        If inv.Contains("Error") Then inv = "Admin dosent Has Inventory, Choose Any Team Instead."
        divInventory.InnerHtml = inv
        ' myquery = "select subscription, teamId from subscriptionMaster left join subscriptionDetail on uid = subscriptionID and  gameid=" & Session("gameid") & " and teamid=" & Session("teamid")
        myquery = "select subscriptionID from subscriptionDetail where gameid=" & Session("gameid") & " and teamid=" & Session("teamid")
        If Session("teamID") Is Nothing Then myquery = "select -1 as subscriptionID from subscriptionDetail"
        Dim dtSubscriptionDetail = getDBTable(myquery).DefaultView
        Dim dtSubscriptionMaster = getDBTable("select uid, subscription from subscriptionMaster")
        '' now replace teamid with Yes
        Dim str1 = ""
        For Each dr In dtSubscriptionMaster.Rows
            'If String.IsNullOrEmpty(dr(1).ToString) Then
            '    '     str1 = str1 & "<li>" & dr(0).ToString & ": " & dr(1).ToString.Replace(Session("teamId"), "Yes") & "</li>"
            'Else
            dtSubscriptionDetail.RowFilter = "subscriptionID = " & dr("uid")
            If dtSubscriptionDetail.Count > 0 Then
                str1 = str1 & "<li><div class=Iyes>" & dr(1).ToString & ": Yes</div></li>"

            Else
                str1 = str1 & "<li><div class=Ino>" & dr(1).ToString & ": No</div></li>"

            End If
            'End If
            ' DisplayMessage(dr(0).ToString)
        Next
        'gvFixedCost.DataSource = dtSubscription
        'gvFixedCost.DataBind()
        ' DisplayMessage(myquery & dtSubscription.Rows.Count)
        divSubscription.InnerHtml = str1

        Return True
    End Function
    Function showDashboard(ByVal isAdmin As Boolean) As Boolean
        '<div class=info>Info message</div>
        '<div class="success">Successful operation message</div>
        '<div class=warning>Warning message</div>
        '<div class="error">Error message</div>
        Dim str = "<p>"
       
        If isAdmin Then
            divDashboard.InnerHtml = "<p><div class=warning>Result Generation Pending </div></p>"
            divDashboard1.InnerHtml = "<div class=warning>Tender Process Pending </div>"
            divDashboard2.InnerHtml = "<div class=info>Sales Allocation Pending </div>"
        Else
            Dim myquery = "select teamid from decisionProduction where gameid = " & Session("gameID") & " and quarter = " & Session("CurrentQuarter") & " and teamid = " & Session("teamID")
            If getDBsingle(myquery).Contains("Error") Then
                divDashboard.InnerHtml = "<p><div class=warning>Production Decision Pending </div></p>"
            Else
                divDashboard.InnerHtml = "<p><div class=success>Production Decision Done. </div></p>"
            End If
            myquery = "select teamid from decisionCapacity where gameid=" & Session("gameid") & " and teamid=" & Session("teamid") & " and quarter=" & Session("currentQuarter") & " "
            If getDBsingle(myquery).Contains("Error") Then
                divDashboard1.InnerHtml = "<p><div class=info>Capacity Not Added </div></p>"
                DisplayMessage(myquery)
            Else
                divDashboard1.InnerHtml = "<p><div class=success>Capacity Decision Done. </div></p>"
            End If
            myquery = "select teamid from decisionFinance where gameid=" & Session("gameid") & " and teamid=" & Session("teamid") & " and quarter=" & Session("currentQuarter") & " "
            If getDBsingle(myquery).Contains("Error") Then
                divDashboard2.InnerHtml = "<p><div class=info>Finance Decision Not Done. </div></p>"

            Else
                divDashboard2.InnerHtml = "<p><div class=success>Finance Decision Done. </div></p>"
            End If
           '' pnlLogistics.Visible = True
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


    
    Protected Sub rbTeamsView_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rbTeamsView.SelectedIndexChanged
        Dim teamid = rbTeamsView.SelectedItem.Value
        Session("teamID") = teamid
        showDashboard(False)
        showGameConditions()
    End Sub
End Class
