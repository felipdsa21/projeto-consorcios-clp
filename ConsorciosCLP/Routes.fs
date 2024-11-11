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
            elif today.CompareTo c.DataInicio > 0 && today.CompareTo c.DataFim > 0 then
                jsonResponse gone { Mensagem = ERRO_FORA_DO_PRAZO }
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


let rotaListarParticipantes options =
    GET
    >=> pathScan "/consorcios/%d/participantes" (fun consorcioId ->
        let db = new AppDbContext(options)
        let c = db.Consorcios.Find consorcioId

        if isNullObj c then
            jsonResponse not_found { Mensagem = ERRO_CONSORCIO_NAO_EXISTE }
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
    >=> pathScan "/participantes/%d" (fun usuarioId ->
        let db = new AppDbContext(options)

        let consorcios =
            db.Participa
            |> Seq.filter (fun p -> p.UsuarioId = usuarioId)
            |> Seq.map (fun p -> p.ConsorcioId)
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
          rotaParticiparEmConsorcio options
          rotaListarParticipantes options
          rotaListarConsorciosParticipando options
          rotaNaoExiste ]
