Imports common
Imports dbOperation
Partial Class messages
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then

            Dim submenu = 0
             If Session("gameID") Is Nothing Then
                Response.Redirect("admin.aspx?source=messages")
            End If
            Session("mode") = Request.Params("mode")
            If Request.Params("mode") Is Nothing Then
                Session("mode") = "production"
            End If

          
            If Session("mode") = "inbox" Then

                'pnlProduction.Visible = True
                gvInbox.datasource = getDBTable("select subject, sender from messages where gameid=" & Session("gameID"))
                gvinbox.databind()
            ElseIf Session("mode") = "compose" Then
                'nlCapacity.Visible = True
                submenu = 1

            End If

            div_submenu.InnerHtml = getsubMenu("messages", submenu)
            myinfo.InnerHtml = "Wealth: $xxx " & Session("user") & "<span>|</span><a href=admin.aspx?mode=kill>Logout</a>"

            ' showDashboard()
        End If
    End Sub
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

 
    Protected Sub gvInbox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvInbox.SelectedIndexChanged

    End Sub
End Class
