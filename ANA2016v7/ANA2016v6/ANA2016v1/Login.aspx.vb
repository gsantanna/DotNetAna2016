Imports System.Web
Imports System.Web.UI
Imports System.IO
Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.EntityFramework
Imports Microsoft.AspNet.Identity.Owin
Imports Microsoft.Owin.Security
Imports Owin

Partial Public Class Login
    Inherits Page

    Dim mvSessionID As String

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        ' Gera um ID de sessão randômico
        Dim R As New Random
        mvSessionID = Trim(Str(R.Next(1000000000)))

    End Sub

    Protected Sub cmdEntrar_Click(sender As Object, e As EventArgs) Handles cmdEntrar.Click
        UserName.Text = Trim(UserName.Text)
        Call AnalisaLogin()
    End Sub

    Protected Sub AnalisaLogin()
        Dim Retorno As String = ValidaCPF(UserName.Text)
        If Retorno <> "" Then
            Me.MessageError.Text = TextoVermelho(Retorno)
        Else
            ' OK, sintaxe correta até aqui
            Dim LICobject As New LogInCode

            ' Passa CPF e senha
            Retorno = LICobject.RecebeLogin(UserName.Text, Password.Text)
            LICobject = Nothing

            ' O retorno é uma lista de campos coletados do cadastro
            Dim tmp As String() = Split(Retorno, "|")

            If Left(Retorno, 2) <> "OK" Then
                ' Mostra o erro para o operador
                MessageError.Text = TextoVermelho(Retorno)
            ElseIf ubound(tmp) < 6 Then
                MessageError.Text = TextoVermelho("Houve um erro no acesso ao banco de dados.")
            Else
                Dim NomePagina As String
                Dim URLpagina As String

                If tmp(1) = "1" Then
                    'If tmp(4) = "BR" Then
                    '    URLpagina = "~/Account/EscolhaUFbase.aspx"
                    '    NomePagina = "EscolhaUFbase"
                    'Else
                    URLpagina = "~/BemVindo.aspx"
                    NomePagina = "BemVindo"
                    'End If

                Else
                    URLpagina = "~/Account/NovaSenha.aspx"
                    NomePagina = "NovaSenha"
                End If
                ' Inicia uma sessão
                Call RelogioIniciaLogin(UserName.Text, mvSessionID, tmp(4))
                ' Como foi lido: SELECT p.CPF + '|' + p.NO_PESSOA + '|' + p.SG_UF_ALOC + '|' + NO_FUNCAO + '|' + ID_POLO"
                Response.Redirect(ResolveUrl(URLpagina & MontaQueryString("Login,*", UserName.Text, tmp(4), tmp(5), tmp(6), tmp(3), "@", NomePagina, gcFiltrosInicializacao, mvSessionID)))
            End If
        End If
    End Sub




    Protected Sub AnalisaCadastroAvulso()
        Dim Retorno As String = ValidaCPF(UserName.Text)

        If Retorno <> "" Then
            Me.MessageError.Text = TextoVermelho(Retorno)
        Else
            ' OK, sintaxe correta até aqui
            Dim myCPF As String = ColetaValorString("SELECT CPF from PESSOAL where CPF='" & UserName.Text & "'")
            If myCPF IsNot Nothing Then
                Me.MessageError.Text = TextoVermelho("O CPF " & UserName.Text & " já foi cadastrado.")
            Else
                ' Grava porque é preciso entrar em CadastroPessoal, mesmo que não tenha UFbase
                Call RelogioIniciaLogin(UserName.Text, mvSessionID, "Todos")
                ' Direciona para a tela de cadastro
                Response.Redirect(ResolveUrl("~/CadastroPessoal.aspx" & MontaQueryString("Login", UserName.Text, "@", "@", "@", "@", "@", "@", gcFiltrosInicializacao, mvSessionID)))
            End If
        End If
    End Sub

    Protected Sub LinkButton1_Click(sender As Object, e As EventArgs) Handles LinkButton1.Click

        Dim Retorno As String = ValidaCPF(UserName.Text)

        If Retorno <> "" Then
            Me.MessageError.Text = Retorno
        Else
            ' OK, sintaxe correta até aqui
            Dim sSQL As String = "SELECT p.TX_EMAIL FROM PESSOAL p WHERE p.CPF='" & UserName.Text & "'"
            Retorno = ColetaValorString(sSQL)

            If Retorno = "" Then
                MessageError.Text = "O CPF " & UserName.Text & " não está cadastrado."
                Exit Sub
            End If

            ' Atualiza os dados da senha provisória
            Dim NovaSenha As String = SenhaRandomica()

            ' EXEMPLO DA STACKOVEFLOW
            ' Abre uma conexão
            Dim MyC = New System.Data.SqlClient.SqlConnection(Ligacao())
            MyC.Open()
            Dim MeuComando = New System.Data.SqlClient.SqlCommand()
            Try
                MeuComando.Connection = MyC
                MeuComando.CommandText = "UPDATE PESSOAL set SenhaProvisoria=@NovaSenha, DataValidadeSenha=@Validade WHERE CPF=@CPF"
                MeuComando.Parameters.Add("@NovaSenha", SqlDbType.VarChar).Value = Hash512(NovaSenha, UserName.Text)
                MeuComando.Parameters.Add("@CPF", SqlDbType.VarChar).Value = UserName.Text
                MeuComando.Parameters.Add("@Validade", SqlDbType.DateTime, 32).Value = DateAdd(DateInterval.Day, 2, DateTime.Now)
                MeuComando.ExecuteNonQuery()
            Catch ex As Exception
                MessageError.Text = TextoVermelho("Erro na gravação da senha provisória.  " & ex.Message)
                Exit Sub
            Finally
                MeuComando.Dispose()
                MyC.Dispose()
            End Try

            Dim x As String() = Split(Retorno, "|")

            ' Lê o modelo do e-mail
            Dim Texto As String
            Try
                Texto = File.ReadAllText(Request.PhysicalApplicationPath & "NOTIFICACOES/Notificacao_Esqueci_Senha.htm")
                'Texto = File.ReadAllText("~/NOTIFICACOES/Notificacao_Esqueci_Senha.htm")
            Catch ex As Exception
                MessageError.Text = TextoVermelho("Erro na preparação do e-mail.  " & ex.Message)
                Exit Sub
            End Try
            ' Envia o e-mail
            Dim CP = New EnviaMail
            Texto = Replace(Texto, "cadastro_CPF_CGC", "CPF")
            Texto = Replace(Texto, "cadastro_CPF", UserName.Text)
            Texto = Replace(Texto, "cadastro_data", DateTime.Now.ToString("dd/MM/yyyy"))
            Texto = Replace(Texto, "cadastro_hora", DateTime.Now.ToString("H:mm"))
            'Texto = Replace(Texto, "senha_data", DateAdd(DateInterval.Day, 2, DateTime.Now).ToString("dd/MM/yyyy"))
            'Texto = Replace(Texto, "senha_horas", DateAdd(DateInterval.Day, 2, DateTime.Now).ToString("H:mm"))
            Texto = Replace(Texto, "senha_nova", NovaSenha)

            Texto = CP.Enviar(x(0), "Portal FGV - ANA2016 :: Recuperação de senha", Texto)
            CP = Nothing
            ' Notifica o operador
            If Texto <> "OK" Then
                MessageError.Text = TextoVermelho(Texto)
            Else
                MessageError.Text = "Uma nova senha foi enviada para o e-mail " & x(0)
            End If
        End If
    End Sub

    Protected Sub cmdNovoCadastro_Click(sender As Object, e As EventArgs) Handles cmdNovoCadastro.Click
        Call AnalisaCadastroAvulso()
    End Sub

End Class
