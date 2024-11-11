module ConsorciosCLP.Program

open Microsoft.EntityFrameworkCore
open Microsoft.Extensions.Configuration
open Models
open Routes
open Suave
open System.IO

let createTables options =
    let db = new AppDbContext(options)
    db.Database.EnsureCreated()

[<EntryPoint>]
let main argv =
    let config =
        ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build()

    let conexaoString = config.GetConnectionString "Consorcios"

    let options =
        DbContextOptionsBuilder<AppDbContext>().UseSqlite(conexaoString).Options

    createTables options |> ignore
    startWebServer routesConfig (getRoutes options)
    0
