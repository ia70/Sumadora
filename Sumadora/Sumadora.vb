﻿Imports Libersoft
Public Class Sumadora

#Region "Variables Globales"

    '// Variables para calculo de operaciones //////////////////////////////////////////////////////////|
    Private Total As Decimal = 0            'Especifica el total de la las operaciones hasta el ommento |
    Private Punto As Boolean = False        'Especifica si ya se ha precionado la tecla punto           |
    Private Operacion As Char = "0"         'Especifica que operación se ha seleccionado                |
    Private OpeAct As Boolean = False       'Especifica si ya se ha seleccionado una operacion          |
    Private OpeEnd As Boolean = True        'Especifica si ya se realizó el calculo                     |
    '\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\|

    '// Variables usados en manipulación de tabla //////////////////////////////////////////////////////|
    Private NumFilas As Integer = 3         'Especifica el número de filas actuales de la tabla         |
    '\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\|

    '// Variables usados en manipulación de tabla //////////////////////////////////////////////////////|
    'Private Ticket As New cImpresoraTickets 'Variable para comunicarse con la impresora                 |
    '\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\|

#End Region

#Region "Motor KeyPress"
    Private Function ValidadorNumero(ByVal Valor As KeyPressEventArgs) As KeyPressEventArgs
        Dim Auxiliar As String
        Auxiliar = tbResultado.Text

        If Asc(Valor.KeyChar) = 8 Then          'Tecla de retroceso
            If OpeEnd Then
                tbResultado.Text = ""
            End If
            Return Valor
        ElseIf Asc(Valor.KeyChar) = 13 Then     'Tecla Enter
            If Operacion = "0" Then
                Valor.KeyChar = ""
            ElseIf Not tbResultado.Text = "" And Not tbResultado.Text = "." Then
                If Not OpeAct Then
                    Valor = RealizarCalculo(Valor)
                    Tabla_Agregar(Auxiliar, Operacion)
                End If
                Nueva_operación()
                If cbPrint.Checked Then
                    Imprimir()
                End If
            Else
                Valor.KeyChar = ""
            End If
        ElseIf (Asc(Valor.KeyChar) < 42 Or Asc(Valor.KeyChar) > 57 Or Asc(Valor.KeyChar) = 44) Then
            Valor.KeyChar = ""
        ElseIf Asc(Valor.KeyChar) = 42 Then     'Operación "x"
            If tbResultado.Text = "" Or tbResultado.Text = "." Then
                Valor.KeyChar = ""
                Return Valor
            End If
            If OpeAct Then
                Operacion = "x"
                Valor.KeyChar = ""
                Return Valor
            ElseIf Operacion = "0" Then
                Operacion = "x"
            End If
            Valor = RealizarCalculo(Valor)
            Tabla_Agregar(Auxiliar, Operacion)
            Operacion = "x"
        ElseIf Asc(Valor.KeyChar) = 43 Then     'Operación "+"
            If tbResultado.Text = "" Or tbResultado.Text = "." Then
                Valor.KeyChar = ""
                Return Valor
            End If
            If OpeAct Then
                Operacion = "+"
                Valor.KeyChar = ""
                Return Valor
            ElseIf Operacion = "0" Then
                Operacion = "+"
            End If
            Valor = RealizarCalculo(Valor)
            Tabla_Agregar(Auxiliar, Operacion)
            Operacion = "+"
        ElseIf Asc(Valor.KeyChar) = 45 Then     'Operación "-"
            If tbResultado.Text = "" Or tbResultado.Text = "." Then
                Valor.KeyChar = ""
                Return Valor
            End If
            If OpeAct Then
                Operacion = "-"
                Valor.KeyChar = ""
                Return Valor
            ElseIf Operacion = "0" Then
                Operacion = "-"
            End If
            Valor = RealizarCalculo(Valor)
            Tabla_Agregar(Auxiliar, Operacion)
            Operacion = "-"
        ElseIf Asc(Valor.KeyChar) = 47 Then     'Operación "/"
            If tbResultado.Text = "" Or tbResultado.Text = "." Then
                Valor.KeyChar = ""
                Return Valor
            End If
            If OpeAct Then
                Operacion = "/"
                Valor.KeyChar = ""
                Return Valor
            ElseIf Operacion = "0" Then
                Operacion = "/"
            End If
            Valor = RealizarCalculo(Valor)
            Tabla_Agregar(Auxiliar, Operacion)
            Operacion = "/"
        ElseIf Asc(Valor.KeyChar) = 46 Then     'Punto

            If InStr(tbResultado.Text, ".") Then
                Valor.KeyChar = ""
                Return Valor
            Else
                If OpeAct Then
                    tbResultado.Text = ""
                    OpeAct = False
                End If
            End If

        ElseIf OpeAct Then
            tbResultado.Text = ""
            If OpeEnd And dgvHistorial.Rows.Count > 3 Then
                Nueva_operación()
                Tabla_Limpiar()
            End If
            OpeAct = False
            Punto = False
        Else
            OpeEnd = False
            OpeAct = False
        End If
        'tbResultado.Text = Format(Val(tbResultado.Text), “##,##0.00”).ToString
        Return Valor
    End Function

    Private Function RealizarCalculo(ByVal Valor As KeyPressEventArgs) As KeyPressEventArgs

        If Not tbResultado.Text = "" And Not tbResultado.Text = "." And Not OpeAct Then         'Antes de operacion que no sea vacio o solo punto
            If Total = 0 Then
                Total = Val(tbResultado.Text)

                'Tabla_Agregar(tbResultado.Text, Operacion) '2 = Resta
            Else
                Select Case Operacion
                    Case "+"
                        Total = Total + Val(tbResultado.Text)
                    Case "-"
                        Total = Total - Val(tbResultado.Text)
                    Case "x"
                        Total = Total * Val(tbResultado.Text)
                    Case "/"
                        Total = Total / Val(tbResultado.Text)
                End Select

                'Tabla_Agregar(tbResultado.Text, Operacion)

                tbResultado.Text = Format(Total, "##,##0.00")
            End If
            OpeAct = True
        End If
        Valor.KeyChar = ""
        Return Valor
    End Function

    Private Sub Nueva_operación()
        OpeEnd = True
        Punto = False
        OpeAct = True
        Operacion = "0"
        Total = 0
        'NumFilas = 3
    End Sub
