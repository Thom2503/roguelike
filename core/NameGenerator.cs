using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace roguelike.core;

public class NameGenerator {
	private readonly Dictionary<char, Dictionary<char, WordFreq>> _baseMap;
	private List<char> _startChars;
	private List<char> _availableChars;
	private string _filePath;

	public NameGenerator() {
		Random rand = new Random((int)DateTime.Now.Ticks);
		_baseMap = new Dictionary<char, Dictionary<char, WordFreq>>();
		_startChars = new List<char>();
		_availableChars = new List<char>();
	}

	public void InputFile(string filePath) => _filePath = filePath;

	public void ProcessFile(bool noWhitespaceSkip = false) {
		HashSet<char> startCharsSet = new HashSet<char>();
		HashSet<char> availableCharsSet = new HashSet<char>();

		string text = File.ReadAllText(_filePath);
		string[] words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);

		foreach (string line in words) {
			string word = noWhitespaceSkip ? line.Trim().ToUpper() : string.Concat(line.Where(c => !char.IsWhiteSpace(c))).ToUpper();
			if (word.Length <= 1) continue;

			startCharsSet.Add(word[0]);
			availableCharsSet.Add(word[0]);

			for (int i = 0; i < word.Length - 1; i++) {
				char baseChar = word[i];
				char sequenceChar = word[i + 1];

				if (!_baseMap.ContainsKey(baseChar)) _baseMap[baseChar] = [];
				if (!_baseMap[baseChar].ContainsKey(sequenceChar)) _baseMap[baseChar][sequenceChar] = new WordFreq();

				availableCharsSet.Add(sequenceChar);
				WordFreq wf = _baseMap[baseChar][sequenceChar];

				if (i == 0)
					wf.IncrementCountBeginning();
				else if (i + 1 >= word.Length - 1)
					wf.IncrementCountEnd();
				else
					wf.IncrementCountWithin();
			}
		}

		_startChars = [.. startCharsSet];
		_availableChars = [.. availableCharsSet];
	}

	public void OutputList(string filePath) {
		using StreamWriter writer = new StreamWriter(filePath);
		foreach (var basePair in _baseMap) {
			foreach (var seqPair in basePair.Value) {
				WordFreq wf = seqPair.Value;
				writer.WriteLine($"{basePair.Key} {seqPair.Key} {wf.CountBeginning} " +
				                 $"{wf.CountWithin} {wf.CountEnd}");
			}
		}
	}

	public string OutputName(int minLength = 3, int maxLength = 8) {
		if (_startChars.Count == 0)
			throw new InvalidOperationException("No start characters available. Did you process a valid file?");

		Random rand = new Random();
		int length = rand.Next(minLength, maxLength + 1);

		char curChar = _startChars[rand.Next(_startChars.Count)];
		string name = curChar.ToString();

		for (int i = 1; i < length; i++) {
			List<char> freqVec = new List<char>();

			if (_baseMap.ContainsKey(curChar)) {
				foreach (char nextChar in _availableChars) {
					if (_baseMap[curChar].TryGetValue(nextChar, out WordFreq value)) {
						WordFreq wf = value;
						int count = i == 1 ? wf.CountBeginning : (i + 1 >= length - 1 ? wf.CountEnd : wf.CountWithin);

						for (int c = 0; c < count; c++) {
							freqVec.Add(nextChar);
						}
					}
				}
			}

			if (freqVec.Count == 0)
				break;

			for (int s = 0; s < 3; s++)
				freqVec = Shuffle(freqVec);

			char next = freqVec[rand.Next(freqVec.Count)];
			name += next;
			curChar = next;
		}

		return name;
	}

	private static List<T> Shuffle<T>(List<T> lst) {
		Random rand = new Random();
		int n = lst.Count;

		for (int i = n - 1; i > 0; i--) {
			int j = rand.Next(i + 1);
			(lst[i], lst[j]) = (lst[j], lst[i]);
		}

		return lst;
	}
}