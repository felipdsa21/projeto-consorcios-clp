module ConsorciosCLP.Tests.Program

open ConsorciosCLP.Models
open ConsorciosCLP.Requests
open ConsorciosCLP.Routes
open Expecto
open Expecto.Logging
open Microsoft.EntityFrameworkCore
open Suave
open Suave.Json
open System
open System.Net
open System.Net.Http
open System.Text
open Testing

[<Tests>]
let tests =
    let config =
        defaultConfig.withBindings [ HttpBinding.create HTTP IPAddress.Loopback 9001us ]

    let options =
        DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("Consorcios")
            .Options

    let runWebServer () = runWith config (getRoutes options)
    let objToJson obj = Some(new ByteArrayContent(toJson obj))
    let jsonToObj (json: string) = fromJson (Encoding.UTF8.GetBytes json)

    let getResponse (res: HttpResponseMessage) =
        res.StatusCode, res.Content.ReadAsStringAsync().Result

    testList
        "rotas básicas"
        [ testCase "criar um consórcio"
          <| fun _ ->
              let year = DateTime.Now.Year

              let reqData =
                  { Nome = "Consórcio"
                    Descricao = "Lorem ipsum"
                    Imagem = ""
                    ValorTotal = 100
                    DataInicio = DateTime(year, 1, 1).ToString "O"
                    DataFim = DateTime(year, 12, 31).ToString "O"
                    QtdParcelas = 12
                    TaxaAdministrativa = 5
                    TaxaFundoReserva = 10
                    LimiteParticipantes = 10
                    Status = "Criado" }
                  |> objToJson

              let code, res =
                  runWebServer ()
                  |> reqResp POST "/consorcios" "" reqData None DecompressionMethods.None id getResponse

              Expect.equal HttpStatusCode.OK code "should not error"
              jsonToObj res
              : ResponseCriarConsorcio
              |> ignore

          testCase "listar consórcios"
          <| fun _ ->
              let code, res =
                  runWebServer ()
                  |> reqResp GET "/consorcios" "" None None DecompressionMethods.None id getResponse

              Expect.equal HttpStatusCode.OK code "should not error"
              jsonToObj res
              : ResponseListarConsorcios
              |> ignore

          testCase "detalhar um consórcio"
          <| fun _ ->
              let url = sprintf "/consorcios/%d" 1

              let code, res =
                  runWebServer ()
                  |> reqResp GET url "" None None DecompressionMethods.None id getResponse

              Expect.equal HttpStatusCode.OK code "should not error"
              jsonToObj res
              : ResponseDetalharConsorcio
              |> ignore

          testCase "alterar consórcio"
          <| fun _ ->
              let year = DateTime.Now.Year
              let url = sprintf "/consorcios/%d" 1

              let reqData =
                  { Nome = "Consórcio 2.0"
                    Descricao = "Sit dolor amet"
                    Imagem = ""
                    DataInicio = DateTime(year, 1, 1).ToString "O"
                    DataFim = DateTime(year, 12, 31).ToString "O"
                    ValorTotal = 200
                    TaxaAdministrativa = 10
                    TaxaFundoReserva = 3
                    QtdParcelas = 10
                    LimiteParticipantes = 15
                    Status = "Criado" }
                  |> objToJson

              let code, res =
                  runWebServer ()
                  |> reqResp PUT url "" reqData None DecompressionMethods.None id getResponse

              Expect.equal HttpStatusCode.OK code "should not error"
              Expect.equal "" res "response should be empty"

          testCase "participar em um consórcio"
          <| fun _ ->
              let reqData = { UsuarioId = 1 } |> objToJson
              let url = sprintf "/consorcios/%d/participantes" 1

              let code, res =
                  runWebServer ()
                  |> reqResp POST url "" reqData None DecompressionMethods.None id getResponse

              Expect.equal HttpStatusCode.OK code "should not error"
              Expect.equal "" res "response should be empty"

          testCase "contemplar um participante do consórcio"
          <| fun _ ->
              let url = sprintf "/consorcios/%d/contemplar" 1

              let code, res =
                  runWebServer ()
                  |> reqResp POST url "" None None DecompressionMethods.None id getResponse

              Expect.equal HttpStatusCode.OK code "should not error"
              jsonToObj res
              : ResponseContemplarParticipante
              |> ignore

          testCase "listar participantes do consórcio"
          <| fun _ ->
              let url = sprintf "/consorcios/%d/participantes" 1

              let code, res =
                  runWebServer ()
                  |> reqResp GET url "" None None DecompressionMethods.None id getResponse

              Expect.equal HttpStatusCode.OK code "should not error"
              jsonToObj res
              : ResponseListarParticipantes
              |> ignore

          testCase "listar consórcio em que o usuário participa"
          <| fun _ ->
              let url = sprintf "/participantes/%d" 1

              let code, res =
                  runWebServer ()
                  |> reqResp GET url "" None None DecompressionMethods.None id getResponse

              Expect.equal HttpStatusCode.OK code "should not error"
              jsonToObj res
              : ResponseListarConsorciosParticipando
              |> ignore

          testCase "sair do consórcio"
          <| fun _ ->
              let db = new AppDbContext(options)
              let participa = db.Participa.Find(1, 1)
              participa.Status <- "Participando"
              db.Participa.Update participa |> ignore
              db.SaveChanges() |> ignore

              let url = sprintf "/consorcios/%d/participantes/%d" 1 1

              let code, res =
                  runWebServer ()
                  |> reqResp DELETE url "" None None DecompressionMethods.None id getResponse

              Expect.equal HttpStatusCode.OK code "should not error"
              Expect.equal "" res "response should be empty"

          testCase "apagar um consórcio"
          <| fun _ ->
              let url = sprintf "/consorcios/%d" 1

              let code, res =
                  runWebServer ()
                  |> reqResp DELETE url "" None None DecompressionMethods.None id getResponse

              Expect.equal HttpStatusCode.OK code "should not error"
              Expect.equal "" res "response should be empty" ]


[<EntryPoint>]
let main argv =
    runTestsInAssemblyWithCLIArgs [ Verbosity Verbose; Sequenced ] argv
