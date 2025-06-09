using roguelike.core;
using roguelike.render;
using Microsoft.Xna.Framework;
using roguelike.action;
using System;
using roguelike.level;
using roguelike.player;

#nullable enable

namespace roguelike.ai;

public class Villager : Actor {
	public static readonly Color mForeground = Color.SteelBlue;
	public static readonly Color mBackground = Color.SaddleBrown;
	public static readonly Color fForeground = Color.MediumOrchid;
	public static readonly Color fBackground = Color.SaddleBrown;

	private readonly Random rand = new Random();

	private readonly int _beginX;
	private readonly int _beginY;

	private static int _movesMade = 0;
	private const int _maxMovesMade = 5;
	private static bool _wanderBack = false;

	public Villager(string name, int maxHealth, int x, int y) : base(name, maxHealth, x, y) {
		bool gender = rand.Next(0, 100) < 50; // 50 50 chance of being a woman or man
		(Color, Color) colours = GenderColours(gender);
		tile = new AsciiTile {
			Character = GenderChar(gender),
			Foreground = colours.Item1,
			Background = colours.Item2,
			Type = GenderTileType(gender),
		};
		_beginX = x;
		_beginY = y;
	}

	private static char GenderChar(bool gender) => gender ? ((char)11) : ((char)12);
	private static (Color, Color) GenderColours(bool gender) => gender ? (mForeground, mBackground) : (fForeground, mForeground);
	private static TileType GenderTileType(bool gender) => gender ? TileType.TILE_VILLAGER_MALE : TileType.TILE_VILLAGER_FEMALE;

	public override bool CanMove(int x, int y) {
		return base.CanMove(x, y);
	}

	private static bool CanEnterTile(int x, int y) => Engine.tileMap[x, y].IsWalkable;

	private bool GetActorAt(int x, int y) {
		foreach (var actor in GameLoop.instance.GetActors()) {
			if (actor != this && (actor is not Pienus || actor is not Player) && actor.x == x && actor.y == y)
				return false;
		}
		return true;
	}

	public override GameAction GetGameAction() {
		if (_movesMade <= _maxMovesMade) {
			int destX = _wanderBack ? _beginX : x + rand.Next(10);
			int destY = _wanderBack ? _beginY : y + rand.Next(10);
			Vector2<int> dest = new Vector2<int>(destX, destY);
			Vector2<int> from = new Vector2<int>(x, y);

			_movesMade++;
		}
		_wanderBack = true;
		_movesMade = 0;
		return new WaitAction(this);
	}
}