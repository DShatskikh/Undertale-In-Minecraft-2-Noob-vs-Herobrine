using UnityEngine;


    // Расширение для работы с цветами
    public static class ColorExtensions
    {
        public static Color SetA(this Color color, float alpha)
        {
            color.a = alpha;
            return color;
        }
        
        public static Color AddA(this Color color, float alpha)
        {
            color.a += alpha;
            return color;
        }
    }
