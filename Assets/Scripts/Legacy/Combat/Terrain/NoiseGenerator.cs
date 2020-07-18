using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// NoiseGenerator generates height maps from different parameters using Perlin Noise algorithm
/// Responsibility : HeightMap generation
/// </summary>
public static class NoiseGenerator
{

    /// <summary>
    /// Generate a 2D float array that represents a height map
    /// </summary>
    /// <param name="size">The size of the map</param>
    /// <param name="scale">The scale of the Perlin Noise</param>
    /// <param name="amplitude">The amplitude of the Perlin Noise</param>
    /// <param name="frequency">The frequency of the Perlin Noise</param>
    /// <param name="octave">The octave of the Perlin Noise</param>
    /// <returns>A 2D float array that represents a height map</returns>
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
