using UnityEngine;
using System.Collections.Generic;

public class ControlledRandom {
    private List<int> availableIndices;
    private int lastIndex = -1;
    private int removedIndex = -1;
    private int sameIndexCounter = 0;
    private int repetitions;

    public ControlledRandom(int highestIndex, int maxRepetition) {
        Debug.Assert(highestIndex >= 1, "You must set HighestIndex to a value greater than 0");
        Debug.Assert(maxRepetition >= 1, "You must set MaxRepetition to a value greater than 0");
        availableIndices = new List<int>();
        for (var i = 0; i <= highestIndex; ++i) {
            availableIndices.Add(i);
        }
        repetitions = maxRepetition;
    }

    public int NextIndex() {
        var index = availableIndices[Random.Range(0, availableIndices.Count - 1)];
        if (index == lastIndex) {
            sameIndexCounter++;
        }

        lastIndex = index;
        // Always be sure to only instantiate two of the same object in a row
        // by set aside indices that has occured twice
        if (sameIndexCounter == repetitions) {
            removedIndex = lastIndex;
            availableIndices.Remove(lastIndex);
            sameIndexCounter = 0;
        }

        // Add the removedIndex to the availableIndices list when an index diffirent from removeIndex has been used
        if (removedIndex != -1 && removedIndex != lastIndex && availableIndices.Contains(removedIndex) == false) {
            availableIndices.Add(removedIndex);
        }
        return index;
    }
}