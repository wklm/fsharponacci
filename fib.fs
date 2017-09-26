open System
open System.Collections.Concurrent

let memo = ConcurrentDictionary<'a, 'b>()

let memoize (f : 'a -> 'b) arg =
  memo.GetOrAdd(arg, f)

let mapAsync (f: 'a -> 'b) (s: seq<'a>) = 
   seq { for element in s do yield async {return f element} }

let rec fib = 
  memoize <| function 
  | 0L | 1L as n -> n
  | n -> (fib (n - 1L) + fib (n - 2L))

let go (f : int64 -> int64) (range : int64) = 
  [0L..range]
  |> mapAsync f 
  |> Async.Parallel 
  |> Async.RunSynchronously

[<EntryPoint>]
let main argv =
  go fib (int64 argv.[0]) 
  |> printfn "%A"
  0