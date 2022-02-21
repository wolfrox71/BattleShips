using System;
using System.Collections.Generic;
using ship;
using player;

class Program
{
    static int numberOfPlayers = 1;
    static int boardWidth = 9;
    static int boardHeight = 9;
    static Player[] players = new Player[numberOfPlayers]; // create an array of players
    public static void round(bool inputs=true)
    {

        for (int i = 0; i < numberOfPlayers; i++)
        {
            Player currentPlayer = players[i];
            Player playerToHit = players[i];
            int playerToHitID;
            string playerToHitStr;
            bool repeat = true;
            Console.WriteLine($"You are player {i + 1}");
            players[i].turns++;
            if (numberOfPlayers == 1)
            {
                playerToHitID = 0;
                playerToHit = players[playerToHitID];
            }  
            else
            {
                do
                {
                    if (inputs)
                    {
                        Console.WriteLine($"Enter a player to hit: (1-{players.Length})");
                        playerToHitStr = Console.ReadLine();
                    }
                    else
                    {
                        playerToHitStr = $"{(i + 1) % players.Length + 1}";
                    }
                    if (Int32.TryParse(playerToHitStr, out playerToHitID))
                    {
                        playerToHitID--;
                        if (playerToHitID >= 0 && playerToHitID < players.Length)
                        {
                            repeat = false;
                            playerToHit = players[playerToHitID];
                            continue;
                        }
                    }
                } while (repeat);
            }
            int hitResult;
            do
            {
                Console.Clear();
                playerToHit.displayBoard();
                string enteredX;
                string enteredY;
                int x;
                int y;
                Random rnd = new Random();
                do {
                    Console.WriteLine("Enter X to hit:");
                    if (inputs) {
                        enteredX = Console.ReadLine();
                    }
                    else
                    {
                        enteredX = Convert.ToString(rnd.Next(1, boardWidth + 1));
                    }
                } while (!Int32.TryParse(enteredX, out x));
                do
                {
                    Console.WriteLine("Enter Y to Hit");
                    if (inputs)
                    {
                        enteredY = Console.ReadLine();
                    }
                    else
                    {
                        enteredY = Convert.ToString(rnd.Next(1, boardHeight + 1));
                    }
                    } while (!Int32.TryParse(enteredY, out y));
                hitResult = playerToHit.hit(x, y);
                switch (hitResult)
                {
                    case 0:
                        Console.WriteLine("Miss");
                        continue;
                    case 1:
                        Console.WriteLine("Hit");
                        continue;
                    case 2:
                        Console.WriteLine("Hit preguessed Square");
                        continue;
                    default:
                        Console.WriteLine("Shot off of board");
                        continue;
                }

            } while (hitResult == 4);
        }

    }
    public static void fillPlayers(bool computerFill = true)
    {
        for (int i = 0; i < numberOfPlayers; i++) // for each player
        {
            players[i] = new Player(boardHeight, boardWidth); // create a new player
        }
        foreach (Player player in players)
        {
            if (numberOfPlayers == 1) // if only player is playing
            {
                player.populateBoard(computerFill);  // set the computer to fill the board so the user doesnt know what is one the board
            }
            else // if more that 1 player is playing
            {
                player.populateBoard(computerFill);// populate each players board
            }
        }
    }
    public static bool shipsRemaining()
    {
        foreach (Player player in players)
        {
            if (!player.shipsRemaining()) { return false; }
        }
        return true;
    }
    public static void Main(string[] args)
    {
        fillPlayers(false);

        while (shipsRemaining())
        {
            round(true);
        }
        for (int i = 0;i < players.Length; i++)
        {
            if (!players[i].shipsRemaining()) { Console.WriteLine($"Player {i+1} has no ships remaining, it took {players[i].turns} turns"); }
        }

        for (int i = 0; i < players.Length; i++)
        {
            Console.WriteLine($"Player {i+1}:");
            players[i].displayBoard(true);
            Console.WriteLine("--------------");
        }
    }
}