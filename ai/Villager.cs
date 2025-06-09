using roguelike.core;
using roguelike.render;
using Microsoft.Xna.Framework;
using roguelike.action;
using System;
using roguelike.level;
using roguelike.player;

#nullable enable

namespace roguelike.ai;

//TODO: Eventually want to make it that they have goals and habits like villagers in minecraft
//      maybe even add some sort of alligment with stores etcetera
public class Villager : Actor {
	public static readonly Color mForeground = Color.SteelBlue;
	public static readonly Color mBackground = Color.SaddleBrown;
	public static readonly Color fForeground = Color.MediumOrchid;
	public static readonly Color fBackground = Color.SaddleBrown;

	private readonly Random rand = new Random();

	private readonly int _beginX;
	private readonly int _beginY;

	private int _movesMade = 0;
	private const int _maxMovesMade = 5;
	private bool _wanderBack = false;
	private Vector2<int>? _dest = null;

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
		if (_dest == null) {
			if (_wanderBack) {
				_dest = new Vector2<int>(_beginX, _beginY);
			} else {
				int destX, destY;
				// weird fix but avoids getting stuck at point zero (ie _beginX and _beginY)
				do {
					destX = x + rand.Next(-5, 6);
					destY = y + rand.Next(-5, 6);
				} while (destX == x && destY == y);
				_dest = new Vector2<int>(destX, destY);
			}
		}

		int dx = _dest.Value.x - this.x;
		int dy = _dest.Value.y - this.y;

		int stepX = Math.Sign(dx);
		int stepY = Math.Sign(dy);

		if (dx == 0 && dy == 0) {
			IncrementMoves();
			_dest = null;
			return new WaitAction(this);
		}

		if (dx != 0 && dy != 0) {
			if (CanMove(x + stepX, y + stepY)) {
				IncrementMoves();
				return new MoveAction(stepX, stepY, this);
			}
		}
	
		if (Math.Abs(dx) > Math.Abs(dy) && CanMove(x + stepX, y)) {
			IncrementMoves();
			return new MoveAction(stepX, 0, this);
		}
	
		if (Math.Abs(dy) > Math.Abs(dx) && CanMove(x, y + stepY)) {
			IncrementMoves();
			return new MoveAction(0, stepY, this);
		}

		return new WaitAction(this);
	}

	private void IncrementMoves() {
		_movesMade++;
		if (_wanderBack && x == _beginX && y == _beginY) {
			_wanderBack = false;
			_movesMade = 0;
		} else if (!_wanderBack && _movesMade >= _maxMovesMade) {
			_wanderBack = true;
			_movesMade = 0;
		}
	}
}