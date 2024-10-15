module Program

open Suave

[<EntryPoint>]
let main argv =
    let app = choose [ Routes.rotaCriarConsorcio; Routes.rotaParticiparConsorcio ]
    startWebServer defaultConfig app
    0
