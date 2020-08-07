using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CellManager : MonoBehaviour
{

    public RectTransform tagCell;
    public RectTransform startCell;      // 起点
    public float distance;
    public float speed;
    public RectTransform player;

    public DirectionEnum characterDir;   // 角色的移动方向
    public bool clear = false;           // 通关标识
    public bool over = false;            // 游戏结束标识
    public int actionPoint = 1;          // 行动点

    private Vector2 dir;
    private DragHandler[,] grids;

    void Update()
    {
        if (CommonFunction.Instance.GameStart)
        {
            this.UpdateGridsCells();
            this.dir = (this.tagCell.position - this.player.position).normalized;
            this.player.Translate(this.dir * Time.deltaTime * this.speed);

            this.distance = Vector2.Distance(this.tagCell.position, this.player.position);

            if (this.distance < 10.0f)
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
                        this.tagCell = this.BlankCell();
                        break;
                    case CellEnum.cArrow:
                        this.tagCell = this.ArrowCell(this.tagCell.GetComponent<DragHandler>().currentDir);
                        break;
                    case CellEnum.cActionPoint:
                        this.tagCell = this.ActionPointCell();
                        break;
                    case CellEnum.cUnknown:
                        this.tagCell = this.UnknownCell();
                        break;
                    case CellEnum.cDamage:
                        this.tagCell = this.DamageCell();
                        break;
                    default:
                        Debug.Log("Unknown cell type!");
                        break;

                }
            }
        }

    }

    public void UpdateGridsCells()
    {
        int row = (int)(this.GetComponent<RectTransform>().sizeDelta.y) / (int)(this.GetComponent<GridLayoutGroup>().cellSize.y);
        int col = (int)(this.GetComponent<RectTransform>().sizeDelta.x) / (int)(this.GetComponent<GridLayoutGroup>().cellSize.x);
        grids = new DragHandler[row, col];
        DragHandler[] cells = GetComponentsInChildren<DragHandler>();
        int index = 0;
        for (int i = 0; i < grids.GetLength(0); i++)
        {
            for (int j = 0; j < grids.GetLength(1); j++)
            {
                grids[i, j] = cells[index];
                Debug.Log(grids[i, j].gameObject.name);
                index++;
            }

        }
    }

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
        return this.tagCell;
    }
    #endregion

    // 空白格
    #region
    public RectTransform BlankCell()
    {
        return this.SelectNewCell(this.characterDir);
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
    public RectTransform ActionPointCell()
    {
        actionPoint++;
        return this.SelectNewCell(this.characterDir);
    }
    #endregion

    // 问号格
    #region
    public RectTransform UnknownCell()
    {
        DirectionEnum dir = this.tagCell.GetComponent<DragHandler>().currentDir;
        // ... 修改格子属性
        return this.SelectNewCell(dir);
    }
    #endregion

    // 伤害格
    #region
    public RectTransform DamageCell()
    {
        over = true;
        // ... 初始化全局
        return this.startCell;  // 返回起点位置
    }
    #endregion

    // 移动 
    #region
    public RectTransform SelectNewCell(DirectionEnum dir)
    {
        if(dir == DirectionEnum.cUpDir)
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
            if (this.GetRow() == (this.grids.GetLength(0)-1))
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
    #endregion


    //公共方法
    #region
    private int GetRow()
    {

        for (int i = 0; i < this.grids.GetLength(0); i++)
        {
            for (int j = 0; j < this.grids.GetLength(1); j++)
            {
                if(this.tagCell.GetComponent<DragHandler>() == this.grids[i, j])
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

}
