using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseGenerator
{
    public static float[,] GenerateNoiseMap(int size, float scale, float amplitude, float frequency, float octave) {
        float[,] noiseMap = new float[size, size];

        for (int y = 0; y < size; y++) {
            for (int x = 0; x < size; x++) {
                float sampleX = (x + octave) / scale * frequency;
                float sampleY = (y + octave) / scale * frequency;

                noiseMap[x, y] = Mathf.PerlinNoise(sampleX, sampleY) * amplitude - (amplitude / 2);
            }
        }

        return noiseMap;
    }
}
