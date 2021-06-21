Imports common
Imports dbOperation
Partial Class decisions
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("gameID") Is Nothing Then
            Response.Redirect("admin.aspx?source=decisions")
        End If
        If Not Page.IsPostBack Then

            Dim submenu = 0
            pnlProduction.Visible = False
            pnlCapacity.Visible = False
            pnlFinances.Visible = False
            pnlSubscriptions.Visible = False

            Session("mode") = Request.Params("mode")
            If Request.Params("mode") Is Nothing Then
                Session("mode") = "production"
            End If

            rbTeams.DataSource = getDBTable("select teamid as v, teamname as d from teamMaster where gameid=" & Session("gameID"))
            rbTeams.DataBind()
            txtQuarter.Text = Session("CurrentQuarter")
            DisplayMessage("Enter the decision of each Team by Admin/ Self by logged in Team.")
            colorTeams(Session("mode"))

            If Session("mode") = "production" Then

                pnlProduction.Visible = True

            ElseIf Session("mode") = "capacity" Then
                pnlCapacity.Visible = True
                submenu = 1
            ElseIf Session("mode") = "subscriptions" Then
                pnlSubscriptions.Visible = True
                cbSubscription.DataSource = getDBTable("select uid as v, subscription || '('|| cost || ')' as d from subscriptionMaster")
                cbSubscription.DataBind()
                submenu = 6
            ElseIf Session("mode") = "finances" Then
                pnlFinances.Visible = True
                submenu = 2


            End If

            div_submenu.InnerHtml = getsubMenu("decisions", submenu)
           
            showDashboard()
        End If
        Dim wealth = "NA"
        If Not Session("teamID") Is Nothing Then wealth = getDBsingle("select wealth from teamMaster where teamID=" & Session("teamID") & " and gameID=" & Session("gameID"))
        myinfo.InnerHtml = "Wealth: $ " & wealth & " " & Session("user") & "<span>|</span><a href=admin.aspx?mode=kill>Logout</a>"

    End Sub
    Function colorTeams(ByVal mode As String) As Boolean
        '' Color green for Teams who have entered Data
        Dim myquery = ""
        Try
            If mode = "production" Then
                myquery = "select teamid from decisionProduction where gameid = " & Session("gameID") & " and quarter = " & Session("CurrentQuarter")
            ElseIf mode = "capacity" Then
                myquery = "select teamid from decisionCapacity where gameid = " & Session("gameID") & " and quarter = " & Session("CurrentQuarter")
            ElseIf mode = "subscriptions" Then
                myquery = "select teamid from subscriptionDetail where gameid = " & Session("gameID")

            End If
            Dim dt = getDBTable(myquery)
            Dim dv = dt.DefaultView
            dv.Sort = "teamid"
            For Each i As ListItem In rbTeams.Items
                If dv.Find(i.Value) = -1 Then
                    i.Attributes("style") = "background-color:yellow;"
                Else
                    i.Attributes("style") = "background-color:lime;"
                End If
                '' unlock for admin and disable for users
                If Session("user") = i.Value Then
                    i.Enabled = True
                ElseIf Session("user") = "admin" Then
                    i.Enabled = True
                Else
                    i.Enabled = False
                End If
            Next
            ''''''''''''''''
        Catch ex As Exception
            DisplayMessage("Error Coloring Teams: " & myquery)
        End Try

    End Function
    Sub showDashboard()
        '<div class="info">Info message</div>
        '<div class="success">Successful operation message</div>
        '<div class="warning">Warning message</div>
        '<div class="error">Error message</div>
        Dim str = "<h4>Checks</h4>"

        str = str & "<div class='info'>" & _
            getDBsingle("select 'DemandA-' || demandA || '(' || minA || '/' || maxA || ') ' || '<br/>DemandB-' || demandB || '(' || minB || '/' || maxB || ') ' || '<br/>DemandC-' || demandC || '(' || minC || '/' || maxC || ') ' || '<br/>DemandD-' || demandD || '(' || minD || '/' || maxD || ') ' as v from forecastQuarter where gameid=12 and quarter = " & Session("currentQuarter")) & _
            "</div>"
        divDashboard.InnerHtml = "<h3>Decision Dashboard </h3><br/>" & str
    End Sub
    Public Sub DisplayMessage(ByVal text As String)
        'divMsg.InnerHtml = text & "<br/>"
        txtConsole.Text = CType(ViewState("displayMessage"), String) & Now.ToString("%H:mm:ss") & ">>" & text & vbCrLf
        divInfo.InnerHtml = Now.ToString("%H:mm:ss") & "> " & text
        ViewState("displayMessage") = txtConsole.Text
    End Sub

    Protected Sub rbTeams_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rbTeams.SelectedIndexChanged
        Dim myquery = ""
        Try
            If Session("gameID") Is Nothing Then
                Response.Redirect("admin.aspx?source=decisions")
            End If
            ' Dim teamid = rbTeams.SelectedIndex + 1
            Dim teamid = rbTeams.SelectedValue
            Session("teamID") = teamid
            DisplayMessage("Entering Data for Team" & teamid)
            ' Exit Sub
            If Session("mode") = "production" Then
                '' Color Teams for Production
                colorTeams(Session("mode"))
                ''''Disabling textbox
                DisplayMessage("Disabling Product not forecasted for this quarter.")
                Dim demand = getDBTable("select demandA, demandB, demandC, demandD from forecastQuarter where gameid = " & Session("gameID") & " and quarter = " & Session("currentQuarter"))
                Dim i = 1
                For Each cell In demand.Rows(0).ItemArray
                    ' DisplayMessage(cell)
                    If cell = "0" Or String.IsNullOrEmpty(cell) Then
                        If i = 1 Then txtPriceA.Enabled = False
                        If i = 2 Then txtPriceB.Enabled = False
                        If i = 3 Then txtPriceC.Enabled = False
                        If i = 4 Then txtPriceD.Enabled = False
                        If i = 1 Then txtProdA.Enabled = False
                        If i = 2 Then txtProdB.Enabled = False
                        If i = 3 Then txtProdC.Enabled = False
                        If i = 4 Then txtProdD.Enabled = False
                    Else
                        If i = 1 Then txtPriceA.Enabled = True
                        If i = 2 Then txtPriceB.Enabled = True
                        If i = 3 Then txtPriceC.Enabled = True
                        If i = 4 Then txtPriceD.Enabled = True
                        If i = 1 Then txtProdA.Enabled = True
                        If i = 2 Then txtProdB.Enabled = True
                        If i = 3 Then txtProdC.Enabled = True
                        If i = 4 Then txtProdD.Enabled = True
                    End If
                    i = i + 1
                Next
                ''''''''''''''''
                '''' Showing Products
                myquery = "select productA, productB, productC, productD  from productMaster where business = '" & Session("BusinessType") & "'"

                Dim dt = getDBTable(myquery)

                lblA.Text = dt.Rows(0)(0)
                lblB.Text = dt.Rows(0)(1)
                lblC.Text = dt.Rows(0)(2)
                lblD.Text = dt.Rows(0)(3)
                DisplayMessage("Default Products Shown." & myquery)

                '''''''''''''''''''''''''

                myquery = "select teamid from decisionProduction where gameid = " & Session("gameID") & " and quarter = " & txtQuarter.Text & " and teamid = " & teamid
                If getDBsingle(myquery).Contains("Error") Then


                    DisplayMessage("You are allowed to enter data for this quarter.")
                    '   btndecisionProduction.Enabled = True
                    ''Prefill
                    txtProdA.Text = ""
                    txtProdB.Text = ""
                    txtProdC.Text = ""
                    txtProdD.Text = ""
                    txtPriceA.Text = ""
                    txtPriceB.Text = ""
                    txtPriceC.Text = ""
                    txtPriceD.Text = ""
                    txtRawX.Text = ""
                    txtRawY.Text = ""

                Else
                    DisplayMessage("Data already Exist for This Quarter for Selected Team")
                    'btndecisionProduction.Enabled = False
                    '' prefill existing data
                    myquery = "select productA, productB, productC, productD, priceA,priceB,priceC,priceD, rawX,rawY from decisionProduction where gameid = " & Session("gameID") & " and quarter = " & txtQuarter.Text & " and teamid = " & teamid
                    Dim unitPrice = getDBTable(myquery)

                    txtProdA.Text = unitPrice.Rows(0)(0)
                    txtProdB.Text = unitPrice.Rows(0)(1)
                    txtProdC.Text = unitPrice.Rows(0)(2)
                    txtProdD.Text = unitPrice.Rows(0)(3)
                    txtPriceA.Text = unitPrice.Rows(0)(4)
                    txtPriceB.Text = unitPrice.Rows(0)(5)
                    txtPriceC.Text = unitPrice.Rows(0)(6)
                    txtPriceD.Text = unitPrice.Rows(0)(7)
                    txtRawX.Text = unitPrice.Rows(0)(8)
                    txtRawY.Text = unitPrice.Rows(0)(9)
                End If
            ElseIf Session("mode") = "capacity" Then
                '' Color Teams for Production
                colorTeams(Session("mode"))
                myquery = "select productA, productB, productC, productD, rawX, rawY  from resultInventory where  gameid=" & Session("gameid") & " and teamid=" & Session("teamid") & " and quarter=" & Session("currentQuarter") & "-1 "
                gvInventoryPrevQtr.DataSource = getDBTable(myquery)
                gvInventoryPrevQtr.DataBind()
                myquery = "select plantcapacity+addcapacity from teamMaster where teamid=" & teamid & " and gameid =" & Session("gameID")
                lblCapacity.Text = getDBsingle(myquery)

                myquery = "select CapacityCost from productMaster where business='" & Session("BusinessType") & "'"
                lblCapacityCost.Text = getDBsingle(myquery)
                DisplayMessage("Capacity Decision")
                myquery = "select teamid from decisionCapacity where gameid=" & Session("gameid") & " and teamid=" & Session("teamid") & " and quarter=" & Session("currentQuarter") & " "
                If Not getDBsingle(myquery).Contains("Error") Then
                    ''Data already exist so populate
                    myquery = "select addcapacity from decisionCapacity where gameid=" & Session("gameid") & " and teamid=" & Session("teamid") & " and quarter=" & Session("currentQuarter") & " "
                    txtAddCapacity.Text = getDBsingle(myquery)
                End If
            ElseIf Session("mode") = "subscriptions" Then
                colorTeams(Session("mode"))
                myquery = "select subscriptionID from subscriptionDetail where gameid=" & Session("gameid") & " and teamid=" & Session("teamid")
                Dim dtSubscription = getDBTable(myquery).DefaultView
                For Each item As ListItem In cbSubscription.Items
                    dtSubscription.RowFilter = "subscriptionID = " & item.Value
                    '   DisplayMessage(dtSubscription.Count)
                    If dtSubscription.Count > 0 Then item.Selected = True Else item.Selected = False
                Next
            End If
                    divDashboard1.InnerHtml = "<div class=success>Plant Capacity:" & getDBsingle("select plantcapacity+addcapacity from teamMaster where teamid=" & teamid & " and gameid =" & Session("gameID")) & "</div>"
                Catch exp As Exception
            DisplayMessage("Error:rbTeams: " & exp.Message & myquery)
                End Try
    End Sub

    Protected Sub btndecisionProduction_Click(sender As Object, e As EventArgs) Handles btndecisionProduction.Click
        Dim myquery = ""
        Dim prodOK = False
        Dim unitpriceOK = False
        Try
            If Not rbTeams.SelectedIndex > -1 Then
                ''Some teams are not selected
                DisplayMessage("Select a Team First")
                Exit Sub
            End If

            myquery = "select demandA, demandB, demandC, demandD from forecastQuarter where gameid = " & Session("gameID") & " and quarter =0"

            Dim demand = getDBTable(myquery)
            ''################################
            '' \\\\Check validity of production
            myquery = "select plantcapacity + addcapacity from teamMaster where gameid=" & Session("gameID") & " and teamid=" & Session("teamID")
            Dim plantcapacity = getDBsingle(myquery)
            If plantcapacity.Contains("Error") Then DisplayMessage("Plant Capacity NA")
            Dim prodA, prodB, prodC, prodD, rawX, rawY As Integer
            If String.IsNullOrEmpty(txtProdA.Text) Then prodA = 0 Else prodA = CType(txtProdA.Text, Int32)
            If String.IsNullOrEmpty(txtProdB.Text) Then prodB = 0 Else prodB = CType(txtProdB.Text, Int32)
            If String.IsNullOrEmpty(txtProdC.Text) Then prodC = 0 Else prodC = CType(txtProdC.Text, Int32)
            If String.IsNullOrEmpty(txtProdD.Text) Then prodD = 0 Else prodD = CType(txtProdD.Text, Int32)
            If String.IsNullOrEmpty(txtRawX.Text) Then rawX = 0 Else rawX = CType(txtRawX.Text, Int32)
            If String.IsNullOrEmpty(txtRawY.Text) Then rawY = 0 Else rawY = CType(txtRawY.Text, Int32)

            ''\\\Check for Raw material ordering should in range of +-50% of last quarter order
            Dim res = ""
            myquery = "select LastQtrPercent from vendorMarket v, teammaster t where t.myvendor = v.vendorid and v.business ='" & Session("BusinessType") & "' and t.teamID= " & Session("teamID") & " and t.gameid = " & Session("gameID")
            '''' for empanneled vendor check my vendor
            Dim lastOrderpercentage = getDBsingle(myquery)
            Dim lastRawX, lastRawY As Integer
            myquery = "select rawX from decisionProduction where gameid=" & Session("gameID") & " and teamid=" & Session("teamID") & " and quarter = " & Session("currentQuarter") & "-1"
            res = getDBsingle(myquery)
            If res.Contains("Error") Then lastRawX = 0 Else lastRawX = res
            myquery = "select rawY from decisionProduction where gameid=" & Session("gameID") & " and teamid=" & Session("teamID") & " and quarter = " & Session("currentQuarter") & "-1"
            res = getDBsingle(myquery)
            If res.Contains("Error") Then lastRawY = 0 Else lastRawY = res
            If CType(rawX, Int32) >= CType(lastRawX, Int32) * CType(lastOrderpercentage, Int32) / 100 And CType(rawX, Int32) <= CType(lastRawX, Int32) * (100 + CType(lastOrderpercentage, Int32)) / 100 Then
                DisplayMessage("RawX is Valid +-" & lastOrderpercentage & " % of your last order ")
            ElseIf lastRawX > 0 Then
                '' lastRawX is zero means +-50% rules will not be applied
                DisplayMessage("RawX is not +-" & lastOrderpercentage & " % of your last order.Fail to Submit. ")
                Exit Sub
            End If
            If CType(rawY, Int32) >= CType(lastRawY, Int32) * CType(lastOrderpercentage, Int32) / 100 And CType(rawY, Int32) <= CType(lastRawY, Int32) * (100 + CType(lastOrderpercentage, Int32)) / 100 Then
                DisplayMessage("RawY is valid +-" & lastOrderpercentage & " % of your last order")
            ElseIf lastRawY > 0 Then
                '' lastRawY is zero means +-50% rules will not be applied
                DisplayMessage("RawY is not +-" & lastOrderpercentage & " % of your last order. Fail to Submit.")
                Exit Sub
            End If

            If CType(plantcapacity, Int32) >= prodA + prodB + prodC + prodD Then
                DisplayMessage("Production quantities valid.")
                divDashboard2.InnerHtml = "<div class=success>Production quantities valid.</div>"
                prodOK = True
            Else
                DisplayMessage("Production quantities invalid. Exceeding Plant Capacity of " & plantcapacity & ". Please Reduce Any (A/B/C/D).")
                divDashboard2.InnerHtml = "<div class=warning>Production quantities invalid.</div>"
                Exit Sub
            End If
            ''################################
            '' Check validity of production////

            ''################################
            '' Check validity of unitPrice

            myquery = "select minA, maxA, minB, maxB, minC,maxC,minD,maxD from forecastQuarter where gameid = " & Session("gameID") & " and quarter =" & Session("currentQuarter")

            Dim unitPrice = getDBTable(myquery)

            Dim priceA, priceB, priceC, priceD As Integer
            If String.IsNullOrEmpty(txtPriceA.Text) Then priceA = 0 Else priceA = CType(txtPriceA.Text, Int32)
            If String.IsNullOrEmpty(txtPriceB.Text) Then priceB = 0 Else priceB = CType(txtPriceB.Text, Int32)
            If String.IsNullOrEmpty(txtPriceC.Text) Then priceC = 0 Else priceC = CType(txtPriceC.Text, Int32)
            If String.IsNullOrEmpty(txtPriceD.Text) Then priceD = 0 Else priceD = CType(txtPriceD.Text, Int32)

            Dim minA, maxA, minB, maxB, minC, maxC, minD, maxD As Integer
            minA = unitPrice.Rows(0)(0)
            maxA = unitPrice.Rows(0)(1)
            minB = unitPrice.Rows(0)(2)
            maxB = unitPrice.Rows(0)(3)
            minC = unitPrice.Rows(0)(4)
            maxC = unitPrice.Rows(0)(5)
            minD = unitPrice.Rows(0)(6)
            maxD = unitPrice.Rows(0)(7)
            If InRange(priceA, minA, maxA) And InRange(priceB, minB, maxB) And InRange(priceC, minC, maxC) And InRange(priceD, minD, maxD) Then
                DisplayMessage("All Unit Prices are Valid and inRange")
                divDashboard3.InnerHtml = "<div class=success>Unit Prices are Valid.</div>"
                unitpriceOK = True
            Else
                DisplayMessage("Any/All of Unit Price is not in Range(min/max)")
                divDashboard3.InnerHtml = "<div class=warning>Unit Prices are not in range.</div>"
                Exit Sub
            End If
            ''################################
            '' Check validity of unitPrice////
            ''################################
            '' Insert or Update Data if valid

            If prodOK And unitpriceOK Then
                DisplayMessage("Inserting data to database")
                ''Check if data already exist
                myquery = "select teamid from decisionProduction where gameid=" & Session("gameid") & " and teamid=" & Session("teamid") & " and quarter=" & Session("currentQuarter") & " "
                If getDBsingle(myquery).Contains("Error") Then
                    'insert
                    DisplayMessage("Record dosen't Exist. Proceding with Insert. ")
                    myquery = "insert into decisionProduction (gameid, teamid, quarter, productA, productB, productC, productD, priceA, priceB, priceC, priceD, rawX, rawY) values (" & Session("gameID") & ", " & Session("teamid") & ", " & Session("currentQuarter") & ", " & prodA & ", " & prodB & ", " & prodC & ", " & prodD & ", " & priceA & ", " & priceB & ", " & priceC & ", " & priceD & ", " & rawX & ", " & rawY & " )"

                    If Not executeDB(myquery).Contains("Error") Then
                        DisplayMessage("Data Saved Succesfully")
                        divDashboard1.InnerHtml = "<div class=success>Production Decision Completed.</div>"

                    Else
                        DisplayMessage("Problem Saving Data.. " & myquery)
                        divDashboard1.InnerHtml = "<div class=warning>Production Decision Pending.</div>"
                    End If

                Else
                    'update
                    DisplayMessage("Record  Exist. Proceding with update. ")
                    myquery = "update decisionProduction set productA = " & prodA & ", productB = " & prodB & ", productC = " & prodC & ", productD = " & prodD & ", priceA = " & priceA & ", priceB = " & priceB & ", priceC = " & priceC & ", priceD = " & priceD & ", rawX = " & rawX & ", rawY = " & rawY & " where gameid=" & Session("gameid") & " and teamid=" & Session("teamid") & " and quarter=" & Session("currentQuarter") & "  "

                    If Not executeDB(myquery).Contains("Error") Then
                        DisplayMessage("Data updated Succesfully")
                        divDashboard1.InnerHtml = "<div class=success>Production Decision Completed.</div>"

                    Else
                        DisplayMessage("Problem Updating Data.. " & myquery)
                        divDashboard1.InnerHtml = "<div class=warning>Production Decision Pending.</div>"
                    End If
                End If

            End If
            colorTeams(Session("mode"))
        Catch exp As Exception
            DisplayMessage("Error:btnDecisionProduction: " & exp.Message & myquery)
        End Try

    End Sub
    Public Shared Function InRange(ByVal value As Double, ByVal min As Double, ByVal max As Double) As Boolean
        Return (value >= min AndAlso value <= max)
    End Function

    Protected Sub btnDecisionCapacity_Click(sender As Object, e As EventArgs) Handles btnDecisionCapacity.Click
        If Not rbTeams.SelectedIndex > -1 Then
            ''Some teams are not selected
            DisplayMessage("Select a Team First")
            Exit Sub
        End If
        Dim myquery = "select teamid from decisionCapacity where gameid=" & Session("gameid") & " and teamid=" & Session("teamid") & " and quarter=" & Session("currentQuarter") & " "
        Try
            Dim addCapacity = 0
            If String.IsNullOrEmpty(txtAddCapacity.Text) Then Exit Sub Else addCapacity = CType(txtAddCapacity.Text, Int32)

            If getDBsingle(myquery).Contains("Error") Then
                'insert
                DisplayMessage("Record dosen't Exist. Proceding with Insert. ")
                myquery = "insert into decisionCapacity (gameid, teamid, quarter, addCapacity) values (" & Session("gameID") & ", " & Session("teamid") & ", " & Session("currentQuarter") & ", " & addCapacity & ")"

                If Not executeDB(myquery).Contains("Error") Then
                    DisplayMessage("Capacity Data Saved Succesfully")
                    '   divDashboard1.InnerHtml = "<div class=success>Production Decision Completed.</div>"
                    '' also add to teamMaster
                    myquery = "update teamMaster set addCapacity=addCapacity+" & addCapacity & " where teamID=" & Session("teamID") & " and gameID=" & Session("gameID")

                    If Not executeDB(myquery).Contains("Error") Then DisplayMessage("unable to update add capacity in team Master")


                Else
                    DisplayMessage("Problem Saving Capacity Data.. " & myquery)
                    '  divDashboard1.InnerHtml = "<div class=warning>Production Decision Pending.</div>"
                End If

            Else
                'update
                DisplayMessage("Record  Exist. Proceding with update. ")
                myquery = "update decisionCapacity set addCapacity = " & addCapacity & " where gameid=" & Session("gameid") & " and teamid=" & Session("teamid") & " and quarter=" & Session("currentQuarter") & "  "

                If Not executeDB(myquery).Contains("Error") Then
                    DisplayMessage("Data Capacity updated Succesfully")
                    ' divDashboard1.InnerHtml = "<div class=success>Production Decision Completed.</div>"

                Else
                    DisplayMessage("Problem addCapacity Updating Data.. " & myquery)
                    '  divDashboard1.InnerHtml = "<div class=warning>Production Decision Pending.</div>"
                End If
            End If
            colorTeams(Session("mode"))
        Catch exp As Exception
            DisplayMessage("Error:btnDecisionCapacity: " & exp.Message & myquery)
        End Try

    End Sub

    Protected Sub btnSubscription_Click(sender As Object, e As EventArgs) Handles btnSubscription.Click
        If Not rbTeams.SelectedIndex > -1 Then
            ''Some teams are not selected
            DisplayMessage("Select a Team First")
            Exit Sub
        End If
        Dim dtSubscriptionDetail As New System.Data.DataTable
        Dim dtCol21 As New System.Data.DataColumn("gameID", System.Type.GetType("System.Decimal"))
        dtCol21.DefaultValue = Session("gameID")
        dtSubscriptionDetail.Columns.Add(dtCol21)
        Dim dtCol22 As New System.Data.DataColumn("teamID", System.Type.GetType("System.Decimal"))
        dtCol22.DefaultValue = Session("teamID")
        dtSubscriptionDetail.Columns.Add(dtCol22)
        dtSubscriptionDetail.Columns.Add("subscriptionID", System.Type.GetType("System.Decimal"))

        For Each item As ListItem In cbSubscription.Items
            Dim dr = dtSubscriptionDetail.NewRow
            If item.Selected Then
                dr(2) = item.Value
                dtSubscriptionDetail.Rows.Add(dr)
            End If

        Next
        Dim res = updateTableinDBwithDeleteOld(dtSubscriptionDetail, "subscriptionDetail", "delete from subscriptionDetail where gameID=" & Session("gameID") & " and teamID=" & Session("teamID"))
        If res.Contains("ok") Then DisplayMessage("Your Subscription updated.") Else DisplayMessage("dtSubscriptionDetail data not updated followed by old delete." & res)

    End Sub

    Protected Sub btnDecisionFinances_Click(sender As Object, e As EventArgs) Handles btnDecisionFinances.Click
        If Not rbTeams.SelectedIndex > -1 Then
            ''Some teams are not selected
            DisplayMessage("Select a Team First")
            Exit Sub
        End If
    End Sub
End Class
