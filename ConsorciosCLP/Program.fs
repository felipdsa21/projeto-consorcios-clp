module ConsorciosCLP.Program

open Database
open Microsoft.EntityFrameworkCore
open Microsoft.Extensions.Configuration
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
        DbContextOptionsBuilder<AppDbContext>().UseNpgsql(conexaoString).Options

    createTables options |> ignore
    startWebServer defaultConfig (getRoutes options)
    0
