namespace roguelike.core;

public class WordFreq {
	public int CountBeginning { get; private set; }
	public int CountEnd { get; private set; }
	public int CountWithin { get; set; }

	public WordFreq() {
		CountBeginning = 0;
		CountEnd = 0;
		CountWithin = 0;
	}

	public void IncrementCountBeginning() => CountBeginning++;
	public void IncrementCountEnd() => CountEnd++;
	public void IncrementCountWithin() => CountWithin++;
}