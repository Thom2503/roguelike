using roguelike.core;
using roguelike.items;
using roguelike.render;
using Microsoft.Xna.Framework;

namespace roguelike.player;

public class Player : Actor {
	public Player(string name, int maxHealth, int x, int y) : base(name, maxHealth, x, y) {
		this.inventory = new Inventory();
		this.tile = new AsciiTile {
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
}