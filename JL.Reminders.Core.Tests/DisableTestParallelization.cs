// This is necessary because AutoMapper's static API throws errors if it
// gets initialized more than once, and normally XUnit runs different test
// classes (each requiring their own AutoMapper configuration) in parallel.

// Alternatives are:

// 1) Use a test base class that initialises *all* AutoMapper config, and set
//    it up for mutex-locked one-time initialisation.

// 2) Switch to the AutoMapper instance API.

// ...but - am dubious about having the first in unit tests, and the second
// is more complexity with DI/mocking than I want right now!

// so for the time being..

[assembly: Xunit.CollectionBehavior(DisableTestParallelization = true)]