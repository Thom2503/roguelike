using roguelike.core;
using roguelike.items;
using roguelike.render;
using Microsoft.Xna.Framework;
using roguelike.action;
using System;

#nullable enable

namespace roguelike.ai;

public class Pienus : Actor {
	public Pienus(string name, int maxHealth, int x, int y) : base(name, maxHealth, x, y) {
		inventory = new Inventory();
		tile = new AsciiTile {
			Character = 'ã',
			Foreground = Color.Yellow,
			Background = Color.Black,
		};
	}

	private Actor? GetPlayer() {
		foreach (var actor in GameLoop.instance.GetActors()) {
			if (actor is roguelike.player.Player) {
				return actor;
			}
		}
		return null;
	}

	public override bool CanMove(int x, int y) {
		foreach (var actor in GameLoop.instance.GetActors()) {
			if (actor != this && actor.x == x && actor.y == y)
				return false;
		}
		return true;
	}

	public override GameAction GetGameAction() {
		Actor? player = GetPlayer();
		if (player == null) return new WaitAction(this);

		int dx = player.x - this.x;
		int dy = player.y - this.y;

		int stepX = Math.Sign(dx);
		int stepY = Math.Sign(dy);

		if (dx != 0 && dy != 0) {
			if (CanMove(x + stepX, y + stepY)) {
				return new MoveAction(stepX, stepY, this);
			}
		}

		if (Math.Abs(dx) > Math.Abs(dy)) {
			if (dx != 0 && CanMove(x + stepX, y)) {
				return new MoveAction(stepX, 0, this);
			}
		}

		if (Math.Abs(dy) > Math.Abs(dx)) {
			if (dy != 0 && CanMove(x, y + stepY)) {
				return new MoveAction(0, stepY, this);
			}
		}

		return new WaitAction(this);
	}
}