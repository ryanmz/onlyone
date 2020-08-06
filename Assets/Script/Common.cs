using UnityEngine;

public enum DirectionEnum 
{
    cUpDir = 0,
    cLeftDir = 1,
    cDownDir = 2,
    cRightDir = 3,
    cNone = 4,

}

public enum CellEnum
{
    cStart = 0,
    cEnd = 1,
    cBlank = 2,
    cArrow = 3,
    cActionPoint = 4,
    cBlock = 5,
    cUnknown = 6,
    cDamage = 7,

}


public class CommonFunction:Singleton<CommonFunction>
{
    public bool GameStart = false;


    public void SetGameState(bool start)
    {
        this.GameStart = start;
    }

    public float DirectValue(DirectionEnum dir)
    {
        if(dir == DirectionEnum.cUpDir)
        {
            return 90.0f;

        }else if (dir == DirectionEnum.cDownDir)
        {
            return 270.0f;
        }
        else if (dir == DirectionEnum.cLeftDir)
        {
            return 180.0f;
        }
        else if (dir == DirectionEnum.cRightDir)
        {
            return 0.0f;
        }
        return 0.0f;
    }

    public DirectionEnum JudgeDir(float angle)
    {
        if(angle == 90.0f)
        {
            return DirectionEnum.cUpDir;

        }
        else if (angle == 270.0f)
        {
            return DirectionEnum.cDownDir;

        }
        else if (angle == 180.0f)
        {
            return DirectionEnum.cLeftDir;
        }
        else if (angle == 0.0f)
        {
            return DirectionEnum.cRightDir;
        }
        return DirectionEnum.cNone;

    }

    public Sprite CellImage(CellEnum cell)
    {
        if(cell == CellEnum.cStart)
        {
            return null;
        }
        else if (cell == CellEnum.cEnd)
        {
            return null;
        }
        else if (cell == CellEnum.cBlank)
        {
            return Resources.Load("Image/blank", typeof(Sprite)) as Sprite;
        }
        else if (cell == CellEnum.cArrow)
        {
            return Resources.Load("Image/direct", typeof(Sprite)) as Sprite;
        }
        else if (cell == CellEnum.cActionPoint)
        {
            return null;
        }
        else if (cell == CellEnum.cBlock)
        {
            return null;
        }
        else if (cell == CellEnum.cUnknown)
        {
            return null;
        }
        else if (cell == CellEnum.cDamage)
        {
            return null;
        }
        return null;
    }
}