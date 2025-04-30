using roguelike.core;
using roguelike.items;
using roguelike.render;
using Microsoft.Xna.Framework;
using roguelike.action;

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

	public override bool CanMove(int x, int y) {
		return base.CanMove(x, y);
	}

	public override bool CanMove(Actor actor) {
		return base.CanMove(actor);
	}

	public override GameAction GetGameAction() {
		int dx = 0, dy = 0;
		return new MoveAction(dx, dy, this);
	}
}