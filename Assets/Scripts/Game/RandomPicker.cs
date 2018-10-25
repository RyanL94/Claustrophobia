using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Range {
	public int min;
	public int max;
}

public class RandomPicker {

    // Randomly pick an item from the available list.
    public static T Pick<T>(List<T> available) {
        var index = Random.Range(0, available.Count);
        return available[index];
    }

    // Randomly pick items from the available list.
    //
    // The number of items picked is within the given range.
    // The same item cannot be picked twice.
	public static List<T> Pick<T>(List<T> available, Range quantityRange) {
		var chosen = new List<T>();
        var quantity = Random.Range(quantityRange.min, quantityRange.max + 1);
		for (int i = 0; i < quantity; ++i) {
			var index = Random.Range(0, available.Count);
			chosen.Add(available[index]);
			available.RemoveAt(index);
		}
		return chosen;
	}
}