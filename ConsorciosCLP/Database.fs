module ConsorciosCLP.Database

open Microsoft.EntityFrameworkCore
open System
open System.ComponentModel.DataAnnotations
open System.ComponentModel.DataAnnotations.Schema

[<CLIMutable>]
type Consorcio =
    { [<Key>]
      [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>]
      Id: int

      [<Required>]
      Nome: string

      [<Required>]
      ValorTotal: float

      [<Required>]
      DataInicio: DateOnly

      [<Required>]
      DataFim: DateOnly

      [<Required>]
      NumeroParticipantes: int

      [<Required>]
      Status: string

      [<Required>]
      Parcelas: int }


[<CLIMutable>]
[<PrimaryKey("UsuarioId", "ConsorcioId")>]
type Participa =
    { UsuarioId: int

      ConsorcioId: int

      [<Required>]
      DataEntrada: DateTime

      [<Required>]
      Status: string }


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
