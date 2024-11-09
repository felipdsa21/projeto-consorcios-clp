module ConsorciosCLP.Routes

open Database
open Requests
open Suave
open Suave.Filters
open Suave.Json
open Suave.Operators
open Suave.Successful
open System

let rotaCriarConsorcio options =
    POST
    >=> path "/consorcios"
    >=> mapJson (fun (reqData: RequestCriarConsorcio) ->
        let novoConsorcio: Consorcio =
            { Id = 0
              Nome = reqData.Nome
              ValorTotal = reqData.ValorTotal
              DataInicio = DateOnly.Parse(reqData.DataInicio)
              DataFim = DateOnly.Parse(reqData.DataFim)
              NumeroParticipantes = reqData.NumeroParticipantes
              Status = reqData.Status
              Parcelas = reqData.Parcelas }

        let db = new AppDbContext(options)
        db.Consorcios.Add(novoConsorcio) |> ignore
        db.SaveChanges() |> ignore

        let response: ResponseCriarConsorcio = { Id = novoConsorcio.Id }
        response)


let rotaParticiparConsorcio options =
    POST
    >=> pathScan "/consorcio/%d/participar" (fun consorcioId ->
        mapJson (fun (reqData: RequestParticiparConsorcio) ->
            let novoParticipa: Participa =
                { UsuarioId = reqData.UsuarioId
                  ConsorcioId = consorcioId
                  DataEntrada = DateTime.Now
                  Status = "Participando" }

            let db = new AppDbContext(options)
            db.Participa.Add(novoParticipa) |> ignore
            db.SaveChanges() |> ignore

            let response = ResponseParticiparConsorcio()
            response))


let rotaListarConsorcios options =
    GET
    >=> path "/consorcios"
    >=> request (fun _ ->
        let db = new AppDbContext(options)

        let consorcios: ResponseConsorcio list =
            db.Consorcios
            |> Seq.toList
            |> List.map (fun c ->
                { Id = c.Id
                  Nome = c.Nome
                  ValorTotal = c.ValorTotal
                  DataInicio = c.DataInicio.ToString("O")
                  DataFim = c.DataFim.ToString("O")
                  NumeroParticipantes = c.NumeroParticipantes
                  Status = c.Status
                  Parcelas = c.Parcelas })

        let response: ResponseListarConsorcios = { Consorcios = consorcios }
        ok (toJson response))


let getRoutes options =
    choose
        [ rotaCriarConsorcio options
          rotaParticiparConsorcio options
          rotaListarConsorcios options ]