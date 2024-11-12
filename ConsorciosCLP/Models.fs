module ConsorciosCLP.Models

open Microsoft.EntityFrameworkCore
open System
open System.ComponentModel.DataAnnotations
open System.ComponentModel.DataAnnotations.Schema


type Consorcio() =
    [<Key>]
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>]
    member val Id = 0 with get, set

    [<Required>]
    member val DataCriacao = DateTime.MinValue with get, set

    [<Required>]
    member val Nome = "" with get, set

    [<Required>]
    member val Descricao = "" with get, set

    [<Required>]
    member val Imagem = "" with get, set

    [<Required>]
    member val DataInicio = DateOnly.MinValue with get, set

    [<Required>]
    member val DataFim = DateOnly.MinValue with get, set

    [<Required>]
    member val ValorTotal = 0.0 with get, set

    [<Required>]
    member val TaxaAdministrativa = 0.0 with get, set

    [<Required>]
    member val TaxaFundoReserva = 0.0 with get, set

    [<Required>]
    member val QtdParcelas = 0 with get, set

    [<Required>]
    member val LimiteParticipantes = 0 with get, set

    [<Required>]
    member val Status = "" with get, set


[<PrimaryKey("UsuarioId", "ConsorcioId")>]
type Participa() =
    member val ConsorcioId = 0 with get, set

    member val Consorcio = Unchecked.defaultof<Consorcio> with get, set

    member val UsuarioId = 0 with get, set

    [<Required>]
    member val DataEntrada = DateTime.MinValue with get, set

    [<Required>]
    member val Status = "" with get, set


type AppDbContext(options: DbContextOptions<AppDbContext>) =
    inherit DbContext(options)

    [<DefaultValue>]
    val mutable consorcios: DbSet<Consorcio>

    member public this.Consorcios
        with get () = this.consorcios
        and set c = this.consorcios <- c

    [<DefaultValue>]
    val mutable participa: DbSet<Participa>

    member public this.Participa
        with get () = this.participa
        and set c = this.participa <- c
