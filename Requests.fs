module Requests

open System.Runtime.Serialization

[<DataContract>]
type RequestCriarConsorcio =
    { [<field: DataMember(Name = "nome", IsRequired = true)>]
      Nome: string

      [<field: DataMember(Name = "valor_total", IsRequired = true)>]
      ValorTotal: float

      // Should be parsed as DateOnly
      [<field: DataMember(Name = "data_inicio", IsRequired = true)>]
      DataInicio: string

      // Should be parsed as DateOnly
      [<field: DataMember(Name = "data_fim", IsRequired = true)>]
      DataFim: string

      [<field: DataMember(Name = "numero_participantes", IsRequired = true)>]
      NumeroParticipantes: int

      [<field: DataMember(Name = "status", IsRequired = true)>]
      Status: string

      [<field: DataMember(Name = "parcelas", IsRequired = true)>]
      Parcelas: int }


[<DataContract>]
type ResponseCriarConsorcio =
    { [<field: DataMember(Name = "status", IsRequired = true)>]
      Status: string }


[<DataContract>]
type RequestParticiparConsorcio =
    { [<field: DataMember(Name = "usuario_id", IsRequired = true)>]
      UsuarioId: int

      [<field: DataMember(Name = "consorcio_id", IsRequired = true)>]
      ConsorcioId: int }
