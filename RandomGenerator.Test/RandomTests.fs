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
        let gen = new Generator()

        let numericRandom = 
            Chars(['0'..'9']) 
            |> gen.Generate 5

        // This should only return positive integers, so UInt32 allows a large enough output
        System.UInt32.Parse(numericRandom) |> should be (greaterThan 0)

    [<Test>]
    member test.``only random alpha`` ()=
        let gen = new Generator()
        let alphaChars = Chars(['A'..'Z'])
        let alphaRandom = 
            alphaChars
            |> gen.Generate 5
        
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
        let gen = new Generator()
        let chars = "_()[]{}<>!?;:=*-+/\\%.,$£&#@".ToCharArray() |> Array.toList |> Chars
        let characterRandom = chars |> gen.Generate 5

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
        let gen = new Generator()
        let charList = CharSet(Chars ['A'..'Z'], CharSet(Chars ['0'..'9'], "_()[]{}<>!?;:=*-+/\\%.,$£&#@".ToCharArray() 
                                                                            |> Array.toList 
                                                                            |> Chars))
        let characterRandom = charList |> gen.Generate 25

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
        let gen = new Generator()
        let charList = 
            CharSet(Chars ['A'..'Z'], Chars ['0'..'9'])

        let characterRandom = charList |> gen.Generate 25

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
        let gen = new Generator()
        let charList = 
            CharSet(Chars ['A'..'Z'], Chars ['0'..'9'])

        let count = 100000
        let error = 
            // Tabulate .003% error
            System.Math.Ceiling((float count) * 0.00003)

        let mutable genList = List.empty
        for i in [0..100000] do 
            genList <- (charList |> gen.Generate 9) :: genList
        
        genList 
        |> findDuplicates 
        |> Seq.length 
        |> should be (lessThanOrEqualTo error)

    [<Test>]
    member test.``no repeats exist in RandomGen`` ()=
        let gen = new Generator()
        let charList = CharSet(Chars ['A'..'Z'], CharSet(Chars ['0'..'9'], "_()[]{}<>!?;:=*-+/\\%.,$£&#@".ToCharArray() 
                                                                                |> Array.toList 
                                                                                |> Chars))
        let count = 100000
        let error = 
            // Tabulate .0025% error
            System.Math.Ceiling((float count) * 0.000025)

        let duplicatesExist = 
            charList
            |> gen.GenerateMultiple count 9 
            |> findDuplicates

        Seq.length duplicatesExist 
        |> should be (lessThanOrEqualTo error)