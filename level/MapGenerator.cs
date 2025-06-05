using roguelike.render;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using roguelike.core;

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
	private static readonly HashSet<char> _nonWalkableTiles = ['#', 'O', 'è'];

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
							Type = GetTileType(c),
							IsWalkable = !_nonWalkableTiles.Contains(c),
						};
					}
				}
				prefabRooms.Add(new PrefabRoom { x = x, y = y, prefab = prefab});
			}
		}

		tiles = ConnectRooms(tiles);

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

	private AsciiTile[,] ConnectRooms(AsciiTile[,] tiles) {
		for (int i = 1; i < prefabRooms.Count; i++) {
			Random rand = new Random();
			PrefabRoom roomA = prefabRooms[i - 1];
			PrefabRoom roomB = prefabRooms[i];

			int roomADoorLength = roomA.prefab.doorPositions.Length;
			int roomAX = roomA.prefab.doorPositions[rand.Next(roomADoorLength)].x;
			int roomAY = roomA.prefab.doorPositions[rand.Next(roomADoorLength)].y;
			Vector2<int> pointA = new Vector2<int>(roomA.x + roomAX, roomA.y + roomAY);

			int roomBDoorLength = roomB.prefab.doorPositions.Length;
			int roomBX = roomB.prefab.doorPositions[rand.Next(roomBDoorLength)].x;
			int roomBY = roomB.prefab.doorPositions[rand.Next(roomBDoorLength)].y;
			Vector2<int> pointB = new Vector2<int>(roomB.x + roomBX, roomB.y + roomBY);
			tiles = CreateCorridor(pointA, pointB, tiles);
		}
		return tiles;
	}

	private AsciiTile[,] CreateCorridor(Vector2<int> to, Vector2<int> from, AsciiTile[,] tiles) {
		List<Node> openSet = new List<Node>();
		HashSet<Vector2<int>> closedSet = new HashSet<Vector2<int>>();

		openSet.Add(new Node(from, null, 0, Node.Heuristic(from, to)));

		while (openSet.Count > 0) {
			openSet.Sort((a, b) => a.F.CompareTo(b.F));
			Node current = openSet[0];

			if (current.pos == to) {
				Node pathNode = current;
				char c = '.';
				while (pathNode != null) {
					Vector2<int> p = pathNode.pos;
					if (tiles[p.x, p.y].Type == TileType.EMPTY) {
						tiles[p.x, p.y] = new AsciiTile {
							Character = c,
							Foreground = GetForeground(c),
							Background = GetBackground(c),
							Type = TileType.TILE_FLOOR,
							IsWalkable = !_nonWalkableTiles.Contains(c),
						};
					}
					pathNode = pathNode.parent;
				}
				break;
			}

			openSet.Remove(current);
			closedSet.Add(current.pos);

			foreach (Vector2<int> neighbor in Node.GetNeighbors(current.pos)) {
				if (!Node.InBounds(neighbor, _mapWidth, _mapHeight) || closedSet.Contains(neighbor))
					continue;

				if (tiles[neighbor.x, neighbor.y].Type != TileType.EMPTY &&
				    tiles[neighbor.x, neighbor.y].Type != TileType.TILE_FLOOR) // Allow going through corridors
					continue;

				int tentativeG = current.G + 1;
				Node existing = openSet.Find(n => n.pos == neighbor);
				if (existing == null) {
					openSet.Add(new Node(neighbor, current, tentativeG, Node.Heuristic(neighbor, to)));
				} else if (tentativeG < existing.G) {
					existing.G = tentativeG;
					existing.parent = current;
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
		' ' => Color.Black,
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
		' ' => Color.Black,
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
		' ' => TileType.EMPTY,
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
		TileType.EMPTY => ' ', 
		_ => ' '
	};


	private TilePrefab GetRandomPrefab() => PrefabLibrary.tilePrefabs[new Random().Next(PrefabLibrary.tilePrefabs.Count)];

}