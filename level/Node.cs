using System;
using System.Collections.Generic;
using roguelike.core;

namespace roguelike.level;

public class Node {
	public Vector2Int pos;
	public Node parent;
	public int G;
	public int H;
	public int F => G + H;

	public Node(Vector2Int pos, Node parent, int g, int h) {
		this.pos = pos;
		this.parent = parent;
		this.G = g;
		this.H = h;
	}

	public static List<Vector2Int> GetNeighbors(Vector2Int pos) {
		List<Vector2Int> result = new List<Vector2Int> {
			new Vector2Int(pos.x + 1, pos.y),
			new Vector2Int(pos.x - 1, pos.y),
			new Vector2Int(pos.x, pos.y + 1),
			new Vector2Int(pos.x, pos.y - 1)
		};
		return result;
	}

	public static bool InBounds(Vector2Int pos, int width, int height) {
		return pos.x >= 0 && pos.x < width && pos.y >= 0 && pos.y < height;
	}

	public static int Heuristic(Vector2Int a, Vector2Int b) => Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
}