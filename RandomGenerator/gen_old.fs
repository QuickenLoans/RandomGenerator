namespace RandomGenerator

module Lib_Old =
    open System

    (* List of numbers *)
    let mutable DigitChars   = ['0'..'9']
    (* List of letters *)
    let mutable LetterChars  = ['A'..'Z']
    (* List of special characters *)
    let mutable SpecialChars = "_()[]{}<>!?;:=*-+/\\%.,$£&#@"
                               |> fun s -> s.ToCharArray()
                               |> List.ofArray

    (* Creates an async computation that generates a random (based on the seed value) string *)
    let private Gen seed allowedChars length = async {
        let random = new Random(seed)
        let count = (allowedChars:char[]).Length
        return
            Array.zeroCreate<char> length
            |> Array.map (fun char -> allowedChars.[random.Next(count)])
            |> fun passwrd -> new String(passwrd) 
        }

    (*  Generate a single random string *)
    let Generate allowDigits allowLetters allowSpecial length =
        [| if allowDigits  then yield! DigitChars
           if allowLetters then yield! LetterChars
           if allowSpecial then yield! SpecialChars |]
        |> fun chars -> Gen Environment.TickCount chars length
        |> Async.RunSynchronously

    (* Generate multiple strings in parallel *)
    let GenerateMultiple allowDigits allowLetters allowSpecial length amount =
        let seed = new Random(Environment.TickCount)
        [| if allowDigits then yield! DigitChars
           if allowLetters then yield! LetterChars
           if allowSpecial then yield! SpecialChars |]
        |> fun chars -> [ for _ in 1 .. amount -> Gen (seed.Next()) chars (length) ]
        |> Async.Parallel
        |> Async.RunSynchronously

    let FindDuplicates xs =
        (Map.empty, xs)
        ||> Seq.scan (fun xs x ->
            match Map.tryFind x xs with
            | None -> Map.add x false xs
            | Some false -> Map.add x true xs
            | Some true -> xs)
        |> Seq.zip xs
        |> Seq.choose (fun (x, xs) ->
            match Map.tryFind x xs with
            | Some false -> Some x
            | None | Some true -> None)