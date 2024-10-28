module ConsorciosCLP.Tests.Program

open ConsorciosCLP.Database
open ConsorciosCLP.Requests
open ConsorciosCLP.Routes
open Expecto
open Expecto.Logging
open Microsoft.EntityFrameworkCore
open Suave
open Suave.Json
open System.Net
open System.Net.Http
open Testing

[<Tests>]
let tests =
    let config =
        { defaultConfig with
            bindings = [ HttpBinding.createSimple HTTP "127.0.0.1" 9001 ] }

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
              let reqData =
                  { Nome = "Consórcio"
                    ValorTotal = 100
                    DataInicio = "2024-01-01"
                    DataFim = "2024-12-31"
                    NumeroParticipantes = 10
                    Status = "Criado"
                    Parcelas = 5 }
                  |> inputToJson

              let code, res =
                  runWebServer ()
                  |> reqResp POST "/consorcios" "" reqData None DecompressionMethods.None id getResponse

              Expect.equal HttpStatusCode.OK code "should not error"


          testCase "participar do consórcio"
          <| fun _ ->
              let reqData = { UsuarioId = 1 } |> inputToJson
              let url = sprintf "/consorcio/%d/participar" 1

              let code, res =
                  runWebServer ()
                  |> reqResp POST url "" reqData None DecompressionMethods.None id getResponse

              Expect.equal HttpStatusCode.OK code "should not error" ]


[<EntryPoint>]
let main argv =
    runTestsInAssemblyWithCLIArgs [ Verbosity Verbose; Sequenced ] argv
