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
					Character = 'è',
					Foreground = Color.White,
					Background = Color.DarkSlateGray,
					Type = TileType.TILE_WALL
				};
	}

	public AsciiTile[,] GenerateMap() {
		AsciiTile[,] tiles = new AsciiTile[_mapWidth, _mapHeight];

		for (int i = 0; i < tiles.GetLength(0); i++)
			for (int j = 0; j < tiles.GetLength(1); j++)
				tiles[i, j] = new AsciiTile {
					Character = 'è',
					Foreground = Color.White,
					Background = Color.DarkSlateGray,
					Type = TileType.TILE_WALL
				};

		Plan levelPlan = new Plan();
		levelPlan.CreateRegions(_mapWidth, _mapHeight);

		foreach (var region in levelPlan.GetRegions()) {
			var prefab = GetRandomPrefabForRegion(region);

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
		'#' => Color.White,
		'.' => Color.LightGray,
		'~' => Color.LightBlue,
		'è' => Color.LightGray,
		_ => Color.LightGray
	};

	private Color GetBackground(char c) => c switch {
		'#' => Color.DarkSlateGray,
		'.' => Color.Black,
		'~' => Color.DarkBlue,
		'è' => Color.AntiqueWhite,
		_ => Color.Black
	};

	private TileType GetTileType(char c) => c switch {
		'#' => TileType.TILE_WALL,
		'.' => TileType.TILE_FLOOR,
		'~' => TileType.TILE_WATER,
		'è' => TileType.TILE_STATUE,
		_ => TileType.TILE_FLOOR
	};

	private char TileTypeToChar(TileType type) => type switch {
		TileType.TILE_WALL => '#',
		TileType.TILE_FLOOR => '.',
		TileType.TILE_WATER => '~',
		TileType.TILE_STATUE => 'è',
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