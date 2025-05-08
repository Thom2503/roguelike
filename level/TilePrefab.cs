namespace roguelike.level;

public class TilePrefab(string name, string[] asciiLayout, (int x, int y)[] doors, string[] tags)
{
	public string name = name;
	public TileType[,] layout = ParseASCIILayout(asciiLayout);
	public (int x, int y)[] doorPositions = doors;
	public string[] tags = tags;

	public int Width => layout.GetLength(0);
	public int Height => layout.GetLength(1);

	private static TileType[,] ParseASCIILayout(string[] lines) {
		int height = lines.Length;
		int width = lines[0].Length;
		TileType[,] layout = new TileType[width, height];

		for (int y = 0; y < height; y++)
			for (int x = 0; x < width; x++)
				layout[x, y] = CharToTile(lines[y][x]);
		return layout;
	}

	private static TileType CharToTile(char c)
	{
		return c switch
		{
			'#' => TileType.TILE_WALL,
			'.' => TileType.TILE_FLOOR,
			'÷' => TileType.TILE_WATER,
			'è' => TileType.TILE_STATUE,
			',' => TileType.TILE_GRASS,
			((char)24) => TileType.TILE_FLOWER,
			_ => TileType.EMPTY,
		};
	}
}