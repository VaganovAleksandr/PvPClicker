using System.Collections.Generic;
using UnityEngine;

class Randomizer
{
    // Generate random unique int numbers on range(min, max) in amount of count (erorr if count exceeds length of range)
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

    // Generate pseudorandom unique float numbers on range(min, max) in amount of count (pseudorandom means
    // that numbers are not truly random, they're distributed discretly with step 1 / (step_coeff * count). step_coeff can
    // be adjusted to generate point more closer)
    public static List<float> GetUniqueRandomFloatArray(float min, float max, int count, int step_coeff=10) {
        float start = UnityEngine.Random.Range(min, min + (max - min) / (step_coeff * count));
        var additives = GetUniqueRandomIntArray(0, step_coeff * count, count);
        List<float> result = new();

        foreach (var add in additives) {
            result.Add(start + add / (step_coeff * count));
        }

        return result;
    }

    // Generate pseudorandom Vector2s, bounded by (0, bounds.x) and (0, bounds.y) rectangle
    public static List<Vector2> GetUniqueRandomVector2Array(Vector2 bounds, int count) {
        var xs = GetUniqueRandomFloatArray(0f, bounds.x, count);
        var ys = GetUniqueRandomFloatArray(0f, bounds.y, count);

        List<Vector2> result = new();

        for (int i = 0; i < count; ++i) {
            result.Add(new(xs[i], ys[i]));
        }

        return result;
    }
}
