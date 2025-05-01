namespace roguelike.action;

public class GameActionResult {
	public static readonly GameActionResult success = new GameActionResult(true);
	public static readonly GameActionResult failure = new GameActionResult(false);

	public GameAction alternative { get; }
	public bool succeeded { get; }

	public GameActionResult(bool succeeded) {
		this.succeeded = succeeded;
		alternative = null;
	}

	public GameActionResult(GameAction alternative) {
		succeeded = true;
		this.alternative = alternative;
	}
}