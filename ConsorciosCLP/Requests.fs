module ConsorciosCLP.Requests

open System.Runtime.Serialization

let ERRO_CONSORCIO_NAO_EXISTE = "Consórcio não existe"
let ERRO_FORA_DO_PRAZO = "Fora de prazo"
let ERRO_LIMITE_PARTICIPANTES_EXCEDIDO = "Limite de participantes excedido"
let ERRO_ROTA_NAO_EXISTE = "Rota não existe"


[<DataContract>]
type ResponseErro =
    { [<field: DataMember(Name = "mensagem", IsRequired = true)>]
      Mensagem: string }


[<DataContract>]
type RequestAlterarConsorcio =
    { [<field: DataMember(Name = "nome", IsRequired = true)>]
      Nome: string

      // Should be parsed as DateOnly
      [<field: DataMember(Name = "data_inicio", IsRequired = true)>]
      DataInicio: string

      // Should be parsed as DateOnly
      [<field: DataMember(Name = "data_fim", IsRequired = true)>]
      DataFim: string

      [<field: DataMember(Name = "valor_total", IsRequired = true)>]
      ValorTotal: float

      [<field: DataMember(Name = "parcelas", IsRequired = true)>]
      Parcelas: int

      [<field: DataMember(Name = "limite_participantes", IsRequired = true)>]
      LimiteParticipantes: int

      [<field: DataMember(Name = "status", IsRequired = true)>]
      Status: string }


[<DataContract>]
type ResponseCriarConsorcio =
    { [<field: DataMember(Name = "id", IsRequired = true)>]
      Id: int }


[<DataContract>]
type ResponseDetalharConsorcio =
    { [<field: DataMember(Name = "id", IsRequired = true)>]
      Id: int

      [<field: DataMember(Name = "nome", IsRequired = true)>]
      Nome: string

      [<field: DataMember(Name = "data_inicio", IsRequired = true)>]
      DataInicio: string

      [<field: DataMember(Name = "data_fim", IsRequired = true)>]
      DataFim: string

      [<field: DataMember(Name = "valor_total", IsRequired = true)>]
      ValorTotal: float

      [<field: DataMember(Name = "parcelas", IsRequired = true)>]
      Parcelas: int

      [<field: DataMember(Name = "limite_participantes", IsRequired = true)>]
      LimiteParticipantes: int

      [<field: DataMember(Name = "status", IsRequired = true)>]
      Status: string }


[<DataContract>]
type ResponseListarConsorcios =
    { [<field: DataMember(Name = "consorcios", IsRequired = true)>]
      Consorcios: ResponseDetalharConsorcio list }


[<DataContract>]
type RequestParticiparEmConsorcio =
    { [<field: DataMember(Name = "usuario_id", IsRequired = true)>]
      UsuarioId: int }


[<DataContract>]
type ResponseListarParticipantes =
    { [<field: DataMember(Name = "usuarios", IsRequired = true)>]
      Usuarios: int list }


[<DataContract>]
type ResponseListarConsorciosParticipando =
    { [<field: DataMember(Name = "consorcios", IsRequired = true)>]
      Consorcios: int list }
