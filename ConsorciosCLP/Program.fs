module ConsorciosCLP.Program

open Microsoft.EntityFrameworkCore
open Microsoft.Extensions.Configuration
open Models
open Routes
open Suave
open System.IO
open System.Net

let createTables options =
    let db = new AppDbContext(options)
    db.Database.EnsureCreated()


let parsePort (portString: string) =
    match Port.TryParse(portString) with
    | (true, port) -> port
    | _ -> failwithf "Invalid port: %s" portString


[<EntryPoint>]
let main argv =
    let config =
        ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build()

    let conexaoString = config.GetConnectionString "Consorcios"
    let porta = parsePort (config.GetSection("AppSettings")["Porta"])

    let options =
        DbContextOptionsBuilder<AppDbContext>().UseSqlite(conexaoString).Options

    createTables options |> ignore

    let finalConfig =
        routesConfig.withBindings [ HttpBinding.create HTTP IPAddress.Loopback porta ]

    startWebServer finalConfig (getRoutes options)
    0
