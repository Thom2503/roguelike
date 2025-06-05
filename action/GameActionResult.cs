namespace roguelike.action;

public class GameActionResult {
	public static readonly GameActionResult success = new GameActionResult(true);
	public static readonly GameActionResult failure = new GameActionResult(false);

	public GameAction alternative { get; }
	public bool Succeeded { get; }

	public GameActionResult(bool succeeded) {
		this.Succeeded = succeeded;
		alternative = null;
	}

	public GameActionResult(GameAction alternative) {
		Succeeded = true;
		this.alternative = alternative;
	}
}