# PGN2ABK
This is the simple tool to generate ABK (Arena's book format) files using a collection of games saved in the PGN file. There are several sources of PGN files, my favorite one is https://database.lichess.org/. It takes about 30 minutes to parse 190 GB file on normal HDD.

# Usage

```
-i, --input             Required. PGN input file to process.
-o, --output            Required. ABK output file.
-p, --plies             (Default: 2147483647) Maximal number of plies to parse.
-e, --elo               Minimal average ELO of players to parse the game.
-t, --time              Minimal initial game time to parse the game.
-m, --multithreading    Enable support for multithreading.
--help                  Display this help screen.
--version               Display version information.
```

# Example

Input: `-i D:\DB\lichess_db_standard_rated_2013-01.pgn -o C:\Users\Pawel\Desktop\generated.abk -p 20 -m`

Output:
![Output](https://i.imgur.com/J4FcrwN.png)