#End Region
#Region "Motor Button"


    Private Sub MoverCursor(ByVal Valor As String)
        Dim e As New KeyPressEventArgs("")
        e.KeyChar = Valor
        e = ValidadorNumero(e)
        If tbResultado.TextLength < 10 And Not e.KeyChar = "" Then
            tbResultado.Text = tbResultado.Text + e.KeyChar
        End If
        tbResultado.SelectionStart = tbResultado.TextLength
        tbResultado.Focus()
    End Sub
#End Region
#Region "Operaciones con la Tabla"
    Private Sub Tabla_Agregar(ByVal Valor As Decimal, ByVal Ope As String)
        NumFilas += 1
        dgvHistorial.Rows.Add()
        dgvHistorial(0, NumFilas - 1).Value = Format(Total, "##,##0.00")
        dgvHistorial(0, NumFilas - 2).Value = "--------------------"
        dgvHistorial(0, NumFilas - 3).Value = Format(Valor, "##,##0.00")
        dgvHistorial(1, NumFilas - 1).Value = "0"
        dgvHistorial(1, NumFilas - 2).Value = "0"
        dgvHistorial(1, NumFilas - 3).Value = Ope
        dgvHistorial.FirstDisplayedCell = dgvHistorial(0, NumFilas - 1)
    End Sub
    Private Sub Tabla_Limpiar()
        dgvHistorial.Rows.Clear()
        NumFilas = 3
        dgvHistorial.Rows.Add(3)
        dgvHistorial(0, 0).Value = "0.00"
        dgvHistorial(0, 1).Value = "--------------------"
        dgvHistorial(0, 2).Value = "0.00"
        dgvHistorial(1, 0).Value = "0"
        dgvHistorial(1, 1).Value = "0"
        dgvHistorial(1, 2).Value = "0"
        dgvHistorial.ClearSelection()
        tbResultado.Focus()
    End Sub
    Private Sub Tabla_Calcular()
        Dim i As Integer
        If NumFilas > 3 Then
            Total = 0

            For i = 0 To NumFilas - 3
                dgvHistorial(0, i).Value = Format(Val(dgvHistorial(0, i).Value), "##,##0.00")
                Total = Total + Val(dgvHistorial(0, i).Value)
            Next
            'For i = 0 To NumFilas - 3
            'Total = Total + Val(dgvHistorial(0, i).Value)
            'Next
            dgvHistorial(0, NumFilas - 1).Value = Format(Total, "##,##0.00")
            dgvHistorial(0, NumFilas - 2).Value = "--------------------"
            tbResultado.Text = Format(Total, "##,##0.00")
            dgvHistorial.FirstDisplayedCell = dgvHistorial(0, NumFilas - 1)
        End If
    End Sub

