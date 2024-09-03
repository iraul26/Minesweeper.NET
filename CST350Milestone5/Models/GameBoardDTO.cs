using System.Globalization;

namespace CST350Milestone5.Models
{
    public class GameBoardDTO
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public DateTime SaveDate { get; set; }

        public GameBoardDTO(int id, DateTime saveDate)
        {
            Id = id;
            SaveDate = saveDate;
            Date = $"{CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(saveDate.Month)} {saveDate.Day}, {saveDate.Year} @ {saveDate.ToShortTimeString()}";
        }
    }
}
