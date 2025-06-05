using roguelike.core;

namespace roguelike.action;

public class WaitAction(Actor actor) : GameAction {
	private readonly Actor _actor = actor;

	public override GameActionResult Execute() {
		return GameActionResult.success;
	}
}