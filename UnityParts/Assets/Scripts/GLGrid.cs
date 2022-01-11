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
    //�����
    public Vector3 zero;
    public Vector3 endPos;
    public Vector3 rightPos;
    public Vector3 upPos;
    public Vector3[] corners = new Vector3[4];
    //��ʾ���ֵ�Ԥ���塢
    public GameObject word;

    //������ʾ��Χ
    public float minY, maxY;
    //������
    public int numY;
    public int numX;
    //�����Ĵ�С(rect��
    //[HideInInspector]
    public float gridX;
    private float gridY;
    //��ʾ����ֵ���ֵ�
    public Dictionary<string, Vector3> ruler;// = new Dictionary<string, Vector3>();
    void Awake()
    {
        instence = this;
    }
    void Start()
    {
        Culcuate();//��������ӵ���������
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
        //����̶�
        float offset = gridY / numY;//������
        ruler = new Dictionary<string, Vector3>();
        for (float i = 0; i <= gridY; i += offset)
        {
            if (i == 0)
            {//��ȥ��һ����
                continue;
            }
            //����������ʾ������
            float num = i * (maxY - minY) / gridY + minY;
            ruler.Add(num.ToString("0"), zero + gridBg.transform.up * i);
        }
    }
    void OnPostRender()
    {
        #region GL��ͼ
        GL.PushMatrix();

        //��������ϵ
        DrawAxis();

        GL.PopMatrix();
        #endregion
        Culcuate();
    }
    ///
    /// ������
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
        //������
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
    /// ���Ʊ������
    ///
    private void DrawChart()
    {
        foreach (var item in ruler)
        {
            //�����֣�����������
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
    //�õ�����ֵy�������ԭ��ĳ��ȣ�
    public float GetPosYByWorths(float flo)
    {
        //������ֵ����
        flo = (flo - minY) * gridY / (maxY - minY);
        return flo;
    }
}
