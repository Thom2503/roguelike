using System.Collections.Generic;
using roguelike.action;

namespace roguelike.core;

public class GameLoop {
	private List<Actor> _actors = new List<Actor>();
	private int _currentActor = 0;
	public bool running = true;

	public void AddActor(Actor actor) => _actors.Add(actor);

	public void Process() {
		if (_actors.Count == 0) return;
		GameAction action = _actors[_currentActor].GetGameAction();
		if (action == null) return;
		action.Execute();
		_currentActor = (_currentActor + 1) % _actors.Count;
	}

	public IEnumerable<Actor> GetActors() => _actors;
}