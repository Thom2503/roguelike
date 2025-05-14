namespace roguelike.core;

public struct Vector2Int {
	public int x;
	public int y;

	public Vector2Int(int x, int y) {
		this.x = x;
		this.y = y;
	}

	public static bool operator ==(Vector2Int a, Vector2Int b) => a.x == b.x && a.y == b.y;

	public static bool operator !=(Vector2Int a, Vector2Int b) => !(a == b);

	public override bool Equals(object obj) => obj is Vector2Int other && this == other;

	public override int GetHashCode() => base.GetHashCode();
}