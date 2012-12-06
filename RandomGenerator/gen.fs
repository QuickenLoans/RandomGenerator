(* ============================
 *    Original inspiration found here: 
 *       http://community.solidshellsecurity.com/topic/11236-f-strong-random-password-generator-multi-core/
 * ============================ *)

namespace RandomGenerator

module Lib =
    open System

    type CharacterTypes =
        | Chars of char list   
        | CharSet of CharacterTypes * CharacterTypes

    let private combine chars =
        let rec merge xs ys =
            match xs with
            | [] -> ys
            | h::t -> merge t (h::ys)

        and source a (b:char list) =
            match a with
            | Chars x -> merge x b
            | CharSet (x,y) -> (source x b) |> merge <| (source y b)

        source chars [] |> List.toArray

    let private syncObj = new Object()
    let private rand = new Random(Environment.TickCount)
    let private seed size = 
        lock syncObj (fun () -> rand.Next(size))
        
    (* Creates an async computation that generates a random (based on the seed value) string *)
    let private gen length (allowedChars:char[]) = 
        let count = allowedChars.Length
        async {
            return Array.zeroCreate<char> length
            |> Array.map (fun c -> allowedChars.[seed <| count])
            |> fun rand -> new String(rand) 
            }

    let Generate length chars =
        combine chars 
        |> gen length 
        |> Async.RunSynchronously
    
    let GenerateMultiple amount length chars =
        // Store the cleaned up char list so it's not reprocessed w/ map
        let cleanChars = combine chars

        {1..amount} 
        |> Seq.map (fun _ -> gen length cleanChars)
        |> Async.Parallel
        |> Async.RunSynchronously

module Dupe =
    let findDuplicates xs =
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