module ConsorciosCLP.Utils

open Suave
open Suave.Json
open Suave.Operators

let jsonRequest f =
    request (fun r -> f (fromJson r.rawForm))

let jsonResponse p o =
    p (toJson o) >=> Writers.setMimeType "application/json"
