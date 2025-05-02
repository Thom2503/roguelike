using roguelike.core;

namespace roguelike.action;

public class WaitAction : GameAction {
	public WaitAction(Actor actor) {}
	public override GameActionResult Execute() {
		return GameActionResult.success;
	}
}