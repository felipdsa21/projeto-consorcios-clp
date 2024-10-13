//Define as rotas HTTP e suas lógicas associadas.

module Routes

open Suave
open Suave.Filters
open Suave.Operators
open Suave.Successful
open BancoDeDados

let rotaCriarConsorcio =
    POST >=> path "/consorcio/criar" >=> request (fun req ->
        // Pegue os dados do consórcio da requisição
        let nome = req.formData.["nome"]
        let valorTotal = decimal req.formData.["valorTotal"]
        let dataInicio = System.DateTime.Parse(req.formData.["dataInicio"])
        let dataFim = System.DateTime.Parse(req.formData.["dataFim"])
        let numeroParticipantes = int req.formData.["numeroParticipantes"]
        let status = req.formData.["status"]
        let parcelas = int req.formData.["parcelas"]
        
        criarConsorcio nome valorTotal dataInicio dataFim numeroParticipantes status parcelas
        OK "Consórcio criado com sucesso!"
    )

let rotaParticiparConsorcio =
    POST >=> path "/consorcio/participar" >=> request (fun req ->
        let usuarioId = int (req.formData.["usuarioId"])
        let consorcioId = int (req.formData.["consorcioId"])
        
        participarConsorcio usuarioId consorcioId
        OK "Usuário adicionado ao consórcio!"
    )
