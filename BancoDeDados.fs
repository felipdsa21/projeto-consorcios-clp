//Contém as funções para conectar e interagir com o banco de dados.

module BancoDeDados

open Npgsql

let conexaoString = "Host=localhost;Port=5432;Username=seu_usuario;Password=sua_senha;Database=oxebanking"

let conectarBanco () =
    let conexao = new NpgsqlConnection(conexaoString)
    conexao.Open()
    conexao

let inserirUsuario nome tipo =
    use conexao = conectarBanco()
    let comando = conexao.CreateCommand()
    comando.CommandText <- "INSERT INTO USUARIOS (Nome, DataCadastro, Tipo) VALUES (@nome, @dataCadastro, @tipo)"
    comando.Parameters.AddWithValue("@nome", nome) |> ignore
    comando.Parameters.AddWithValue("@dataCadastro", System.DateTime.Now) |> ignore
    comando.Parameters.AddWithValue("@tipo", tipo) |> ignore
    comando.ExecuteNonQuery() |> ignore

let criarConsorcio nome valorTotal dataInicio dataFim numeroParticipantes status parcelas =
    use conexao = conectarBanco()
    let comando = conexao.CreateCommand()
    comando.CommandText <- """
        INSERT INTO CONSORCIOS (Nome, ValorTotal, DataInicio, DataFim, NumeroParticipantes, Status, Parcelas) 
        VALUES (@nome, @valorTotal, @dataInicio, @dataFim, @numeroParticipantes, @status, @parcelas)
    """
    comando.Parameters.AddWithValue("@nome", nome) |> ignore
    comando.Parameters.AddWithValue("@valorTotal", valorTotal) |> ignore
    comando.Parameters.AddWithValue("@dataInicio", dataInicio) |> ignore
    comando.Parameters.AddWithValue("@dataFim", dataFim) |> ignore
    comando.Parameters.AddWithValue("@numeroParticipantes", numeroParticipantes) |> ignore
    comando.Parameters.AddWithValue("@status", status) |> ignore
    comando.Parameters.AddWithValue("@parcelas", parcelas) |> ignore
    comando.ExecuteNonQuery() |> ignore

let participarConsorcio usuarioId consorcioId =
    use conexao = conectarBanco()
    let comando = conexao.CreateCommand()
    comando.CommandText <- """
        INSERT INTO PARTICIPA (UsuarioId, ConsorcioId, DataEntrada, Status) 
        VALUES (@usuarioId, @consorcioId, @dataEntrada, @status)
    """
    comando.Parameters.AddWithValue("@usuarioId", usuarioId) |> ignore
    comando.Parameters.AddWithValue("@consorcioId", consorcioId) |> ignore
    comando.Parameters.AddWithValue("@dataEntrada", System.DateTime.Now) |> ignore
    comando.Parameters.AddWithValue("@status", "ativo") |> ignore
    comando.ExecuteNonQuery() |> ignore
