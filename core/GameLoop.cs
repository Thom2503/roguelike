using System.Collections.Generic;
using roguelike.action;

namespace roguelike.core;

public class GameLoop {
	private List<Actor> _actors = new List<Actor>();
	private int _currentActor = 0;

	public static GameLoop instance;

	public void AddActor(Actor actor) => _actors.Add(actor);

	public void Process() {
		while (true) {
			if (_actors.Count == 0) break;
			GameAction action = _actors[_currentActor].GetGameAction();
			if (action == null) break;
			action.Execute();
			_currentActor = (_currentActor + 1) % _actors.Count;
		}
		return;
	}

	public IEnumerable<Actor> GetActors() => _actors;
}