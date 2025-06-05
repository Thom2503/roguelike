using Microsoft.Xna.Framework;
using roguelike.level;

namespace roguelike.render;

public class AsciiTile {
	public char Character;
	public Color Foreground;
	public Color Background;
	public TileType Type = TileType.EMPTY;
	public bool IsWalkable = true;
};