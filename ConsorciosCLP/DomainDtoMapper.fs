module ConsorciosCLP.DomainDtoMapper

open Database
open Requests
open System

let requestCriarConsorcioToConsorcio id (r: RequestCriarConsorcio) : Consorcio =
    { Id = id
      Nome = r.Nome
      DataInicio = DateOnly.Parse r.DataInicio
      DataFim = DateOnly.Parse r.DataFim
      ValorTotal = r.ValorTotal
      Parcelas = r.Parcelas
      LimiteParticipantes = r.LimiteParticipantes
      Status = r.Status }


let consorcioToResponseConsorcio (c: Consorcio) : ResponseDetalharConsorcio =
    { Id = c.Id
      Nome = c.Nome
      DataInicio = c.DataInicio.ToString "O"
      DataFim = c.DataFim.ToString "O"
      ValorTotal = c.ValorTotal
      Parcelas = c.Parcelas
      LimiteParticipantes = c.LimiteParticipantes
      Status = c.Status }
