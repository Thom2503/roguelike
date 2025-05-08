using System;
using System.Collections.Generic;

namespace roguelike.level;

public class Plan {
	private List<Region> _regions;
	private Random _rand;
	private int _gridWidth = 3;
	private int _gridHeight = 3;

	public Plan() {
		_regions = new List<Region>();
		_rand = new Random();
	}

	public void CreateRegions(int totalWidth, int totalHeight) {
		int cellHeight = totalHeight / _gridHeight;
		int cellWidth = totalWidth / _gridWidth;

		for (int row = 0; row < _gridHeight; row++) {
			for (int col = 0; col < _gridWidth; col++) {
				int maxWidth = cellWidth - 2;
				int maxHeight = cellHeight - 2;

				int width = _rand.Next(maxWidth / 2, maxWidth);
				int height = _rand.Next(maxHeight / 2, maxHeight);

				int cellX = col * cellWidth;
				int cellY = row * cellHeight;

				int offsetX = _rand.Next(1, cellWidth - width - 1);
				int offsetY = _rand.Next(1, cellHeight - height - 1);

				int x = cellX + offsetX;
				int y = cellY + offsetY;

				_regions.Add(new Region(RegionType.ROOM, x, y, width, height));
			}
		}
	}

	public IEnumerable<Region> GetRegions() => _regions;
}