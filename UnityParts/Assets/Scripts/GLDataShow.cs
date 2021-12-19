using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GLDataShow : MonoBehaviour
{
    //x方向的数据量
    public int numX;//真实数据个数
    // 用于坐标数据的保存
    public List<float> dataList = new List<float>();
    //坐标偏移量
    private float offsetX;
    // 
    public Material mat;
    IEnumerator Start()
    {
        yield return new WaitForFixedUpdate();
        //调试代码
        InvokeRepeating("Test", 1, 1);
        //计算偏移
        offsetX = GLGrid.instence.gridX / numX;
        Debug.Log(GLGrid.instence.gridX);
    }
    private void GetNewItem(float num)
    {
        if (dataList.Count > numX)
        {
            dataList.RemoveAt(0);
        }
        //将数据处理为坐标数据
        dataList.Add(GLGrid.instence.GetPosYByWorths(num));
    }
    void Test()
    {
        float num = Random.Range(10, 20f);
        GetNewItem(num);
    }
    void OnPostRender()
    {
        GL.PushMatrix();
        mat.SetPass(0);
        GL.Begin(GL.LINES);
        for (int i = 0; i < dataList.Count - 1; i++)
        {
            //float z = GLGrid.instence.zero.z;
            Vector3 start = GLGrid.instence.zero + dataList[i] * GLGrid.instence.gridBg.up + GLGrid.instence.gridBg.right * offsetX * i;
            Vector3 end = GLGrid.instence.zero + dataList[i + 1] * GLGrid.instence.gridBg.up + GLGrid.instence.gridBg.right * offsetX * (i + 1);
            Debug.Log(GLGrid.instence.zero - start);
            //Vector3 end = Vector3.zero;
            //绘制线  
            GL.Vertex(start);
            GL.Vertex(end);
        }
        GL.End();
        GL.PopMatrix();
    }
}
