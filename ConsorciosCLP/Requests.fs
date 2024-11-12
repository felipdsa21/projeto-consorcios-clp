module ConsorciosCLP.Requests

open System.Runtime.Serialization

let ERRO_CONSORCIO_NAO_EXISTE = "consorcio_nao_existe"
let ERRO_FORA_DO_PRAZO = "fora_do_prazo"
let ERRO_JA_CONTEMPLADO = "ja_contemplado"
let ERRO_JA_PARTICIPANDO = "ja_participando"
let ERRO_LIMITE_PARTICIPANTES_EXCEDIDO = "limite_participantes_excedido"
let ERRO_NAO_PARTICIPANDO = "nao_participando"
let ERRO_ROTA_NAO_EXISTE = "rota_nao_existe"
let ERRO_SEM_PARTICIPANTES = "sem_participantes"


[<DataContract>]
type ResponseErro =
    { [<field: DataMember(Name = "mensagem", IsRequired = true)>]
      Mensagem: string }


[<DataContract>]
type RequestAlterarConsorcio =
    { [<field: DataMember(Name = "nome", IsRequired = true)>]
      Nome: string

      [<field: DataMember(Name = "descricao", IsRequired = true)>]
      Descricao: string

      [<field: DataMember(Name = "imagem", IsRequired = true)>]
      Imagem: string

      // Should be parsed as DateOnly
      [<field: DataMember(Name = "data_inicio", IsRequired = true)>]
      DataInicio: string

      // Should be parsed as DateOnly
      [<field: DataMember(Name = "data_fim", IsRequired = true)>]
      DataFim: string

      [<field: DataMember(Name = "valor_total", IsRequired = true)>]
      ValorTotal: float

      [<field: DataMember(Name = "taxa_administrativa", IsRequired = true)>]
      TaxaAdministrativa: float

      [<field: DataMember(Name = "taxa_fundo_reserva", IsRequired = true)>]
      TaxaFundoReserva: float

      [<field: DataMember(Name = "qtd_parcelas", IsRequired = true)>]
      QtdParcelas: int

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

      [<field: DataMember(Name = "data_criacao", IsRequired = true)>]
      DataCriacao: string

      [<field: DataMember(Name = "nome", IsRequired = true)>]
      Nome: string

      [<field: DataMember(Name = "descricao", IsRequired = true)>]
      Descricao: string

      [<field: DataMember(Name = "imagem", IsRequired = true)>]
      Imagem: string

      [<field: DataMember(Name = "data_inicio", IsRequired = true)>]
      DataInicio: string

      [<field: DataMember(Name = "data_fim", IsRequired = true)>]
      DataFim: string

      [<field: DataMember(Name = "valor_total", IsRequired = true)>]
      ValorTotal: float

      [<field: DataMember(Name = "valor_administrativa", IsRequired = true)>]
      TaxaAdministrativa: float

      [<field: DataMember(Name = "valor_fundo_reserva", IsRequired = true)>]
      TaxaFundoReserva: float

      [<field: DataMember(Name = "qtd_parcelas", IsRequired = true)>]
      QtdParcelas: int

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
type ResponseParticipante =
    { [<field: DataMember(Name = "usuario_id", IsRequired = true)>]
      UsuarioId: int

      [<field: DataMember(Name = "data_entrada", IsRequired = true)>]
      DataEntrada: string

      [<field: DataMember(Name = "status", IsRequired = true)>]
      Status: string }

[<DataContract>]
type ResponseListarParticipantes =
    { [<field: DataMember(Name = "usuarios", IsRequired = true)>]
      Participantes: ResponseParticipante list }


[<DataContract>]
type ResponseConsorcioParticipando =
    { [<field: DataMember(Name = "consorcio_id", IsRequired = true)>]
      ConsorcioId: int

      [<field: DataMember(Name = "data_entrada", IsRequired = true)>]
      DataEntrada: string

      [<field: DataMember(Name = "status", IsRequired = true)>]
      Status: string }


[<DataContract>]
type ResponseListarConsorciosParticipando =
    { [<field: DataMember(Name = "consorcios", IsRequired = true)>]
      Consorcios: ResponseConsorcioParticipando list }
