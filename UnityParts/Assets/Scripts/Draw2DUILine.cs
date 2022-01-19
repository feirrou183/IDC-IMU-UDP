using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Draw2DUILine : MonoBehaviour {

    [Header("Color Setting")]
    public Color colorLine1 = Color.black;
    public Color colorLineX = Color.red;
    public Color colorLineY = Color.blue;
    public Color colorLineZ = Color.yellow;
    public Color32 BackGround = Color.white;
    public Color32 zeroColor = Color.red;

    [Header("Transform Setting")]
    public Transform m_Neck;
    public Transform m_Spine;
    public Transform m_UpperArmLeft;
    public Transform m_LowerArmLeft;
    public Transform m_UpperArmRight;
    public Transform m_LowerArmRight;

    [Header("Raw Image Setting")]
    [SerializeField]
    public RawImage bgImage;
    public RawImage m_NeckImage_X;
    public RawImage m_NeckImage_Y;
    public RawImage m_NeckImage_Z;
    public RawImage m_SpineImage_X;
    public RawImage m_SpineImage_Y;
    public RawImage m_SpineImage_Z;
    public RawImage m_UpperArmLeftImage_X;
    public RawImage m_UpperArmLeftImage_Y;
    public RawImage m_UpperArmLeftImage_Z;
    public RawImage m_LowerArmLeftImage_X;
    public RawImage m_LowerArmLeftImage_Y;
    public RawImage m_LowerArmLeftImage_Z;
    public RawImage m_UpperArmRightImage_X;
    public RawImage m_UpperArmRightImage_Y;
    public RawImage m_UpperArmRightImage_Z;
    public RawImage m_LowerArmRightImage_X;
    public RawImage m_LowerArmRightImage_Y;
    public RawImage m_LowerArmRightImage_Z;

    [Header("Pixels Setting")]
    [SerializeField]
    public float height = 0.34f;
    [SerializeField]
    public float width=0.35f;

    private int widthPixels;
    private int heightPixels;


    //Texture2D是用来画图的
    private Texture2D bgTexture;//测试用，可删除
    private Texture2D NeckXTexture;
    private Texture2D NeckYTexture;
    private Texture2D NeckZTexture;
    private Texture2D SpineXTexture;
    private Texture2D SpineYTexture;
    private Texture2D SpineZTexture;
    private Texture2D UpperArmLeftXTexture;
    private Texture2D UpperArmLeftYTexture;
    private Texture2D UpperArmLeftZTexture;
    private Texture2D UpperArmRightXTexture;
    private Texture2D UpperArmRightYTexture;
    private Texture2D UpperArmRightZTexture;
    private Texture2D LowerArmLeftXTexture;
    private Texture2D LowerArmLeftYTexture;
    private Texture2D LowerArmLeftZTexture;
    private Texture2D LowerArmRightXTexture;
    private Texture2D LowerArmRightYTexture;
    private Texture2D LowerArmRightZTexture;

    //各数据像素矩阵，通过改变像素矩阵的值来显示
    private Color32[] pixelsBg;
    private Color32[] pixelsDrawLine;
    private Color32[] pixelsDrawLineNeckX;
    private Color32[] pixelsDrawLineNeckY;
    private Color32[] pixelsDrawLineNeckZ;
    private Color32[] pixelsDrawLineSpineX;
    private Color32[] pixelsDrawLineSpineY;
    private Color32[] pixelsDrawLineSpineZ;
    private Color32[] pixelsDrawLineUpperArmLeftX;
    private Color32[] pixelsDrawLineUpperArmLeftY;
    private Color32[] pixelsDrawLineUpperArmLeftZ;
    private Color32[] pixelsDrawLineUpperArmRightX;
    private Color32[] pixelsDrawLineUpperArmRightY;
    private Color32[] pixelsDrawLineUpperArmRightZ;
    private Color32[] pixelsDrawLineLowerArmLeftX;
    private Color32[] pixelsDrawLineLowerArmLeftY;
    private Color32[] pixelsDrawLineLowerArmLeftZ;
    private Color32[] pixelsDrawLineLowerArmRightX;
    private Color32[] pixelsDrawLineLowerArmRightY;
    private Color32[] pixelsDrawLineLowerArmRightZ;

    //存放数据点
    private List<Vector2> listPoints = new List<Vector2>();
    private List<Vector2> listPointsNeckX = new List<Vector2>();
    private List<Vector2> listPointsNeckY = new List<Vector2>();
    private List<Vector2> listPointsNeckZ = new List<Vector2>();
    private List<Vector2> listPointsSpineX = new List<Vector2>();
    private List<Vector2> listPointsSpineY = new List<Vector2>();
    private List<Vector2> listPointsSpineZ = new List<Vector2>();
    private List<Vector2> listPointsUpperArmLeftX = new List<Vector2>();
    private List<Vector2> listPointsUpperArmLeftY = new List<Vector2>();
    private List<Vector2> listPointsUpperArmLeftZ = new List<Vector2>();
    private List<Vector2> listPointsUpperArmRightX = new List<Vector2>();
    private List<Vector2> listPointsUpperArmRightY = new List<Vector2>();
    private List<Vector2> listPointsUpperArmRightZ = new List<Vector2>();
    private List<Vector2> listPointsLowerArmLeftX = new List<Vector2>();
    private List<Vector2> listPointsLowerArmLeftY = new List<Vector2>();
    private List<Vector2> listPointsLowerArmLeftZ = new List<Vector2>();
    private List<Vector2> listPointsLowerArmRightX = new List<Vector2>();
    private List<Vector2> listPointsLowerArmRightY = new List<Vector2>();
    private List<Vector2> listPointsLowerArmRightZ = new List<Vector2>();

    private System.Random ra;
    private float countTime;
    private int dataNum;//暂定一次显示10个数据
    private int dx;//每份数据横坐标间隔
    private int index;

    

    // Use this for initialization
    void Start () {

        ra = new System.Random();
        countTime = 0;
        dataNum = 10;
        
        widthPixels = (int)(Screen.width * width);
        heightPixels = (int)(Screen.height * height);
        
        //创建背景贴图

        bgTexture = new Texture2D(widthPixels, heightPixels);//测试用，可删除

        NeckXTexture = new Texture2D(widthPixels, heightPixels);
        NeckYTexture = new Texture2D(widthPixels, heightPixels);
        NeckZTexture = new Texture2D(widthPixels, heightPixels);
        SpineXTexture = new Texture2D(widthPixels, heightPixels);
        SpineYTexture = new Texture2D(widthPixels, heightPixels);
        SpineZTexture = new Texture2D(widthPixels, heightPixels);
        UpperArmLeftXTexture = new Texture2D(widthPixels, heightPixels);
        UpperArmLeftYTexture = new Texture2D(widthPixels, heightPixels);
        UpperArmLeftZTexture = new Texture2D(widthPixels, heightPixels);
        UpperArmRightXTexture = new Texture2D(widthPixels, heightPixels);
        UpperArmRightYTexture = new Texture2D(widthPixels, heightPixels);
        UpperArmRightZTexture = new Texture2D(widthPixels, heightPixels);
        LowerArmLeftXTexture = new Texture2D(widthPixels, heightPixels);
        LowerArmLeftYTexture = new Texture2D(widthPixels, heightPixels);
        LowerArmLeftZTexture = new Texture2D(widthPixels, heightPixels);
        LowerArmRightXTexture = new Texture2D(widthPixels, heightPixels);
        LowerArmRightYTexture = new Texture2D(widthPixels, heightPixels);
        LowerArmRightZTexture = new Texture2D(widthPixels, heightPixels);


        bgImage.texture = bgTexture;
        bgImage.SetNativeSize();
        m_NeckImage_X.texture = NeckXTexture;
        m_NeckImage_Y.texture = NeckYTexture;
        m_NeckImage_Z.texture = NeckZTexture;
        m_NeckImage_X.SetNativeSize();
        m_NeckImage_Y.SetNativeSize();
        m_NeckImage_Z.SetNativeSize();
        m_SpineImage_X.texture = SpineXTexture;
        m_SpineImage_Y.texture = SpineYTexture;
        m_SpineImage_Z.texture = SpineZTexture;
        m_SpineImage_X.SetNativeSize();
        m_SpineImage_Y.SetNativeSize();
        m_SpineImage_Z.SetNativeSize();
        m_UpperArmLeftImage_X.texture = UpperArmLeftXTexture;
        m_UpperArmLeftImage_Y.texture = UpperArmLeftYTexture;
        m_UpperArmLeftImage_Z.texture = UpperArmLeftZTexture;
        m_UpperArmLeftImage_X.SetNativeSize();
        m_UpperArmLeftImage_Y.SetNativeSize();
        m_UpperArmLeftImage_Z.SetNativeSize();
        m_UpperArmRightImage_X.texture = UpperArmRightXTexture;
        m_UpperArmRightImage_Y.texture = UpperArmRightYTexture;
        m_UpperArmRightImage_Z.texture = UpperArmRightZTexture;
        m_UpperArmRightImage_X.SetNativeSize();
        m_UpperArmRightImage_Y.SetNativeSize();
        m_UpperArmRightImage_Z.SetNativeSize();
        m_LowerArmLeftImage_X.texture = LowerArmLeftXTexture;
        m_LowerArmLeftImage_Y.texture = LowerArmLeftYTexture;
        m_LowerArmLeftImage_Z.texture = LowerArmLeftZTexture;
        m_LowerArmLeftImage_X.SetNativeSize();
        m_LowerArmLeftImage_Y.SetNativeSize();
        m_LowerArmLeftImage_Z.SetNativeSize();
        m_LowerArmRightImage_X.texture = LowerArmRightXTexture;
        m_LowerArmRightImage_Y.texture = LowerArmRightYTexture;
        m_LowerArmRightImage_Z.texture = LowerArmRightZTexture;
        m_LowerArmRightImage_X.SetNativeSize();
        m_LowerArmRightImage_Y.SetNativeSize();
        m_LowerArmRightImage_Z.SetNativeSize();

        pixelsDrawLine = new Color32[widthPixels * heightPixels];
        pixelsDrawLineNeckX = new Color32[widthPixels * heightPixels];
        pixelsDrawLineNeckY = new Color32[widthPixels * heightPixels];
        pixelsDrawLineNeckZ = new Color32[widthPixels * heightPixels];
        pixelsDrawLineSpineX = new Color32[widthPixels * heightPixels];
        pixelsDrawLineSpineY = new Color32[widthPixels * heightPixels];
        pixelsDrawLineSpineZ = new Color32[widthPixels * heightPixels];
        pixelsDrawLineUpperArmLeftX = new Color32[widthPixels * heightPixels];
        pixelsDrawLineUpperArmLeftY = new Color32[widthPixels * heightPixels];
        pixelsDrawLineUpperArmLeftZ = new Color32[widthPixels * heightPixels];
        pixelsDrawLineUpperArmRightX = new Color32[widthPixels * heightPixels];
        pixelsDrawLineUpperArmRightY = new Color32[widthPixels * heightPixels];
        pixelsDrawLineUpperArmRightZ = new Color32[widthPixels * heightPixels];
        pixelsDrawLineLowerArmLeftX = new Color32[widthPixels * heightPixels];
        pixelsDrawLineLowerArmLeftY = new Color32[widthPixels * heightPixels];
        pixelsDrawLineLowerArmLeftZ = new Color32[widthPixels * heightPixels];
        pixelsDrawLineLowerArmRightX = new Color32[widthPixels * heightPixels];
        pixelsDrawLineLowerArmRightY = new Color32[widthPixels * heightPixels];
        pixelsDrawLineLowerArmRightZ = new Color32[widthPixels * heightPixels];

        //设置背景像素颜色
        pixelsBg = new Color32[widthPixels * heightPixels];
        for (int i = 0; i < pixelsBg.Length; ++i)
        {
            pixelsBg[i] = BackGround;
        }
        Debug.LogWarning("width/Screen.width/widthPixels :" + width.ToString() + " / " + Screen.width.ToString() + " / " + widthPixels.ToString());
        Debug.LogWarning("height/Screen.height/heightPixels :" + height.ToString() + " / " + Screen.height.ToString() + " / " + heightPixels.ToString());

        //横坐标是根据widthPixels确定的
        dx = widthPixels / dataNum;
        index = 0;//记录数据下下标
    }
	
	// Update is called once per frame
	void Update () {

        countTime += Time.deltaTime;
        if (countTime > 1)//一秒更新一次数据，测试用，可删除
        {
            //目前采取的方案是暂时显示10个数据（dataNum）
            //
            int x;
            int y;
            
            x = dx * index;
            index++;

            y = ra.Next(10, widthPixels);
            Vector2 point = new Vector2(x, y);//测试用
            //获取transform的值，构建点
            Vector2 pointNeckX = new Vector2(x, m_Neck.eulerAngles.x);
            Vector2 pointNeckY = new Vector2(x, m_Neck.eulerAngles.y);
            Vector2 pointNeckZ = new Vector2(x, m_Neck.eulerAngles.z);
            Vector2 pointSpineX = new Vector2(x, m_Spine.eulerAngles.x);
            Vector2 pointSpineY = new Vector2(x, m_Spine.eulerAngles.y);
            Vector2 pointSpineZ = new Vector2(x, m_Spine.eulerAngles.z);
            Vector2 pointUpperArmLeftX = new Vector2(x, m_UpperArmLeft.eulerAngles.x);
            Vector2 pointUpperArmLeftY = new Vector2(x, m_UpperArmLeft.eulerAngles.y);
            Vector2 pointUpperArmLeftZ = new Vector2(x, m_UpperArmLeft.eulerAngles.z);
            Vector2 pointUpperArmRightX = new Vector2(x, m_UpperArmRight.eulerAngles.x);
            Vector2 pointUpperArmRightY = new Vector2(x, m_UpperArmRight.eulerAngles.y);
            Vector2 pointUpperArmRightZ = new Vector2(x, m_UpperArmRight.eulerAngles.z);
            Vector2 pointLowerArmLeftX = new Vector2(x, m_LowerArmLeft.eulerAngles.x);
            Vector2 pointLowerArmLeftY = new Vector2(x, m_LowerArmLeft.eulerAngles.y);
            Vector2 pointLowerArmLeftZ = new Vector2(x, m_LowerArmLeft.eulerAngles.z);
            Vector2 pointLowerArmRightX = new Vector2(x, m_LowerArmRight.eulerAngles.x);
            Vector2 pointLowerArmRightY = new Vector2(x, m_LowerArmRight.eulerAngles.y);
            Vector2 pointLowerArmRightZ = new Vector2(x, m_LowerArmRight.eulerAngles.z);

            //添加进list中
            listPoints.Add(point);
            listPointsNeckX.Add(pointNeckX);
            listPointsNeckY.Add(pointNeckY);
            listPointsNeckZ.Add(pointNeckZ);
            listPointsSpineX.Add(pointSpineX);
            listPointsSpineY.Add(pointSpineY);
            listPointsSpineZ.Add(pointSpineZ);
            listPointsUpperArmLeftX.Add(pointUpperArmLeftX);
            listPointsUpperArmLeftY.Add(pointUpperArmLeftY);
            listPointsUpperArmLeftZ.Add(pointUpperArmLeftZ);
            listPointsUpperArmRightX.Add(pointUpperArmRightX);
            listPointsUpperArmRightY.Add(pointUpperArmRightY);
            listPointsUpperArmRightZ.Add(pointUpperArmRightZ);
            listPointsLowerArmLeftX.Add(pointLowerArmLeftX);
            listPointsLowerArmLeftY.Add(pointLowerArmLeftY);
            listPointsLowerArmLeftZ.Add(pointLowerArmLeftZ);
            listPointsLowerArmRightX.Add(pointLowerArmRightX);
            listPointsLowerArmRightY.Add(pointLowerArmRightY);
            listPointsLowerArmRightZ.Add(pointLowerArmRightZ);


            //如果超过dataNum个数据，则依次往后移
            if (listPointsNeckX.Count > dataNum)
            {
                //移除第一个元素
                listPoints.RemoveAt(0);
                listPointsNeckX.RemoveAt(0);
                listPointsNeckY.RemoveAt(0);
                listPointsNeckZ.RemoveAt(0);
                listPointsSpineX.RemoveAt(0);
                listPointsSpineY.RemoveAt(0);
                listPointsSpineZ.RemoveAt(0);
                listPointsUpperArmLeftX.RemoveAt(0);
                listPointsUpperArmLeftY.RemoveAt(0);
                listPointsUpperArmLeftZ.RemoveAt(0);
                listPointsUpperArmRightX.RemoveAt(0);
                listPointsUpperArmRightY.RemoveAt(0);
                listPointsUpperArmRightZ.RemoveAt(0);
                listPointsLowerArmLeftX.RemoveAt(0);
                listPointsLowerArmLeftY.RemoveAt(0);
                listPointsLowerArmLeftZ.RemoveAt(0);
                listPointsLowerArmRightX.RemoveAt(0);
                listPointsLowerArmRightY.RemoveAt(0);
                listPointsLowerArmRightZ.RemoveAt(0);

                //创建临时list存放往后移之后的数据
                List<Vector2> tmpList = new List<Vector2>();//测试用
                List<Vector2> tmpListNeckX = new List<Vector2>();
                List<Vector2> tmpListNeckY = new List<Vector2>();
                List<Vector2> tmpListNeckZ = new List<Vector2>();
                List<Vector2> tmpListSpineX = new List<Vector2>();
                List<Vector2> tmpListSpineY = new List<Vector2>();
                List<Vector2> tmpListSpineZ = new List<Vector2>();
                List<Vector2> tmpListUpperArmLeftX = new List<Vector2>();
                List<Vector2> tmpListUpperArmLeftY = new List<Vector2>();
                List<Vector2> tmpListUpperArmLeftZ = new List<Vector2>();
                List<Vector2> tmpListUpperArmRightX = new List<Vector2>();
                List<Vector2> tmpListUpperArmRightY = new List<Vector2>();
                List<Vector2> tmpListUpperArmRightZ = new List<Vector2>();
                List<Vector2> tmpListLowerArmLeftX = new List<Vector2>();
                List<Vector2> tmpListLowerArmLeftY = new List<Vector2>();
                List<Vector2> tmpListLowerArmLeftZ = new List<Vector2>();
                List<Vector2> tmpListLowerArmRightX = new List<Vector2>();
                List<Vector2> tmpListLowerArmRightY = new List<Vector2>();
                List<Vector2> tmpListLowerArmRightZ = new List<Vector2>();

                //后移（动态）
                for (int j = 0; j < listPointsNeckX.Count; j++)
                {
                    int tmpx = dx * j;
                    Vector2 tmpPoint = new Vector2(tmpx, listPoints[j].y);//测试用
                    Vector2 tmpPointNeckX = new Vector2(tmpx, listPointsNeckX[j].y);
                    Vector2 tmpPointNeckY = new Vector2(tmpx, listPointsNeckY[j].y);
                    Vector2 tmpPointNeckZ = new Vector2(tmpx, listPointsNeckZ[j].y);
                    Vector2 tmpPointSpineX = new Vector2(tmpx, listPointsSpineX[j].y);
                    Vector2 tmpPointSpineY = new Vector2(tmpx, listPointsSpineY[j].y);
                    Vector2 tmpPointSpineZ = new Vector2(tmpx, listPointsSpineZ[j].y);
                    Vector2 tmpPointUpperArmLeftX = new Vector2(tmpx, listPointsUpperArmLeftX[j].y);
                    Vector2 tmpPointUpperArmLeftY = new Vector2(tmpx, listPointsUpperArmLeftY[j].y);
                    Vector2 tmpPointUpperArmLeftZ = new Vector2(tmpx, listPointsUpperArmLeftZ[j].y);
                    Vector2 tmpPointUpperArmRightX = new Vector2(tmpx, listPointsUpperArmRightX[j].y);
                    Vector2 tmpPointUpperArmRightY = new Vector2(tmpx, listPointsUpperArmRightY[j].y);
                    Vector2 tmpPointUpperArmRightZ = new Vector2(tmpx, listPointsUpperArmRightZ[j].y);
                    Vector2 tmpPointLowerArmLeftX = new Vector2(tmpx, listPointsLowerArmLeftX[j].y);
                    Vector2 tmpPointLowerArmLeftY = new Vector2(tmpx, listPointsLowerArmLeftY[j].y);
                    Vector2 tmpPointLowerArmLeftZ = new Vector2(tmpx, listPointsLowerArmLeftZ[j].y);
                    Vector2 tmpPointLowerArmRightX = new Vector2(tmpx, listPointsLowerArmRightX[j].y);
                    Vector2 tmpPointLowerArmRightY = new Vector2(tmpx, listPointsLowerArmRightY[j].y);
                    Vector2 tmpPointLowerArmRightZ = new Vector2(tmpx, listPointsLowerArmRightZ[j].y);
                    
                    tmpList.Add(tmpPoint);
                    tmpListNeckX.Add(tmpPointNeckX);
                    tmpListNeckY.Add(tmpPointNeckY);
                    tmpListNeckZ.Add(tmpPointNeckZ);
                    tmpListSpineX.Add(tmpPointSpineX);
                    tmpListSpineY.Add(tmpPointSpineY);
                    tmpListSpineZ.Add(tmpPointSpineZ);
                    tmpListUpperArmLeftX.Add(tmpPointUpperArmLeftX);
                    tmpListUpperArmLeftY.Add(tmpPointUpperArmLeftY);
                    tmpListUpperArmLeftZ.Add(tmpPointUpperArmLeftZ);
                    tmpListUpperArmRightX.Add(tmpPointUpperArmRightX);
                    tmpListUpperArmRightY.Add(tmpPointUpperArmRightY);
                    tmpListUpperArmRightZ.Add(tmpPointUpperArmRightZ);
                    tmpListLowerArmLeftX.Add(tmpPointLowerArmLeftX);
                    tmpListLowerArmLeftY.Add(tmpPointLowerArmLeftY);
                    tmpListLowerArmLeftZ.Add(tmpPointLowerArmLeftZ);
                    tmpListLowerArmRightX.Add(tmpPointLowerArmRightX);
                    tmpListLowerArmRightY.Add(tmpPointLowerArmRightY);
                    tmpListLowerArmRightZ.Add(tmpPointLowerArmRightZ);
                }
                listPoints = tmpList;
                listPointsNeckX = tmpListNeckX;
                listPointsNeckY = tmpListNeckY;
                listPointsNeckZ = tmpListNeckZ;
                listPointsSpineX = tmpListSpineX;
                listPointsSpineY = tmpListSpineY;
                listPointsSpineZ = tmpListSpineZ;
                listPointsUpperArmLeftX = tmpListUpperArmLeftX;
                listPointsUpperArmLeftY = tmpListUpperArmLeftY;
                listPointsUpperArmLeftZ = tmpListUpperArmLeftZ;
                listPointsUpperArmRightX = tmpListUpperArmRightX;
                listPointsUpperArmRightY = tmpListUpperArmRightY;
                listPointsUpperArmRightZ = tmpListUpperArmRightZ;
                listPointsLowerArmLeftX = tmpListLowerArmLeftX;
                listPointsLowerArmLeftY = tmpListLowerArmLeftY;
                listPointsLowerArmLeftZ = tmpListLowerArmLeftZ;
                listPointsLowerArmRightX = tmpListLowerArmRightX;
                listPointsLowerArmRightY = tmpListLowerArmRightY;
                listPointsLowerArmRightZ = tmpListLowerArmRightZ;


                index = dataNum - 1;
            }
                    
               
            countTime = 0;

        }


        // Clear.
        Array.Copy(pixelsBg, pixelsDrawLine, pixelsBg.Length);
        Array.Copy(pixelsBg, pixelsDrawLineNeckX, pixelsBg.Length);
        Array.Copy(pixelsBg, pixelsDrawLineNeckY, pixelsBg.Length);
        Array.Copy(pixelsBg, pixelsDrawLineNeckZ, pixelsBg.Length);
        Array.Copy(pixelsBg, pixelsDrawLineSpineX, pixelsBg.Length);
        Array.Copy(pixelsBg, pixelsDrawLineSpineY, pixelsBg.Length);
        Array.Copy(pixelsBg, pixelsDrawLineSpineZ, pixelsBg.Length);
        Array.Copy(pixelsBg, pixelsDrawLineUpperArmLeftX, pixelsBg.Length);
        Array.Copy(pixelsBg, pixelsDrawLineUpperArmLeftY, pixelsBg.Length);
        Array.Copy(pixelsBg, pixelsDrawLineUpperArmLeftZ, pixelsBg.Length);
        Array.Copy(pixelsBg, pixelsDrawLineUpperArmRightX, pixelsBg.Length);
        Array.Copy(pixelsBg, pixelsDrawLineUpperArmRightY, pixelsBg.Length);
        Array.Copy(pixelsBg, pixelsDrawLineUpperArmRightZ, pixelsBg.Length);
        Array.Copy(pixelsBg, pixelsDrawLineLowerArmLeftX, pixelsBg.Length);
        Array.Copy(pixelsBg, pixelsDrawLineLowerArmLeftY, pixelsBg.Length);
        Array.Copy(pixelsBg, pixelsDrawLineLowerArmLeftZ, pixelsBg.Length);
        Array.Copy(pixelsBg, pixelsDrawLineLowerArmRightX, pixelsBg.Length);
        Array.Copy(pixelsBg, pixelsDrawLineLowerArmRightY, pixelsBg.Length);
        Array.Copy(pixelsBg, pixelsDrawLineLowerArmRightZ, pixelsBg.Length);

        // 基准线
        //DrawLine(new Vector2(0f, heightPixels * 0.5f), new Vector2(widthPixels, heightPixels * 0.5f), zeroColor);

        for (int i = 0; i < listPoints.Count-1; i++)
        {
            Vector2 from = listPoints[i];
            Vector2 to = listPoints[i + 1];
            DrawLine(ref pixelsDrawLine, new Vector2(0f, heightPixels * 0.5f), new Vector2(widthPixels, heightPixels * 0.5f), zeroColor);
            DrawLine(ref pixelsDrawLine, from, to, colorLine1);
        }
        bgTexture.SetPixels32(pixelsDrawLine);
        bgTexture.Apply();

        for (int i = 0; i < listPointsNeckX.Count - 1; i++)
        {
            Vector2 from = listPointsNeckX[i];
            Vector2 to = listPointsNeckX[i + 1];
            DrawLine(ref pixelsDrawLineNeckX, new Vector2(0f, heightPixels * 0.5f), new Vector2(widthPixels, heightPixels * 0.5f), zeroColor);
            DrawLine(ref pixelsDrawLineNeckX, from, to, colorLineX);
        }
        NeckXTexture.SetPixels32(pixelsDrawLineNeckX);
        NeckXTexture.Apply();

        for (int i = 0; i < listPointsNeckY.Count - 1; i++)
        {
            Vector2 from = listPointsNeckY[i];
            Vector2 to = listPointsNeckY[i + 1];
            DrawLine(ref pixelsDrawLineNeckY, new Vector2(0f, heightPixels * 0.5f), new Vector2(widthPixels, heightPixels * 0.5f), zeroColor);
            DrawLine(ref pixelsDrawLineNeckY, from, to, colorLineY);
        }
        NeckYTexture.SetPixels32(pixelsDrawLineNeckY);
        NeckYTexture.Apply();

        for (int i = 0; i < listPointsNeckZ.Count - 1; i++)
        {
            Vector2 from = listPointsNeckZ[i];
            Vector2 to = listPointsNeckZ[i + 1];
            DrawLine(ref pixelsDrawLineNeckZ, new Vector2(0f, heightPixels * 0.5f), new Vector2(widthPixels, heightPixels * 0.5f), zeroColor);
            DrawLine(ref pixelsDrawLineNeckZ, from, to, colorLineZ);
        }
        NeckZTexture.SetPixels32(pixelsDrawLineNeckZ);
        NeckZTexture.Apply();

        for (int i = 0; i < listPointsSpineX.Count - 1; i++)
        {
            Vector2 from = listPointsSpineX[i];
            Vector2 to = listPointsSpineX[i + 1];
            DrawLine(ref pixelsDrawLineSpineX, new Vector2(0f, heightPixels * 0.5f), new Vector2(widthPixels, heightPixels * 0.5f), zeroColor);
            DrawLine(ref pixelsDrawLineSpineX, from, to, colorLineX);
        }
        SpineXTexture.SetPixels32(pixelsDrawLineSpineX);
        SpineXTexture.Apply();

        for (int i = 0; i < listPointsUpperArmLeftX.Count - 1; i++)
        {
            Vector2 from = listPointsUpperArmLeftX[i];
            Vector2 to = listPointsUpperArmLeftX[i + 1];
            DrawLine(ref pixelsDrawLineUpperArmLeftX, new Vector2(0f, heightPixels * 0.5f), new Vector2(widthPixels, heightPixels * 0.5f), zeroColor);
            DrawLine(ref pixelsDrawLineUpperArmLeftX, from, to, colorLineX);
        }
        UpperArmLeftXTexture.SetPixels32(pixelsDrawLineUpperArmLeftX);
        UpperArmLeftXTexture.Apply();

        for (int i = 0; i < listPointsUpperArmRightX.Count - 1; i++)
        {
            Vector2 from = listPointsUpperArmRightX[i];
            Vector2 to = listPointsUpperArmRightX[i + 1];
            DrawLine(ref pixelsDrawLineUpperArmRightX, new Vector2(0f, heightPixels * 0.5f), new Vector2(widthPixels, heightPixels * 0.5f), zeroColor);
            DrawLine(ref pixelsDrawLineUpperArmRightX, from, to, colorLineX);
        }
        UpperArmRightXTexture.SetPixels32(pixelsDrawLineUpperArmRightX);
        UpperArmRightXTexture.Apply();

        for (int i = 0; i < listPointsLowerArmLeftX.Count - 1; i++)
        {
            Vector2 from = listPointsLowerArmLeftX[i];
            Vector2 to = listPointsLowerArmLeftX[i + 1];
            DrawLine(ref pixelsDrawLineLowerArmLeftX, new Vector2(0f, heightPixels * 0.5f), new Vector2(widthPixels, heightPixels * 0.5f), zeroColor);
            DrawLine(ref pixelsDrawLineLowerArmLeftX, from, to, colorLineX);
        }
        LowerArmLeftXTexture.SetPixels32(pixelsDrawLineLowerArmLeftX);
        LowerArmLeftXTexture.Apply();

        for (int i = 0; i < listPointsLowerArmRightX.Count - 1; i++)
        {
            Vector2 from = listPointsLowerArmRightX[i];
            Vector2 to = listPointsLowerArmRightX[i + 1];
            DrawLine(ref pixelsDrawLineLowerArmRightX, new Vector2(0f, heightPixels * 0.5f), new Vector2(widthPixels, heightPixels * 0.5f), zeroColor);
            DrawLine(ref pixelsDrawLineLowerArmRightX, from, to, colorLineX);
        }
        LowerArmRightXTexture.SetPixels32(pixelsDrawLineLowerArmRightX);
        LowerArmRightXTexture.Apply();

    }

    void DrawLine(ref Color32[] pixelsDrawLine, Vector2 from, Vector2 to, Color32 color)
    {
        int i;
        int j;

        if (Mathf.Abs(to.x - from.x) > Mathf.Abs(to.y - from.y))
        {
            // Horizontal line.
            i = 0;
            j = 1;
        }
        else
        {
            // Vertical line.
            i = 1;
            j = 0;
        }

        int x = (int)from[i];
        int delta = (int)Mathf.Sign(to[i] - from[i]);
        while (x != (int)to[i])
        {
            int y = (int)Mathf.Round(from[j] + (x - from[i]) * (to[j] - from[j]) / (to[i] - from[i]));

            int index;
            if (i == 0)
                index = y * widthPixels + x;
            else
                index = x * widthPixels + y;

            index = Mathf.Clamp(index, 0, pixelsDrawLine.Length - 1);
            pixelsDrawLine[index] = color;

            x += delta;
        }
    }
}
