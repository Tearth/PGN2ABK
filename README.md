# PGN2ABK
This is the simple tool to generate [ABK](https://www.chessprogramming.org/ABK) (Arena's book format) files using a collection of games saved in the single PGN file. There are several sources of PGN files, my favorite one is https://database.lichess.org/. It takes about 30 minutes to parse 190 GB file on normal HDD.

Program has been written in C# for .NET Core 3 platform. It uses [CommandLineParser](https://github.com/commandlineparser/commandline) (to parse parameters) and [ShellProgressBar](https://github.com/Mpdreamz/shellprogressbar).

# Usage

```
-i, --input             Required. PGN input file to process.
-o, --output            Required. ABK output file.
-p, --plies             (Default: 2147483647) Maximal number of plies to parse.
-e, --elo               Minimal average ELO of players to parse the game.
-t, --time              Minimal initial game time (in minutes) to parse the game.
-m, --multithreading    Enable support for multithreading.
--help                  Display this help screen.
--version               Display version information.
```

# Example

Input: `PGN2ABK.exe -i .\lichess_db_standard_rated_2013-01.pgn -o .\generated.abk -p 20 -t 15 -m`

Output:
![Output](https://i.imgur.com/J4FcrwN.png)