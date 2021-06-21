
Partial Class temp
    Inherits System.Web.UI.Page
    Dim finalDT As New System.Data.DataTable()

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then

            'finalDT = dbOperation.getDBTable("select * from decisionMaster")
            'gvTemp.DataSource = finalDT
            'ViewState("mytable") = finalDT
            ''dim dt = ctype(viewstate("mytable"),datatable)
            'gvTemp.DataBind
            'Label1.Text = dbOperation.encryptDB("jeannie16")
            Label1.Text = dbOperation.removeDBPassword()
           
        End If
    End Sub

    Protected Sub gvTemp_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles gvTemp.RowCancelingEdit

    End Sub

    Protected Sub gvTemp_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles gvTemp.RowEditing
        'Dim row = gvTemp.Rows(e.NewEditIndex)
        '  Label1.Text = row.Cells(2).Text
        gvTemp.EditIndex = e.NewEditIndex
        gvTemp.DataSource = ViewState("mytable")
        gvTemp.DataBind()
        Label2.Text = "Edit clicked"
    End Sub

    Protected Sub gvTemp_RowUpdated(sender As Object, e As GridViewUpdatedEventArgs) Handles gvTemp.RowUpdated
        'Dim row = gvTemp.Rows(e.AffectedRows)
        'Label1.Text = e.AffectedRows.ToString + row.Cells(2).Text + e.NewValues(1).ToString
        Label2.Text = "updated"
        gvTemp.EditIndex = -1

    End Sub

    Protected Sub gvTemp_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles gvTemp.RowUpdating
        Try
            gvTemp.EditIndex = -1

            Dim row = gvTemp.Rows(e.RowIndex)

            Label1.Text = (CType((row.Cells(2).Controls(0)), TextBox)).Text ' + e.NewValues(1).ToString
            Dim dt = CType(ViewState("mytable"), System.Data.DataTable)
            For i = 0 To row.Cells.Count - 2 Step 1
                dt.Rows(row.DataItemIndex)(i) = (CType((row.Cells(i + 1).Controls(0)), TextBox)).Text
            Next
            'GridView1.DataSource = dt
            'GridView1.DataBind()
            gvTemp.DataSource = dt
            gvTemp.DataBind()
            Label2.Text = "updating"
            e.Cancel = False
        Catch ex As Exception
            Label2.Text = "updating.." + ex.Message
        End Try
        
    End Sub
End Class
