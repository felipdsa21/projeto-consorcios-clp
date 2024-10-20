module Routes

open Database
open Suave.Filters
open Suave.Json
open Suave.Operators
open Suave.Successful
open System

let rotaCriarConsorcio =
    POST
    >=> path "/consorcio/criar"
    >=> mapJson (fun (form: Requests.RequestCriarConsorcio) ->
        let novoConsorcio =
            { Id = 0
              Nome = form.Nome
              ValorTotal = form.ValorTotal
              DataInicio = DateOnly.Parse(form.DataInicio)
              DataFim = DateOnly.Parse(form.DataFim)
              NumeroParticipantes = form.NumeroParticipantes
              Status = form.Status
              Parcelas = form.Parcelas }

        let ctx = new ConsorciosDbContext()
        ctx.Consorcios.Add(novoConsorcio) |> ignore
        ctx.SaveChanges() |> ignore
        OK "Consórcio criado com sucesso!")


let rotaParticiparConsorcio =
    POST
    >=> path "/consorcio/participar"
    >=> mapJson (fun (form: Requests.RequestParticiparConsorcio) ->
        let novoParticipa =
            { UsuarioId = form.UsuarioId
              ConsorcioId = form.ConsorcioId }

        let ctx = new ConsorciosDbContext()

        ctx.Participa.Add(novoParticipa) |> ignore
        ctx.SaveChanges() |> ignore
        OK "Usuário adicionado ao consórcio!")


let rotaListarConsorcios =
    GET
    >=> path "/consorcios"
    >=> fun _ ->
        let ctx = new ConsorciosDbContext()
        
        // Recuperar todos os consórcios disponíveis
        let consorcios =
            ctx.Consorcios
            |> Seq.toList

        // Converter os consórcios em um formato JSON
        let response =
            consorcios |> List.map (fun c ->
                { Id = c.Id
                  Nome = c.Nome
                  ValorTotal = c.ValorTotal
                  DataInicio = c.DataInicio.ToString("yyyy-MM-dd")
                  DataFim = c.DataFim.ToString("yyyy-MM-dd")
                  NumeroParticipantes = c.NumeroParticipantes
                  Status = c.Status
                  Parcelas = c.Parcelas })

        OK (toJson response)
