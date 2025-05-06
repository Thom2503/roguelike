namespace roguelike.level;

public class Region {
	public RegionType type;
	public int x, y, width, height;
	public bool isConnected = false;

	public Region(RegionType type, int x, int y, int width, int height) {
		this.type = type;
		this.x = x;
		this.y = y;
		this.width = width;
		this.height = height;
	}

	public (int, int) Center() => (x + width / 2, y + height / 2);
}