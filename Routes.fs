module ConsorciosCLP.Routes

open Database
open Requests
open Suave
open Suave.Filters
open Suave.Json
open Suave.Operators
open System

let rotaCriarConsorcio options =
    POST
    >=> path "/consorcios"
    >=> mapJson (fun (reqData: RequestCriarConsorcio) ->
        let novoConsorcio =
            { Id = 0
              Nome = reqData.Nome
              ValorTotal = reqData.ValorTotal
              DataInicio = DateOnly.Parse(reqData.DataInicio)
              DataFim = DateOnly.Parse(reqData.DataFim)
              NumeroParticipantes = reqData.NumeroParticipantes
              Status = reqData.Status
              Parcelas = reqData.Parcelas }

        let ctx = new AppDbContext(options)
        ctx.Consorcios.Add(novoConsorcio) |> ignore
        ctx.SaveChanges() |> ignore

        let response: ResponseCriarConsorcio = { Id = novoConsorcio.Id }
        response)


let rotaParticiparConsorcio options =
    POST
    >=> pathScan "/consorcio/%d/participar" (fun consorcioId ->
        mapJson (fun (reqData: RequestParticiparConsorcio) ->
            let novoParticipa =
                { UsuarioId = reqData.UsuarioId
                  ConsorcioId = consorcioId
                  DataEntrada = DateTime.Now
                  Status = "Participando" }

            let ctx = new AppDbContext(options)
            ctx.Participa.Add(novoParticipa) |> ignore
            ctx.SaveChanges() |> ignore

            ResponseParticiparConsorcio()))


let getRoutes options =
    choose [ rotaCriarConsorcio options; rotaParticiparConsorcio options ]
