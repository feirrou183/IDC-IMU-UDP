using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GLGrid : MonoBehaviour
{
    public static GLGrid instence;
    public RectTransform gridBg;
    public Canvas canvas;
    public Image image;
    public Material insideMat;
    public Material outMat;
    //坐标点
    public Vector3 zero;
    public Vector3 endPos;
    public Vector3 rightPos;
    public Vector3 upPos;
    public Vector3[] corners = new Vector3[4];
    //显示数字的预制体、
    public GameObject word;

    //数据显示范围
    public float minY, maxY;
    //数据量
    public int numY;
    public int numX;
    //背景的大小(rect）
    //[HideInInspector]
    public float gridX;
    private float gridY;
    //显示数据值的字典
    public Dictionary<string, Vector3> ruler;// = new Dictionary<string, Vector3>();
    void Awake()
    {
        instence = this;
    }
    void Start()
    {
        Culcuate();
        //计算出格子的世界坐标
        DrawChart();
    }
    void Culcuate()
    {
        gridBg.GetWorldCorners(corners);
        zero = gridBg.position;
        rightPos = corners[3];
        upPos = corners[1];
        gridX = gridX > corners[2].x - corners[1].x ? gridX : corners[2].x - corners[1].x;
        gridY = gridY > corners[1].y - corners[0].y ? gridY : corners[1].y - corners[0].y;
        Debug.Log(gridX);
        //计算刻度
        float offset = gridY / numY;//坐标间隔
        ruler = new Dictionary<string, Vector3>();
        for (float i = 0; i <= gridY; i += offset)
        {
            if (i == 0)
            {//舍去第一个点
                continue;
            }
            //计算用于显示的数据
            float num = i * (maxY - minY) / gridY + minY;
            ruler.Add(num.ToString("0"), zero + gridBg.transform.up * i);
        }
    }
    void OnPostRender()
    {
        #region GL绘图
        GL.PushMatrix();

        //绘制坐标系
        DrawAxis();

        GL.PopMatrix();
        #endregion
        Culcuate();
    }
    ///
    /// 绘制轴
    ///
    void DrawAxis()
    {
        insideMat.SetPass(0);
        GL.Begin(GL.LINES);
        foreach (var item in ruler)
        {
            GL.Vertex(item.Value);
            GL.Vertex(item.Value + rightPos - zero);
        }
        GL.End();
        //主界面
        outMat.SetPass(0);
        GL.Begin(GL.QUADS);
        GL.Vertex(zero);
        GL.Vertex(zero + Vector3.up);
        GL.Vertex(rightPos + Vector3.up);
        GL.Vertex(rightPos);

        GL.Vertex(zero + Vector3.right);
        GL.Vertex(zero);
        GL.Vertex(upPos);
        GL.Vertex(upPos + Vector3.right);
        GL.End();
    }
    ///
    /// 绘制表格数据
    ///
    private void DrawChart()
    {
        foreach (var item in ruler)
        {
            //创建字，并设置坐标
            GameObject go = Instantiate(word, item.Value - Vector3.right * 3f, transform.rotation) as GameObject;
            go.transform.SetParent(gridBg);
            go.transform.localScale = Vector3.one;
            go.GetComponent<Text>().text = item.Key;
        }
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log(Input.mousePosition);
            //Vector3 startPoss = Camera.main.ScreenToWorldPoint(new Vector3(gridBg.rect.x, gridBg.rect.y, Vector3.Distance(transform.position, gridBg.position)));
            //Debug.Log(startPoss);
        }
    }
    //得到坐标值y（相对于原点的长度）
    public float GetPosYByWorths(float flo)
    {
        //利用数值反求
        flo = (flo - minY) * gridY / (maxY - minY);
        return flo;
    }
}
