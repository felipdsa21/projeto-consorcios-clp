module ConsorciosCLP.Routes

open ModelDtoMapper
open Models
open Requests
open Suave
open Suave.Filters
open Suave.Operators
open Suave.RequestErrors
open Suave.ServerErrors
open Suave.Successful
open System
open Utils

let rotaCriarConsorcio options =
    POST
    >=> path "/consorcios"
    >=> jsonRequest (fun (reqData: RequestAlterarConsorcio) ->
        let c = Consorcio()
        updateConsorcioFromRequest reqData c
        c.DataCriacao <- DateTime.Now

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
        let consorcios = db.Consorcios |> Seq.toList |> List.map consorcioToResponse

        let response: ResponseListarConsorcios = { Consorcios = consorcios }
        jsonResponse ok response)


let rotaDetalharConsorcio options =
    GET
    >=> pathScan "/consorcios/%d" (fun consorcioId ->
        let db = new AppDbContext(options)
        let c = db.Consorcios.Find consorcioId

        if isNullObj c then
            jsonResponse not_found { Mensagem = ERRO_CONSORCIO_NAO_EXISTE }
        else
            let response: ResponseDetalharConsorcio = consorcioToResponse c
            jsonResponse ok response)


let rotaAlterarConsorcio options =
    PUT
    >=> pathScan "/consorcios/%d" (fun consorcioId ->
        jsonRequest (fun (reqData: RequestAlterarConsorcio) ->
            let db = new AppDbContext(options)
            let c = db.Consorcios.Find consorcioId

            if isNullObj c then
                jsonResponse not_found { Mensagem = ERRO_CONSORCIO_NAO_EXISTE }
            else
                updateConsorcioFromRequest reqData c
                db.Consorcios.Update c |> ignore
                db.SaveChanges() |> ignore
                OK ""))


let rotaApagarConsorcio options =
    DELETE
    >=> pathScan "/consorcios/%d" (fun consorcioId ->
        let db = new AppDbContext(options)
        let c = db.Consorcios.Find consorcioId

        if isNullObj c then
            jsonResponse not_found { Mensagem = ERRO_CONSORCIO_NAO_EXISTE }
        else
            db.Consorcios.Remove(c) |> ignore
            db.SaveChanges() |> ignore
            OK "")


let rotaContemplarParticipante options =
    POST
    >=> pathScan "/consorcios/%d/contemplar" (fun consorcioId ->
        let db = new AppDbContext(options)
        let c = db.Consorcios.Find consorcioId

        if isNullObj c then
            jsonResponse not_found { Mensagem = ERRO_CONSORCIO_NAO_EXISTE }
        else
            let participa =
                db.Participa |> Seq.filter (fun p -> p.Status = "Participando") |> Seq.head

            if isNullObj participa then
                jsonResponse not_found { Mensagem = ERRO_SEM_PARTICIPANTES }
            else
                participa.Status <- "Contemplado"
                db.Participa.Update participa |> ignore
                db.SaveChanges() |> ignore

                let response: ResponseContemplarParticipante = { UsuarioId = participa.UsuarioId }
                jsonResponse ok response)


let rotaParticiparEmConsorcio options =
    POST
    >=> pathScan "/consorcios/%d/participantes" (fun consorcioId ->
        jsonRequest (fun (reqData: RequestParticiparEmConsorcio) ->
            let db = new AppDbContext(options)
            let c = db.Consorcios.Find consorcioId

            let now = DateTime.Now
            let today = DateOnly.FromDateTime now

            if isNullObj c then
                jsonResponse not_found { Mensagem = ERRO_CONSORCIO_NAO_EXISTE }
            else if today < c.DataInicio || today > c.DataFim then
                jsonResponse gone { Mensagem = ERRO_FORA_DO_PRAZO }
            else if not (isNullObj (db.Participa.Find(consorcioId, reqData.UsuarioId))) then
                jsonResponse conflict { Mensagem = ERRO_JA_PARTICIPANDO }
            else
                let qtdParticipantes =
                    db.Participa |> Seq.filter (fun p -> p.ConsorcioId = consorcioId) |> Seq.length

                if qtdParticipantes > c.LimiteParticipantes then
                    jsonResponse forbidden { Mensagem = ERRO_LIMITE_PARTICIPANTES_EXCEDIDO }
                else
                    let novoParticipa = Participa()
                    novoParticipa.UsuarioId <- reqData.UsuarioId
                    novoParticipa.ConsorcioId <- consorcioId
                    novoParticipa.DataEntrada <- now
                    novoParticipa.Status <- "Participando"

                    db.Participa.Add novoParticipa |> ignore
                    db.SaveChanges() |> ignore
                    OK ""))


let rotaSairDoConsorcio options =
    DELETE
    >=> pathScan "/consorcios/%d/participantes/%d" (fun (consorcioId, usuarioId) ->
        let db = new AppDbContext(options)
        let c = db.Consorcios.Find consorcioId

        let now = DateTime.Now
        let today = DateOnly.FromDateTime now

        if isNullObj c then
            jsonResponse not_found { Mensagem = ERRO_CONSORCIO_NAO_EXISTE }
        else if today < c.DataInicio || today > c.DataFim then
            jsonResponse gone { Mensagem = ERRO_FORA_DO_PRAZO }
        else
            let participa = db.Participa.Find(consorcioId, usuarioId)

            if isNullObj participa then
                jsonResponse not_found { Mensagem = ERRO_NAO_PARTICIPANDO }
            else if participa.Status = "Contemplado" then
                jsonResponse gone { Mensagem = ERRO_JA_CONTEMPLADO }
            else
                db.Participa.Remove participa |> ignore
                db.SaveChanges() |> ignore
                OK "")

let rotaListarParticipantes options =
    GET
    >=> pathScan "/consorcios/%d/participantes" (fun consorcioId ->
        let db = new AppDbContext(options)
        let c = db.Consorcios.Find consorcioId

        if isNullObj c then
            jsonResponse not_found { Mensagem = ERRO_CONSORCIO_NAO_EXISTE }
        else
            let participantes =
                db.Participa
                |> Seq.filter (fun p -> p.ConsorcioId = consorcioId)
                |> Seq.map participaToResponse
                |> Seq.toList

            let response: ResponseListarParticipantes = { Participantes = participantes }
            jsonResponse ok response)


let rotaListarConsorciosParticipando options =
    GET
    >=> pathScan "/participantes/%d" (fun usuarioId ->
        let db = new AppDbContext(options)

        let consorcios =
            db.Participa
            |> Seq.filter (fun p -> p.UsuarioId = usuarioId)
            |> Seq.map consorcioParticipaToResponse
            |> Seq.toList

        let response: ResponseListarConsorciosParticipando = { Consorcios = consorcios }
        jsonResponse ok response)


let rotaNaoExiste = jsonResponse not_found { Mensagem = ERRO_ROTA_NAO_EXISTE }


let rotaErro (ex: Exception) (msg: string) (ctx: HttpContext) =
    jsonResponse internal_error { Mensagem = ex.Message } ctx


let routesConfig = defaultConfig.withErrorHandler (rotaErro)


let getRoutes options =
    choose
        [ rotaCriarConsorcio options
          rotaDetalharConsorcio options
          rotaListarConsorcios options
          rotaAlterarConsorcio options
          rotaApagarConsorcio options
          rotaContemplarParticipante options
          rotaParticiparEmConsorcio options
          rotaSairDoConsorcio options
          rotaListarParticipantes options
          rotaListarConsorciosParticipando options
          rotaNaoExiste ]
