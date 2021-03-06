Imports Microsoft.VisualBasic

Public Class common
    Public Shared Function getsubMenu(ByVal type As String, ByVal subtype As Integer) As String
        Dim ret = ""
        Dim st(10) As String
        For i = 0 To 9
            st(i) = If(subtype = i, "<span class=highlight>", "<span>")
        Next

        If type = "admin" Then
            ret = "<ul><li>" & st(0) & "<a href=admin.aspx?mode=game>Create Game</a></span></li>" & _
                "<li>" & st(1) & "<a href=admin.aspx?mode=teams>Teams</a></span></li>" & _
                "<li>" & st(2) & "<a href=admin.aspx?mode=conditions>Conditions</a></span></li>" & _
                     "<li>" & st(3) & "<a href=admin.aspx?mode=logistics>Logistics</a></span></li>" & _
                                        "<li>" & st(4) & "<a href=admin.aspx?mode=tenders>Tenders</a></span></li>" & _
                        "<li>" & st(5) & "<a href=#>Upload Docs</a></span></li>" & _
                        "<li>" & st(6) & "<a href=#>Send Message</a></span></li>" & _
                         "<li>" & st(7) & "<a href=#>Feed Decision</a></span></li>" & _
                         "</ul>"
        ElseIf type = "decisions" Then
            ret = "<ul><li>" & st(0) & "<a href=decisions.aspx?mode=production>Production</a></span></li>" & _
               "<li>" & st(1) & "<a href=decisions.aspx?mode=capacity>Capacity</a></span></li>" & _
               "<li>" & st(2) & "<a href=decisions.aspx?mode=finances>Finances</a></span></li>" & _
                       "<li>" & st(3) & "<a href=#>Marketing</a></span></li>" & _
                       "<li>" & st(4) & "<a href=#>HumanResource</a></span></li>" & _
                       "<li>" & st(5) & "<a href=#>Key Decisions</a></span></li>" & _
                        "<li>" & st(6) & "<a href=decisions.aspx?mode=subscriptions>Subscriptions</a></span></li>" & _
                       "</ul>"
        ElseIf type = "messages" Then
            ret = "<ul><li>" & st(0) & "<a href=messages.aspx?mode=inbox>Inbox</a></span></li>" & _
               "<li>" & st(1) & "<a href=decisions.aspx?mode=capacity>Compose</a></span></li>" & _
                      "</ul>"
        ElseIf type = "results" Then
            ret = "<ul><li>" & st(0) & "<a href=results.aspx?mode=generate>Generate</a></span></li>" & _
               "<li>" & st(1) & "<a href=results.aspx?mode=view>View</a></span></li>" & _
                      "</ul>"
        End If
        Return ret
    End Function
    
End Class
