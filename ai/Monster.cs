using roguelike.core;
using roguelike.items;
using roguelike.render;
using Microsoft.Xna.Framework;
using System;
using System.Security.Cryptography;
using roguelike.action;

namespace roguelike.ai;

public class Monster : Actor {
	public bool hasMoved = true;
	public Monster(string name, int maxHealth, int x, int y) : base(name, maxHealth, x, y) {
		this.inventory = new Inventory();
		this.tile = new AsciiTile {
			Character = 'M',
			Foreground = Color.Red,
			Background = Color.Black,
		};
	}

	public override bool CanMove(int x, int y) {
		return base.CanMove(x, y);
	}

	public override bool CanMove(Actor actor) {
		return base.CanMove(actor);
	}

	public override GameAction GetGameAction() {
		(int dx, int dy) = GetRandomMovement();
		return new MoveAction(dx, dy, this);
	}

	public Tuple<int, int> GetRandomMovement() {
		Random rnd = new Random();
		int moveX = rnd.Next(-1, 2);
		int moveY = rnd.Next(-1, 2);

		int newX = this.x + moveX;
		int newY = this.y + moveY;
		if (newX > 20)  newX = this.x;
		if (newX > 20)  newY = this.y;

		newX = Math.Clamp(newX, 0, 74);
		newY = Math.Clamp(newY, 0, 29);
		return new Tuple<int, int>(newX, newY);
	}
}