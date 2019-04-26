Imports System
Imports System.Data
Imports System.IO
Imports System.Configuration
Imports System.Web
Imports System.Net.Mail

Public Class EnviaMail

    Function Enviar(QueTo As String, QueSubject As String, QueTexto As String) As String

        Dim Retorno As String = "OK"
        Try
            Dim mail As New MailMessage()
            ' Gmail
            'mail.From = New MailAddress("pedro.zilli@gmail.com")

            'FGV
            mail.From = New MailAddress("ANA2016_FGV@fgv.br")

            mail.To.Add(QueTo)
            mail.Subject = QueSubject
            mail.IsBodyHtml = True
            mail.Body = QueTexto

            ' FGV
            Dim SMTP As New SmtpClient("smtpapp.fgv.br")
            'Gmail
            'Dim SMTP As New SmtpClient("smtp.gmail.com")

            ' Gmail
            'SMTP.Port = 587
            'SMTP.Credentials = New Net.NetworkCredential("pedro.zilli@gmail.com", "canterbury")
            'SMTP.EnableSsl = True

            ' FGV
            SMTP.Port = 25

            SMTP.Send(mail)
        Catch ex As Exception
            Retorno = "Erro no envio do e-mail." & ex.ToString
        End Try
        Return Retorno
    End Function
End Class
