using roguelike.core;
using roguelike.items;
using roguelike.render;
using Microsoft.Xna.Framework;
using roguelike.action;
using System;

namespace roguelike.ai;

public class Pienus : Actor {
	int playerX, playerY;
	public Pienus(string name, int maxHealth, int x, int y) : base(name, maxHealth, x, y) {
		inventory = new Inventory();
		tile = new AsciiTile {
			Character = 'ã',
			Foreground = Color.Yellow,
			Background = Color.Black,
		};
	}

	public override bool CanMove(int x, int y) {
		if (x == playerX && y == playerY)
			return false;
	
		foreach (var actor in GameLoop.instance.GetActors()) {
			if (actor != this && actor.x == x && actor.y == y)
				return false;
		}
		return true;
	}

	public override bool CanMove(Actor actor) {
		return base.CanMove(actor);
	}

	public override GameAction GetGameAction() {
		int dx = playerX - this.x;
		int dy = playerY - this.y;

		int stepX = Math.Sign(dx);
		int stepY = Math.Sign(dy);

		if (dx != 0 && dy != 0 && CanMove(x + stepX, y + stepY)) {
			return new MoveAction(stepX, stepY, this);
		}

		if (dx != 0 && CanMove(x + stepX, y)) {
			return new MoveAction(stepX, 0, this);
		}

		if (dy != 0 && CanMove(x, y + stepY)) {
			return new MoveAction(0, stepY, this);
		}

		return null;
	}

	public void SetPlayerCoordinates(int x, int y) {
		playerX = x;
		playerY = y;
	}
}