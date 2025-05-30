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

	//TODO: add entrances
	private static void LoadPrefabs() {
		tilePrefabs.Add(new TilePrefab(
			"Circled Bathhouse",
			[
				"#############",
				"#...........#",
				"#.#.......#.#",
				"#...÷÷÷÷÷...#",
				"#..÷÷÷÷÷÷÷..#",
				"#..÷÷÷÷÷÷÷..#",
				"#..÷÷÷÷÷÷÷..#",
				"#...÷÷÷÷÷...#",
				"#.#.......#.#",
				"#...........#",
				"######.######",
			],
			[(5, 8)],
			["bath", "water", "ruins"]
		));
		tilePrefabs.Add(new TilePrefab(
			"Big Bathhouse",
			[
				"###############",
				"#÷÷÷÷÷÷÷÷÷÷÷÷÷#",
				"#÷÷÷÷÷÷÷÷÷÷÷÷÷#",
				"#÷÷÷÷÷÷÷÷÷÷÷÷÷#",
				"#÷÷÷÷÷÷÷÷÷÷÷÷÷#",
				"#÷÷÷÷÷÷÷÷÷÷÷÷÷#",
				"#÷÷÷÷÷÷÷÷÷÷÷÷÷#",
				"#.....÷÷÷.....#",
				"#...#.....#...#",
				"#.............#",
				"#######.#######",
			],
			[(5, 8)],
			["bath", "water", "ruins"]
		));
		tilePrefabs.Add(new TilePrefab(
			"Centered Bathhouse",
			[
				"#####.#####",
				"#.........#",
				"#.#.....#.#",
				"#...÷÷÷...#",
				"#..÷÷÷÷÷..#",
				"...÷÷÷÷÷...",
				"#..÷÷÷÷÷..#",
				"#...÷÷÷...#",
				"#.#.....#.#",
				"#.........#",
				"#####.#####",
			],
			[(5, 8), (5, 0), (0, 4), (10, 4)],
			["bath", "water"]
		));
		tilePrefabs.Add(new TilePrefab(
			"Lawfully Wild Temple",
			[
				"#############",
				"#...........#",
				"#..#..è..#..#",
				"#...........#",
				"#...........#",
				"#...........#",
				"#..#.....#..#",
				"#...........#",
				"#...........#",
				"#...........#",
				"#..#.....#..#",
				"#...........#",
				"######.######",
			],
			[(5, 8)],
			["temple", "olympians", "wild"]
		));
		tilePrefabs.Add(new TilePrefab(
			"Hidden Warriors Temple",
			[
				"#############",
				"#...........#",
				"#...........#",
				"#.....è.....#",
				"#...........#",
				"#...........#",
				"#..#..#..#..#",
				"#...........#",
				"#..#..#..#..#",
				"#...........#",
				"##.###.####.#",
			],
			[(5, 8), (5, 0), (0, 4), (10, 4)],
			["temple", "warriors", "olympians"]
		));
		tilePrefabs.Add(new TilePrefab(
			"Big Temple",
			[
				"######.######",
				"#...........#",
				"#..#..#..#..#",
				"#...........#",
				"#...........#",
				"#...........#",
				"......è......",
				"#...........#",
				"#...........#",
				"#...........#",
				"#..#..#..#..#",
				"#...........#",
				"######.######",
			],
			[(5, 8)],
			["temple", "olympians", "wild", "warriors", "underworld"]
		));
		tilePrefabs.Add(new TilePrefab(
			"Big Villa",
			[
				"###################",
				"#........#........#",
				"#####....#........#",
				"#...#....#######.##",
				"#...#.............#",
				"#...#.........##.##",
				"#...#.........#...#",
				"#...#.........#...#",
				"#...#.........#...#",
				"#######..#######.##",
				"#.....#..#........#",
				"#.....#..#........#",
				"#######..##########",
			],
			[(5, 8)],
			["villa", "big"]
		));
		string f = ((char)24).ToString();
		tilePrefabs.Add(new TilePrefab(
			"Garden Villa",
			[
				"####################",
				"#..................#",
				"#..................#",
				"#....##########....#",
				"##..##........##..##",
				$"#....#.{f},,{f},,.#....#",
				$"#......,{f},,{f},......#",
				$"#....#.,,,{f},,.#....#",
				$"#......,,{f},{f},......#",
				$"#....#.,{f},,,{f}.#....#",
				"#..................#",
				"####.####...###.####",
				"#......#.....#.....#",
				"#......#.....#.....#",
				"#########...########",
			],
			[(5, 8)],
			["villa", "big", "garden"]
		));
		tilePrefabs.Add(new TilePrefab(
			"Center Catacombs",
			[
				"########...########",
				"#..0.#.......#.0..#",
				"#....#.......#....#",
				"####...........####",
				"#0......000......0#",
				"####...........####",
				"#.................#",
				"#..#..#.....#..#..#",
				"#.0#0.#.....#.0#0.#",
				"########...########",
			],
			[(5, 8)],
			["catacombs", "center"]
		));
		tilePrefabs.Add(new TilePrefab(
			"Room Catacombs",
			[
				"    ##### ##### #####    ",
				"    #.0.# #.0.# #.0.#    ",
				"    #...# #...# #...#    ",
				"######.#####.#####.##    ",
				"#...................#    ",
				"######..............#####",
				"#0.....................0#",
				"######..............#####",
				"#0..................#    ",
				"######..............#####",
				"#0.....................0#",
				"######..............#####",
				"#...................#    ",
				"#...................#    ",
				"########...##########    ",
			],
			[(5, 8)],
			["catacombs", "rooms"]
		));
	}

	public static TilePrefab? GetRandomByTag(string tag) {
		Random random = new Random();
		List<TilePrefab> filtered = tilePrefabs.Where(p => p.tags.Contains(tag)).ToList();
		return filtered.Count > 0 ? filtered[random.Next(0, filtered.Count)] : null;
	}
}