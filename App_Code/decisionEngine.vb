Imports Microsoft.VisualBasic


Public Class decisionEngine
    Public Shared Function allocateProductAsPerDemand(ByVal forecastedDemand As Integer, ByVal TotalTeams As Integer, ByVal myUnitCost As Integer, ByVal avgMarketCost As Double, ByVal Elasticity As Integer, ByVal myProduction As Integer) As Integer
        'myDemandAllocation = forecastedDemand/TotalTeams/(myUnitCost/avgMarketCost)^elasticity
        Dim myDemandAllocation = 0
        myDemandAllocation = Math.Round(forecastedDemand / TotalTeams / (myUnitCost / avgMarketCost) ^ Elasticity)

        'Additional Logic if mysells are more then production then reduce it.
        If myDemandAllocation > myProduction Then myDemandAllocation = myProduction

        Return myDemandAllocation
    End Function
    Public Shared Function getManpowerSlabBasedCost(ByVal dt As System.Data.DataTable, ByVal dtslab As System.Data.DataTable) As System.Data.DataTable
        ' dt must be teamID	Laptop	Servers	Peripherals	Storage	
        Try

            dt.Columns.Add("TotalCost")
            For Each dr In dt.Rows
                Dim totalCost = 0
                For i = 1 To 4
                    Dim manpowercost = 0
                    '18000 < 1000   '25000
                    If dr(i) > dtslab.Rows(0)(4) Then manpowercost = dtslab.Rows(0)(0) * dtslab.Rows(0)(4) Else manpowercost = dtslab.Rows(0)(0) * dr(i)
                    '18000 < 3000                '60000
                    If dr(i) > dtslab.Rows(0)(5) Then manpowercost = manpowercost + dtslab.Rows(0)(1) * (dtslab.Rows(0)(5) - dtslab.Rows(0)(4)) Else manpowercost = manpowercost + dtslab.Rows(0)(1) * (dr(i) - dtslab.Rows(0)(4))
                    '18000 < 7000               '140000
                    If dr(i) > dtslab.Rows(0)(6) Then
                        manpowercost = manpowercost + dtslab.Rows(0)(2) * (dtslab.Rows(0)(6) - dtslab.Rows(0)(5))
                        manpowercost = manpowercost + dtslab.Rows(0)(3) * (dr(i) - dtslab.Rows(0)(6))
                    ElseIf dr(i) > dtslab.Rows(0)(5) Then
                        manpowercost = manpowercost + dtslab.Rows(0)(2) * (dr(i) - dtslab.Rows(0)(5))
                    ElseIf dr(i) = 0 Then
                        Continue For '' 0 production- 0 cost


                    End If
                    ' If dr(1) > dtslab.Rows(0)(6) Then manpowercost = manpowercost + dtslab.Rows(0)(3) * dtslab.Rows(0)(7) - dtslab.Rows(0)(6) Else manpowercost = manpowercost + dtslab.Rows(0)(3) * dr(1) - dtslab.Rows(0)(7)
                    dr(i) = manpowercost
                    totalCost = totalCost + manpowercost
                Next
                dr(5) = totalCost
            Next
            Return dt
        Catch e As Exception
            'lblDebug.text = e.Message
            Dim dt1 As New System.Data.DataTable
            dt1.Columns.Add("Error")
            Dim tmprow = dt1.NewRow

            tmprow(0) = "Error: " & e.Message
            dt1.Rows.Add(tmprow)
            ' Return dt.NewRow("Error in getdatatable")
            Return dt1
        End Try
    End Function
    Public Shared Function getWareHousingCost(ByVal dt As System.Data.DataTable, ByVal dtwarehousecost As System.Data.DataTable) As System.Data.DataTable
        ' dt must be teamID	Laptop	Servers	Peripherals	Storage	closingX	closingY
        Try

            dt.Columns.Add("WarehousingCost")
            For Each dr In dt.Rows
                Dim totalCost = 0
                For i = 1 To 6
                    totalCost = totalCost + dr(i) * dtwarehousecost.Rows(0)(i - 1)

                Next
                dr("WarehousingCost") = totalCost
            Next
            Return dt
        Catch e As Exception
            'lblDebug.text = e.Message
            Dim dt1 As New System.Data.DataTable
            dt1.Columns.Add("Error")
            Dim tmprow = dt1.NewRow

            tmprow(0) = "Error: " & e.Message
            dt1.Rows.Add(tmprow)
            ' Return dt.NewRow("Error in getdatatable")
            Return dt1
        End Try
    End Function
    Public Shared Function pivot2colTable(ByVal oldTable As System.Data.DataTable) As System.Data.DataTable

        Dim newTable As New System.Data.DataTable
        Dim dr As System.Data.DataRow
        Dim pivotColumnOrdinal = 0
        ' add pivot column name
        newTable.Columns.Add(oldTable.Columns(pivotColumnOrdinal).ColumnName)

        ' add pivot column values in each row as column headers to new Table
        For Each row In oldTable.Rows
            newTable.Columns.Add(row(pivotColumnOrdinal))
        Next

        ' loop through columns
        For col = 0 To oldTable.Columns.Count - 1
            'pivot column doen't get it's own row (it is already a header)
            If col = pivotColumnOrdinal Then Continue For

            ' each column becomes a new row
            dr = newTable.NewRow()

            ' add the Column Name in the first Column
            dr(0) = oldTable.Columns(col).ColumnName

            ' add data from every row to the pivoted row
            For row = 0 To oldTable.Rows.Count - 1
                dr(row + 1) = oldTable.Rows(row)(col)
            Next

            'add the DataRow to the new table
            newTable.Rows.Add(dr)
        Next

        Return newTable
    End Function
End Class
