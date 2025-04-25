using roguelike.core;

namespace roguelike.action;

public abstract class GameAction {
	public abstract void Execute(Actor actor);
}