using System.Collections.Generic;
using UnityEngine;

class Randomizer
{
    public static int[] GetUniqueRandomIntArray(int min, int max, int count) {
        int[] result = new int[count];
        List<int> numbersInOrder = new();
        for (var x = min; x < max; x++) {
            numbersInOrder.Add(x);
        }
        for (var x = 0; x < count; x++) {
            var randomIndex = UnityEngine.Random.Range(0, numbersInOrder.Count);
            result[x] = numbersInOrder[randomIndex];
            numbersInOrder.RemoveAt(randomIndex);
        }

        return result;
    }

    public List<float> GetUniqueRandomFloatArray(float min, float max, int count) {
        float start = UnityEngine.Random.Range(min, min + (max - min) / (10 * count));
        var additives = GetUniqueRandomIntArray(0, 10 * count, count);
        List<float> result = new();

        foreach (var add in additives) {
            result.Add(start + add);
        }

        return result;
    }

    public List<Vector2> GetUniqueRandomVector2Array(Vector2 bounds, int count) {
        var xs = GetUniqueRandomFloatArray(0f, bounds.x, count);
        var ys = GetUniqueRandomFloatArray(0f, bounds.y, count);

        List<Vector2> result = new();

        for (int i = 0; i < count; ++i) {
            result.Add(new(xs[i], ys[i]));
        }

        return result;
    }
}
