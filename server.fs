module Server

open System
open Chiron
open Suave
open Suave.Filters
open Suave.Successful
open Fibonacci

let config = 
  let port = match Environment.GetEnvironmentVariable("port") with
    | null -> 8080 | p -> int p
  { defaultConfig with bindings =
    [ HttpBinding.createSimple HTTP "0.0.0.0" port ] }

type QueryParameter = UpperBound of int | LowerUpperBound of (int * int)

type Result = 
  { fibArray : string [] }
  static member ToJson(result : Result) =
    json { do! Json.write "Fibonacci sequence" result.fibArray }

let fibJsonArray (range : option<bigint> * bigint) = 
  { fibArray =
      range 
      |> go fib
      |> Seq.map string
      |> Seq.toArray }
  |> Json.serialize
  |> Json.format

let transformParamTuple (n : int * int) =
  fst n |> bigint |> Some, snd n |> bigint

let route (param : QueryParameter) =
  match param with
  | UpperBound param -> param |> bigint |> fun n -> (None, n)
  | LowerUpperBound param -> param |> transformParamTuple 
  |> fibJsonArray 
  |> OK

let app =
  choose [
    pathScan "/%d" (UpperBound >> route)
    pathScan "/%d/%d" (LowerUpperBound >> route)
  ]

[<EntryPoint>]
let main argv = 
  startWebServer config app
  0