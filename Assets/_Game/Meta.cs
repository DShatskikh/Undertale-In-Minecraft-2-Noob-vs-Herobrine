using System;

[Serializable]
public sealed class Meta
{
    public static Meta Instance;
    public float Volume = 1f;
    public bool IsCompleteTutorial;

    public static Meta GetDefault()
    {
        return new Meta
        {
           
        };
    }
}
