//Arquivo principal que configura o servidor e define as rotas.

open Suave
open System

[<EntryPoint>]
let main argv =
    let app =
        choose [ Routes.rotaCriarConsorcio; Routes.rotaParticiparConsorcio ]

    startWebServer defaultConfig app
    0
