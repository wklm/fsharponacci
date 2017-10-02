open Suave
open Suave.Filters
open Suave.Successful
open Chiron
open Fibonacci

let config =
 { defaultConfig with
     bindings = [ HttpBinding.createSimple HTTP "0.0.0.0" 3459 ] }

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
     pathScan "/%d" ((bigint:int -> bigint) >> fibJsonArray >> OK)
 
[<EntryPoint>]
let main argv = 
    startWebServer config app
    0
