namespace RandomGenerator.Test.RandomTests

open System
open NUnit.Framework
open RandomGenerator
open RandomGenerator.Lib
open RandomGenerator.Dupe

[<TestFixture>]
type ``Given random strings`` () =   
    [<Test>]
    member test.``only random numeric`` ()=

        let numericRandom = 
            Chars(['0'..'9']) 
            |> Generate 5

        // This should only return positive integers, so UInt32 allows a large enough output
        Assert.Greater(System.UInt32.Parse(numericRandom) , 0)
        //|> should be (greaterThan 0)

    [<Test>]
    member test.``only random alpha`` ()=
        let alphaChars = Chars(['A'..'Z'])
        let alphaRandom = 
            alphaChars
            |> Generate 5
        
        let arrayLength = 
            alphaRandom.ToCharArray()
            |> Array.filter (fun x -> 
                                match alphaChars with
                                | Chars a -> 
                                    match a |> List.tryFind (fun y -> x = y) with
                                    | Some x -> true
                                    | None -> false
                                | CharSet _ -> false)
            |> Array.length
        Assert.AreEqual(5, arrayLength)

    [<Test>]
    member test.``only random characters`` ()=
        let chars = "_()[]{}<>!?;:=*-+/\\%.,$£&#@".ToCharArray() |> Array.toList |> Chars
        let characterRandom = chars |> Generate 5

        let arrayLength =
            characterRandom.ToCharArray()
            |> Array.filter (fun x ->
                                match chars with 
                                | Chars a ->
                                    match a |> List.tryFind (fun y -> x = y) with
                                    | Some x -> true
                                    | None -> false
                                | CharSet _ -> false)
            |> Array.length
        Assert.AreEqual(5, arrayLength)

    [<Test>]
    member test.``all characters`` ()=
        let charList = CharSet(Chars ['A'..'Z'], CharSet(Chars ['0'..'9'], "_()[]{}<>!?;:=*-+/\\%.,$£&#@".ToCharArray() 
                                                                            |> Array.toList 
                                                                            |> Chars))
        let characterRandom = charList |> Generate 25

        let arrayLength =
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
            |> Array.length
        Assert.AreEqual(25, arrayLength)

    [<Test>]
    member test.``alpha and numeric characters`` ()=
        let charList = 
            CharSet(Chars ['A'..'Z'], Chars ['0'..'9'])

        let characterRandom = charList |> Generate 25

        let arrayLength =
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
            |> Array.length
        Assert.AreEqual(25, arrayLength)

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
            genList <- (charList |> Generate 9) :: genList
        
        let seqLength =
            genList 
            |> findDuplicates 
            |> Seq.length 

        Assert.LessOrEqual(seqLength, error)

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
            |> GenerateMultiple count 9 
            |> findDuplicates

        let seqLength = duplicatesExist |> Seq.length 

        Assert.LessOrEqual(seqLength, error)