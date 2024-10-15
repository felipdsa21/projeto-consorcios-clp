module Database

open Microsoft.EntityFrameworkCore
open System
open System.ComponentModel.DataAnnotations
open System.ComponentModel.DataAnnotations.Schema

let conexaoString =
    "Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=consorcios"

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
type Participa =
    { [<Required>]
      UsuarioId: int

      [<Required>]
      ConsorcioId: int }


type ConsorciosDbContext() =
    inherit DbContext()

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

    override __.OnConfiguring(optionsBuilder: DbContextOptionsBuilder) =
        optionsBuilder.UseNpgsql(conexaoString) |> ignore
