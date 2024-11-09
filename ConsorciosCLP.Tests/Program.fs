module ConsorciosCLP.Tests.Program

open ConsorciosCLP.Database
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
    let inputToJson obj = Some(new ByteArrayContent(toJson obj))

    let getResponse (res: HttpResponseMessage) =
        res.StatusCode, res.Content.ReadAsStringAsync().Result

    testList
        "rotas básicas"
        [ testCase "criar consórcio"
          <| fun _ ->
              let year = DateTime.Now.Year

              let reqData =
                  { Nome = "Consórcio"
                    ValorTotal = 100
                    DataInicio = DateTime(year, 1, 1).ToString "O"
                    DataFim = DateTime(year, 12, 31).ToString "O"
                    LimiteParticipantes = 10
                    Status = "Criado"
                    Parcelas = 5 }
                  |> inputToJson

              let code, res =
                  runWebServer ()
                  |> reqResp POST "/consorcios" "" reqData None DecompressionMethods.None id getResponse

              Expect.equal HttpStatusCode.OK code "should not error"

          testCase "detalhes do consórcio"
          <| fun _ ->
              let url = sprintf "/consorcios/%d" 1

              let code, res =
                  runWebServer ()
                  |> reqResp GET url "" None None DecompressionMethods.None id getResponse

              Expect.equal HttpStatusCode.OK code "should not error"

          testCase "listar consórcios"
          <| fun _ ->
              let code, res =
                  runWebServer ()
                  |> reqResp GET "/consorcios" "" None None DecompressionMethods.None id getResponse

              Expect.equal HttpStatusCode.OK code "should not error"

          testCase "alterar consórcio"
          <| fun _ ->
              let year = DateTime.Now.Year
              let url = sprintf "/consorcios/%d" 1

              let reqData =
                  { Nome = "Consórcio 2.0"
                    ValorTotal = 200
                    DataInicio = DateTime(year, 1, 1).ToString "O"
                    DataFim = DateTime(year, 12, 31).ToString "O"
                    LimiteParticipantes = 15
                    Status = "Criado"
                    Parcelas = 10 }
                  |> inputToJson

              let code, res =
                  runWebServer ()
                  |> reqResp PUT url "" reqData None DecompressionMethods.None id getResponse

              Expect.equal HttpStatusCode.OK code "should not error"

          testCase "participar do consórcio"
          <| fun _ ->
              let reqData = { UsuarioId = 1 } |> inputToJson
              let url = sprintf "/consorcios/%d/participantes" 1

              let code, res =
                  runWebServer ()
                  |> reqResp POST url "" reqData None DecompressionMethods.None id getResponse

              Expect.equal HttpStatusCode.OK code "should not error"


          testCase "listar participantes do consórcio"
          <| fun _ ->
              let url = sprintf "/consorcios/%d/participantes" 1

              let code, res =
                  runWebServer ()
                  |> reqResp GET url "" None None DecompressionMethods.None id getResponse

              Expect.equal HttpStatusCode.OK code "should not error"

          testCase "listar consórcio em que o usuário participa"
          <| fun _ ->
              let url = sprintf "/participantes/%d" 1

              let code, res =
                  runWebServer ()
                  |> reqResp GET url "" None None DecompressionMethods.None id getResponse

              Expect.equal HttpStatusCode.OK code "should not error" ]


[<EntryPoint>]
let main argv =
    runTestsInAssemblyWithCLIArgs [ Verbosity Verbose; Sequenced ] argv
