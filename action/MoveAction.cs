using roguelike.core;

namespace roguelike.action;

public class MoveAction : GameAction {
	public int dx;
	public int dy;

	public MoveAction(int dx, int dy) {
		this.dx = dx;
		this.dy = dy;
	}

	public override void Execute(Actor actor) {
		if (actor.CanMove(actor.x + dx, actor.y + dy)) {
			actor.x += dx;
			actor.y += dy;
		}
	}
}