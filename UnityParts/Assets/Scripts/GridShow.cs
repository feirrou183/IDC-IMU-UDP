//using UnityEngine;
//using System.Collections.Generic;
//using System.Collections;
//using UnityEngine.UI;

//public class GridShow : MonoBehaviour
//{

//    ///
//    /// ��Ҫ��ʾ������Ϣ�����ӹ���
//    ///
//    public static GridShow instence;
//    //���ݵķ�Χ(y����)
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
//    //y������ʾ�����ݸ���
//    public Slider numYSlider;
//    public int numY
//    {
//        get { return (int)numYSlider.value; }
//        set { numYSlider.value = value; }
//    }
//    //��������
//    public RectTransform backGround;
//    //��ʾ����ֵ���ֵ�
//    public Dictionary<string, Vector3> ruler;
//    //���������ϵ�������ʾ
//    public RectTransform word;
//    public GameObject posTextPfb;

//    //�������Render
//    public LineRenderer axisX;
//    public LineRenderer axisY;
//    //������ʾ
//    private List texts = new List();

//    ///
//    /// ����Ӧ���ֵ����Сֵ��Y��
//    ///
//    void OnEnable()
//    {
//        instence = this;
//        StartCoroutine("WaitToInit");
//    }
//    IEnumerator WaitToInit()
//    {
//        yield return null;
//        //��ʼ������ֵ 
//        InitGridData();
//        //���㲢���ɿ̶ȣ���ʾ
//        CalcAndDrawGrid(numY);
//    }
//    ///
//    /// ��ʼ��������Ϣ���������� ��
//    ///
//    private void InitGridData()
//    {
//        #region �õ��Ľ����ꡢ���ȡ����

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
//    /// �̶����꣬�����浽ָ���ֵ���
//    ///
//    private void CalcScale(ref Dictionary<string, Vector3> ruler, int numY)
//    {
//        if (numY == 0)
//        {
//            Debug.Log(false);
//            return;
//        }
//        #region ����̶�
//        //��ʼ���ֵ�
//        ruler = new Dictionary<string, Vector3>();

//        float offset = Chart.hight / numY;//������
//        for (float i = 0; i <= Chart.hight; i += offset)
//        {
//            //��ȥ��һ����
//            if (i == 0) { continue; }
//            //����������ʾ������
//            float num = i * (maxY - minY) / Chart.hight + minY;
//            ruler.Add(num.ToString("0"), transform.position + transform.up * i);
//        }
//    }
//    #endregion
//    ///
//    /// ����Chart
//    ///
//    private void DrawChart(List texts)
//    {
//        #region ����x,y��

//        axisX.SetVertexCount(2);
//        axisX.SetPositions(new Vector3[2] { Vector3.zero, Chart.length * new Vector3(1, 0, 0) });//length * axisX.transform.right
//        axisY.SetVertexCount(2);
//        axisY.SetPositions(new Vector3[2] { Vector3.zero, Chart.hight * new Vector3(0, 1, 0) });

//        //���ڱ��洴������������
//        texts = new List();
//        //��������
//        foreach (var item in ruler)
//        {

//            GameObject go = ObjectPool.Instence.GetPoolObject(posTextPfb, word, item.Value);
//            //�����޶������������޷���ȡ��canvas�������⣩��
//            go.transform.localPosition = item.Value - transform.position;
//            //���� ������
//            go.GetComponent().SetPositions(new Vector3[] { Vector3.zero, Chart.length * Vector3.right });
//            //���� ����
//            go.GetComponent().text = item.Key;
//            //��¼
//            texts.Add(go);
//        }
//        #endregion
//    }

//    ///
//    /// �������ò���
//    ///
//    /// �ṩ��������ֵ����ȡ�������

//    public float GetPosYByWorths(float num)
//    {
//        num = (num - minY) * Chart.hight / (maxY - minY);
//        return num;
//    }
//    ///
//    /// ����Ϣ�����ı����Ҫ���µõ���������
//    public void CalcAndDrawGrid(int numy)
//    {
//        //�������
//        for (int i = 0; i < texts.Count; i++)
//        {
//            Destroy(texts[i].gameObject);
//        }
//        //���¼���̶� 
//        CalcScale(ref ruler, numy);
//        //���Ʊ��
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
