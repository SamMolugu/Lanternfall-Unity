using UnityEngine;
using UnityEngine.Tilemaps;

namespace Lanternfall.World
{
    [CreateAssetMenu(menuName = "Lanternfall/World/Biome Tile Kit", fileName = "Biome_")]
    public class BiomeTileKit : ScriptableObject
    {
        public string Id;
        public string DisplayName;
        public TileBase[] FloorTiles;
        public TileBase[] WallFaceTiles;
        public TileBase[] WallFillTiles;
        public Color AmbientLight = new Color(1f, 0.75f, 0.4f);
    }
}
