namespace ship
{
    class ship
    {
        public int size;
        public enum Direction
        {
            horizontal,
            vertical
        }
        enum Symbols
        {
            EmptySpace,
            unfoundShip,
            foundShip,
            missedShot,
            ghostShip
        }
        int[,] board;
        public ship(int enteredSize, ref int[,] enteredBoard)
        {
            size = enteredSize;
            board = enteredBoard;
        }
        public bool canBePlaced(int x, int y)
        {
            if (y >= board.GetLength(0) || x>= board.GetLength(1) || x <=0 || y <=0)
            {
                return false;
            }
            if (board[y, x] == (int)Symbols.EmptySpace)
            {
                return true;
            }
            return false;
        }
        public bool place(int x, int y, int direction, bool rec = false, int symbolToPlaceInt=(int)Symbols.unfoundShip)
        {
            Direction shipDirection = (Direction)direction;
            Symbols symbolToPlace = (Symbols)symbolToPlaceInt;
            if (!canBePlaced(x, y))
            { // see if the user can be placed
                return false; // if the ship cannot be placed in that location return false
            }
            if (!rec && shipDirection == Direction.horizontal)
            {
                for (int i = 0; i < size; i++)
                {
                    if (!canBePlaced(x + i, y))
                    {
                        return false;
                    }
                }
                for (int i = 0; i < size; i++)
                {
                    place(x + i, y, (int)shipDirection, true);
                }
            }
            if (!rec && shipDirection == Direction.vertical)
            {
                for (int i = 0; i < size; i++)
                {
                    if (!canBePlaced(x, y+i))
                    {
                        return false;
                    }
                }
                for (int i = 0; i < size; i++)
                {
                    place(x, y + i, (int)shipDirection, true);
                }
            }
            board[y, x] = (int)symbolToPlace;
            return true;
        }
    }
}