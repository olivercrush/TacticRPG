using System;
using GameCore.Terrain;

namespace GameCore
{
    class Program
    {
        static void Main(string[] args)
        {
            Terrain.Terrain terrain = new Terrain.Terrain((10, 10));
            terrain.UpdateCell((5, 5), 3);
            terrain.LogTerrain();
        }
    }
}
