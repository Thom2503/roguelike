using roguelike.render;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace roguelike.level;

public class MapGenerator {
	private readonly AsciiTile[,] _tiles = new AsciiTile[_mapWidth, _mapHeight]; 
	private const int _mapWidth = 150;
	private const int _mapHeight = 60;

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

		Plan levelPlan = new Plan();
		levelPlan.CreateRegions(_mapWidth, _mapHeight);

		foreach (Region region in levelPlan.GetRegions()) {
			TilePrefab prefab = GetRandomPrefabForRegion(region);

			if (prefab == null || prefab.Width > region.width || prefab.Height > region.height)
				continue;

			int offsetX = region.x + (region.width - prefab.Width) / 2;
			int offsetY = region.y + (region.height - prefab.Height) / 2;

			for (int y = 0; y < prefab.Height; y++) {
				for (int x = 0; x < prefab.Width; x++) {
					TileType tile = prefab.layout[x, y];
					char c = TileTypeToChar(tile);

					tiles[offsetX + x, offsetY + y] = new AsciiTile {
						Character = c,
						Foreground = GetForeground(c),
						Background = GetBackground(c),
						Type = GetTileType(c)
					};
				}
			}
		}

		return tiles;
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


	private TilePrefab GetRandomPrefabForRegion(Region region) {
		List<TilePrefab> validPrefabs = PrefabLibrary.tilePrefabs
			.Where(p => p.Width <= region.width && p.Height <= region.height)
			.ToList();

		if (validPrefabs.Count == 0)
			return null;

		return validPrefabs[new Random().Next(validPrefabs.Count)];
	}

}