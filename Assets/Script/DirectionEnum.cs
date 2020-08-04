public enum DirectionEnum 
{
    cUpDir = 0,
    cLeftDir = 1,
    cDownDir = 2,
    cRightDir = 3,

}

public class CommonFunction:Singleton<CommonFunction>
{
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
        return DirectionEnum.cRightDir;

    }
}