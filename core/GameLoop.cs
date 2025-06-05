using System.Collections.Generic;
using roguelike.action;
using roguelike.player;

namespace roguelike.core;

public class GameLoop {
	private readonly List<Actor> _actors = [];
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
			if (result.Succeeded || !(actor is Player)) {
				_currentActor = (_currentActor + 1) % _actors.Count;
			}
			break;
		}
	}

	public IEnumerable<Actor> GetActors() => _actors;
}