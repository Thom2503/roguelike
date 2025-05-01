using System.Collections.Generic;
using roguelike.action;

namespace roguelike.core;

public class GameLoop {
	private List<Actor> _actors = new List<Actor>();
	private int _currentActor = 0;

	public static GameLoop instance;

	public void AddActor(Actor actor) => _actors.Add(actor);

	public void Process() {
		if (_actors.Count == 0) return;
		Actor actor = _actors[_currentActor];
		GameAction action = actor.GetGameAction();

		int maxAttempts = 10;
		int attempts = 0;

		while (action != null && attempts++ < maxAttempts) {
			GameActionResult result = action.Execute();
			if (result.alternative != null) {
				action = result.alternative;
				continue;
			}
			if (result.succeeded) {
				_currentActor = (_currentActor + 1) % _actors.Count;
			}
			break;
		}
	}

	public IEnumerable<Actor> GetActors() => _actors;
}