namespace CST350Milestone5.Models
{
    public class GameCellModel
    {
        public int CellRow { get; set; }
        public int CellColumn { get; set; }
        public int LiveNeighbors { get; set; }
        public bool CellVisited { get; set; }
        public bool CellLive { get; set; }
        public bool CellFlagged { get; set; }

        /// Construct a game cell with the given row and column
        /// </summary>
        /// <param name="cellRow"></param>
        /// <param name="cellColumn"></param>
        public GameCellModel(int cellRow, int cellColumn)
        {
            CellRow = cellRow;
            CellColumn = cellColumn;
        }
    }
}
