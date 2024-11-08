module ConsorciosCLP.Program

open Database
open Microsoft.EntityFrameworkCore
open Microsoft.Extensions.Configuration
open Routes
open Suave
open System.IO

[<EntryPoint>]
let main argv =
    let config =
        ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build()

    let conexaoString = config.GetConnectionString("Consorcios")

    let options =
        DbContextOptionsBuilder<AppDbContext>().UseNpgsql(conexaoString).Options

    startWebServer defaultConfig (getRoutes options)
    0
