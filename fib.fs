module Fibonacci

open System.Collections.Concurrent

let memo = ConcurrentDictionary<'a, 'b>()

let memoize (f : 'a -> 'b) arg =
  memo.GetOrAdd(arg, f)

let mapAsync (f: 'a -> 'b) (s: seq<'a>) = 
  seq { for element in s do yield async {return f element} }

let rec fib = 
  memoize <| fun n ->
  if n < 2I then n
  else (fib (n - 1I) + fib (n - 2I))

let go (f : bigint -> bigint) (range : bigint) = 
  [0I..range]
  |> mapAsync f
  |> Async.Parallel 
  |> Async.RunSynchronously