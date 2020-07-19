using ShipCore.Terrain;

namespace ShipCore
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Terrain.Terrain terrain = new Terrain.Terrain((10, 10));
            terrain.UpdateCell((5, 5), 3, HeightUpdateMethod.SET);
            terrain.LogTerrain();
        }
    }
}