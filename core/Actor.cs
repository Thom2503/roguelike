using System;
using roguelike.items;
using roguelike.action;
using roguelike.render;

namespace roguelike.core;

public class Actor {
	public int x;
	public int y;
	public readonly string name;
	public Inventory inventory;
	public readonly int maxHealth; 
	private int _health;
	public int Health {
		get {
			return _health;
		}
		private set {
			_health = Math.Clamp(value, 0, maxHealth);
		}
	}
	public AsciiTile tile = null;
	public GameAction nextAction = null;

	public Actor(int x, int y) {
		this.x = x;
		this.y = y;
	}
	public Actor(string name, int maxHealth, int x, int y) {
		this.x = x;
		this.y = y;
		this.name = name;
		this.maxHealth = maxHealth;
		this._health = maxHealth;
	}

	public static void Perform(GameAction action) => action.Execute();

	public virtual GameAction GetGameAction() {
		GameAction action = nextAction;
		nextAction = null;
		return action;
	}

	public virtual bool CanMove(int x, int y) => true;
	public virtual bool CanMove(Actor actor) => true;

}