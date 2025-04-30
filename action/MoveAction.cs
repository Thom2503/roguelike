using roguelike.core;

namespace roguelike.action;

public class MoveAction(int dx, int dy, Actor actor) : GameAction {
	public int dx = dx;
	public int dy = dy;
	public Actor actor = actor;

	public override void Execute() {
		if (actor.CanMove(actor.x + dx, actor.y + dy)) {
			actor.x += dx;
			actor.y += dy;
		}
	}
}