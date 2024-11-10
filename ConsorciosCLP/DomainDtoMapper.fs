module ConsorciosCLP.DomainDtoMapper

open Database
open Requests
open System


let updateConsorcioFromRequest (r: RequestAlterarConsorcio) (c: Consorcio) =
    c.Nome <- r.Nome
    c.DataInicio <- DateOnly.Parse r.DataInicio
    c.DataFim <- DateOnly.Parse r.DataFim
    c.ValorTotal <- r.ValorTotal
    c.Parcelas <- r.Parcelas
    c.LimiteParticipantes <- r.LimiteParticipantes
    c.Status <- r.Status


let consorcioToResponse (c: Consorcio) : ResponseDetalharConsorcio =
    { Id = c.Id
      Nome = c.Nome
      DataInicio = c.DataInicio.ToString "O"
      DataFim = c.DataFim.ToString "O"
      ValorTotal = c.ValorTotal
      Parcelas = c.Parcelas
      LimiteParticipantes = c.LimiteParticipantes
      Status = c.Status }