#End Region
#Region "Botones de Operaciones"
    Private Sub cbDividir_Click(sender As Object, e As EventArgs) Handles cbDividir.Click
        MoverCursor("/")
    End Sub

    Private Sub cbPor_Click(sender As Object, e As EventArgs) Handles cbPor.Click
        MoverCursor("*")
    End Sub

    Private Sub cbMenos_Click(sender As Object, e As EventArgs) Handles cbMenos.Click
        MoverCursor("-")
    End Sub

    Private Sub cbIgual_Click(sender As Object, e As EventArgs) Handles cbIgual.Click
        MoverCursor(Chr(13))
    End Sub

    Private Sub cbMas_Click(sender As Object, e As EventArgs) Handles cbMas.Click
        MoverCursor("+")
    End Sub
#End Region
#Region "Botones de Números"
    Private Sub cb0_Click(sender As Object, e As EventArgs) Handles cb0.Click
        MoverCursor("0")
    End Sub

    Private Sub cbPunto_Click(sender As Object, e As EventArgs) Handles cbPunto.Click
        MoverCursor(".")
    End Sub

    Private Sub cb1_Click(sender As Object, e As EventArgs) Handles cb1.Click
        MoverCursor("1")
    End Sub

    Private Sub cb2_Click(sender As Object, e As EventArgs) Handles cb2.Click
        MoverCursor("2")
    End Sub

    Private Sub cb3_Click(sender As Object, e As EventArgs) Handles cb3.Click
        MoverCursor("3")
    End Sub

    Private Sub cb4_Click(sender As Object, e As EventArgs) Handles cb4.Click
        MoverCursor("4")
    End Sub

    Private Sub cb5_Click(sender As Object, e As EventArgs) Handles cb5.Click
        MoverCursor("5")
    End Sub

    Private Sub cb6_Click(sender As Object, e As EventArgs) Handles cb6.Click
        MoverCursor("6")
    End Sub

    Private Sub cb7_Click(sender As Object, e As EventArgs) Handles cb7.Click
        MoverCursor("7")
    End Sub

    Private Sub cb8_Click(sender As Object, e As EventArgs) Handles cb8.Click
        MoverCursor("8")
    End Sub

    Private Sub cb9_Click(sender As Object, e As EventArgs) Handles cb9.Click
        MoverCursor("9")
    End Sub

#End Region
#Region "Elementos de Formulario Eventos"
    Private Sub tbResultado_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tbResultado.KeyPress  'Tecla pulsada
        ValidadorNumero(e)
    End Sub
    Private Sub Form1_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint
        dgvHistorial.Rows.Add(2)
        dgvHistorial(0, 0).Value = "0.00"
        dgvHistorial(0, 1).Value = "--------------------"
        dgvHistorial(0, 2).Value = "0.00"
        dgvHistorial.ClearSelection()
        tbResultado.Focus()
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Nueva_operación()
        Tabla_Limpiar()
        tbResultado.Text = ""
        tbResultado.Focus()
    End Sub
    Private Sub dgvHistorial_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvHistorial.CellEndEdit
        Tabla_Calcular()
        tbResultado.SelectionStart = tbResultado.TextLength
        tbResultado.Focus()
    End Sub
#End Region
#Region "Impresora"
    Private Sub Imprimir()
        Dim Ticket As New cImpresoraTickets
        Ticket.Tabla = dgvHistorial
        If cbLogo.Checked Then
            Ticket.Logotipo = Image.FromFile("Logo.jpeg")
        End If

        If cbFecha.Checked Then
            Ticket.Fecha = Format(Date.Now, "dd/MM/yyyy").ToString
        End If
        Ticket.Empresa = tbEmpresa.Text
        Ticket.ImprimirTicket()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        Imprimir()
        tbResultado.SelectionStart = tbResultado.TextLength
        tbResultado.Focus()
    End Sub

#End Region

    Private Sub cbFecha_Click(sender As Object, e As EventArgs) Handles cbFecha.Click
        tbResultado.SelectionStart = tbResultado.TextLength
        tbResultado.Focus()
    End Sub

    Private Sub cbLogo_Click(sender As Object, e As EventArgs) Handles cbLogo.Click
        tbResultado.SelectionStart = tbResultado.TextLength
        tbResultado.Focus()
    End Sub

    Private Sub cbPrint_Click(sender As Object, e As EventArgs) Handles cbPrint.Click
        tbResultado.SelectionStart = tbResultado.TextLength
        tbResultado.Focus()
    End Sub

End Class
