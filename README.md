# Barbajuan

To execute a test with 1 MCTS-player with 25 determinizations and 50 iterations against 3 random playing agents, execute the following commands in the terminal.

- dotnet restore
- dotnet build
- dotnet run --project .\Barbajuan\

Following that, the program should execute, printing in the console whenever it is done with a game, after 100 games have finished, a summary of how each agents performed is printed in the terminal.

Additional parameters can be added on in 'dotnet run --project .\Barbajuan\ ' to test different values of determinizations and iterations, in specified amount of games. For example:

* dotnet run --project .\Barbajuan\ 10 25 50

Which would execute 50 games, with 1 MCTS-player with 10 determinizations and 25 iterations against 3 random playing agents. After it is done with the games a summary of how each agents performed is printed in the terminal

To execute the supplied test suite use the following commands in the terminal.

* dotnet restore
* dotnet test

One of the tests checks whether a deck is shuffled by looking at the top, sometimes this test fails, as after the shuffle the same card have been shuffled back on top. If this occurs , retry the test command.

Barbajuan (also spelled barbagiuan or barbagiuai) is an appetizer mainly found in the eastern part of the French Riviera, in the western part of Liguria and in Monaco.
