namespace RandomGenerator.Test.RandomTests

open NUnit.Framework
open FsUnit
open RandomGenerator
open RandomGenerator.Lib
open RandomGenerator.Dupe

[<TestFixture>]
type ``Given random strings`` () =   
    [<Test>]
    member test.``only random numeric`` ()=
        let numericRandom = 
            Chars(['0'..'9']) 
            |> generate 5

        // This should only return positive integers, so UInt32 allows a large enough output
        System.UInt32.Parse(numericRandom) |> should be (greaterThan 0)

    [<Test>]
    member test.``only random alpha`` ()=
        let alphaChars = Chars(['A'..'Z'])
        let alphaRandom = 
            alphaChars
            |> generate 5
        
        alphaRandom.ToCharArray()
        |> Array.filter (fun x -> 
                            match alphaChars with
                            | Chars a -> 
                                match a |> List.tryFind (fun y -> x = y) with
                                | Some x -> true
                                | None -> false
                            | CharSet _ -> false)
        |> should haveLength 5

    [<Test>]
    member test.``only random characters`` ()=
        let chars = "_()[]{}<>!?;:=*-+/\\%.,$£&#@".ToCharArray() |> Array.toList |> Chars
        let characterRandom = chars |> generate 5

        characterRandom.ToCharArray()
        |> Array.filter (fun x ->
                            match chars with 
                            | Chars a ->
                                match a |> List.tryFind (fun y -> x = y) with
                                | Some x -> true
                                | None -> false
                            | CharSet _ -> false)
        |> should haveLength 5

    [<Test>]
    member test.``all characters`` ()=
        let charList = CharSet(Chars ['A'..'Z'], CharSet(Chars ['0'..'9'], "_()[]{}<>!?;:=*-+/\\%.,$£&#@".ToCharArray() 
                                                                            |> Array.toList 
                                                                            |> Chars))
        let characterRandom = charList |> generate 25

        characterRandom.ToCharArray()
        |> Array.filter (fun x ->
                            let rec find c =
                                match c with 
                                | Chars a ->
                                    match a |> List.tryFind (fun y -> x = y) with
                                    | Some x -> true
                                    | None -> false
                                | CharSet (d,e) -> find d || find e
                            find charList)
        |> should haveLength 25

    [<Test>]
    member test.``alpha and numeric characters`` ()=
        let charList = 
            CharSet(Chars ['A'..'Z'], Chars ['0'..'9'])

        let characterRandom = charList |> generate 25

        characterRandom.ToCharArray()
        |> Array.filter (fun x ->
                            let rec find c =
                                match c with 
                                | Chars a ->
                                    match a |> List.tryFind (fun y -> x = y) with
                                    | Some x -> true
                                    | None -> false
                                | CharSet (d,e) -> find d || find e
                            find charList)
        |> should haveLength 25

    [<Test>]
    member test.``no duplicates when running single generated iteratively`` ()=
        let charList = 
            CharSet(Chars ['A'..'Z'], Chars ['0'..'9'])

        let count = 100000
        let error = 
            // Tabulate .003% error
            System.Math.Ceiling((float count) * 0.00003)

        let mutable genList = List.empty
        for i in [0..100000] do 
            genList <- (charList |> generate 9) :: genList
        
        genList 
        |> findDuplicates 
        |> Seq.length 
        |> should be (lessThanOrEqualTo error)

    [<Test>]
    member test.``no repeats exist in RandomGen`` ()=
        let charList = CharSet(Chars ['A'..'Z'], CharSet(Chars ['0'..'9'], "_()[]{}<>!?;:=*-+/\\%.,$£&#@".ToCharArray() 
                                                                                |> Array.toList 
                                                                                |> Chars))
        let count = 100000
        let error = 
            // Tabulate .0025% error
            System.Math.Ceiling((float count) * 0.000025)

        let duplicatesExist = 
            charList
            |> generateMultiple count 9 
            |> findDuplicates

        Seq.length duplicatesExist 
        |> should be (lessThanOrEqualTo error)