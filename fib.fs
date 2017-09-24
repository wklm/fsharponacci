let rec fib = function 
  | 0L | 1L as n -> n
  | n -> (fib (n - 1L) + fib (n - 2L))

let memoize f = 
  let memo = ref Map.empty
  fun arg ->
    try Map.find arg !memo
    with :? System.Collections.Generic.KeyNotFoundException ->    
      let result = f arg
      memo := Map.add arg result !memo
      result

let rec fibm = 
  memoize <| fun n ->
  match n with
    | 0L | 1L as n -> n
    | n -> fibm (n - 1L) + fibm (n - 2L)


let go (f : int64 -> int64) (range : int64) = 
  seq { for i in [0L..range] do yield async {return f i} } 
  |> Async.Parallel 
  |> Async.RunSynchronously


[<EntryPoint>]
let main argv =
  let res = match argv.[0] |> string with
    | "fib" -> go fib (int64 argv.[1])
    | "fibm" -> go fibm (int64 argv.[1])
  printfn "%A" res
  0