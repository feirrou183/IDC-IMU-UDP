// See https://aka.ms/new-console-template for more information
// 参考：https://blog.csdn.net/u011484013/article/details/51131267

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Drawer;
using Player;
using System.IO;

class IMU
{
    public int deviceMac = 0;
    public float accX = 0;
    public float accY = 0;
    public float accZ = 0;
    public float RotX_Q = 0;  //四元数x,y,z,w
    public float RotY_Q = 0;
    public float RotZ_Q = 0;
    public float RotW_Q = 0;
};

public enum BodyPart:int
{
    Neck = 0,
    Spine = 2,
    UpperArmRight = 1,
    LowerArmRight = 3,
    UpperArmLeft = 4,
    LowerArmLeft = 5
}



public class PlayerMove : MonoBehaviour
{
    private const int NumOfIMU = 7;
    private const int NumOfQuaternion = 28;   //4 x 7
    //imu 由 1 - 7
    public Transform m_Neck;
    public Transform m_Spine;
    public Transform m_UpperArmLeft;
    public Transform m_LowerArmLeft;
    public Transform m_UpperArmRight;
    public Transform m_LowerArmRight;

    private Socket server;

    private bool closeFlag = false;

    //private IMU imu_Neck = new IMU();
    //private IMU imu_ClavicleLeft = new IMU();
    //private IMU imu_ClavicleRight = new IMU();
    //private IMU imu_UpperArmLeft = new IMU();
    //private IMU imu_UpperArmRight = new IMU();
    //private IMU imu_LowerArmLeft = new IMU();
    //private IMU imu_LowerArmRight = new IMU();

    private Dictionary<BodyPart,IMU> IMUs;
    private EndPoint point;
    private EndPoint pointEnd;
    private Dictionary<BodyPart, Quaternion> refQuatation;     //参考值
    private Dictionary<BodyPart, Quaternion> originQuatation;  //传感器初始值
    private Dictionary<BodyPart, Quaternion> defaultValue;     //校准Body初始值

    private DrawController drawController;
    private PlayerMoveController playerMoveController = new PlayerMoveController();

    private int timeStep = 0;
    private int timeCount = 0;

    private bool saveFlag = false;      //保存文件标志位，true:不断向字典内写入四元数

    private Dictionary<int,Dictionary<BodyPart, Quaternion>> saveFile;


    private void Awake()
    {
        defaultValue = new Dictionary<BodyPart, Quaternion>(NumOfIMU);
        defaultValue[BodyPart.Neck] = new Quaternion(m_Neck.transform.rotation.x, m_Neck.transform.rotation.y,
            m_Neck.transform.rotation.z, m_Neck.transform.rotation.w);
        defaultValue[BodyPart.Spine] = new Quaternion(m_Spine.transform.rotation.x, m_Spine.transform.rotation.y,
            m_Spine.transform.rotation.z, m_Spine.transform.rotation.w);
        defaultValue[BodyPart.UpperArmLeft] = new Quaternion(m_UpperArmLeft.transform.rotation.x, m_UpperArmLeft.transform.rotation.y,
            m_UpperArmLeft.transform.rotation.z, m_UpperArmLeft.transform.rotation.w);
        defaultValue[BodyPart.LowerArmLeft] = new Quaternion(m_LowerArmLeft.transform.rotation.x, m_LowerArmLeft.transform.rotation.y,
            m_LowerArmLeft.transform.rotation.z, m_LowerArmLeft.transform.rotation.w);
        defaultValue[BodyPart.UpperArmRight] = new Quaternion(m_UpperArmRight.transform.rotation.x, m_UpperArmRight.transform.rotation.y,
            m_UpperArmRight.transform.rotation.z, m_UpperArmRight.transform.rotation.w);
        defaultValue[BodyPart.LowerArmRight] = new Quaternion(m_LowerArmRight.transform.rotation.x, m_LowerArmRight.transform.rotation.y,
            m_LowerArmRight.transform.rotation.z, m_LowerArmRight.transform.rotation.w);

        playerMoveController.Neck = m_Neck;
        playerMoveController.Spine = m_Spine;
        playerMoveController.UpperArmLeft = m_UpperArmLeft;
        playerMoveController.UpperArmRight = m_UpperArmRight;
        playerMoveController.LowerArmLeft = m_LowerArmLeft;
        playerMoveController.LowerArmRight = m_LowerArmRight;
    }

