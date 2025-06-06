"""
RandomName Generator test
"""
import random
import time
from collections import defaultdict
from typing import DefaultDict, List


class WordFreq():
    """_summary_
    """
    _count_beginning: int
    _count_end: int
    _count_within: int

    def __init__(self) -> None:
        self._count_beginning = 0
        self._count_end = 0
        self._count_within = 0

    def increment_count_beginning(self) -> None:
        """dwadwad
        """
        self._count_beginning += 1

    def increment_count_end(self) -> None:
        """dwadawd
        """
        self._count_end += 1

    def increment_count_within(self) -> None:
        """dwadawd
        """
        self._count_within += 1

    def return_count_beginning(self) -> int:
        """dwadawd
        """
        return self._count_beginning

    def return_count_end(self) -> int:
        """dawdaw
        """
        return self._count_end

    def return_count_within(self) -> int:
        """dwadw
        """
        return self._count_within


class RandomName():
    """dwadwa
    """
    _base_map: DefaultDict
    _start_chars: List[str]
    _available_chars: List[str]
    _file_path: str

    def __init__(self) -> None:
        random.seed(time.time())
        self._base_map = defaultdict(lambda: defaultdict(WordFreq))
        self._start_chars = []
        self._available_chars = []

    def input_file(self, file_path: str) -> None:
        """dwadaw
        """
        self._file_path = file_path

    def process_file(self, no_whitespace_skip: bool = False) -> None:
        """dawdawd
        """
        start_chars_set = set()
        available_chars_set = set()

        with open(self._file_path, "r", encoding="utf-8") as file:
            lines = file.readlines()

        for line in lines:
            if no_whitespace_skip:
                word = line.strip().upper()
            else:
                word = "".join(line.split()).upper()
            if len(word) > 1:
                start_chars_set.add(word[0])
                available_chars_set.add(word[0])
                for i in range(len(word) - 1):
                    base = word[i]
                    sequence = word[i + 1]
                    wf = self._base_map[base][sequence]

                    available_chars_set.add(sequence)

                    if i == 0:
                        wf.increment_count_beginning()
                    elif i + 1 >= len(word) - 1:
                        wf.increment_count_end()
                    else:
                        wf.increment_count_within()

        self._start_chars = list(start_chars_set)
        self._available_chars = list(available_chars_set)

    def output_list(self, file_path: str) -> None:
        """dawdawd
        """
        with open(file_path, "w", encoding="utf-8") as out:
            for base, sequence_map in self._base_map.items():
                for sequence, wf in sequence_map.items():
                    out.write(f"{base} {sequence} {wf.return_count_beginning()} "
                              f"{wf.return_count_within()} {wf.return_count_end()}\n")

    def output_name(self, min_length: int, max_length: int) -> str:
        """dawdawd
        """
        name = ''
        length = random.randint(int(min_length), int(max_length))

        if not self._start_chars:
            raise RuntimeError("No start characters available. Did you process a valid file?")

        current_char = random.choice(self._start_chars)
        name += current_char

        for i in range(1, length):
            freq_vector = []
            cdc = 0
            if current_char in self._base_map:
                for next_char in self._available_chars:
                    if next_char in self._base_map[current_char]:
                        wf = self._base_map[current_char][next_char]
                        if i == 1:
                            freq_vector.extend([next_char] * wf.return_count_beginning())
                        elif i + 1 >= length - 1:
                            freq_vector.extend([next_char] * wf.return_count_end())
                        else:
                            freq_vector.extend([next_char] * wf.return_count_within())
            if not freq_vector:
                break
            for _ in range(3):
                random.shuffle(freq_vector)
            next_char = random.choice(freq_vector)
            name += next_char
            current_char = next_char

        return name


def main():
    """
    Main function
    """
    random_name = RandomName()
    random_name.input_file("prototyping/namegen/names.txt")
    random_name.process_file(no_whitespace_skip=True)
    random_name.output_list("prototyping/namegen/name_stats.txt")
    for _ in range(5):
        print(random_name.output_name(5, 8))


if __name__ == "__main__":
    main()
