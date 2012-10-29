namespace RandomGenerator.Test.RandomTests

open NUnit.Framework
open FsUnit
open RandomGenerator

[<TestFixture>]
type ``Given random strings`` () =   
    [<Test>]
    member test.``only random numeric`` ()=
        let numericRandom = Lib.Generate true false false 5
        // This should only return positive integers, so UInt32 allows a large enough output
        System.UInt32.Parse(numericRandom) |> should be (greaterThan 0)

    [<Test>]
    member test.``only random alpha`` ()=
        let alphaRandom = Lib.Generate false true false 5
        
        alphaRandom.ToCharArray()
        |> Array.filter (fun x -> 
                            match Lib.LetterChars |> List.tryFind (fun y -> x = y) with
                            | Some x -> true
                            | None -> false)
        |> should haveLength 5

    [<Test>]
    member test.``only random characters`` ()=
        let characterRandom = Lib.Generate false false true 5

        characterRandom.ToCharArray()
        |> Array.filter (fun x ->
                            match Lib.SpecialChars |> List.tryFind (fun y -> x = y) with
                            | Some x -> true
                            | None -> false)
        |> should haveLength 5

    [<Test>]
    member test.``no repeats exist in RandomGen`` ()=
        let count = 100000
        let error = 
            // Tabulate .025% error
            System.Math.Round((float count) * 0.00025)

        let duplicatesExist = 
            Lib.GenerateMultiple true true false 9 count 
            |> Lib.FindDuplicates
        // There should be less than .025% (of total count) duplicates present.
        // This is because we cannot accurately create truly distinct records.
        // As a result we end up w/ ~2 duplicates per 100,000 generated strings
        Seq.length duplicatesExist 
        |> should be (lessThanOrEqualTo error)
        