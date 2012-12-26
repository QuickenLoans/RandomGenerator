#Random Generator (F\#\\C\#)#

This F# library (plus C# API) is capable of generating random strings of variable length.  The library can be configured to return strings containing alpha ['A'..'Z'], numeric ['0'..'9'], and/or characters (~!@#$%^&\*()_+-=). 

It can also generate large numbers of strings asynchronously with high randomness (less than 0.0025% duplicates in 100,000 generated strings).

Tests (NUnit) are provided to show how to use the library as well as ensure basic requirements of the libraries.

##F\# Library##
Random Generator relies on the caller to provide the instance of characters to use to generate the string(s). It accomplishes this through a recursive discriminated union:

	open RandomGenerator.Lib
	let charList = CharSet(Chars ['A'..'Z'], CharSet(Chars ['0'..'9'])

Once the characters are defined	the call to generate is simple:

	charList |> Generate 25
	
Here the 25 represents how many characters should be contained in your string: "H0FPT9J6Y9PGU7SJQA72XQTWQ"

Additionally, Random Generator is capable of generating multiple strings with low probability of repeating.

	// Generate 100000 strings of 9 characters each
	let genStrings = charList |> GenerateMultiple 100000 9
	
There's also a helper in the Dupe module that I use to determine if the Generator has created duplicate results:

	// findDuplicates returns a list of the duplicate values
	genStrings |> Dupe.findDuplicates

*Note: This was written back in F# 2.0 w/o the F# PowerPack, this could be replaced w/ LINQ or a simpler helper now.

##C\# API##
The C# API wrapper was written to abstract some of the complexity of dealing with F# discriminated unions.
The call from C# looks like:

	//open RandomGenerator.Interop;
	var gen = new Generator();
	let myString = gen.Singe(25);

There are several defaults built into the API, but you can override them in the constructor if need be:

	// Three bools can be passed in which tell the generator which type(s) of characters to use: alpha, numeric, symbol
	// By default it assumes alpha and numeric (not symbol)
	var gen = new Generator(true, true, false);
	let myString = gen.Singe(25);

You can also generate multiple strings just as simply:

	var gen = new Generator();
	let myStrings = gen.Multiple(100000, 25);
