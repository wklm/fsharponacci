module Server

open System
open Chiron
open Suave
open Suave.Filters
open Suave.Successful
open Fibonacci

let config = 
  let port = match Environment.GetEnvironmentVariable("port") with
    | null -> 8080 
    | p -> int p
  { defaultConfig with bindings =
    [ HttpBinding.createSimple HTTP "0.0.0.0" port ] }

type Result = 
  { fibArray : string [] }
  static member ToJson(result : Result) =
    json { do! Json.write "Fibonacci sequence" result.fibArray }

let fibJsonArray (n : bigint) = 
  { fibArray = (go fib n)
    |> Seq.map string
    |> Seq.toArray }
  |> Json.serialize
  |> Json.format

let app = 
  pathScan "/%d" 
  <| ((bigint : int -> bigint) >> fibJsonArray >> OK)

[<EntryPoint>]
let main argv = 
  startWebServer config app
  0
