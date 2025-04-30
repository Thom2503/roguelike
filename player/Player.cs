using roguelike.core;
using roguelike.items;
using roguelike.render;
using Microsoft.Xna.Framework;
using roguelike.action;
using Microsoft.Xna.Framework.Input;
using System;

namespace roguelike.player;

public class Player : Actor {
	private KeyboardState current, previous;

	public Player(string name, int maxHealth, int x, int y) : base(name, maxHealth, x, y) {
		inventory = new Inventory();
		tile = new AsciiTile {
			Character = '@',
			Foreground = Color.Purple,
			Background = Color.Black,
		};
	}

	public override bool CanMove(int x, int y) {
		return base.CanMove(x, y);
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
        if (IsKeyPressed(Keys.Up)) {
            return new MoveAction(0, -1, this);
        }
        else if (IsKeyPressed(Keys.Down)) {
            return new MoveAction(0, 1, this);
        }
        else if (IsKeyPressed(Keys.Left)) {
            return new MoveAction(-1, 0, this);
        }
        else if (IsKeyPressed(Keys.Right)) {
            return new MoveAction(1, 0, this);
        }
		return null;
	}
}