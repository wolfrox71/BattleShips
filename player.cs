namespace player
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using ship;

    class Player
    {
        int height;
        int width;
        int numberOfShips = 10;
        int totalShipsOnBoard = 0;
        int minShipSize = 2;
        int maxShipSize = 4;
        public int turns = 0;

        Dictionary<int, int> typesOfShip = new Dictionary<int, int>()
        {
            { 1, 3 },
            { 2, 3 },
            { 3, 2 },
            { 4, 2 }
        };

        enum Symbols
        {
            emptySpace,
            unfoundShip,
            foundShip,
            missedShot,
            ghostShip
        }
        enum Direction
        {
            horizontal,
            vertical
        }

        int[,] numberOfEachShip;

        int[,] board;

        public Player(int enterdWidth, int enteredHeight)
        {
            width = enterdWidth;
            height = enteredHeight;
            numberOfEachShip = new int[numberOfShips, 2];
            board = new int[height, width];
        }

        public void displayBoard(bool show=false)
        {
            // output the x values
            Console.Write(" ");
            for (int i = 0; i < width; i++)
            {
                Console.Write($" {i+1} ");
            } Console.WriteLine(); // move to the next line

            for (int y = 0; y < height; y++)
            {
                Console.Write($"{y + 1}"); // output the y value
                for (int x = 0; x < width; x++)
                {
                    string toWrite;
                    switch (board[y, x]) // get the value of the board location
                    {
                        case (int)Symbols.unfoundShip: // if its an unfound ship
                            if (!show) { // if unfound ships are not being shown
                                toWrite = " N "; // output it as an unshot location
                                Console.ForegroundColor = ConsoleColor.Gray;
                            } else { // if unfound ships are being shown as unfound ships
                                toWrite = " S "; // show it was a ship
                                Console.ForegroundColor = ConsoleColor.Blue;
                            }
                            break;

                        case (int)Symbols.foundShip:
                            Console.ForegroundColor = ConsoleColor.Green;
                            toWrite = " F ";
                            break;

                        case (int)Symbols.ghostShip:
                            Console.ForegroundColor= ConsoleColor.Yellow;
                            toWrite = " G ";
                            break;

                        case ((int)Symbols.missedShot):
                            Console.ForegroundColor = ConsoleColor.Red;
                            toWrite = " X ";
                            break;

                        default:
                            toWrite = " N ";
                            Console.ForegroundColor = ConsoleColor.Gray;
                            break;
                    }
                    Console.Write(toWrite);
                    Console.ResetColor(); // reset the colour back so that it doesnt blead into the input
                }
                Console.WriteLine(); // move to the next line
            }
        }
        public bool shipsRemaining()
        {
            foreach (int square in board) // go through each location on the board
            {
                if (square == (int)Symbols.unfoundShip) // if the block is a ship
                {
                    return true; // return that there are 1+ remaining ships
                }
            }
            return false; // if non of the blocks were unfound ships, no ships are remaining
        }
        public int hit(int x, int y)
        {
            if (y > board.GetLength(0) || x > board.GetLength(1) || x <= 0 || y <= 0) // if the ship is outside the board
            {
                return 4; // return 4 to return it was off the board so that it can be reshot
            }
            switch (board[y - 1, x - 1]) {
                case (int)Symbols.unfoundShip: 
                    board[y - 1, x - 1] = (int)Symbols.foundShip; // set the zone to be a found ship
                    return 1; // return 1 to indicate it hit

                case (int)Symbols.foundShip:
                case (int)Symbols.missedShot: // if it ship an all ready hit space
                    return 2; // return 2 to indicate a previously hit space

                default: // if it didnt hit anything else
                    board[y - 1, x - 1] = (int)Symbols.missedShot; // change the zone to be a missed shot
                    return 0; // return 0 to indicate a missed shot
            }

        }

        public void computerPopulateBoard()
        {
            while (totalShipsOnBoard <= numberOfShips)
            {
                Random rnd = new Random();
                int shipSize;
                do
                {
                    // get the size of the ship
                    shipSize = rnd.Next(minShipSize, maxShipSize);
                    if (!typesOfShip.ContainsKey(shipSize)) { typesOfShip.Add(shipSize, 0); }
                } while (typesOfShip[shipSize] == 0);

                ship testShip = new ship(shipSize, ref board);
                Direction shipFaceing = (Direction)rnd.Next(0, 2);
                // try place a ship on the board with a random direction, width, x, and y coordinate
                if (testShip.place(rnd.Next(0, height), rnd.Next(0, width), (int)shipFaceing)) 
                {
                    // if it was placed

                    // update the count of ships on the board
                    totalShipsOnBoard++;
                }
            }
        }
        protected (int,int) nextAvalableSpace()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (board[y, x] == (int)Symbols.emptySpace)
                    {
                        return ( -1, -1 );
                    }
                }
            }
            return (-1, -1);
        }
        public void humanPopulateBoard()
        {
            
            foreach (int shipsSizes in typesOfShip.Keys)
            {
                for (int currentShipSize = 0; currentShipSize < typesOfShip[shipsSizes]; currentShipSize++)
                {
                    ship currentShip = new ship(currentShipSize, ref board);
                    int x, y;
                    (x,y) = nextAvalableSpace();
                    currentShip.place(x+1,y+1,1, false, (int)Symbols.ghostShip);
                    displayBoard();
                }
            }
            
        }

        public void humanPopulateBoard2()
        {
            int xInt = 0;
            int yInt = 0;
            string[] horizontalValues = { "h", "horizontal" };
            string[] verticalValues = { "v", "vertical" };
            Direction direction;
            Random rnd = new Random();

            while (totalShipsOnBoard < numberOfShips)
            {
                //show the board with the locations of the ships on
                displayBoard(true);
                int shipSize;
                // repeat until a valid ship size is given
                do
                {
                    // get a ship size
                    shipSize = rnd.Next(minShipSize, maxShipSize);
                    // if the ship size doesnt exit, add it with a value of 0 ships
                    if (!typesOfShip.ContainsKey(shipSize)) { typesOfShip.Add(shipSize, 0); }
                } while (typesOfShip[shipSize] == 0); // check to see if any of the ship are left to be placed
                
                // create a ship
                ship testShip = new ship(shipSize, ref board);
                // output the size of the ship
                Console.WriteLine($"The ship is {testShip.size} blocks wide");

                // get the x pos of the ship
                Console.WriteLine($"Enter x position: (1-{width})");
                string enteredX = Console.ReadLine();

                // get the y pos of the ship
                Console.WriteLine($"Enter y position: (1-{height})");
                string enteredY = Console.ReadLine();

                //get the direction of the ship
                Console.WriteLine("Enter direction: (h/v)");
                string enteredDirection = Console.ReadLine().ToLower();

                // if the direction is horizontal
                if (horizontalValues.Contains(enteredDirection))
                {
                    // set the ships direction to horizontal
                    direction = Direction.horizontal;
                }

                // if the direction is vertical
                else if (verticalValues.Contains(enteredDirection))
                {
                    direction = Direction.vertical;
                } else
                //if it isnt either
                {
                    // give an error and restart the loop
                    Console.WriteLine("Entered direction has to be either horizontal or virtical");
                    continue;
                }


                // if either the x and/or the y are not a number
                if (!(Int32.TryParse(enteredX, out xInt) && Int32.TryParse(enteredY, out yInt)))
                {
                    //output it
                    Console.WriteLine("X and Y both need to be ints");
                    // restart the loop
                    continue;
                }
                //if the x is to wide or less than 1 -> eg not on the board
                if ((xInt > width || xInt < 1 ))
                {
                    // output the error
                    Console.WriteLine($"X needs to be between 1 and {width}");
                    // restart the loop
                    continue;
                }
                // if the y is to tall or less than 1 -> eg not on the board
                if ((yInt > height || yInt < 1))
                {
                    //output the error
                    Console.WriteLine($"Y needs to be between 1 and {height}");
                    // restart the loop
                    continue;
                }

                // try place the ship on the board
                if (testShip.place(xInt-1, yInt-1, (int)direction))
                {
                    // if it worked

                    // add 1 to the count of ships on the board
                    totalShipsOnBoard++;
                    // restart the loop for the next ship
                    continue;
                }
                else
                {
                    // if it didnt

                    // output the error
                    Console.WriteLine("The ship cannot be placed in that location");
                    // restart the loop
                    continue;
                }

            }
        }
        public void populateBoard(bool computer = true)
        {
            if (!computer) { humanPopulateBoard(); }
            else { computerPopulateBoard(); }
        }
    }
}