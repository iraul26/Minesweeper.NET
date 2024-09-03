using CST350Milestone5.Services;
using Newtonsoft.Json;
using System.Text;

namespace CST350Milestone5.Models
{
	public class GameBoardModel
	{
		public int Id { get; set; }
		public int Height { get; set; }
		public int Width { get; set; }
		public double Difficulty { get; set; }
		public GameCellModel[][] CellGrid { get; set; }

		public GameBoardModel(int id, int height, int width, double difficulty, GameCellModel[][] cellGrid)
		{
			Id = id;
			Height = height;
			Width = width;
			Difficulty = difficulty;
            CellGrid = cellGrid;
		}

		public string Serialize()
		{
			return JsonConvert.SerializeObject(this);
		}

        public static GameBoardModel? Deserialize(string data)
		{
			return JsonConvert.DeserializeObject<GameBoardModel>(data);
        }
	}
}