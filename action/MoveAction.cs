using roguelike.core;

namespace roguelike.action;

public class MoveAction : GameAction {
	public int dx;
	public int dy;
	public Actor actor;

	public MoveAction(int dx, int dy, Actor actor) {
		this.dx = dx;
		this.dy = dy;
		this.actor = actor;
	}

	public override void Execute() {
		if (actor.CanMove(actor.x + dx, actor.y + dy)) {
			actor.x += dx;
			actor.y += dy;
		}
	}
}