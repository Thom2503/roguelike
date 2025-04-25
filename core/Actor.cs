using System;
using roguelike.items;
using roguelike.action;

namespace roguelike.core;

public class Actor {
	public int x;
	public int y;
	public readonly string name;
	public Inventory inventory;
	public int maxHealth { get; }
	public int health {
		get {
			return health;
		}
		private set {
			health = Math.Clamp(value, 0, maxHealth);
		}
	}

	public void Perform(GameAction action) => action.Execute(this);

	public virtual bool CanMove(int x, int y) => true;
	public virtual bool CanMove(Actor actor) => true;

}