using System;
using System.Collections.Generic;
using roguelike.core;

namespace roguelike.level;

public class Node {
	public Vector2<int> pos;
	public Node parent;
	public int G;
	public int H;
	public int F => G + H;

	public Node(Vector2<int> pos, Node parent, int g, int h) {
		this.pos = pos;
		this.parent = parent;
		this.G = g;
		this.H = h;
	}

	public static List<Vector2<int>> GetNeighbors(Vector2<int> pos) {
		List<Vector2<int>> result = new List<Vector2<int>> {
			new Vector2<int>(pos.x + 1, pos.y),
			new Vector2<int>(pos.x - 1, pos.y),
			new Vector2<int>(pos.x, pos.y + 1),
			new Vector2<int>(pos.x, pos.y - 1)
		};
		return result;
	}

	public static bool InBounds(Vector2<int> pos, int width, int height) {
		return pos.x >= 0 && pos.x < width && pos.y >= 0 && pos.y < height;
	}

	public static int Heuristic(Vector2<int> a, Vector2<int> b) => Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
}