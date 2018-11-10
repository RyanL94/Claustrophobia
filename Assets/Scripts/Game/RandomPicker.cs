using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IntRange {
	public int min;
	public int max;
}

[System.Serializable]
public class FloatRange {
	public float min;
	public float max;
}

public class RandomPicker {

    // Randomly pick an item from the available list.
    public static T Pick<T>(List<T> available, bool remove = false) {
        if (available.Count == 0) {
			return default(T);
		}
		var index = Random.Range(0, available.Count);
		var item = available[index];
		if (remove) {
			available.RemoveAt(index);
		}
        return item;
    }

    // Randomly pick items from the available list.
    //
    // The number of items picked is within the given range.
    // The same item cannot be picked twice.
	public static List<T> Pick<T>(List<T> available, IntRange quantityRange) {
		var availableCopy = new List<T>(available);
		var chosen = new List<T>();
        var quantity = Random.Range(quantityRange.min, quantityRange.max + 1);
		for (int i = 0; i < quantity; ++i) {
			if (availableCopy.Count == 0) {
				break;
			}
			var index = Random.Range(0, availableCopy.Count);
			chosen.Add(availableCopy[index]);
			availableCopy.RemoveAt(index);
		}
		return chosen;
	}
}