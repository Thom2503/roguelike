using roguelike.core;
using roguelike.player;

namespace roguelike.action;

public class MoveAction(int dx, int dy, Actor actor) : GameAction {
	public int dx = dx;
	public int dy = dy;
	public Actor actor = actor;

	public override GameActionResult Execute() {
		int targetX = actor.x + dx;
		int targetY = actor.y + dy;

		foreach (Actor other in GameLoop.instance.GetActors()) {
			if (other != actor && other.x == targetX && other.y == targetY) {
				if (actor is Player) {
					return SkipTiles();
				}
				return GameActionResult.failure;
			}
		}

		if (!actor.CanMove(targetX, targetY)) {
			return GameActionResult.failure;
		}
		actor.x = targetX;
		actor.y = targetY;
		return GameActionResult.success;
	}

	private GameActionResult SkipTiles() {
		int skippedX = actor.x + dx * 3;
		int skippedY = actor.y + dy * 3;

		bool blocked = false;
		foreach (Actor a in GameLoop.instance.GetActors()) {
			if (a != actor && a.x == skippedX && a.y == skippedY) {
				blocked = true;
				break;
			}
		}

		if (!blocked && actor.CanMove(skippedX, skippedY)) {
			actor.x = skippedX;
			actor.y = skippedY;
			return GameActionResult.success;
		}
		return GameActionResult.failure;
	}
}