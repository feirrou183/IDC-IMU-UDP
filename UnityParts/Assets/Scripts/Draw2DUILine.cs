using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Draw2DUILine : MonoBehaviour {

    public Color colorLine1 = Color.white;
    public List<Vector2> listPoints = new List<Vector2>(20);

    public Color32 bgColor = Color.white;
    public Color32 zeroColor = Color.black;

    [SerializeField]
    private RawImage bgImage;

    [SerializeField]
    private float height = 0.34f;
    [SerializeField]
    private float width=0.35f;

    private Texture2D bgTexture;
    private int widthPixels;
    private int heightPixels;

    private Color32[] pixelsBg;
    private Color32[] pixelsDrawLine;

    private System.Random ra;
    private float countTime;

    // Use this for initialization
    void Start () {

        ra = new System.Random();
        countTime = 0;

        //创建背景贴图
        widthPixels = (int)(Screen.width * width);
        heightPixels = (int)(Screen.height * height);
        bgTexture = new Texture2D(widthPixels, heightPixels);

        bgImage.texture = bgTexture;
        bgImage.SetNativeSize();

        pixelsDrawLine = new Color32[widthPixels * heightPixels];
        pixelsBg = new Color32[widthPixels * heightPixels];

        for (int i = 0; i < pixelsBg.Length; ++i)
        {
            pixelsBg[i] = bgColor;
        }
        Debug.LogWarning("width/Screen.width/widthPixels :" + width.ToString() + " / " + Screen.width.ToString() + " / " + widthPixels.ToString());
        Debug.LogWarning("height/Screen.height/heightPixels :" + height.ToString() + " / " + Screen.height.ToString() + " / " + heightPixels.ToString());

    }
	
	// Update is called once per frame
	void Update () {

        countTime += Time.deltaTime;
        if (countTime >= 2)
        {
            for (int i = 0; i < listPoints.Count-1; i++)
            {
                int x = ra.Next(1, 8);
                int y = ra.Next(1, 8);
                Vector2 point = new Vector2(x, y);
                listPoints[i] = point;
                Debug.Log("point: " + point.ToString());
            }
            countTime = 0;
        }
        
        
        // Clear.
        Array.Copy(pixelsBg, pixelsDrawLine, pixelsBg.Length);

        // 基准线
        DrawLine(new Vector2(0f, heightPixels * 0.5f), new Vector2(widthPixels, heightPixels * 0.5f), zeroColor);

        for (int i = 0; i < listPoints.Count-1; i++)
        {
            Vector2 from = listPoints[i];
            Vector2 to = listPoints[i + 1];
            DrawLine(from, to, colorLine1);
        }

        bgTexture.SetPixels32(pixelsDrawLine);
        bgTexture.Apply();
    }

    void DrawLine(Vector2 from, Vector2 to, Color32 color)
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
