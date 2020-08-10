using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class DragHandler : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    //公有变量
    #region
    public CellEnum currentCellType;
    public SpCellEnum spCellType = SpCellEnum.sNone;
    public bool enableClick = false;
    public static RectTransform parentCell;
    public DirectionEnum currentDir;

    public bool visited = false;

    #endregion

    //问号格专属
    #region
    public bool isUnknown = false;
    public CellEnum hiddenCellType = CellEnum.cBlank;
    //public DirectionEnum hiddenDir;
    #endregion

    //私有变量
    #region
    private RectTransform rectTran;
    private CanvasGroup tempBlock;
    private Vector2 offset= new Vector3();
    private Image cellImage;
    private Color originCol;

    private Vector3 fromScale = new Vector3(1.0f, 1.0f, 1.0f);
    private Vector3 toScale = new Vector3(0.8f, 0.8f, 1.0f);
    #endregion

    //周期函数
    #region
    void Start()
    {
        this.rectTran = this.GetComponent<RectTransform>();
        this.cellImage = this.GetComponent<Image>();
        this.originCol = this.GetComponent<Image>().color;
        parentCell = this.rectTran.parent.gameObject.GetComponent<RectTransform>();
        this.tempBlock = this.gameObject.AddComponent<CanvasGroup>();
        this.SetCellInfo(this.spCellType,this.currentCellType, this.currentDir);

    }

    //方格信息设置
    public void SetCellInfo(SpCellEnum spCellType,CellEnum cellType, DirectionEnum dir)
    {
        if (spCellType == SpCellEnum.sNone)
        {
            this.currentCellType = cellType;

            if (cellType != CellEnum.cArrow)
                this.currentDir = DirectionEnum.cNone;
            else
                this.currentDir = dir;

            this.rectTran.localRotation = Quaternion.Euler(0, 0, CommonFunction.Instance.DirectValue(this.currentDir));
            this.cellImage.sprite = CommonFunction.Instance.CellImage(cellType);
        }
        else
        {
            this.currentCellType = cellType;
            if (spCellType != SpCellEnum.sArrow)
                this.currentDir = DirectionEnum.cNone;
            else
                this.currentDir = dir;

            this.rectTran.localRotation = Quaternion.Euler(0, 0, CommonFunction.Instance.DirectValue(this.currentDir));
            this.cellImage.sprite = CommonFunction.Instance.SpCellImage(spCellType);
        }
    }

    #endregion

    //接口函数
    #region
    //当鼠标按下时调用 接口对应 IPointerDownHandler
    public void OnPointerDown(PointerEventData eventData)
    {
        if (this.enableClick&&!CommonFunction.Instance.GameStart)
        {
            this.rectTran.localScale = this.toScale;
            this.cellImage = this.GetComponent<Image>();
            this.cellImage.color = Color.green;

            parentCell = this.rectTran.parent.GetComponent<RectTransform>();
            RectTransform canvas = GameObject.Find("Canvas").GetComponent<RectTransform>();
            Vector2 mouseDown = eventData.position;
            Vector2 mouseUguiPos = new Vector2();

            bool isRect = RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, mouseDown, eventData.enterEventCamera, out mouseUguiPos);
            this.rectTran.SetParent(canvas);
           

            if (isRect)
            {
                this.offset = this.rectTran.anchoredPosition - mouseUguiPos;
            }
            this.tempBlock.blocksRaycasts = false;
        }



    }

    //当鼠标拖动时调用 对应接口 IDragHandler
    public void OnDrag(PointerEventData eventData)
    {
        if (this.enableClick && !CommonFunction.Instance.GameStart)
        {

            //拖拽移动
            RectTransform canvas = GameObject.Find("Canvas").GetComponent<RectTransform>();
            Vector2 mouseDown = eventData.position;
            Vector2 mouseUguiPos = new Vector2();

            bool isRect = RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, mouseDown, eventData.enterEventCamera, out mouseUguiPos);
            this.rectTran.SetAsLastSibling();
            if (isRect)
            {
                this.rectTran.anchoredPosition = this.offset + mouseUguiPos;
            }

            //拉扯方向
            #region
            /*
            RectTransform canvas = GameObject.Find("Canvas").GetComponent<RectTransform>();
            Vector2 mouseDown = eventData.position;
            Vector2 mouseUguiPos = new Vector2();

            bool isRect = RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, mouseDown, eventData.enterEventCamera, out mouseUguiPos);

            if (isRect)
            {
                Vector2 dir = (mouseUguiPos - this.rectTran.anchoredPosition).normalized;
                float faceToValue = 0.0f;

                if (Math.Abs(dir.y) > Math.Abs(dir.x) && dir.y > 0)
                {

                    faceToValue = CommonFunction.Instance.DirectValue(DirectionEnum.cUpDir);
                }
                else if (Math.Abs(dir.y) > Math.Abs(dir.x) && dir.y < 0)
                {

                    faceToValue = CommonFunction.Instance.DirectValue(DirectionEnum.cDownDir);
                }
                else if (Math.Abs(dir.y) < Math.Abs(dir.x) && dir.x > 0)
                {

                    faceToValue = CommonFunction.Instance.DirectValue(DirectionEnum.cRightDir);
                }
                else if (Math.Abs(dir.y) < Math.Abs(dir.x) && dir.x < 0)
                {

                    faceToValue = CommonFunction.Instance.DirectValue(DirectionEnum.cLeftDir);
                }
                this.currentDir = CommonFunction.Instance.JudgeDir(faceToValue);
                this.rectTran.localRotation = Quaternion.Euler(0, 0, faceToValue);
            }*/
            #endregion
        }

    }

    //当鼠标抬起时调用 对应接口 IPointerUpHandler
    public void OnPointerUp(PointerEventData eventData)
    {
        if (this.enableClick && !CommonFunction.Instance.GameStart)
        {
            Debug.Log("ssssssssssssssssssssssssssssss");
            this.rectTran.localScale = this.fromScale;
            this.cellImage = this.GetComponent<Image>();
            this.cellImage.color = originCol;

            if(eventData.pointerEnter == null)
            {
                this.rectTran.SetParent(parentCell);
                this.rectTran.localPosition = Vector2.zero;
                this.tempBlock.blocksRaycasts = true;
                return;
            }


            RectTransform targetObj = eventData.pointerEnter.GetComponent<RectTransform>();



            if (targetObj.GetComponent<DragHandler>() != null)
            {

                if (targetObj.GetComponent<DragHandler>().currentCellType == CellEnum.cBlank)
                {
                    this.rectTran.SetParent(targetObj.parent);
                    targetObj.SetParent(parentCell);
                    this.rectTran.localPosition = Vector2.zero;
                    targetObj.localPosition = Vector2.zero;

                }
                else
                {
                    this.rectTran.SetParent(parentCell);
                    this.rectTran.localPosition = Vector2.zero;

                }

                //CommonFunction.Instance.SetGameState(true);
            }
            else
            {
                this.rectTran.SetParent(parentCell);
                this.rectTran.localPosition = Vector2.zero;
                
            }
            this.tempBlock.blocksRaycasts = true;
            EventListener.Instance.FunctionExecute();

        }
        
    }
    #endregion

}
