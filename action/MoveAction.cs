using roguelike.core;

namespace roguelike.action;

public class MoveAction(int dx, int dy, Actor actor) : GameAction {
	public int dx = dx;
	public int dy = dy;
	public Actor actor = actor;

	public override GameActionResult Execute() {
		int targetX = actor.x + dx;
		int targetY = actor.y + dy;
		if (!actor.CanMove(targetX, targetY)) {
			return GameActionResult.failure;
		}
		actor.x = targetX;
		actor.y = targetY;
		return GameActionResult.success;
	}
}