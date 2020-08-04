using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragHandler : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    //公共变量
    #region
    public DirectionEnum currentDir;
    public bool enableClick = false;
    #endregion

    //私有变量
    #region
    private RectTransform rectTran;
    private Image colorImg;
    private Color originCol;
    #endregion

    //周期函数
    #region
    void Start()
    {
        this.rectTran = this.GetComponent<RectTransform>();
        this.originCol = this.GetComponent<Image>().color;


        currentDir = CommonFunction.Instance.JudgeDir(rectTran.transform.localEulerAngles.z);

    }

    #endregion

    //接口函数
    #region
    //当鼠标按下时调用 接口对应 IPointerDownHandler
    public void OnPointerDown(PointerEventData eventData)
    {
        if (this.enableClick)
        {
            this.colorImg = this.GetComponent<Image>();
            this.colorImg.color = Color.green;
        }

    }

    //当鼠标拖动时调用 对应接口 IDragHandler
    public void OnDrag(PointerEventData eventData)
    {
        if (this.enableClick)
        {
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
            }
        }

    }

    //当鼠标抬起时调用 对应接口 IPointerUpHandler
    public void OnPointerUp(PointerEventData eventData)
    {
        if (this.enableClick)
        {
            this.colorImg = this.GetComponent<Image>();
            this.colorImg.color = originCol;
        }
    }
    #endregion




}
