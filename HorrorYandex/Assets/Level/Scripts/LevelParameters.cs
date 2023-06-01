using System;

[Serializable]
public class LevelParameters
{
    public enum LevelType
    {
        Survival,
        Escape
    }

    public LevelType Type;
    public int Number;

    public bool Equals(LevelParameters param)
    {
        if (this.Type == param.Type)
            if (this.Number == param.Number)
                return true;
            else return false;
        else return false;
    }
}
