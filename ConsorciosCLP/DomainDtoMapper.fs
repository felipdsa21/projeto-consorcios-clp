module ConsorciosCLP.DomainDtoMapper

open Database
open Requests
open System


let updateConsorcioFromRequest (r: RequestAlterarConsorcio) (c: Consorcio) =
    c.Nome <- r.Nome
    c.Descricao <- r.Descricao
    c.Imagem <- r.Imagem
    c.DataInicio <- DateOnly.Parse r.DataInicio
    c.DataFim <- DateOnly.Parse r.DataFim
    c.ValorTotal <- r.ValorTotal
    c.TaxaAdministrativa <- r.TaxaAdministrativa
    c.TaxaFundoReserva <- r.TaxaFundoReserva
    c.QtdParcelas <- r.QtdParcelas
    c.LimiteParticipantes <- r.LimiteParticipantes
    c.Status <- r.Status


let consorcioToResponse (c: Consorcio) : ResponseDetalharConsorcio =
    { Id = c.Id
      Nome = c.Nome
      Descricao = c.Descricao
      Imagem = c.Imagem
      DataInicio = c.DataInicio.ToString "O"
      DataFim = c.DataFim.ToString "O"
      ValorTotal = c.ValorTotal
      TaxaAdministrativa = c.TaxaAdministrativa
      TaxaFundoReserva = c.TaxaFundoReserva
      QtdParcelas = c.QtdParcelas
      LimiteParticipantes = c.LimiteParticipantes
      Status = c.Status }
