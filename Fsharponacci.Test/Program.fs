module Program = 
    let [<EntryPoint>] main _ =
        Fibonacci.fib 4I |> int

