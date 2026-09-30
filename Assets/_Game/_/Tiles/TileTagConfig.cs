using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

    // Конфиг хранящий тип тайла
    [CreateAssetMenu(fileName = "TileTagConfig", menuName = "Data/TileTagConfig", order = 81)]
    public class TileTagConfig : ScriptableObject
    {
        [Header("Tiles")]
        public List<TileBase> Stone;
        public List<TileBase> Grass;
        public List<TileBase> Dirt;
        public List<TileBase> Wood;
        public List<TileBase> Sponge;
        public List<TileBase> Sand;

        [Header("SFX")]
        public AudioClip[] StoneSFX;
        public AudioClip[] GrassSFX;
        public AudioClip[] DirtSFX;
        public AudioClip[] WoodSFX;
        public AudioClip[] SpongeSFX;
        public AudioClip[] SandSFX;
        
        public AudioClip[] GetPair(TileBase tile)
        {
            if (Stone.Any(currentTile => tile == currentTile))
                return StoneSFX;

            if (Grass.Any(currentTile => tile == currentTile))
                return GrassSFX;

            if (Dirt.Any(currentTile => tile == currentTile))
                return DirtSFX;

            if (Wood.Any(currentTile => tile == currentTile))
                return WoodSFX;
            
            if (Sponge.Any(currentTile => tile == currentTile))
                return SpongeSFX;
            
            if (Sand.Any(currentTile => tile == currentTile))
                return SandSFX;

            return StoneSFX;
        }
}