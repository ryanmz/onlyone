using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class CellManager : MonoBehaviour
{
    //数据管理
    #region


    //角色信息
    #region
    public RectTransform tagCell;
    public RectTransform startCell;      // 起点
    public float distance;
    public float speed;
    public RectTransform player;
    


    public DirectionEnum characterDir;   // 角色的移动方向
    public bool clear = false;           // 通关标识
    public bool over = false;            // 游戏结束标识

    public DragHandler[] actionCell;
    public int actionPoint = 1;          // 行动点
    private int blankPoint = 0;
    #endregion

    //游戏状态
    #region
    public GameState currentState;

    //准备开始gSet
    public Button btnStartGame;
    public Button btnMenuGame;

    //游戏暂停gPause
    public RectTransform gameMenu;
    public Button btnExitGame1;
    public Button btnReStartGame1;
    public Button btnReturn;

    //游戏重设gReset
    public RectTransform gameReset;
    public Button btnResetGame1;
    public Button btnReStartGame2;
    public Button btnExitGame2;

    //游戏结果gWin
    public RectTransform gameOver;
    public Text resultGame;
    public Button btnReStartGame3;
    public Button btnExitGame3;
    #endregion

    //地图方格信息
    #region
    private Vector2 dir;
    private DragHandler[,] grids;
    #endregion


    #endregion

    //生命周期
    #region
    void Start()
    {
        this.SetState();
        this.EventBind();
        this.UpdateActionCells();

    }//初始化

    void EventBind()
    {
        this.btnStartGame.onClick.AddListener(this.GamingState);
        this.btnMenuGame.onClick.AddListener(this.PauseState);
        this.btnReturn.onClick.AddListener(this.ReturnState);
        this.btnReStartGame1.onClick.AddListener(CommonFunction.Instance.ReStartGame);
        this.btnReStartGame2.onClick.AddListener(CommonFunction.Instance.ReStartGame);
        this.btnReStartGame3.onClick.AddListener(CommonFunction.Instance.ReStartGame);
        this.btnExitGame1.onClick.AddListener(CommonFunction.Instance.ExitGame);
        this.btnExitGame2.onClick.AddListener(CommonFunction.Instance.ExitGame);
        this.btnExitGame3.onClick.AddListener(CommonFunction.Instance.ExitGame);
        EventListener.Instance.execute += new EventListener.ListenerHandler(this.UpdateActionCells);


    }//事件绑定

    void UpdateActionCells()
    {
        GameObject parentAction = GameObject.Find("Content");
        Text actionText = GameObject.Find("BarText").GetComponent<Text>();
        this.actionCell = parentAction.GetComponentsInChildren<DragHandler>();
        int blankTem = 0;
        for(int i = 0; i < this.actionCell.Length; i++)
        {
            if(this.actionCell[i].currentCellType == CellEnum.cBlank)
            {
                blankTem++;
            }
        }
        if (blankTem > this.blankPoint)
        {
            this.actionPoint--;

        }
        else if(blankTem < this.blankPoint)
        {
            this.actionPoint++;
        }
        this.blankPoint = blankTem;
        actionText.text = Convert.ToString(this.actionPoint);
        if (this.actionPoint <= 0)
        {
            for (int i = 0; i < this.actionCell.Length; i++)
            {
                this.actionCell[i].enableClick = false;
            }
        }
        else
        {
            for (int i = 0; i < this.actionCell.Length; i++)
            {
                if (this.actionCell[i].currentCellType != CellEnum.cBlank)
                {
                    this.actionCell[i].enableClick = true;
                }
            }
        }
    }

    void Update()
    {
        if (CommonFunction.Instance.GameStart)
        {

            this.dir = (this.tagCell.position - this.player.position).normalized;
            this.player.Translate(this.dir * Time.deltaTime * this.speed);

            float distance = Vector2.Distance(this.tagCell.position, this.player.position);

            if (distance < this.distance)
            {
                //this.tagCell = this.SelectNewCell(this.tagCell.GetComponent<DragHandler>().currentDir);
                CellEnum cellType = this.tagCell.GetComponent<DragHandler>().currentCellType;
                switch (cellType)
                {
                    case CellEnum.cStart:
                        this.tagCell = this.StartCell(this.characterDir);
                        break;
                    case CellEnum.cEnd:
                        this.tagCell = this.EndCell();
                        break;
                    case CellEnum.cBlank:
                        this.tagCell = this.BlankCell(this.characterDir);
                        break;
                    case CellEnum.cArrow:
                        this.tagCell = this.ArrowCell(this.tagCell.GetComponent<DragHandler>().currentDir);
                        break;
                    case CellEnum.cActionPoint:
                        this.tagCell = this.ActionPointCell(this.characterDir);
                        break;
                    case CellEnum.cUnknown:
                        this.tagCell = this.UnknownCell();
                        break;
                    case CellEnum.cDamage:
                        this.DamageCell();
                        break;
                    default:
                        Debug.Log("Unknown cell type!");
                        break;

                }

                
            }
        }
        else
        {

        }

    }//运行游戏
    #endregion


    //方格类型
    #region
    // 起点(输入为起始方向)
    #region
    public RectTransform StartCell(DirectionEnum dir)
    {
        return this.SelectNewCell(dir);
    }
    #endregion

    // 终点
    #region
    public RectTransform EndCell()
    {
        clear = true;
        this.EndState(GameState.gWin);
        return this.tagCell;
    }
    #endregion

    // 空白格
    #region
    public RectTransform BlankCell(DirectionEnum dir)
    {
        return this.SelectNewCell(dir);
    }
    #endregion

    // 指向格
    #region
    public RectTransform ArrowCell(DirectionEnum dir)
    {
        return this.SelectNewCell(dir);
    }
    #endregion

    // 行动点+1格
    #region
    public RectTransform ActionPointCell(DirectionEnum dir)
    {
        actionPoint++;
        Debug.Log("Action Point: " + actionPoint);
        return this.SelectNewCell(dir);
    }
    #endregion

    // 问号格
    #region
    public RectTransform UnknownCell()
    {
        DragHandler currentCell = this.tagCell.GetComponent<DragHandler>();
        // 修改格子属性-即时触发(种类与方向检查？)
        if (currentCell.isUnknown == true)
        {
            // 这里不能设置成false，以保证重置后可以恢复为unknown
            //currentCell.isUnknown = false;
            currentCell.currentCellType = currentCell.hiddenCellType;
            //currentCell.currentDir = currentCell.hiddenDir;
        }
        return this.SelectNewCell(currentCell.currentDir);
    }
    #endregion

    // 伤害格
    #region
    public void DamageCell()
    {
        over = true;
        this.EndState(GameState.gFail);
    }
    #endregion
    #endregion


    //公共方法
    #region
    // 移动 
    public RectTransform SelectNewCell(DirectionEnum dir)
    {
        if (dir == DirectionEnum.cUpDir)
        {
            if (this.GetRow() < 1)
            {
                return this.tagCell;
            }
            else
            {
                this.characterDir = DirectionEnum.cUpDir;
                return this.grids[this.GetRow() - 1, this.GetCol()].gameObject.GetComponent<RectTransform>();
            }
        }
        else if (dir == DirectionEnum.cDownDir)
        {
            if (this.GetRow() == (this.grids.GetLength(0) - 1))
            {
                return this.tagCell;
            }
            else
            {
                this.characterDir = DirectionEnum.cDownDir;
                return this.grids[this.GetRow() + 1, this.GetCol()].gameObject.GetComponent<RectTransform>();
            }
        }
        else if (dir == DirectionEnum.cLeftDir)
        {
            if (this.GetCol() < 1)
            {
                return this.tagCell;
            }
            else
            {
                this.characterDir = DirectionEnum.cLeftDir;
                return this.grids[this.GetRow(), this.GetCol() - 1].gameObject.GetComponent<RectTransform>();
            }
        }
        else if (dir == DirectionEnum.cRightDir)
        {
            if (this.GetCol() == (this.grids.GetLength(1) - 1))
            {
                return this.tagCell;
            }
            else
            {
                this.characterDir = DirectionEnum.cRightDir;
                return this.grids[this.GetRow(), this.GetCol() + 1].gameObject.GetComponent<RectTransform>();
            }
        }
        return this.tagCell;
    }

    // 处理特殊方格(可选择加入动画效果)
    #region
    private void SpCellHandler()
    {
        for (int i = 0; i < this.grids.GetLength(0); i++)
        {
            for (int j = 0; j < this.grids.GetLength(1); j++)
            {
                SpCellEnum sp = this.grids[i, j].spCellType;
                if (sp != SpCellEnum.sNone)
                {
                    if (i != 0)
                    {
                        switch (sp)
                        {
                            case SpCellEnum.sBlank:
                                this.grids[i - 1, j].currentCellType = CellEnum.cBlank;
                                break;
                            case SpCellEnum.sClockwise:
                                this.grids[i - 1, j].currentDir = CommonFunction.Instance.JudgeDir((CommonFunction.Instance.DirectValue(this.grids[i - 1, j].currentDir) + 270.0f) % 360);
                                break;
                            case SpCellEnum.sAntiClockwise:
                                this.grids[i - 1, j].currentDir = CommonFunction.Instance.JudgeDir((CommonFunction.Instance.DirectValue(this.grids[i - 1, j].currentDir) + 90.0f) % 360);
                                break;
                        }
                    }
                    if (j != 0)
                    {
                        switch (sp)
                        {
                            case SpCellEnum.sBlank:
                                this.grids[i, j - 1].currentCellType = CellEnum.cBlank;
                                break;
                            case SpCellEnum.sClockwise:
                                this.grids[i, j - 1].currentDir = CommonFunction.Instance.JudgeDir((CommonFunction.Instance.DirectValue(this.grids[i, j - 1].currentDir) + 270.0f) % 360);
                                break;
                            case SpCellEnum.sAntiClockwise:
                                this.grids[i, j - 1].currentDir = CommonFunction.Instance.JudgeDir((CommonFunction.Instance.DirectValue(this.grids[i, j - 1].currentDir) + 90.0f) % 360);
                                break;
                        }
                    }
                    if (i != this.grids.GetLength(0) - 1)
                    {
                        switch (sp)
                        {
                            case SpCellEnum.sBlank:
                                this.grids[i + 1, j].currentCellType = CellEnum.cBlank;
                                break;
                            case SpCellEnum.sClockwise:
                                this.grids[i + 1, j].currentDir = CommonFunction.Instance.JudgeDir((CommonFunction.Instance.DirectValue(this.grids[i + 1, j].currentDir) + 270.0f) % 360);
                                break;
                            case SpCellEnum.sAntiClockwise:
                                this.grids[i + 1, j].currentDir = CommonFunction.Instance.JudgeDir((CommonFunction.Instance.DirectValue(this.grids[i + 1, j].currentDir) + 90.0f) % 360);
                                break;
                        }
                    }
                    if (j != this.grids.GetLength(1) - 1)
                    {
                        switch (sp)
                        {
                            case SpCellEnum.sBlank:
                                this.grids[i, j + 1].currentCellType = CellEnum.cBlank;
                                break;
                            case SpCellEnum.sClockwise:
                                this.grids[i, j + 1].currentDir = CommonFunction.Instance.JudgeDir((CommonFunction.Instance.DirectValue(this.grids[i, j + 1].currentDir) + 270.0f) % 360);
                                break;
                            case SpCellEnum.sAntiClockwise:
                                this.grids[i, j + 1].currentDir = CommonFunction.Instance.JudgeDir((CommonFunction.Instance.DirectValue(this.grids[i, j + 1].currentDir) + 90.0f) % 360);
                                break;
                        }
                    }
                }
            }
        }
    }
    #endregion

    //游戏状态管理
    #region
    //准备游戏
    private void SetState()
    {
        CommonFunction.Instance.SetGameState(false);
        this.GameButton(GameState.gSet);
    }

    //进行游戏
    private void GamingState()
    {
        this.InitGridsCells();
        CommonFunction.Instance.SetGameState(true);
        this.GameButton(GameState.gGaming);
        // search for SpCell and deal with them
        SpCellHandler();
    }

    //暂停游戏
    private void PauseState()
    {
        this.GameButton(GameState.gPause);
    }

    //继续游戏
    private void ReturnState()
    {
        this.GameButton(this.currentState);
    }

    //返回起点
    private void ResetState()
    {
        CommonFunction.Instance.SetGameState(false);
        this.GameButton(GameState.gReset);
    }

    //游戏获胜
    private void EndState(GameState state)
    {
        CommonFunction.Instance.SetGameState(false);
        if (state == GameState.gWin)
        {  
            this.GameButton(GameState.gWin);
        }else if(state == GameState.gFail)
        {
            this.GameButton(GameState.gFail);
        }
    }

    private void GameButton(GameState state)
    {
        CommonFunction.Instance.ContinueGame();
        if (state == GameState.gSet)
        {
            this.btnStartGame.gameObject.SetActive(true);
            this.gameMenu.gameObject.SetActive(false);
            this.gameReset.gameObject.SetActive(false);
            this.gameOver.gameObject.SetActive(false);
            this.currentState = state;

        }
        else if(state == GameState.gGaming)
        {
            this.btnStartGame.gameObject.SetActive(false);
            this.gameMenu.gameObject.SetActive(false);
            this.gameReset.gameObject.SetActive(false);
            this.gameOver.gameObject.SetActive(false);
            this.currentState = state;
        }
        else if(state == GameState.gPause)
        {

            this.gameMenu.gameObject.SetActive(true);
            this.gameReset.gameObject.SetActive(false);
            this.gameOver.gameObject.SetActive(false);
            CommonFunction.Instance.PauseGame();
        }
        else if(state == GameState.gReset)
        {

            this.gameMenu.gameObject.SetActive(false);
            this.gameReset.gameObject.SetActive(true);
            this.gameOver.gameObject.SetActive(false);
            this.currentState = state;
        }
        else if(state == GameState.gWin)
        {
            this.gameMenu.gameObject.SetActive(false);
            this.gameOver.gameObject.SetActive(true);
            this.resultGame.text = CommonFunction.Instance.ResultGame(state);
            this.resultGame.color = Color.red;
            this.currentState = state;
        }
        else if (state == GameState.gFail)
        {
            this.gameMenu.gameObject.SetActive(false);
            this.gameOver.gameObject.SetActive(true);
            this.resultGame.text = CommonFunction.Instance.ResultGame(state);
            this.resultGame.color = Color.black;
            this.currentState = state;
        }
    }
    #endregion


    //地图方块管理
    #region
    public void InitGridsCells()
    {
        int row = (int)(this.GetComponent<RectTransform>().sizeDelta.y) / (int)(this.GetComponent<GridLayoutGroup>().cellSize.y);
        int col = (int)(this.GetComponent<RectTransform>().sizeDelta.x) / (int)(this.GetComponent<GridLayoutGroup>().cellSize.x);
        this.grids = new DragHandler[row, col];
        DragHandler[] cells = this.GetComponentsInChildren<DragHandler>();
        int index = 0;
        for (int i = 0; i < this.grids.GetLength(0); i++)
        {
            for (int j = 0; j < this.grids.GetLength(1); j++)
            {
                this.grids[i, j] = cells[index];
                Debug.Log(grids[i, j].gameObject.name);
                index++;
            }

        }
        

    }
    private int GetRow()
    {

        for (int i = 0; i < this.grids.GetLength(0); i++)
        {
            for (int j = 0; j < this.grids.GetLength(1); j++)
            {
                if (this.tagCell.GetComponent<DragHandler>() == this.grids[i, j])
                {
                    return i;
                }
            }

        }
        return 0;
    }
    private int GetCol()
    {

        for (int i = 0; i < this.grids.GetLength(0); i++)
        {
            for (int j = 0; j < this.grids.GetLength(1); j++)
            {
                if (this.tagCell.GetComponent<DragHandler>() == this.grids[i, j])
                {
                    return j;
                }
            }

        }
        return 0;
    }
    #endregion


    #endregion
}