    void Start()
    {
        drawController = GetComponent<DrawController>();

        // 调用摄像头函数
        ToOpenCamera();

        IMUs = new Dictionary<BodyPart, IMU>(NumOfIMU);
        refQuatation = new Dictionary<BodyPart, Quaternion>(NumOfIMU);
        originQuatation = new Dictionary<BodyPart, Quaternion>(NumOfIMU);
        saveFile = new Dictionary<int, Dictionary<BodyPart, Quaternion>>();
        foreach (BodyPart item in Enum.GetValues(typeof(BodyPart)))
        {
            IMUs.Add(item, new IMU());
            refQuatation.Add(item, new Quaternion(0, 0, 0, 1));
            originQuatation.Add(item, new Quaternion(0, 0, 0, 1));
        }

        string str = "#1;2;3";
        string ss = str.Substring(1);
        string[] strArray = ss.Split(';');

        Debug.LogWarning(string.Format("{0}  {1}  {2}", strArray[0], strArray[1], strArray[2]));

        server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        server.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9999));//绑定端口号和IP
        point = new IPEndPoint(IPAddress.Any, 0);//用来保存发送方的ip和端口号
        pointEnd = (EndPoint)point;

        Debug.LogWarning("服务端已经开启");
        Thread t = new Thread(ReciveMsg);//开启接收消息线程
        //t.IsBackground = true;
        t.Start();

        // Thread t2 = new Thread(sendMsg);//开启发送消息线程
        // t2.Start();

        Debug.LogWarning("执行完毕！");

    }

    /// <summary>
    /// 打开摄像机
    /// </summary>
    public void ToOpenCamera()
    {
        //StartCoroutine("OpenCamera");
    }

    void test()
    {
        for (int i = 0; i < 10000; i++)
            Debug.LogError("1");
    }

    private void resetMethod()
    {
        //重置方法
        m_Neck.rotation = defaultValue[BodyPart.Neck];
        m_Spine.rotation = defaultValue[BodyPart.Spine];
        m_UpperArmLeft.rotation = defaultValue[BodyPart.UpperArmLeft];
        m_LowerArmLeft.rotation = defaultValue[BodyPart.LowerArmLeft];
        m_UpperArmRight.rotation = defaultValue[BodyPart.UpperArmRight];
        m_LowerArmRight.rotation = defaultValue[BodyPart.LowerArmRight];

        timeStep = 0;
        drawController.ClearGraph();
        Debug.Log("Reset!");

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))   //重置命令
        {
            foreach (BodyPart item in Enum.GetValues(typeof(BodyPart)))
            {
                originQuatation[item] = refQuatation[item] * defaultValue[item];
            }
            resetMethod();
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            //保存文件
            SaveCsvFile();
        }
        setOri(IMUs);
    }

    void SaveCsvFile()
    {
        //写csv文件
        string vFileName = "F:\\IDC\\IDC-IMU-UDP\\Data\\data.csv";
        FileStream vFileStream = new FileStream(vFileName, FileMode.OpenOrCreate, FileAccess.Write);
        using (StreamWriter vStreamWriter = new StreamWriter(vFileStream, Encoding.UTF8))
        {
            StringBuilder vStringBuilder = new StringBuilder();
            //vStringBuilder.Append("00");
            //vStringBuilder.Append("01");
            //vStringBuilder.Append("02");
            //vStringBuilder.Append("03");
            //vStringBuilder.Append("04");
            //vStringBuilder.Append("05");
            //vStringBuilder.AppendLine("");

            for (int i = 1; i < saveFile.Count-1; i++)
            {

                //vStringBuilder.Append(string.Format("{0},",saveFile[i][BodyPart.Neck].ToString()));
                //vStringBuilder.Append(string.Format("{0},", saveFile[i][BodyPart.Spine].ToString()));
                vStringBuilder.Append(string.Format("{0},", saveFile[i][BodyPart.UpperArmLeft].ToString()));
                vStringBuilder.Append(string.Format("{0},", saveFile[i][BodyPart.UpperArmRight].ToString()));
                vStringBuilder.Append(string.Format("{0},", saveFile[i][BodyPart.LowerArmLeft].ToString()));
                vStringBuilder.Append(string.Format("{0},", saveFile[i][BodyPart.LowerArmRight].ToString()));
                //vStringBuilder.Append($"{i + 10},");
                //vStringBuilder.Append($"\"{i},{i * 10},{i * 100}\"");

                vStringBuilder.AppendLine("");
            }

            vStreamWriter.Write(vStringBuilder);
            vStreamWriter.Flush();
            vStreamWriter.Close();
            Debug.Log("SaveFile");
        }


    }

    void sendMsg()
    {
        EndPoint point = new IPEndPoint(IPAddress.Parse("169.254.202.67"), 6000);
        while (true)
        {
            string msg = Console.ReadLine();
            server.SendTo(Encoding.UTF8.GetBytes(msg), point);
        }


    }

    void ReciveMsg()
    {
        //for (int i = 0; i < 100; i++)
        for(; ; )
        {
            byte[] buffer = new byte[1024];
            int length = server.ReceiveFrom(buffer, ref pointEnd);//接收数据报
            string message = Encoding.UTF8.GetString(buffer, 0, length);
            //Debug.LogWarning(message);
            UpdateVal(message);
            //UnityEngine.Debug.LogWarning(point.ToString() + message);
            //UnityEngine.Debug.LogWarning(string.Format("imu_Neck.RotX_Q: {0}, {1}, {2}, {3}", IMUs[1].RotW_Q, IMUs[1].RotX_Q, IMUs[1].RotY_Q, IMUs[1].RotZ_Q));
            if (closeFlag) break;
        }
    }

    /// <summary>
    /// 字符串切割，并分别把值放进对应的对象中
    /// </summary>
    /// s的格式 “#1;2;3;4;5;...;28”  共有28个数据，（7IMU，每个IMU4个数据代表四元数w,x,y,z)
    void UpdateVal(string s)
    {
        //UnityEngine.Debug.LogWarning("UpdateVal");
        if (s == null) return;
        char leading = s[0];
        if (leading != '#') return;

        string str = s.Substring(1);
        string[] strArray = str.Split(';');

        if (strArray.Length < NumOfQuaternion)
        {
            Debug.LogError("strArray's length i less than 1");
        }
        else
        {
            timeCount += 1;
                //for (int i = 0; i < IMUs.Count; i++)
                for (int i = 0; i < 7; i++)
            {
                int index = i * 4;
                float RotX = float.Parse(strArray[index]);
                float RotY = float.Parse(strArray[index + 1]);
                float RotZ = float.Parse(strArray[index + 2]);
                float RotW = float.Parse(strArray[index + 3]);

                if ((RotX + RotY + RotZ + RotW) == 0) continue;
                
                //Vector3 NewAngle = new Quaternion(RotX, RotY, RotZ, RotW).eulerAngles;
                //Vector3 OldAngle = new Quaternion(IMUs[i].RotX_Q, IMUs[i].RotY_Q, IMUs[i].RotZ_Q, IMUs[i].RotW_Q).eulerAngles;
                //Changes[i] =(NewAngle - OldAngle);
                //IMUs[i].RotX_Q = RotX;
                //IMUs[i].RotY_Q = RotY;
                //IMUs[i].RotZ_Q = RotZ;
                //IMUs[i].RotW_Q = RotW;   
                //refQuatation[(BodyPart)i] = new Quaternion(RotX, RotY, RotZ, RotW);
                //refQuatation[(BodyPart)i] = new Quaternion(RotZ, -RotX, RotY, RotW);
                Quaternion data = new Quaternion(RotX, RotY, RotZ, RotW);
                Vector3 angle;


                switch ((BodyPart)i)
                {
                    case BodyPart.UpperArmLeft:
                        angle = new Vector3(data.eulerAngles.z, data.eulerAngles.y, 180 - data.eulerAngles.x);
                        data.eulerAngles = angle;
                        break;

                    case BodyPart.LowerArmLeft:
                        angle = new Vector3(-data.eulerAngles.y, 180 - data.eulerAngles.x, -data.eulerAngles.z);
                        data.eulerAngles = angle;
                        break;

                    case BodyPart.UpperArmRight:
                        angle = new Vector3( data.eulerAngles.x, data.eulerAngles.z, - data.eulerAngles.y);
                        data.eulerAngles = angle;
                        break;

                    case BodyPart.LowerArmRight:
                        angle = new Vector3( - data.eulerAngles.x,  data.eulerAngles.z,  -data.eulerAngles.y);
                        data.eulerAngles = angle;
                        break;

                    default:
                        angle = new Vector3(data.eulerAngles.x, data.eulerAngles.y, data.eulerAngles.z);
                        data.eulerAngles = angle;
                        break;

                }




                refQuatation[(BodyPart)i] = data;

                if (saveFlag)  //数据存入
                {
                    if (!saveFile.ContainsKey(timeCount))
                    {
                        saveFile[timeCount] = new Dictionary<BodyPart, Quaternion>();
                    }
                    saveFile[timeCount][(BodyPart)i] = data;
                }
                
            }
        }
    }


    void setOri(Dictionary<BodyPart,IMU> IMUs)
    {


        //m_Neck.rotation = playerMoveController.CheckLimitation(BodyPart.Neck, Quaternion.Inverse(refQuatation[BodyPart.Neck]) *
        //                originQuatation[BodyPart.Neck]);
        //m_Spine.rotation = playerMoveController.CheckLimitation(BodyPart.Spine, Quaternion.Inverse(refQuatation[BodyPart.Spine]) *
        //                originQuatation[BodyPart.Spine]);
        //m_UpperArmLeft.rotation = playerMoveController.CheckLimitation(BodyPart.UpperArmLeft, Quaternion.Inverse(refQuatation[BodyPart.UpperArmLeft]) *
        //                originQuatation[BodyPart.UpperArmLeft]);
        //m_LowerArmLeft.rotation = playerMoveController.CheckLimitation(BodyPart.LowerArmLeft, Quaternion.Inverse(refQuatation[BodyPart.LowerArmLeft]) *
        //                originQuatation[BodyPart.LowerArmLeft]);
        //m_UpperArmRight.rotation = playerMoveController.CheckLimitation(BodyPart.UpperArmRight, Quaternion.Inverse(refQuatation[BodyPart.UpperArmRight]) *
        //                originQuatation[BodyPart.UpperArmRight]);
        //m_LowerArmRight.rotation = playerMoveController.CheckLimitation(BodyPart.LowerArmRight, Quaternion.Inverse(refQuatation[BodyPart.LowerArmRight]) *
        //                originQuatation[BodyPart.LowerArmRight]);




        //m_Neck.rotation = Quaternion.Inverse(refQuatation[BodyPart.Neck]) *
        //                originQuatation[BodyPart.Neck];
        //m_Spine.rotation = Quaternion.Inverse(refQuatation[BodyPart.Spine]) *
        //                originQuatation[BodyPart.Spine];
        //m_UpperArmLeft.rotation = Quaternion.Inverse(refQuatation[BodyPart.UpperArmLeft]) *
        //                originQuatation[BodyPart.UpperArmLeft];
        //m_LowerArmLeft.rotation = Quaternion.Inverse(refQuatation[BodyPart.LowerArmLeft]) *
        //                originQuatation[BodyPart.LowerArmLeft];
        //m_UpperArmRight.rotation = Quaternion.Inverse(refQuatation[BodyPart.UpperArmRight]) *
        //                originQuatation[BodyPart.UpperArmRight];
        //m_LowerArmRight.rotation = Quaternion.Inverse(refQuatation[BodyPart.LowerArmRight]) *
        //                originQuatation[BodyPart.LowerArmRight];




        m_Neck.rotation = Matrix2Quad(Matrix4x4.Inverse(Quad2Matrix(refQuatation[BodyPart.Neck])) * Quad2Matrix(originQuatation[BodyPart.Neck]));

        m_Spine.rotation = Matrix2Quad(Matrix4x4.Inverse(Quad2Matrix(refQuatation[BodyPart.Spine])) * Quad2Matrix(originQuatation[BodyPart.Spine]));

        m_UpperArmLeft.rotation = Matrix2Quad(Matrix4x4.Inverse(Quad2Matrix(refQuatation[BodyPart.UpperArmLeft])) * Quad2Matrix(originQuatation[BodyPart.UpperArmLeft]));

        m_LowerArmLeft.rotation = Matrix2Quad(Matrix4x4.Inverse(Quad2Matrix(refQuatation[BodyPart.LowerArmLeft])) * Quad2Matrix(originQuatation[BodyPart.LowerArmLeft]));

        m_UpperArmRight.rotation = Matrix2Quad(Matrix4x4.Inverse(Quad2Matrix(refQuatation[BodyPart.UpperArmRight])) * Quad2Matrix(originQuatation[BodyPart.UpperArmRight]));

        m_LowerArmRight.rotation = Matrix2Quad(Matrix4x4.Inverse(Quad2Matrix(refQuatation[BodyPart.LowerArmRight])) * Quad2Matrix(originQuatation[BodyPart.LowerArmRight]));



        foreach (BodyPart item in Enum.GetValues(typeof(BodyPart)))
        {
            drawController.drawLine(timeStep, item, refQuatation[item]);
        }
        
        timeStep++;
        if (timeStep >= 1000)
        {
            timeStep = 0;
            drawController.ClearGraph();
        }
            

        //m_Neck.rotation = Quaternion.Inverse(originQuatation[BodyPart.Neck]) *
        //               refQuatation[BodyPart.Neck] * defaultValue[BodyPart.Neck];
        //m_Spine.rotation = Quaternion.Inverse(originQuatation[BodyPart.Spine]) *
        //                refQuatation[BodyPart.Spine] * defaultValue[BodyPart.Spine];
        //m_UpperArmLeft.rotation = Quaternion.Inverse(originQuatation[BodyPart.UpperArmLeft]) *
        //                refQuatation[BodyPart.UpperArmLeft] * defaultValue[BodyPart.UpperArmLeft];
        //m_LowerArmLeft.rotation = Quaternion.Inverse(originQuatation[BodyPart.LowerArmLeft]) *
        //                refQuatation[BodyPart.LowerArmLeft] * defaultValue[BodyPart.LowerArmLeft];
        //m_UpperArmRight.rotation = Quaternion.Inverse(originQuatation[BodyPart.UpperArmRight]) *
        //                refQuatation[BodyPart.UpperArmRight] * defaultValue[BodyPart.UpperArmRight];
        //m_LowerArmRight.rotation = Quaternion.Inverse(originQuatation[BodyPart.LowerArmRight]) *
        //                refQuatation[BodyPart.LowerArmRight] * defaultValue[BodyPart.LowerArmRight];
    }


    //四元数转旋转矩阵
    private Matrix4x4 Quad2Matrix(Quaternion q)
    {
        Matrix4x4 rot = new Matrix4x4();

        rot.SetTRS(new Vector3(0, 0, 0), q, new Vector3(1, 1, 1));

        return rot;
    }

    //旋转矩阵转四元数
    private Quaternion Matrix2Quad(Matrix4x4 rot)
    {
        Vector4 vy = rot.GetColumn(1);
        Vector4 vz = rot.GetColumn(2);

        Quaternion quad = Quaternion.LookRotation(new Vector3(vz.x, vz.y, vz.z), new Vector3(vy.x, vy.y, vy.z));
        return quad;
    }


    private void OnDestroy()
    {
        closeFlag = true;
    }



    void setValues(IMU tmpImu, string values)
    {
        try
        {
            //tmpImu.accX = double.Parse(values[1]);
            //tmpImu.accY = double.Parse(values[2]);
            //tmpImu.accZ = double.Parse(values[3]);
            //tmpImu.RotX_Q = double.Parse(values[4]);
            //tmpImu.RotY_Q = double.Parse(values[5]);
            //tmpImu.RotZ_Q = double.Parse(values[6]);
            //tmpImu.RotW_Q = double.Parse(values[7]);
        }
        catch (FormatException)
        {
            UnityEngine.Debug.LogError($"Unable to parse '{values[0]}'");
        }
    }


}




