using CST350Milestone5.Models;

namespace CST350Milestone5.Services
{
    public class GameService
    {

        /// <summary>
        /// Initialize the game board with the values for width, height, and difficulty
        /// </summary>
        /// <param name="boardWidth">The width of the board or number of columns</param>
        /// <param name="boardHeight">The height of the board or number of rows</param>
        /// <param name="difficulty">The difficulty from 0-1.0 for the number of cells to be bombs</param>
        /// <returns></returns>
        public static GameBoardModel InitializeBoard(int boardWidth, int boardHeight, double difficulty)
        {
            GameBoardModel gameBoard;

            GameCellModel[][] cellGrid = new GameCellModel[boardWidth][];

            for (int column = 0; column < boardWidth; column++)
                cellGrid[column] = new GameCellModel[boardHeight];

            for (int row = 0; row < boardHeight; row++)
            {
                for (int column = 0; column < boardWidth; column++)
                {
                    cellGrid[column][row] = new GameCellModel(row, column);
                }
            }

            gameBoard = new GameBoardModel(0, boardHeight, boardWidth, difficulty, cellGrid);

            SetupLiveNeighbors(gameBoard);
            CalculateLiveNeighbors(gameBoard);

            return gameBoard;
        }

        /// <summary>
        /// Marks a cell as visited
        /// Recursively reveals adjacent cells with 0 live neighbors when marking a cell with 0 live neighbors
        /// </summary>
        /// <param name="column">The column or X to visit</param>
        /// <param name="row">The row or Y to visit</param>
        /// <returns>True if the cell was marked as visited</returns>
        public static void FloodFill(GameBoardModel boardModel, int column, int row)
        {
            if (column < 0 || row < 0 || column >= boardModel.Width || row >= boardModel.Height || // Check bounds
                boardModel.CellGrid[column][row].CellVisited) // Skip visited cells
                return;

            boardModel.CellGrid[column][row].CellVisited = true;

            if (boardModel.CellGrid[column][row].LiveNeighbors == 0)
            {
                FloodFill(boardModel, column, row - 1); // Up
                FloodFill(boardModel, column + 1, row); // Right
                FloodFill(boardModel, column, row + 1); // Down
                FloodFill(boardModel, column - 1, row); // Left
            }
        }

        /// <summary>
        /// Checks if the game was successfully completed by uncovering all inactive cells
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        public static bool IsGameComplete(GameBoardModel boardModel)
        {
            for (int x = 0; x < boardModel.Width; x++)
                for (int y = 0; y < boardModel.Height; y++)
                    if (!boardModel.CellGrid[x][y].CellLive && !boardModel.CellGrid[x][y].CellVisited)
                        return false; // An inactive cell

            return true;
        }

        /// <summary>
        /// Randomly sets cells to live according to the game difficulty
        /// A difficulty of 0.5 will result in half of the cells being live
        /// The live cells are limited to 1 <= N <= (size * size) - 1
        /// </summary>
        /// <param name="gameDifficulty">Difficulty percentage [0.0 - 1.0]</param>
        public static void SetupLiveNeighbors(GameBoardModel boardModel)
        {
            boardModel.Difficulty = Math.Min(0.99, boardModel.Difficulty);

            int expectedLiveCells = (int)Math.Min((boardModel.Width * boardModel.Height) - 1, // Upper bound of (size * size) - 1
                                                  Math.Max(1, (double)(boardModel.Width * boardModel.Height) * boardModel.Difficulty)), // Lower bound of 1
                currentLiveCells = 0,
                currentCellX, currentCellY;

            Random rnd = new Random();

            while (currentLiveCells < expectedLiveCells)
            {
                currentCellX = rnd.Next(0, boardModel.Width);
                currentCellY = rnd.Next(0, boardModel.Height);

                if (!boardModel.CellGrid[currentCellX][currentCellY].CellLive)
                {
                    boardModel.CellGrid[currentCellX][currentCellY].CellLive = true;
                    currentLiveCells++;
                }
            }
        }

        /// <summary>
        /// Calculates how many neighbors each cell has that are live, including itself
        /// </summary>
        public static void CalculateLiveNeighbors(GameBoardModel boardModel)
        {
            for (int x = 0; x < boardModel.Width; x++)
                for (int y = 0; y < boardModel.Height; y++)
                    CountNearbyLiveNeighbors(boardModel, x, y);
        }

        /// <summary>
        /// Counts the nearby live cells including the current cell and assigns the count to the cell's LiveNeighbors
        /// </summary>
        /// <param name="cellColumn"></param>
        /// <param name="cellRow"></param>
        private static void CountNearbyLiveNeighbors(GameBoardModel boardModel, int cellColumn, int cellRow)
        {
            int liveCount = 0;

            for (int x = -1; x <= 1; x++)
                for (int y = -1; y <= 1; y++)
                    if (cellColumn + x >= 0 && cellColumn + x < boardModel.Width && // Check bounds for width/columns
                        cellRow + y >= 0 && cellRow + y < boardModel.Height && // Check bounds for height/rows
                        boardModel.CellGrid[cellColumn + x][cellRow + y].CellLive) // Check if cell is live
                        liveCount++;

            boardModel.CellGrid[cellColumn][cellRow].LiveNeighbors = liveCount;
        }

        /// <summary>
        /// Toggles flagging a cell within the bounds of the game board
        /// </summary>
        /// <param name="boardModel"></param>
        /// <param name="cellColumn"></param>
        /// <param name="cellRow"></param>
        public static void FlagCell(GameBoardModel boardModel, int cellColumn, int cellRow)
        {
            if (cellColumn >= 0 && cellColumn < boardModel.Width &&
                cellRow >= 0 && cellRow < boardModel.Height)
                boardModel.CellGrid[cellColumn][cellRow].CellFlagged = !boardModel.CellGrid[cellColumn][cellRow].CellFlagged;
		}
    }
}
