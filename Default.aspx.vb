
Partial Class _Default
    Inherits System.Web.UI.Page

    Protected Sub btnNewGame_Click(sender As Object, e As EventArgs) Handles btnNewGame.Click
        TextBox1.Text = dbOperation.executeDB("insert into gameMaster (gdate) values (datetime('now'))")
    End Sub
End Class
