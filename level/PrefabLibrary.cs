using System.Collections.Generic;
using System.Linq;
using System;

#nullable enable

namespace roguelike.level;

public static class PrefabLibrary {
	public static readonly List<TilePrefab> tilePrefabs = new List<TilePrefab>();

	static PrefabLibrary() {
		LoadPrefabs();
	}

	private static void LoadPrefabs() {

	}

	public static TilePrefab? GetRandomByTag(string tag) {
		Random random = new Random();
		List<TilePrefab> filtered = tilePrefabs.Where(p => p.tags.Contains(tag)).ToList();
		return filtered.Count > 0 ? filtered[random.Next(0, filtered.Count)] : null;
	}
}