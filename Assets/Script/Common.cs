using JetBrains.Annotations;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum DirectionEnum 
{
    cUpDir = 0,
    cLeftDir = 1,
    cDownDir = 2,
    cRightDir = 3,
    cNone = 4,

}//角色运动

public enum CellEnum
{
    cStart = 0,
    cEnd = 1,
    cBlank = 2,
    cArrow = 3,
    cActionPoint = 4,
    cUnknown = 5,
    cDamage = 6,

} //方格类型

public enum SpCellEnum
{
    sNone = 0,
    sBlank = 1,
    sClockwise = 2,
    sAntiClockwise = 3,
    sArrow = 4,
} //特殊方格类型

public enum GameState
{
    gSet = 0,
    gGaming = 1,
    gPause = 2,
    gReset = 3,
    gWin = 4,
    gFail = 5,
}  //游戏状态

public class CommonFunction:Singleton<CommonFunction>
{
    public bool GameStart = false;

    public void SetGameState(bool start)
    {
        this.GameStart = start;
    }

    public float DirectValue(DirectionEnum dir)
    {
        //Debug.Log(dir.ToString());
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
        //Debug.Log(angle);
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

    public void SpCell2NormalCell(DragHandler cell)
    {
        if (cell.spCellType == SpCellEnum.sArrow)
        {
            cell.SetCellInfo(SpCellEnum.sNone, CellEnum.cArrow, cell.currentDir);
        }
        else if (cell.spCellType == SpCellEnum.sClockwise)
        {
            cell.SetCellInfo(SpCellEnum.sNone, CellEnum.cBlank, cell.currentDir);
        }
        else if (cell.spCellType == SpCellEnum.sAntiClockwise)
        {
            cell.SetCellInfo(SpCellEnum.sNone, CellEnum.cBlank, cell.currentDir);
        }
        else if (cell.spCellType == SpCellEnum.sBlank)
        {
            cell.SetCellInfo(SpCellEnum.sNone, CellEnum.cBlank, cell.currentDir);
        }
    }//将特殊方格转换成普通格子

    public Sprite CellImage(CellEnum cell)
    {
        if(cell == CellEnum.cStart)
        {
            return Resources.Load("Image/Cell/map8", typeof(Sprite)) as Sprite;
        }
        else if (cell == CellEnum.cEnd)
        {
            return Resources.Load("Image/Cell/map9", typeof(Sprite)) as Sprite;
        }
        else if (cell == CellEnum.cBlank)
        {
            return Resources.Load("Image/Cell/map10", typeof(Sprite)) as Sprite;
        }
        else if (cell == CellEnum.cArrow)
        {
            return Resources.Load("Image/Cell/map1", typeof(Sprite)) as Sprite;
        }
        else if (cell == CellEnum.cActionPoint)
        {
            return Resources.Load("Image/Cell/map5", typeof(Sprite)) as Sprite;
        }
        else if (cell == CellEnum.cUnknown)
        {
            return Resources.Load("Image/Cell/map7", typeof(Sprite)) as Sprite;
        }
        else if (cell == CellEnum.cDamage)
        {
            return Resources.Load("Image/Cell/map6", typeof(Sprite)) as Sprite;
        }
        return null;
    }

    public Sprite SpCellImage(SpCellEnum cell)
    {
        if(cell == SpCellEnum.sAntiClockwise)
        {
            return Resources.Load("Image/OperationalCell/player_hover6", typeof(Sprite)) as Sprite;
        }
        else if (cell == SpCellEnum.sClockwise)
        {
            return Resources.Load("Image/OperationalCell/player_hover5", typeof(Sprite)) as Sprite;
        }
        else if (cell == SpCellEnum.sBlank)
        {
            return Resources.Load("Image/OperationalCell/player7", typeof(Sprite)) as Sprite;
        }
        else if (cell == SpCellEnum.sArrow)
        {
            return Resources.Load("Image/OperationalCell/player1", typeof(Sprite)) as Sprite;
        }
        else if(cell == SpCellEnum.sNone)
        {
            return null;
        }
        return null;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ReStartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void PauseGame()
    {
        Time.timeScale = 0.0f;
    }

    public void ContinueGame()
    {
        Time.timeScale = 1.0f;
    }

    public string ResultGame(GameState state)
    {
        if(state == GameState.gWin)
        {
            return "闯关成功";
        }
        else if(state == GameState.gFail)
        {
            return "闯关失败";
        }
        return "";
    }


} //公共方法

public class EventListener : Singleton<EventListener>
{
    public delegate void ListenerHandler();
    public event ListenerHandler execute = null;
    public void FunctionExecute()
    {
        if (execute != null)
        {
            execute();
        }
    }
}