module ConsorciosCLP.Routes

open Database
open DomainDtoMapper
open Requests
open Suave
open Suave.Filters
open Suave.Operators
open Suave.RequestErrors
open Suave.Successful
open System
open Utils

let rotaCriarConsorcio options =
    POST
    >=> path "/consorcios"
    >=> jsonRequest (fun (reqData: RequestCriarConsorcio) ->
        let c = requestCriarConsorcioToConsorcio 0 reqData

        let db = new AppDbContext(options)
        db.Consorcios.Add c |> ignore
        db.SaveChanges() |> ignore

        let response: ResponseCriarConsorcio = { Id = c.Id }
        jsonResponse ok response)


let rotaListarConsorcios options =
    GET
    >=> path "/consorcios"
    >=> request (fun _ ->
        let db = new AppDbContext(options)

        let consorcios =
            db.Consorcios |> Seq.toList |> List.map consorcioToResponseConsorcio

        let response: ResponseListarConsorcios = { Consorcios = consorcios }
        jsonResponse ok response)


let rotaDetalharConsorcio options =
    GET
    >=> pathScan "/consorcios/%d" (fun consorcioId ->
        let db = new AppDbContext(options)
        let c = db.Consorcios.Find consorcioId

        if isNullObj c then
            jsonResponse not_found { Mensagem = "Consórcio não existe" }
        else
            let response: ResponseDetalharConsorcio = consorcioToResponseConsorcio c
            jsonResponse ok response)


let rotaAlterarConsorcio options =
    PUT
    >=> pathScan "/consorcios/%d" (fun consorcioId ->
        jsonRequest (fun (reqData: RequestCriarConsorcio) ->
            let db = new AppDbContext(options)
            let c = db.Consorcios.Find consorcioId

            if isNullObj c then
                jsonResponse not_found { Mensagem = "Consórcio não existe" }
            else
                let novoC = requestCriarConsorcioToConsorcio consorcioId reqData
                db.Consorcios.Entry(c).CurrentValues.SetValues novoC |> ignore
                db.SaveChanges() |> ignore
                OK ""))


let rotaParticiparEmConsorcio options =
    POST
    >=> pathScan "/consorcios/%d/participantes" (fun consorcioId ->
        jsonRequest (fun (reqData: RequestParticiparEmConsorcio) ->
            let db = new AppDbContext(options)
            let c = db.Consorcios.Find consorcioId

            let now = DateTime.Now
            let today = DateOnly.FromDateTime now

            if isNullObj c then
                jsonResponse not_found { Mensagem = "Consórcio não existe" }
            elif today.CompareTo c.DataInicio > 0 && today.CompareTo c.DataFim > 0 then
                jsonResponse gone { Mensagem = "Fora de prazo" }
            else
                let novoParticipa: Participa =
                    { UsuarioId = reqData.UsuarioId
                      ConsorcioId = consorcioId
                      DataEntrada = now
                      Status = "Participando" }

                db.Participa.Add novoParticipa |> ignore
                db.SaveChanges() |> ignore
                OK ""))


let rotaListarParticipantes options =
    GET
    >=> pathScan "/consorcios/%d/participantes" (fun consorcioId ->
        let db = new AppDbContext(options)
        let c = db.Consorcios.Find consorcioId

        if isNullObj c then
            jsonResponse not_found { Mensagem = "Consórcio não existe" }
        else
            let usuarios =
                db.Participa
                |> Seq.filter (fun p -> p.ConsorcioId = consorcioId)
                |> Seq.map (fun p -> p.UsuarioId)
                |> Seq.toList

            let response: ResponseListarParticipantes = { Usuarios = usuarios }
            jsonResponse ok response)


let rotaListarConsorciosParticipando options =
    GET
    >=> pathScan "/participantes/%d" (fun participanteId ->
        let db = new AppDbContext(options)

        let consorcios =
            db.Participa
            |> Seq.filter (fun p -> p.UsuarioId = participanteId)
            |> Seq.map (fun p -> p.ConsorcioId)
            |> Seq.toList

        let response: ResponseListarConsorciosParticipando = { Consorcios = consorcios }
        jsonResponse ok response)


let rotaNaoExiste = jsonResponse not_found { Mensagem = "Rota não existe" }


let getRoutes options =
    choose
        [ rotaCriarConsorcio options
          rotaDetalharConsorcio options
          rotaListarConsorcios options
          rotaAlterarConsorcio options
          rotaParticiparEmConsorcio options
          rotaListarParticipantes options
          rotaListarConsorciosParticipando options
          rotaNaoExiste ]
