using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CellManager : MonoBehaviour
{
    public RectTransform tagCell;
    public float distance;
    public float speed;
    public RectTransform player;

    public DirectionEnum characterDir;

    private Vector2 dir;
    private DragHandler[,] grids = new DragHandler[2, 2];
    // Start is called before the first frame update
    void Start()
    {
        DragHandler[] cells = GetComponentsInChildren<DragHandler>();
        int index = 0;
        for(int i = 0; i < grids.GetLength(0); i++)
        {
            for(int j = 0; j < grids.GetLength(1); j++)
            {
                grids[i, j] = cells[index];
                Debug.Log(grids[i, j].gameObject.name);
                index++;
            }
            
        }


    }

    // Update is called once per frame
    void Update()
    {
        this.dir = (this.tagCell.position - this.player.position).normalized;
        this.player.Translate(this.dir * Time.deltaTime * this.speed);

        this.distance = Vector2.Distance(this.tagCell.position, this.player.position);

        if (this.distance < 10.0f)
        {
            this.tagCell = this.ArrowSelectNewCell(this.tagCell.GetComponent<DragHandler>().currentDir);

        }

    }

    // 空白格功能
    #region
    public RectTransform BlankSelectNewCell()
    {

        return this.tagCell;
    }
    #endregion

    // 箭头格功能 
    #region
    public RectTransform ArrowSelectNewCell(DirectionEnum dir)
    {
        if(dir == DirectionEnum.cUpDir)
        {
            if (this.GetRow() < 1)
            {
                return this.tagCell;
            }
            else
            {
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
                return this.grids[this.GetRow(), this.GetCol() + 1].gameObject.GetComponent<RectTransform>();
            }
        }
        return this.tagCell;
    }
    #endregion


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

}
