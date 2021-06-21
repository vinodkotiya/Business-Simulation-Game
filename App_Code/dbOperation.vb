Imports Microsoft.VisualBasic
Imports System.Data.SQLite
Imports System.Data


Public Class dbOperation
    Public Function helloWorld() As Boolean
        Return True
    End Function
    Public Shared Function encryptDB(ByVal pwd As String) As String
        ''this function will return single value from table according to myQuery
        'Create Connection String
        Using connection As New SQLiteConnection(System.Configuration.ConfigurationManager.ConnectionStrings("vindb").ConnectionString)
            Try
                connection.Open()
                connection.ChangePassword(pwd)
                connection.Close()
                Return "ok"
            Catch ex As Exception
                Return "Error:" & ex.Message
            End Try

        End Using
    End Function
    Public Shared Function removeDBPassword() As String
        ''this function will return single value from table according to myQuery
        'Create Connection String
        Using connection As New SQLiteConnection(System.Configuration.ConfigurationManager.ConnectionStrings("vindb").ConnectionString)
            Try
                connection.Open()
                connection.ChangePassword("")
                connection.Close()
                Return "ok"
            Catch ex As Exception
                Return "Error:" & ex.Message
            End Try

        End Using
    End Function

    Public Shared Function getDBsingle(ByVal mysql As String) As String
        ''this function will return single value from table according to myQuery
        'Create Connection String
        Using connection As New SQLiteConnection(System.Configuration.ConfigurationManager.ConnectionStrings("vindb").ConnectionString)
            Dim sqlComm As SQLiteCommand
            Dim sqlReader As SQLiteDataReader
            Dim result As String
            Dim dt As New DataTable()
            Dim dataTableRowCount As Integer
            Try
                connection.Open()
                sqlComm = New SQLiteCommand(mysql, connection)
                sqlReader = sqlComm.ExecuteReader()
                dt.Load(sqlReader)
                dataTableRowCount = dt.Rows.Count
                sqlReader.Close()
                sqlComm.Dispose()
                If dataTableRowCount = 1 Then
                    result = dt.Rows(0).Item(0).ToString()
                    connection.Close()
                    Return result
                Else
                    connection.Close()
                    Return "Error:Too many Records Found"
                End If

            Catch e As Exception
                'lblDebug.text = e.Message
                connection.Close()
                Return "Error" + e.Message
            End Try
            connection.Close()
        End Using
    End Function
    Public Shared Function executeDB(ByVal mysql As String) As String

        'Create Connection String
        Using connection As New SQLiteConnection(System.Configuration.ConfigurationManager.ConnectionStrings("vindb").ConnectionString)
            Dim sqlComm As SQLiteCommand
            Dim sqlReader As SQLiteDataReader

            Try
                'connection.Close()
                connection.Open()
                sqlComm = New SQLiteCommand(mysql, connection)
                sqlReader = sqlComm.ExecuteReader()
                'Add Insert Statement
                sqlComm.Dispose()

                connection.Close()
                executeDB = "ok"


            Catch exp As Exception
                'lbldebug.Text = exp.Message
                connection.Close()
                executeDB = "Error:" + exp.Message

            End Try
            'Close Database connection
            'and Dispose Database objects
        End Using

    End Function
    Public Shared Function insertTableinDB(ByVal dt As DataTable, ByVal mytablename As String) As String

        'Create Connection String
        Dim myquery = "select * from " & mytablename & " limit 1"
        Using connection As New SQLiteConnection(System.Configuration.ConfigurationManager.ConnectionStrings("vindb").ConnectionString)

            Try
                'connection.Close()
                connection.Open()
                '' sqlComm = New SQLiteCommand("select * from gamemaster", connection)
                Dim da As New SQLiteDataAdapter(myquery, connection)
                Dim dcmd As SQLiteCommandBuilder = New SQLiteCommandBuilder(da)
                da.MissingSchemaAction = MissingSchemaAction.AddWithKey
                da.Update(dt)

                ''  sqlComm.Dispose()
                da.Dispose()
                connection.Close()
                insertTableinDB = "ok"


            Catch exp As Exception
                'lbldebug.Text = exp.Message
                connection.Close()
                insertTableinDB = "Error:" + exp.Message

            End Try
            '' Close Database connection
            '' and Dispose Database objects
        End Using

    End Function
    Public Shared Function updateTableinDBwithDeleteOld(ByVal dt As DataTable, ByVal mytablename As String, ByVal delQuery As String) As String

        'Create Connection String
        Dim myquery = "select * from " & mytablename & " limit 1"
        Dim updCmd = ""
        Using connection As New SQLiteConnection(System.Configuration.ConfigurationManager.ConnectionStrings("vindb").ConnectionString)

            Try
                'connection.Close()
                connection.Open()
                ''delete old data
                Dim sqlComm = New SQLiteCommand(delQuery, connection)
                Dim sqlReader = sqlComm.ExecuteReader()
                sqlComm.Dispose()


                Dim da As New SQLiteDataAdapter(myquery, connection)
                Dim dcmd As SQLiteCommandBuilder = New SQLiteCommandBuilder(da)
                da.MissingSchemaAction = MissingSchemaAction.AddWithKey
                ' updCmd = dcmd.GetUpdateCommand().CommandText
                'updCmd = dcmd.GetDeleteCommand().CommandText
                da.Update(dt)
                'da.AcceptChangesDuringUpdate()

                ''  sqlComm.Dispose()
                da.Dispose()
                connection.Close()
                Return "ok"


            Catch exp As Exception
                'lbldebug.Text = exp.Message
                connection.Close()
                Return "Error:" + exp.Message & updCmd

            End Try
            '' Close Database connection
            '' and Dispose Database objects
        End Using

    End Function
    Public Shared Function getDBTable(ByVal myQuery As String) As DataTable
        ''this function will return DataTable from table according to myQuery
        Using connection As New SQLiteConnection(System.Configuration.ConfigurationManager.ConnectionStrings("vindb").ConnectionString)
            ' connection.Close()
            connection.Open()

            Dim sqlComm As SQLiteCommand
            Dim sqlReader As SQLiteDataReader
            Dim dt As New DataTable()
            Dim dataTableRowCount As Integer
            Try
                sqlComm = New SQLiteCommand(myQuery, connection)
                sqlReader = sqlComm.ExecuteReader()
                dt.Load(sqlReader)
                dataTableRowCount = dt.Rows.Count
                sqlReader.Close()
                sqlComm.Dispose()
                If Not dt Is Nothing Then

                    connection.Close()
                    Return dt
                Else
                    connection.Close()
                    Return dt.NewRow("Too many Records Found")
                End If

            Catch e As Exception
                'lblDebug.text = e.Message
                dt.Columns.Add("Error")
                Dim tmprow = dt.NewRow
                connection.Close()
                tmprow(0) = "Error: " & e.Message & myQuery
                dt.Rows.Add(tmprow)
                ' Return dt.NewRow("Error in getdatatable")
                Return dt
            End Try
            connection.Close()
        End Using
    End Function
End Class
