using roguelike.core;
using roguelike.items;
using roguelike.render;
using Microsoft.Xna.Framework;
using roguelike.action;
using Microsoft.Xna.Framework.Input;
using roguelike.ai;

namespace roguelike.player;

public class Player : Actor {
	private KeyboardState current, previous;

	public AsciiTile[,] tiles;

	public Player(string name, int maxHealth, int x, int y) : base(name, maxHealth, x, y) {
		inventory = new Inventory();
		tile = new AsciiTile {
			Character = '@',
			Foreground = Color.Purple,
			Background = Color.Black,
		};
	}

	public override bool CanMove(int x, int y) {
		if (tiles == null)
			return false;
		if (!CanEnterTile(x, y))
			return false;
		if (!GetActorAt(x, y))
			return false;
		return true;
	}

	public override bool CanMove(Actor actor) {
		return base.CanMove(actor);
	}

	public void SetInput(KeyboardState current, KeyboardState prev) {
		this.current = current;
		previous = prev;
	}

	private bool IsKeyPressed(Keys key) => current.IsKeyDown(key) && !previous.IsKeyDown(key);

	public override GameAction GetGameAction() {
		if ((IsKeyPressed(Keys.Up) || IsKeyPressed(Keys.K)) && CanMove(x, y - 1)) {
			return new MoveAction(0, -1, this);
		}
		else if ((IsKeyPressed(Keys.Down) || IsKeyPressed(Keys.J)) && CanMove(x, y + 1)) {
			return new MoveAction(0, 1, this);
		}
		else if ((IsKeyPressed(Keys.Left) || IsKeyPressed(Keys.H)) && CanMove(x - 1, y)) {
			return new MoveAction(-1, 0, this);
		}
		else if ((IsKeyPressed(Keys.Right) || IsKeyPressed(Keys.L)) && CanMove(x + 1, y)) {
			return new MoveAction(1, 0, this);
		}
		return null;
	}

	private bool CanEnterTile(int x, int y) => tiles[x, y].IsWalkable;

	private bool GetActorAt(int x, int y) {
		foreach (var actor in GameLoop.instance.GetActors()) {
			if (actor != this && !(actor is Pienus) && actor.x == x && actor.y == y)
				return false;
		}
		return true;
	}
}