Imports common
Imports dbOperation
Partial Class admin
    Inherits System.Web.UI.Page
    Public gameID As String

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            pnlTeam.Visible = False
            pnlGame.Visible = False
            pnlConditions.Visible = False
            pnlLogistics.Visible = False
            pnlL.Visible = False
            pnlTenders.Visible = False
            Dim submenu = 0
            If Request.Params("mode") = "kill" Then
                Session.Abandon()
                Exit Sub
            End If
            If Session("user") Is Nothing Then
                pnlLogin.Visible = True  ''Let them login first
                
                Exit Sub
            ElseIf Not Session("user") = "admin" Then
                Response.Redirect("dashboard.aspx")
                pnlLogin.Visible = False
            Else
                pnlLogin.Visible = False
            End If
            If Session("gameID") Is Nothing And Session("user") = "admin" Then

                pnlGame.Visible = True
                DisplayMessage("Create New Game.. You can also Enter in an Existing  game previously created.")
                ''    dbOperation.executeDB("delete from teamMaster where 1")
                rblBuiseness.DataSource = getDBTable("select bid as v, business as d from productMaster ")
                rblBuiseness.DataBind()
                rblBuiseness.SelectedIndex = 9
                populateProducts(10)
                lbCostCenters.DataSource = getDBTable("select decisions as v, descr as d from decisionMaster")
                lbCostCenters.DataBind()
                Exit Sub
            ElseIf Request.Params("mode") = "game" Then

                DisplayMessage("Game already created with id " & Session("gameID") & " or Click Here to Create <a href=admin.aspx?mode=game> New Game</a>")
            
            ' DisplayMessage("Game already created.")
                End If
                If Request.Params("mode") = "teams" Then

                    ViewState("teamMaster") = getDBTable("select * from teamMaster where gameid=" & Session("gameID"))
                    gvTeams.DataSource = ViewState("teamMaster")
                    gvTeams.DataBind()
                    pnlTeam.Visible = True
                    submenu = 1
                    DisplayMessage("Teams Created. You can change Team Name as well as enter no of members in each team(Optional)")
            ElseIf Request.Params("mode") = "conditions" Then
                ViewState("elasticity") = getDBTable("select elasticityA, elasticityB, elasticityC, elasticityD from productMaster where business='" & Session("BusinessType") & "'")
                gvElasticity.DataSource = ViewState("elasticity")
                gvElasticity.DataBind()
                DisplayMessage("These settings are imported from last Game. Click on Edit to change.")
                DisplayMessage("Initial Demand should be less than no of Teams X production capacity of each.")
                ViewState("forecastQuarter") = getDBTable("select * from forecastQuarter where gameid=" & Session("gameID") & " order by quarter")
                gvForecast.DataSource = ViewState("forecastQuarter")
                gvForecast.DataBind()
                pnlConditions.Visible = True
                txtCurrentQuarter.Text = Session("CurrentQuarter")
                submenu = 2

                ElseIf Request.Params("mode") = "logistics" Then
                    DisplayMessage("These are default settings used in last Game. Click on Edit to Change. <br/> > Manpower Slab (Cost/Unit). <br/> > Warehouse charges productwise (Cost/Unit). <br/> > Capacity Augmentation (Cost/Unit)")
                ViewState("logistics") = getDBTable("select manpowerslab1, manpowerslab2, manpowerslab3, manpowerslab4, manpowerQtySlab1, manpowerQtySlab2,manpowerQtySlab3,manpowerQtySlab4, warehouseA, warehouseB,warehouseC, warehouseD, CapacityCost from productMaster where business='" & Session("BusinessType") & "'")
                    'ViewState("logistics") = getDBTable("select manpowerslab1 from productMaster where business='" & Session("BusinessType") & "'")
                    gvLogistics.DataSource = ViewState("logistics")
                    gvLogistics.DataBind()

                ViewState("fixedCost") = getDBTable("select myvendor, costX, costY, costA, costB, costC, costD, spotcostX, spotcostY, spotcostA, spotcostB, spotcostC, spotcostD, lastQtrPercent from vendorMarket where business='" & Session("BusinessType") & "'")
                gvFixedCost.DataSource = ViewState("fixedCost")
                gvFixedCost.DataBind()

                ViewState("rawRatio") = getDBTable("select QtyRawXforA, QtyRawYforA, QtyRawXforB, QtyRawYforB, QtyRawXforC, QtyRawYforC, QtyRawXforD, QtyRawYforD from productMaster where business='" & Session("BusinessType") & "'")
                gvRawRatio.DataSource = ViewState("rawRatio")
                gvRawRatio.DataBind()
                ViewState("gvCashCollection") = getDBTable("select CashCollectionA, CashCollectionB, CashCollectionC, CashCollectionD from productMaster where business='" & Session("BusinessType") & "'")
                gvCashCollection.DataSource = ViewState("gvCashCollection")
                gvCashCollection.DataBind()
                    pnlLogistics.Visible = True
                    submenu = 3
            ElseIf Request.Params("mode") = "tenders" Then
                pnlTenders.Visible = True
                submenu = 4
                '''' Showing Products
                Dim myquery = "select productA, productB, productC, productD  from productMaster where business = '" & Session("BusinessType") & "'"

                Dim dt = getDBTable(myquery)

                lblA.Text = dt.Rows(0)(0)
                lblB.Text = dt.Rows(0)(1)
                lblC.Text = dt.Rows(0)(2)
                lblD.Text = dt.Rows(0)(3)
                ' DisplayMessage("Default Products Shown." & myquery)
                txtSupplyQuarter.Text = Session("currentQuarter") + 1
                ViewState("gvLiveTenders") = getDBTable("select tenderID, supplyQuarter, productA, productB, productC, productD, prodPriorityA,prodPriorityB, prodPriorityC,prodPriorityD, cashCollectionA, cashCollectionB, cashCollectionC, cashCollectionD from tenderDetail where quarter=" & Session("CurrentQuarter") & " and live=1")
                gvLiveTenders.DataSource = ViewState("gvLiveTenders")
                gvLiveTenders.DataBind()
                myquery = "select tenderID as v, tenderID as d from tenderDetail where quarter=" & Session("CurrentQuarter") & " and live=1"
                rbLiveTenders.DataSource = getDBTable(myquery)
                rbLiveTenders.DataBind()
                gvTenderResults.DataSource = getDBTable("select tenderID, teamid, supplyQuarter, productA, productB,productC, productD, prodPriorityA,prodPriorityB, prodPriorityC,prodPriorityD from resultTender where   gameID=" & Session("gameID") & " order by tenderID")
                gvTenderResults.DataBind()
            End If
                div_submenu.InnerHtml = getsubMenu("admin", submenu)
                myinfo.InnerHtml = "Wealth: $xxx " & Session("user") & "<span>|</span><a href=admin.aspx?mode=kill>Logout</a>"
        End If
        ''Check admin dashboard
        showAdminDashboard()
    End Sub
    Sub showAdminDashboard()
        '<div class="info">Info message</div>
        '<div class="success">Successful operation message</div>
        '<div class="warning">Warning message</div>
        '<div class="error">Error message</div>
        Dim str = ""
        If Not Session("gameID") Is Nothing Then
            str = str & "<div class='success'>Game Created Succesfully</div>"
        Else
            str = str & "<div class='warning'>Game dosen't Exist. Create.</div>" & "<div class='warning'>Forecast Condition dosen't Exist.<br/> Create.</div>" & _
                 "<div class='warning'>Teams dosen't Exist. Create.</div>"
        End If

        If Not Session("gameID") Is Nothing Then
            If getDBsingle("select count(quarter) from forecastQuarter where gameid = " & Session("gameID")) > 0 Then
                str = str & "<div class='success'>Forecast Condition For Quarters.</div>"
            Else
                str = str & "<div class='warning'>Forecast Condition dosen't Exist. <br/>Create.</div>"
            End If
            If getDBsingle("select count(teamid) from teamMaster where gameid = " & Session("gameID")) > 0 Then
                str = str & "<div class='success'>Teams Created.</div>"
            Else
                str = str & "<div class='warning'>Teams dosen't Exist. Create.</div>"
            End If
        End If
        divAdminDashboard.InnerHtml = "<h3>Admin Dashboard </h3><br/>" & str
    End Sub
    Public Sub DisplayMessage(ByVal text As String)
        'divMsg.InnerHtml = text & "<br/>"
        txtConsole.Text = CType(ViewState("displayMessage"), String) & Now.ToString("%H:mm:ss") & ">>" & text & vbCrLf
        divInfo.InnerHtml = text
        ViewState("displayMessage") = txtConsole.Text
    End Sub
    Function createSession(ByVal setgameID As Boolean) As Boolean
        If setgameID Then Session("gameID") = getDBsingle("select max(uid)  from gameMaster")
        Session("CurrentQuarter") = getDBsingle("select runningquarter  from gameMaster where uid = " & Session("gameID"))
        Session("BusinessType") = getDBsingle("select gtype  from gameMaster where uid = " & Session("gameID"))

    End Function
    Protected Sub btnNewGame_Click(sender As Object, e As EventArgs) Handles btnNewGame.Click
        If Not String.IsNullOrEmpty(txtTeam.Text) And IsNumeric(txtTeam.Text) Then
            If executeDB("insert into gameMaster (gdate, gtype,runningQuarter) values(date('now'), '" & rblBuiseness.SelectedValue & "', 0 )").Contains("Error") Then DisplayMessage("Error creating new Game in database.")
            ''create session
            createSession(True)

            Dim dt = getDBTable("select * from teamMaster limit 1")
            Dim dtc = dt.Clone
            Dim dt1 = getDBTable("select * from resultInventory limit 1")
            Dim dtInventory = dt1.Clone

            For i = 1 To Convert.ToInt32(txtTeam.Text) Step 1
                Dim tmprow = dtc.NewRow
                Dim tmprow1 = dtInventory.NewRow
                tmprow(1) = Session("gameID")
                tmprow(2) = i
                tmprow(3) = "Team" + i.ToString()
                tmprow(4) = txtWealth.Text
                tmprow(5) = txtMember.Text
                tmprow(6) = txtPlantCapacity.Text
                tmprow(7) = 0  'add capacity
                tmprow(8) = 0 'My empanneled Vendor ID
                Dim tb As TextBox = New TextBox()
                tb.ID = "txtTeam" + i.ToString()
                tb.Text = "Team" + i.ToString()
                tb.Attributes.Add("runat", "Server")
                pnlTeam.Controls.Add(New LiteralControl("<br />"))
                pnlTeam.Controls.Add(tb)
                ''Set initial inventory
                tmprow1(0) = Session("gameID")
                tmprow1(1) = -1
                tmprow1(2) = i
                tmprow1(3) = 0
                tmprow1(4) = 0
                tmprow1(5) = 0
                tmprow1(6) = 0
                tmprow1(7) = 0
                tmprow1(8) = 0
                dtInventory.Rows.Add(tmprow1)
                dtc.Rows.Add(tmprow)
            Next
            ViewState("teamMaster") = dtc
            gvTeams.DataSource = ViewState("teamMaster")
            gvTeams.DataBind()
            If insertTableinDB(dtc, "teamMaster").Contains("Error") Then DisplayMessage("Teams not created in database") Else DisplayMessage("Teams created in database")
            If insertTableinDB(dtInventory, "resultInventory").Contains("Error") Then DisplayMessage("resultInventory not created in database") Else DisplayMessage("resultInventory created in database for quarter -1")

           
            Response.Redirect("admin.aspx?mode=teams")
            ''  SqlDataSource1.SelectCommand = "select * from teamMaster"
            '' GridView1.DataSource = getDBTable("select * from teamMaster")
            '' GridView1.DataBind()

        End If
    End Sub

    

    Protected Sub btnJoinGame_Click(sender As Object, e As EventArgs) Handles btnJoinGame.Click
        createSession(True)
        Response.Redirect("admin.aspx?mode=teams")
    End Sub
    Protected Sub gvTeams_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles gvTeams.RowCancelingEdit
        gvTeams.EditIndex = -1
        gvTeams.DataSource = ViewState("teamMaster")
        gvTeams.DataBind()
        DisplayMessage("Edit canceled")
    End Sub
    Protected Sub gvTeams_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles gvTeams.RowEditing
        'Dim row = gvTeams.Rows(e.NewEditIndex)
        '  Label1.Text = row.Cells(2).Text
        gvTeams.EditIndex = e.NewEditIndex
        gvTeams.DataSource = ViewState("teamMaster")
        gvTeams.DataBind()
        DisplayMessage("Edit Teams mode enabled")
    End Sub

    Protected Sub gvTeams_RowUpdated(sender As Object, e As GridViewUpdatedEventArgs) Handles gvTeams.RowUpdated
        'Dim row = gvTeams.Rows(e.AffectedRows)
        'Label1.Text = e.AffectedRows.ToString + row.Cells(2).Text + e.NewValues(1).ToString
        gvTeams.EditIndex = -1

    End Sub

    Protected Sub gvTeams_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles gvTeams.RowUpdating
        Dim q = "update teamMaster set "
        Try
            If Session("gameID") Is Nothing Then Response.Redirect("admin.aspx")
            gvTeams.EditIndex = -1

            Dim row = gvTeams.Rows(e.RowIndex)

            ' Label1.Text = (CType((row.Cells(2).Controls(0)), TextBox)).Text ' + e.NewValues(1).ToString
            Dim dt = CType(ViewState("teamMaster"), System.Data.DataTable)

            For i = 0 To row.Cells.Count - 2 Step 1
                dt.Rows(row.DataItemIndex)(i) = If(Not String.IsNullOrEmpty((CType((row.Cells(i + 1).Controls(0)), TextBox)).Text), (CType((row.Cells(i + 1).Controls(0)), TextBox)).Text, "0")
                If i > 3 Then q = q & ","
                If i > 2 Then q = q & gvTeams.HeaderRow.Cells(i + 1).Text & "='" & dt.Rows(row.DataItemIndex)(i) & "'"
            Next
            q = q & " where gameid=" & Session("gameID") & " and teamid = " & dt.Rows(row.DataItemIndex)(2)
            DisplayMessage(q)
            'GridView1.DataSource = dt
            'GridView1.DataBind()
            ViewState("teamMaster") = dt
            gvTeams.DataSource = dt
            gvTeams.DataBind()
            If executeDB(q).Contains("Error") Then DisplayMessage("teamMaster data not upadated in database") Else DisplayMessage("teamMaster old data upadated in database for gameID " & Session("gameid"))

            'DisplayMessage("updating")
            e.Cancel = False
        Catch ex As Exception
            DisplayMessage("Error updating.." + ex.Message & q)
        End Try

    End Sub
    Protected Sub gvLogistics_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles gvLogistics.RowCancelingEdit
        gvLogistics.EditIndex = -1
        gvLogistics.DataSource = ViewState("logistics")
        gvLogistics.DataBind()
        DisplayMessage("Edit canceled")
    End Sub
    Protected Sub gvLogistics_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles gvLogistics.RowEditing
        'Dim row = gvLogistics.Rows(e.NewEditIndex)
        '  Label1.Text = row.Cells(2).Text
        gvLogistics.EditIndex = e.NewEditIndex
        gvLogistics.DataSource = ViewState("logistics")
        gvLogistics.DataBind()
        DisplayMessage("Edit logistics mode enabled")
    End Sub

    Protected Sub gvLogistics_RowUpdated(sender As Object, e As GridViewUpdatedEventArgs) Handles gvLogistics.RowUpdated
        'Dim row = gvLogistics.Rows(e.AffectedRows)
        'Label1.Text = e.AffectedRows.ToString + row.Cells(2).Text + e.NewValues(1).ToString
        gvLogistics.EditIndex = -1

    End Sub

    Protected Sub gvLogistics_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles gvLogistics.RowUpdating
        Dim q = "update productMaster set "
        Try
            If Session("gameID") Is Nothing Then Response.Redirect("admin.aspx")
            gvLogistics.EditIndex = -1

            Dim row = gvLogistics.Rows(e.RowIndex)

            ' Label1.Text = (CType((row.Cells(2).Controls(0)), TextBox)).Text ' + e.NewValues(1).ToString
            Dim dt = CType(ViewState("logistics"), System.Data.DataTable)

            For i = 0 To row.Cells.Count - 2 Step 1
                dt.Rows(row.DataItemIndex)(i) = If(Not String.IsNullOrEmpty((CType((row.Cells(i + 1).Controls(0)), TextBox)).Text), (CType((row.Cells(i + 1).Controls(0)), TextBox)).Text, "0")
                If i > 0 Then q = q & ","
                If i >= 0 Then q = q & gvLogistics.HeaderRow.Cells(i + 1).Text & "='" & dt.Rows(row.DataItemIndex)(i) & "'"
            Next
            q = q & " where business='" & Session("BusinessType") & "'"
            ' DisplayMessage(q)
            'GridView1.DataSource = dt
            'GridView1.DataBind()
            ViewState("logistics") = dt
            gvLogistics.DataSource = dt
            gvLogistics.DataBind()
            If executeDB(q).Contains("Error") Then DisplayMessage("logistics data not upadated in database") Else DisplayMessage("teamMaster logistics old data upadated in database for gameID " & Session("gameid"))

            DisplayMessage(q)
            e.Cancel = False
        Catch ex As Exception
            DisplayMessage("Error updating logistics.." + ex.Message & q)
        End Try

    End Sub
    Protected Sub gvFixedCost_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles gvFixedCost.RowCancelingEdit
        gvFixedCost.EditIndex = -1
        gvFixedCost.DataSource = ViewState("fixedCost")
        gvFixedCost.DataBind()
        DisplayMessage("Edit canceled")
    End Sub
    Protected Sub gvFixedCost_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles gvFixedCost.RowEditing
        'Dim row = gvFixedCost.Rows(e.NewEditIndex)
        '  Label1.Text = row.Cells(2).Text
        ';  gvFixedCost.Rows(e.NewEditIndex).Cells(1).Visible = False
        gvFixedCost.EditIndex = e.NewEditIndex
        gvFixedCost.DataSource = ViewState("fixedCost")
        gvFixedCost.DataBind()
        DisplayMessage("Edit fixedCost mode enabled")
    End Sub

    Protected Sub gvFixedCost_RowUpdated(sender As Object, e As GridViewUpdatedEventArgs) Handles gvFixedCost.RowUpdated
        'Dim row = gvFixedCost.Rows(e.AffectedRows)
        'Label1.Text = e.AffectedRows.ToString + row.Cells(2).Text + e.NewValues(1).ToString
        gvFixedCost.EditIndex = -1

    End Sub

    Protected Sub gvFixedCost_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles gvFixedCost.RowUpdating
        Dim q = "update vendorMarket set "
        Try
            If Session("gameID") Is Nothing Then Response.Redirect("admin.aspx")
            gvFixedCost.EditIndex = -1

            Dim row = gvFixedCost.Rows(e.RowIndex)

            ' Label1.Text = (CType((row.Cells(2).Controls(0)), TextBox)).Text ' + e.NewValues(1).ToString
            Dim dt = CType(ViewState("fixedCost"), System.Data.DataTable)

            For i = 0 To row.Cells.Count - 2 Step 1
                dt.Rows(row.DataItemIndex)(i) = If(Not String.IsNullOrEmpty((CType((row.Cells(i + 1).Controls(0)), TextBox)).Text), (CType((row.Cells(i + 1).Controls(0)), TextBox)).Text, "0")
                If i > 1 Then q = q & ","
                If i >= 1 Then q = q & gvFixedCost.HeaderRow.Cells(i + 1).Text & "='" & dt.Rows(row.DataItemIndex)(i) & "'"
            Next
            q = q & " where business='" & Session("BusinessType") & "' and vendorID= " & dt.Rows(e.RowIndex)(0)
            ' DisplayMessage(q)
            'GridView1.DataSource = dt
            'GridView1.DataBind()
            ViewState("fixedCost") = dt
            gvFixedCost.DataSource = dt
            gvFixedCost.DataBind()
            If executeDB(q).Contains("Error") Then DisplayMessage("fixedCost data not upadated in database") Else DisplayMessage("teamMaster fixedCost old data upadated in database for gameID " & Session("gameid"))

            DisplayMessage(q)
            e.Cancel = False
        Catch ex As Exception
            DisplayMessage("Error updating fixedCost.." + ex.Message & q)
        End Try

    End Sub
    Protected Sub gvRawRatio_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles gvRawRatio.RowCancelingEdit
        gvRawRatio.EditIndex = -1
        gvRawRatio.DataSource = ViewState("rawRatio")
        gvRawRatio.DataBind()
        DisplayMessage("Edit canceled")
    End Sub
    Protected Sub gvRawRatio_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles gvRawRatio.RowEditing
        'Dim row = gvRawRatio.Rows(e.NewEditIndex)
        '  Label1.Text = row.Cells(2).Text
        gvRawRatio.EditIndex = e.NewEditIndex
        gvRawRatio.DataSource = ViewState("rawRatio")
        gvRawRatio.DataBind()
        DisplayMessage("Edit rawRatio mode enabled")
    End Sub

    Protected Sub gvRawRatio_RowUpdated(sender As Object, e As GridViewUpdatedEventArgs) Handles gvRawRatio.RowUpdated
        'Dim row = gvRawRatio.Rows(e.AffectedRows)
        'Label1.Text = e.AffectedRows.ToString + row.Cells(2).Text + e.NewValues(1).ToString
        gvRawRatio.EditIndex = -1

    End Sub

    Protected Sub gvRawRatio_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles gvRawRatio.RowUpdating
        Dim q = "update productMaster set "
        Try
            If Session("gameID") Is Nothing Then Response.Redirect("admin.aspx")
            gvRawRatio.EditIndex = -1

            Dim row = gvRawRatio.Rows(e.RowIndex)

            ' Label1.Text = (CType((row.Cells(2).Controls(0)), TextBox)).Text ' + e.NewValues(1).ToString
            Dim dt = CType(ViewState("rawRatio"), System.Data.DataTable)

            For i = 0 To row.Cells.Count - 2 Step 1
                dt.Rows(row.DataItemIndex)(i) = If(Not String.IsNullOrEmpty((CType((row.Cells(i + 1).Controls(0)), TextBox)).Text), (CType((row.Cells(i + 1).Controls(0)), TextBox)).Text, "0")
                If i > 0 Then q = q & ","
                If i >= 0 Then q = q & gvRawRatio.HeaderRow.Cells(i + 1).Text & "='" & dt.Rows(row.DataItemIndex)(i) & "'"
            Next
            q = q & " where business='" & Session("BusinessType") & "'"
            ' DisplayMessage(q)
            'GridView1.DataSource = dt
            'GridView1.DataBind()
            ViewState("rawRatio") = dt
            gvRawRatio.DataSource = dt
            gvRawRatio.DataBind()
            If executeDB(q).Contains("Error") Then DisplayMessage("rawRatio data not upadated in database") Else DisplayMessage("teamMaster rawRatio old data upadated in database for gameID " & Session("gameid"))

            DisplayMessage(q)
            e.Cancel = False
        Catch ex As Exception
            DisplayMessage("Error updating rawRatio.." + ex.Message & q)
        End Try

    End Sub
    Protected Sub gvCashCollection_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles gvCashCollection.RowCancelingEdit
        gvCashCollection.EditIndex = -1
        gvCashCollection.DataSource = ViewState("gvCashCollection")
        gvCashCollection.DataBind()
        DisplayMessage("Edit canceled")
    End Sub
    Protected Sub gvCashCollection_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles gvCashCollection.RowEditing
        'Dim row = gvCashCollection.Rows(e.NewEditIndex)
        '  Label1.Text = row.Cells(2).Text
        gvCashCollection.EditIndex = e.NewEditIndex
        gvCashCollection.DataSource = ViewState("gvCashCollection")
        gvCashCollection.DataBind()
        DisplayMessage("Edit gvCashCollection mode enabled")
    End Sub

    Protected Sub gvCashCollection_RowUpdated(sender As Object, e As GridViewUpdatedEventArgs) Handles gvCashCollection.RowUpdated
        'Dim row = gvCashCollection.Rows(e.AffectedRows)
        'Label1.Text = e.AffectedRows.ToString + row.Cells(2).Text + e.NewValues(1).ToString
        gvCashCollection.EditIndex = -1

    End Sub

    Protected Sub gvCashCollection_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles gvCashCollection.RowUpdating
        Dim q = "update productMaster set "
        Try
            If Session("gameID") Is Nothing Then Response.Redirect("admin.aspx")
            gvCashCollection.EditIndex = -1

            Dim row = gvCashCollection.Rows(e.RowIndex)

            ' Label1.Text = (CType((row.Cells(2).Controls(0)), TextBox)).Text ' + e.NewValues(1).ToString
            Dim dt = CType(ViewState("gvCashCollection"), System.Data.DataTable)

            For i = 0 To row.Cells.Count - 2 Step 1
                dt.Rows(row.DataItemIndex)(i) = If(Not String.IsNullOrEmpty((CType((row.Cells(i + 1).Controls(0)), TextBox)).Text), (CType((row.Cells(i + 1).Controls(0)), TextBox)).Text, "0")
                If i > 0 Then q = q & ","
                If i >= 0 Then q = q & gvCashCollection.HeaderRow.Cells(i + 1).Text & "='" & dt.Rows(row.DataItemIndex)(i) & "'"
            Next
            q = q & " where business='" & Session("BusinessType") & "'"
            ' DisplayMessage(q)
            'GridView1.DataSource = dt
            'GridView1.DataBind()
            ViewState("rawRatio") = dt
            gvCashCollection.DataSource = dt
            gvCashCollection.DataBind()
            If executeDB(q).Contains("Error") Then DisplayMessage("gvCashCollection data not upadated in database") Else DisplayMessage("teamMaster gvCashCollection old data upadated in database for gameID " & Session("gameid"))

            DisplayMessage(q)
            e.Cancel = False
        Catch ex As Exception
            DisplayMessage("Error updating gvCashCollection.." + ex.Message & q)
        End Try

    End Sub
    Protected Sub gvElasticity_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles gvElasticity.RowCancelingEdit
        gvElasticity.EditIndex = -1
        gvElasticity.DataSource = ViewState("elasticity")
        gvElasticity.DataBind()
        DisplayMessage("Edit canceled")
    End Sub
    Protected Sub gvElasticity_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles gvElasticity.RowEditing
        'Dim row = gvElasticity.Rows(e.NewEditIndex)
        '  Label1.Text = row.Cells(2).Text
        gvElasticity.EditIndex = e.NewEditIndex
        gvElasticity.DataSource = ViewState("elasticity")
        gvElasticity.DataBind()
        DisplayMessage("Edit elasticity mode enabled")
    End Sub

    Protected Sub gvElasticity_RowUpdated(sender As Object, e As GridViewUpdatedEventArgs) Handles gvElasticity.RowUpdated
        'Dim row = gvElasticity.Rows(e.AffectedRows)
        'Label1.Text = e.AffectedRows.ToString + row.Cells(2).Text + e.NewValues(1).ToString
        gvElasticity.EditIndex = -1

    End Sub

    Protected Sub gvElasticity_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles gvElasticity.RowUpdating
        Dim q = "update productMaster set "
        Try
            If Session("gameID") Is Nothing Then Response.Redirect("admin.aspx")
            gvElasticity.EditIndex = -1

            Dim row = gvElasticity.Rows(e.RowIndex)

            ' Label1.Text = (CType((row.Cells(2).Controls(0)), TextBox)).Text ' + e.NewValues(1).ToString
            Dim dt = CType(ViewState("elasticity"), System.Data.DataTable)

            For i = 0 To row.Cells.Count - 2 Step 1
                dt.Rows(row.DataItemIndex)(i) = If(Not String.IsNullOrEmpty((CType((row.Cells(i + 1).Controls(0)), TextBox)).Text), (CType((row.Cells(i + 1).Controls(0)), TextBox)).Text, "0")
                If i > 0 Then q = q & ","
                If i >= 0 Then q = q & gvElasticity.HeaderRow.Cells(i + 1).Text & "='" & dt.Rows(row.DataItemIndex)(i) & "'"
            Next
            q = q & " where business='" & Session("BusinessType") & "'"
            ' DisplayMessage(q)
            'GridView1.DataSource = dt
            'GridView1.DataBind()
            ViewState("elasticity") = dt
            gvElasticity.DataSource = dt
            gvElasticity.DataBind()
            If executeDB(q).Contains("Error") Then DisplayMessage("elasticity data not upadated in database") Else DisplayMessage("teamMaster elasticity old data upadated in database for gameID " & Session("gameid"))

            DisplayMessage(q)
            e.Cancel = False
        Catch ex As Exception
            DisplayMessage("Error updating elasticity.." + ex.Message & q)
        End Try

    End Sub
    Protected Sub gvLiveTenders_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles gvLiveTenders.RowCancelingEdit
        gvLiveTenders.EditIndex = -1
        gvLiveTenders.DataSource = ViewState("gvLiveTenders")
        gvLiveTenders.DataBind()
        DisplayMessage("Edit canceled")
    End Sub
    Protected Sub gvLiveTenders_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles gvLiveTenders.RowEditing
        'Dim row = gvLiveTenders.Rows(e.NewEditIndex)
        '  Label1.Text = row.Cells(2).Text
        gvLiveTenders.EditIndex = e.NewEditIndex
        gvLiveTenders.DataSource = ViewState("gvLiveTenders")
        gvLiveTenders.DataBind()
        DisplayMessage("Edit gvLiveTenders mode enabled")
    End Sub

    Protected Sub gvLiveTenders_RowUpdated(sender As Object, e As GridViewUpdatedEventArgs) Handles gvLiveTenders.RowUpdated
        'Dim row = gvLiveTenders.Rows(e.AffectedRows)
        'Label1.Text = e.AffectedRows.ToString + row.Cells(2).Text + e.NewValues(1).ToString
        gvLiveTenders.EditIndex = -1

    End Sub

    Protected Sub gvLiveTenders_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles gvLiveTenders.RowUpdating
        Dim q = "update tenderDetail set "
        Try
            If Session("gameID") Is Nothing Then Response.Redirect("admin.aspx")
            gvLiveTenders.EditIndex = -1

            Dim row = gvLiveTenders.Rows(e.RowIndex)

            ' Label1.Text = (CType((row.Cells(2).Controls(0)), TextBox)).Text ' + e.NewValues(1).ToString
            Dim dt = CType(ViewState("gvLiveTenders"), System.Data.DataTable)

            For i = 0 To row.Cells.Count - 2 Step 1
                dt.Rows(row.DataItemIndex)(i) = If(Not String.IsNullOrEmpty((CType((row.Cells(i + 1).Controls(0)), TextBox)).Text), (CType((row.Cells(i + 1).Controls(0)), TextBox)).Text, "0")
                If i > 0 Then q = q & ","
                If i >= 0 Then q = q & gvLiveTenders.HeaderRow.Cells(i + 1).Text & "='" & dt.Rows(row.DataItemIndex)(i) & "'"
            Next
            q = q & " where tenderID=" & dt.Rows(e.RowIndex)(0)
            ' DisplayMessage(q)
            'GridView1.DataSource = dt
            'GridView1.DataBind()
            ViewState("gvLiveTenders") = dt
            gvLiveTenders.DataSource = dt
            gvLiveTenders.DataBind()
            If executeDB(q).Contains("Error") Then DisplayMessage("gvLiveTenders data not upadated in database") Else DisplayMessage(" gvLiveTenders old data upadated in database for gameID " & Session("gameid"))

            DisplayMessage(q)
            e.Cancel = False
        Catch ex As Exception
            DisplayMessage("Error updating elasticity.." + ex.Message & q)
        End Try

    End Sub
    Protected Sub gvForecast_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles gvForecast.RowCancelingEdit
        gvForecast.EditIndex = -1
        gvForecast.DataSource = ViewState("forecastQuarter")
        gvForecast.DataBind()
        DisplayMessage("Edit canceled")
    End Sub
    Protected Sub gvForecast_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles gvForecast.RowEditing
        'Dim row = gvForecast.Rows(e.NewEditIndex)
        '  Label1.Text = row.Cells(2).Text
        gvForecast.EditIndex = e.NewEditIndex
        gvForecast.DataSource = ViewState("forecastQuarter")
        gvForecast.DataBind()
        DisplayMessage("Edit mode enabled")
    End Sub

    Protected Sub gvForecast_RowUpdated(sender As Object, e As GridViewUpdatedEventArgs) Handles gvForecast.RowUpdated
        'Dim row = gvForecast.Rows(e.AffectedRows)
        'Label1.Text = e.AffectedRows.ToString + row.Cells(2).Text + e.NewValues(1).ToString
        gvForecast.EditIndex = -1

    End Sub

    Protected Sub gvForecast_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles gvForecast.RowUpdating
        Try
            If Session("gameID") Is Nothing Then Response.Redirect("admin.aspx")
            gvForecast.EditIndex = -1

            Dim row = gvForecast.Rows(e.RowIndex)

            ' Label1.Text = (CType((row.Cells(2).Controls(0)), TextBox)).Text ' + e.NewValues(1).ToString
            Dim dt = CType(ViewState("forecastQuarter"), System.Data.DataTable)
            Dim q = "update forecastQuarter set "
            For i = 0 To row.Cells.Count - 2 Step 1
                dt.Rows(row.DataItemIndex)(i) = If(Not String.IsNullOrEmpty((CType((row.Cells(i + 1).Controls(0)), TextBox)).Text), (CType((row.Cells(i + 1).Controls(0)), TextBox)).Text, "0")
                If i > 2 Then q = q & ","
                If i > 1 Then q = q & gvForecast.HeaderRow.Cells(i + 1).Text & "=" & dt.Rows(row.DataItemIndex)(i)
            Next
            q = q & " where gameid=" & Session("gameID") & " and quarter = " & dt.Rows(row.DataItemIndex)(1)
            DisplayMessage(q)
            'GridView1.DataSource = dt
            'GridView1.DataBind()
            ViewState("forecastQuarter") = dt
            gvForecast.DataSource = dt
            gvForecast.DataBind()
            If executeDB(q).Contains("Error") Then DisplayMessage("forecastQuarter data not upadated in database") Else DisplayMessage("forecastQuarter old data upadated in database for gameID " & Session("gameid"))

            'DisplayMessage("updating")
            e.Cancel = False
        Catch ex As Exception
            DisplayMessage("Error updating.." + ex.Message)
        End Try

    End Sub

    
    Protected Sub btnConditions_Click(sender As Object, e As EventArgs) Handles btnConditions.Click
        '''Create forecast conditions from last game
        ''' 
        If getDBsingle("select count(quarter) from forecastQuarter where gameid=" & Session("gameID")) > 0 Then
            ''forecast already exist
            Response.Redirect("admin.aspx?mode=conditions")
            Exit Sub
        End If
        Dim myquery = "insert into forecastQuarter (gameid,quarter,demandA,minA,maxA,demandB,minB,maxB,demandC,minC,maxC,demandD,minD,maxD) select " & Session("gameid") & ",quarter,demandA,minA,maxA,demandB,minB,maxB,demandC,minC,maxC,demandD,minD,maxD from forecastQuarter where gameid = (select max(gameid) from forecastQuarter)"
        If executeDB(myquery).Contains("Error") Then DisplayMessage("Quarter Forecast not created in database") Else DisplayMessage("Quarter Forecast created in database")
       

        Response.Redirect("admin.aspx?mode=conditions")
    End Sub

    Protected Sub btnForecastReconsile_Click(sender As Object, e As EventArgs) Handles btnForecastReconsile.Click
        '' now set demand = total team X plant capacity *  0.9
        If Not Session("gameID") Is Nothing Then
            If Not IsNumeric(txtDemandFactor.Text) Or Not IsNumeric(txtCurrentQuarter.Text) Then
                DisplayMessage("Demand factor/Current Quarter should be numeric and less then 1")
                Exit Sub
            ElseIf Not CType(txtDemandFactor.Text, Double) < 1 Then
                DisplayMessage("Demand factor/Current Quarter should be numeric and less then 1")
                Exit Sub
            End If
            DisplayMessage("set demand = (total team X plant capacity *  0.95) / 4products")
            Dim myquery = "select count(teamid) * sum(plantcapacity)/count(teamid) * " & txtDemandFactor.Text & " /4 from teammaster where gameid = " & Session("gameID")
            Dim demand = getDBsingle(myquery)
            If Not demand.Contains("Error") Then
                myquery = "update forecastQuarter set demandA = " & demand & " where gameid = " & Session("gameID")
                executeDB(myquery)
                myquery = "update forecastQuarter set demandB = " & demand & " * 0.7 where gameid = " & Session("gameID") & " and quarter > 1"
                executeDB(myquery)
                myquery = "update forecastQuarter set demandC = " & demand & " * 0.8 where gameid = " & Session("gameID") & " and quarter > 2"
                executeDB(myquery)
                myquery = "update forecastQuarter set demandD = " & demand & " * 0.65 where gameid = " & Session("gameID") & " and quarter > 3"
                executeDB(myquery)
                ViewState("forecastQuarter") = getDBTable("select * from forecastQuarter where gameid=" & Session("gameID") & " order by quarter")
                gvForecast.DataSource = ViewState("forecastQuarter")
                gvForecast.DataBind()
                DisplayMessage("Reconsile to be developed further for more automation..")
            Else
                DisplayMessage("Error calculating Demand " & myquery)
            End If

        Else
            Response.Redirect("admin.aspx")
        End If

    End Sub

    Protected Sub rblBuiseness_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rblBuiseness.SelectedIndexChanged
        populateProducts(rblBuiseness.SelectedValue)
        ' btnProduct.Text = rblBuiseness.SelectedItem.Text
    End Sub
    Function populateProducts(index As Integer) As Boolean

        Dim myquery = "select productA, productB, productC, productD  from productMaster where bid = " & index
        Try
            Dim dt = getDBTable(myquery)

            txtNewProductA.Text = dt.Rows(0)(0)
            txtNewProductB.Text = dt.Rows(0)(1)
            txtNewProductC.Text = dt.Rows(0)(2)
            txtNewProductD.Text = dt.Rows(0)(3)
            DisplayMessage("Default Products Shown. You can Change it and Click on Update.")
        Catch ex As Exception
            DisplayMessage("Error Populating Default Products  " & myquery)
        End Try

    End Function

    Protected Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        If txtLogin.Text = "admin" And txtPwd.Text = "nimda" Then
            Session("user") = "admin"
            createSession(False)
        Else
            Dim myquery = "select teamid from teammaster where thepwd='" & txtPwd.Text & "' and teamid=" & txtLogin.Text & " and gameid is (select max(uid) from  gameMaster where gameclosed = 0) "
            Dim teamid = getDBsingle(myquery)
            If teamid.Contains("Error") Then
                DisplayMessage("Wrong Login Information")
            Else
                Session("user") = teamid
                Session("teamID") = teamid
                createSession(True)
            End If

        End If

        Response.Redirect("admin.aspx")
         End Sub

    Protected Sub btnCreateTender_Click(sender As Object, e As EventArgs) Handles btnCreateTender.Click
        Dim prodA, prodB, prodC, prodD, supplyQuarter As Integer
        If String.IsNullOrEmpty(txtProdA.Text) Then prodA = 0 Else prodA = CType(txtProdA.Text, Int32)
        If String.IsNullOrEmpty(txtProdB.Text) Then prodB = 0 Else prodB = CType(txtProdB.Text, Int32)
        If String.IsNullOrEmpty(txtProdC.Text) Then prodC = 0 Else prodC = CType(txtProdC.Text, Int32)
        If String.IsNullOrEmpty(txtProdD.Text) Then prodD = 0 Else prodD = CType(txtProdD.Text, Int32)
        If String.IsNullOrEmpty(txtSupplyQuarter.Text) Then supplyQuarter = Session("currentQuarter") Else supplyQuarter = CType(txtSupplyQuarter.Text, Int32)

        'insert
        DisplayMessage("Record dosen't Exist. Proceding with Insert. ")
        Dim myquery = "insert into tenderDetail ( quarter, supplyQuarter, productA, productB, productC, productD, cashCollectionA, cashCollectionB, cashCollectionC, cashCollectionD, live, prodPriorityA,prodPriorityB, prodPriorityC,prodPriorityD) values (" & Session("currentQuarter") & ", " & supplyQuarter & ", " & prodA & ", " & prodB & ", " & prodC & ", " & prodD & ", 100,100,100,100,1,0,0,0,0  )"

        If Not executeDB(myquery).Contains("Error") Then
            DisplayMessage("Tender Created Succesfully")
            ViewState("gvLiveTenders") = getDBTable("select tenderID, supplyQuarter, productA, productB, productC, productD, cashCollectionA, cashCollectionB, cashCollectionC, cashCollectionD from tenderDetail where quarter=" & Session("CurrentQuarter") & " and live=1")
            gvLiveTenders.DataSource = ViewState("gvLiveTenders")
            gvLiveTenders.DataBind()
            myquery = "select tenderID as v, tenderID as d from tenderDetail where quarter=" & Session("CurrentQuarter") & " and live=1"
            rbLiveTenders.DataSource = getDBTable(myquery)
            rbLiveTenders.DataBind()
        Else
            DisplayMessage("Problem Creating Tender.. " & myquery)
          End If
    End Sub

    Protected Sub btnCloseTender_Click(sender As Object, e As EventArgs) Handles btnCloseTender.Click
        If rbLiveTenders.SelectedIndex < 0 Then
            DisplayMessage("No Live Tender selected to Finalize & Close")
            Exit Sub
        End If

    End Sub
End Class
