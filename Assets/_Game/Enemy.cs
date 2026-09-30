using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public string ID;
    public string Name;
    public List<string> Actions;
    public List<string> Answers;
    public bool IsActive => true;
    public int Health;
    public int MaxHealth;
    public int Relationship;
}
