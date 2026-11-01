using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public sealed class Stats
{
    public static Stats Instance;
    
    public int HP;
    public int MaxHP;
    public int Money;
    public string[] Items = new string[7];
    public List<string> OrdinaryItems = new();
    public string LevelName;
    public Vector2 Position;
    public List<string> Chests = new();
    public float Time;
    
    /*
     * 0 - Начали игру
     * 1 - В меню
     * 2 - Геноцид
     * 3 - Точка возрождения
     * 5 - Поговорили с Херобрином
     * 6 - встали на путь зла
     * 7 - черно-белый мир
     * 8 - Есть ли диалог после боя?
     * 9 - купили золотое яблоко
     * 10 - Головоломка на спавне
     * 11 - Забанили Херобрина
     * 12 - Забанили Бизнесмен
     * 13 - Забанили Нотч
     * 14 - Забанили Fir
     * 15 - Забанили Hacker
     * 16 - Забанили Grum
     * 17 - Забанили Trio
     * 18 - Забанили Admin
     * 19 - Забанили Героя
     * 20 - Забанили Паллисоса
     * 21 - Забанили Висмана
     * 31 - Победили Hacker только 1  3 Сбежали или умерли
     * 32 - Победили Grum  1 Пощада 2 Убийство 3 Сбежали или умерли
     * 33 - Победили Fivfiv
     * 34 - Победили Nastya
     * 35 - Победили Sheep
     * 36 - Победили Herobrine
     * 37 - Победили Героя
     * 40 - Сквидвард сбежал
     * 41 - Активровали головоломку 1 в Подземелье
     * 42 - Активровали головоломку 2 в Подземелье
     * 43 - Активровали головоломку 3 в Подземелье (Которая у дверей)
     * 44 - 
     * 55 - Поговорили с разработчиком
     * 70 - Ушли от админа
     * 90 - Кот прошел за нас игру
     * 100 - Спидран
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
            LevelName = "Home",
            Position = new Vector2(),
            Money = 5,
            Items = new []
            {
                "Steak",
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
        return Items.Count(x => x != string.Empty);
    }

    public static string GetItemName(string item)
    {
        return item switch
        {
            "Steak" => "Стейк",
            "Potion" => "Зелье Л.",
            "GoldApple" => "Зол.Яблк.",
            "Ads" => "Реклама",
            "Ticket" => "<color=yellow>Забагованный билет</color>",
            _ => null
        };
    }
    
    public static string GetItemHPText(string item)
    {
        return item switch
        {
            "Steak" => "5 ",
            "Potion" => "10 ",
            "GoldApple" => "20(4)",
            "Ads" => "15(2)",
            _ => "1 "
        };
    }
    
    public static int GetItemHP(string item)
    {
        return item switch
        {
            "Steak" => 5,
            "Potion" => 10,
            "GoldApple" => 20,
            "Ads" => 15,
            _ => 1
        };
    }
    
    public void RemoveItem(string itemID)
    {
        for (var i = 0; i < Items.Length; i++)
        {
            var item = Items[i];

            if (itemID == item)
            {
                Items[i] = null;
                break;
            }
        }
    }

    public void Reset()
    {
        var items = Instance.Items;
        var money = Instance.Money;
        Instance = GetDefault();
        Instance.Items = items;
        Instance.Money = money;
    }
}
