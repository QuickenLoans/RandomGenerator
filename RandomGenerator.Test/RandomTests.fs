namespace RandomGenerator.Test.RandomTests

open NUnit.Framework
open FsUnit
open RandomGenerator

[<TestFixture>]
type ``Given random strings`` () =   
    [<Test>]
    member test.``only random numeric`` ()=
        let numericRandom = 
            Lib.Chars(['0'..'9']) 
            |> Lib.generate 5

        // This should only return positive integers, so UInt32 allows a large enough output
        System.UInt32.Parse(numericRandom) |> should be (greaterThan 0)

    [<Test>]
    member test.``only random alpha`` ()=
        let alphaChars = Lib.Chars(['A'..'Z'])
        let alphaRandom = 
            alphaChars
            |> Lib.generate 5
        
        alphaRandom.ToCharArray()
        |> Array.filter (fun x -> 
                            match alphaChars with
                            | Lib.Chars a -> 
                                match a |> List.tryFind (fun y -> x = y) with
                                | Some x -> true
                                | None -> false
                            | Lib.CharSet _ -> false)
        |> should haveLength 5

    [<Test>]
    member test.``only random characters`` ()=
        let chars = "_()[]{}<>!?;:=*-+/\\%.,$£&#@".ToCharArray() |> Array.toList |> Lib.Chars
        let characterRandom = chars |> Lib.generate 5

        characterRandom.ToCharArray()
        |> Array.filter (fun x ->
                            match chars with 
                            | Lib.Chars a ->
                                match a |> List.tryFind (fun y -> x = y) with
                                | Some x -> true
                                | None -> false
                            | Lib.CharSet _ -> false)
        |> should haveLength 5

    [<Test>]
    member test.``all characters`` ()=
        let charList = Lib.CharSet(Lib.Chars ['A'..'Z'], Lib.CharSet(Lib.Chars ['0'..'9'], "_()[]{}<>!?;:=*-+/\\%.,$£&#@".ToCharArray() |> Array.toList |> Lib.Chars))
        let characterRandom = charList |> Lib.generate 25

        characterRandom.ToCharArray()
        |> Array.filter (fun x ->
                            let rec find c =
                                match c with 
                                | Lib.Chars a ->
                                    match a |> List.tryFind (fun y -> x = y) with
                                    | Some x -> true
                                    | None -> false
                                | Lib.CharSet (d,e) -> find d || find e
                            find charList)
        |> should haveLength 25

    [<Test>]
    member test.``no repeats exist in RandomGen`` ()=
        let charList = Lib.CharSet(Lib.Chars ['A'..'Z'], Lib.CharSet(Lib.Chars ['0'..'9'], "_()[]{}<>!?;:=*-+/\\%.,$£&#@".ToCharArray() |> Array.toList |> Lib.Chars))
        let count = 100000000
        let error = 
            // Tabulate .000025% error
            System.Math.Round((float count) * 0.00000025)

        let duplicatesExist = 
            charList
            |> Lib.generateMultiple count 9 
            |> Lib.findDuplicates

        Seq.length duplicatesExist 
        |> should be (lessThanOrEqualTo error)