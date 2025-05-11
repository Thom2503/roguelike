using roguelike.render;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace roguelike.level;

public class MapGenerator {

	public struct PrefabRoom {
		public int x;
		public int y;
		public TilePrefab prefab;
	};

	private readonly AsciiTile[,] _tiles = new AsciiTile[_mapWidth, _mapHeight]; 
	private List<PrefabRoom> prefabRooms = new List<PrefabRoom>();
	private const int _mapWidth = 150;
	private const int _mapHeight = 60;
	private const int _maxAttempts = 100;

	public MapGenerator() {
		for (int i = 0; i < _tiles.GetLength(0); i++)
			for (int j = 0; j < _tiles.GetLength(1); j++)
				_tiles[i, j] = new AsciiTile {
					Character = ' ',
					Foreground = Color.Black,
					Background = Color.Black,
					Type = TileType.EMPTY
				};
	}

	public AsciiTile[,] GenerateMap() {
		AsciiTile[,] tiles = new AsciiTile[_mapWidth, _mapHeight];

		for (int i = 0; i < tiles.GetLength(0); i++)
			for (int j = 0; j < tiles.GetLength(1); j++)
				tiles[i, j] = new AsciiTile {
					Character = ' ',
					Foreground = Color.Black,
					Background = Color.Black,
					Type = TileType.EMPTY
				};

		for (int attempt = 0; attempt < _maxAttempts; attempt++) {
			TilePrefab prefab = GetRandomPrefab();
			Random rand = new Random();
			int x = rand.Next(0, _mapWidth - prefab.Width);
			int y = rand.Next(0, _mapHeight - prefab.Height);

			if (!DoesRoomOverlap(x, y, prefab)) {
				for (int i = 0; i < prefab.Height; i++) {
					for (int j = 0; j < prefab.Width; j++) {
						TileType tile = prefab.layout[j, i];
						char c = TileTypeToChar(tile);
	
						tiles[x + j, y + i] = new AsciiTile {
							Character = c,
							Foreground = GetForeground(c),
							Background = GetBackground(c),
							Type = GetTileType(c)
						};
					}
				}
				prefabRooms.Add(new PrefabRoom { x = x, y = y, prefab = prefab});
			}
		}

		return tiles;
	}

	private bool DoesRoomOverlap(int x, int y, TilePrefab prefab) {
		foreach (PrefabRoom room in prefabRooms) {
			if (x + prefab.Width > room.x && x < room.x + room.prefab.Width &&
			    y + prefab.Height > room.y && y < room.y + room.prefab.Height) {
				return true;
			}
		}
		return false;
	}

	private Color GetForeground(char c) => c switch {
		'#' => new Color(170, 170, 170),
		'.' => new Color(130, 110, 100),
		'~' => new Color(80, 120, 170),
		'è' => new Color(120, 130, 100),
		',' => new Color(80, 150, 90),
		'0' => new Color(160, 120, 100),
		(char)24 => new Color(220, 100, 160),
		_ => new Color(130, 110, 100)
	};

	private Color GetBackground(char c) => c switch {
		'#' => new Color(40, 40, 40),
		'.' => new Color(25, 20, 20),
		'~' => new Color(20, 30, 60),
		'è' => new Color(40, 45, 30),
		',' => new Color(30, 50, 30),
		'0' => new Color(50, 35, 30),
		((char)24) => new Color(45, 30, 40),
		_ => new Color(20, 20, 20)
	};

	private TileType GetTileType(char c) => c switch {
		'#' => TileType.TILE_WALL,
		'.' => TileType.TILE_FLOOR,
		'~' => TileType.TILE_WATER,
		'è' => TileType.TILE_STATUE,
		',' => TileType.TILE_GRASS,
		'0' => TileType.TILE_TOMB,
		((char)24) => TileType.TILE_FLOWER,
		_ => TileType.TILE_FLOOR
	};

	private char TileTypeToChar(TileType type) => type switch {
		TileType.TILE_WALL => '#',
		TileType.TILE_FLOOR => '.',
		TileType.TILE_WATER => '~',
		TileType.TILE_STATUE => 'è',
		TileType.TILE_GRASS => ',',
		TileType.TILE_TOMB => '0',
		TileType.TILE_FLOWER => ((char)24),
		_ => ' '
	};


	private TilePrefab GetRandomPrefab() => PrefabLibrary.tilePrefabs[new Random().Next(PrefabLibrary.tilePrefabs.Count)];

}