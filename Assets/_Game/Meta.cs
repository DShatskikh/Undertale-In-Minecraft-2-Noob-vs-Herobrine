using System;

[Serializable]
public sealed class Meta
{
    public static Meta Instance;
    public float Volume = 100f;
    public bool IsCompleteTutorial;
    public bool IsFoundCastle;
    public bool IsFoundDungeous;
    public float Version = 1.0f;
    public bool IsCompleteAdminTutorial;
    public int RecordMoney;
    
    public bool IsEndOldSpawn;
    public bool IsEndGenocide;
    public bool IsEndPacifist;
    public bool IsEndEvil;
    public bool IsEndTheatre;
    
    public bool IsBuyDreamMask;

    public static Meta GetDefault()
    {
        return new Meta
        {
           
        };
    }
}
