module Program

open Suave

[<EntryPoint>]
let main argv =
    let app = choose [ Routes.rotaCriarConsorcio; Routes.rotaParticiparConsorcio; Routes.rotaListarConsorcios ]
    startWebServer defaultConfig app
    0
