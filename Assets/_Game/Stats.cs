using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public sealed class Stats
{
    public static Stats Instance;
    
    public int HP;
    public int MaxHP;
    public int Money;
    public string[] Items = new string[8];
    public float Time;
    public string LevelName;
    public Vector2 Position;
    public List<string> Chests = new();
    
    /*
     * 6 - встали на путь зла
     * 7 - черно-белый мир
     * 10 - Головоломка на спавне
     * 11 - Забанили Херобрина
     * 12 - Забанили Бизнесмен
     * 13 - Забанили Нотч
     * 14 - Забанили Fir
     * 15 - 
     */
    public int[] Data = new int[100];

    public Stats GetLoad()
    {
        var loadData = SaveSystem.Load();

        if (loadData != null)
            return loadData;
        
        return new Stats()
        {
            LevelName = "???",
        };
    }
    
    public static Stats GetDefault()
    {
        return new Stats
        {
            HP = 20,
            MaxHP = 20,
            LevelName = "Level 1",
            Position = new Vector2(),
            Items = new []
            {
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
            },
        };
    }
    
    public static string GetLevelName(string loadDataLevelName)
    {
        return loadDataLevelName switch
        {
            "Level 1" => "Руинино",
            "Level 2" => "Руинино",
            "Level 3" => "Руинино",
            "Level 4" => "Руинино",
            "Level 5" => "Руинино",
            "Level 6" => "Руинино",
            "Level 7" => "Чертаново",
            "Level 8" => "Чертаново",
            "Level 9" => "Чертаново",
            "Level 10" => "Чертаново",
            "Level 11" => "Чертаново",
            "Level 11_2" => "Чертаново",
            "Level 12" => "Чертаново",
            "Level 13" => "Чертаново",
            "Level 14" => "Чертаново",
            "Level 14_1" => "Чертаново",
            "Level 14_2" => "Чертаново",
            "Level 15" => "Руинино-Падик",
            "Level 15_1" => "Руинино",
            "Level 15_2" => "Руинино",
            "Level 15_2_1" => "Руинино",
            "Level 15_2_2" => "Руинино",
            "Level 16" => "Руинино",
            "Level 17" => "Руинино",
            "Level 18" => "Руинино",
            "Level 19" => "Руинино",
            "Level 20" => "Руинино",
            _ => "???"
        };
    }

    public bool TryAddItem(string item)
    {
        for (int i = 0; i < Items.Length; i++)
        {
            if (Items[i] != string.Empty)
                continue;
            
            Items[i] = item;
            return true;
        }
        
        return false;
    }
    
    public int GetItemsCount()
    {
        return 0;
    }
}
