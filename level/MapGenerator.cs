using roguelike.render;
using Microsoft.Xna.Framework;

namespace roguelike.level;

public class MapGenerator {
	private readonly AsciiTile[,] _tiles = new AsciiTile[_mapWidth, _mapHeight]; 
	private const int _mapWidth = 150;
	private const int _mapHeight = 60;

	public MapGenerator() {
		for (int i = 0; i < _tiles.GetLength(0); i++)
			for (int j = 0; j < _tiles.GetLength(1); j++)
				_tiles[i, j] = new AsciiTile {
					Character = 'è',
					Foreground = Color.White,
					Background = Color.DarkSlateGray,
					Type = TileType.TILE_WALL
				};
	}

	public AsciiTile[,] GenerateMap() {
		Plan levelPlan = new Plan();
		levelPlan.CreateRegions(_mapWidth, _mapHeight);
		return _tiles;
	}
}