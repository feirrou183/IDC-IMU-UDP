//using UnityEngine;
//using System.Collections.Generic;
//using System.Collections;
//using UnityEngine.UI;

//public class GridShow : MonoBehaviour
//{

//    ///
//    /// 主要显示坐标信息，格子管理
//    ///
//    public static GridShow instence;
//    //数据的范围(y方向)
//    public InputField minYField;
//    public InputField maxYField;
//    public float minY
//    {
//        get { return float.Parse(minYField.text); }
//        set { minYField.text = value.ToString(); }
//    }
//    public float maxY
//    {
//        get { return float.Parse(maxYField.text); }
//        set { maxYField.text = value.ToString(); }
//    }
//    //y方向显示的数据个数
//    public Slider numYSlider;
//    public int numY
//    {
//        get { return (int)numYSlider.value; }
//        set { numYSlider.value = value; }
//    }
//    //特殊坐标
//    public RectTransform backGround;
//    //显示数据值的字典
//    public Dictionary<string, Vector3> ruler;
//    //用于坐标上的数据显示
//    public RectTransform word;
//    public GameObject posTextPfb;

//    //两坐标的Render
//    public LineRenderer axisX;
//    public LineRenderer axisY;
//    //文字显示
//    private List texts = new List();

//    ///
//    /// 自适应最大值和最小值（Y）
//    ///
//    void OnEnable()
//    {
//        instence = this;
//        StartCoroutine("WaitToInit");
//    }
//    IEnumerator WaitToInit()
//    {
//        yield return null;
//        //初始化坐标值 
//        InitGridData();
//        //计算并生成刻度，显示
//        CalcAndDrawGrid(numY);
//    }
//    ///
//    /// 初始化格子信息（长度坐标 ）
//    ///
//    private void InitGridData()
//    {
//        #region 得到四角坐标、长度、宽度

//        //backGround.GetWorldCorners(Chart.corners);
//        backGround.GetLocalCorners(Chart.corners);
//        Chart.leftBottom = Chart.corners[0];
//        Chart.leftUp = Chart.corners[1];
//        Chart.rightBottom = Chart.corners[2];
//        Chart.rightBottom = Chart.corners[3];
//        Chart.length = Chart.length > Chart.corners[2].x - Chart.corners[1].x ? Chart.length : Chart.corners[2].x - Chart.corners[1].x;
//        Chart.hight = Chart.hight > Chart.corners[1].y - Chart.corners[0].y ? Chart.hight : Chart.corners[1].y - Chart.corners[0].y;
//        //Debug.Log(Chart.corners[0]);
//        //Debug.Log(Chart.corners[1]);
//        //Debug.Log(Chart.corners[2]);
//        //Debug.Log(Chart.corners[3]);
//        //Debug.Log(Chart.hight);
//        //Debug.Log(Chart.length);
//        #endregion
//    }
//    ///
//    /// 刻度坐标，并保存到指定字典中
//    ///
//    private void CalcScale(ref Dictionary<string, Vector3> ruler, int numY)
//    {
//        if (numY == 0)
//        {
//            Debug.Log(false);
//            return;
//        }
//        #region 计算刻度
//        //初始化字典
//        ruler = new Dictionary<string, Vector3>();

//        float offset = Chart.hight / numY;//坐标间隔
//        for (float i = 0; i <= Chart.hight; i += offset)
//        {
//            //舍去第一个点
//            if (i == 0) { continue; }
//            //计算用于显示的数据
//            float num = i * (maxY - minY) / Chart.hight + minY;
//            ruler.Add(num.ToString("0"), transform.position + transform.up * i);
//        }
//    }
//    #endregion
//    ///
//    /// 绘制Chart
//    ///
//    private void DrawChart(List texts)
//    {
//        #region 绘制x,y轴

//        axisX.SetVertexCount(2);
//        axisX.SetPositions(new Vector3[2] { Vector3.zero, Chart.length * new Vector3(1, 0, 0) });//length * axisX.transform.right
//        axisY.SetVertexCount(2);
//        axisY.SetPositions(new Vector3[2] { Vector3.zero, Chart.hight * new Vector3(0, 1, 0) });

//        //用于保存创建出来的字体
//        texts = new List();
//        //绘制数字
//        foreach (var item in ruler)
//        {

//            GameObject go = ObjectPool.Instence.GetPoolObject(posTextPfb, word, item.Value);
//            //坐标修定（世界坐标无法获取（canvas比例问题））
//            go.transform.localPosition = item.Value - transform.position;
//            //设置 坐标线
//            go.GetComponent().SetPositions(new Vector3[] { Vector3.zero, Chart.length * Vector3.right });
//            //设置 文字
//            go.GetComponent().text = item.Key;
//            //记录
//            texts.Add(go);
//        }
//        #endregion
//    }

//    ///
//    /// 公共调用部分
//    ///
//    /// 提供利用坐标值反获取坐标比例

//    public float GetPosYByWorths(float num)
//    {
//        num = (num - minY) * Chart.hight / (maxY - minY);
//        return num;
//    }
//    ///
//    /// 当信息发生改变后，需要重新得到格子坐标
//    public void CalcAndDrawGrid(int numy)
//    {
//        //清理残留
//        for (int i = 0; i < texts.Count; i++)
//        {
//            Destroy(texts[i].gameObject);
//        }
//        //重新计算刻度 
//        CalcScale(ref ruler, numy);
//        //绘制表格
//        DrawChart(texts);

//    }


//    // Start is called before the first frame update
//    void Start()
//    {

//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }
//}
