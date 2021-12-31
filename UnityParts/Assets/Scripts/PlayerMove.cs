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
using System.IO.Ports;
using UnityEditorInternal.VersionControl;

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

enum BodyPart:int
{
    Neck = 0,
    ClavicleLeft = 1,
    UpperArmLeft = 2,
    LowerArmLeft = 3,
    ClavicleRight= 4,
    UpperArmRight= 5,
    LowerArmRight= 6,
}



public class PlayerMove : MonoBehaviour
{
    private const int NumOfIMU = 7;
    private const int NumOfQuaternion = 28;   //4 x 7
    //imu 由 1 - 7
    public Transform m_Neck;
    public Transform m_ClavicleLeft;
    public Transform m_UpperArmLeft;
    public Transform m_LowerArmLeft;
    public Transform m_ClavicleRight;
    public Transform m_UpperArmRight;
    public Transform m_LowerArmRight;

    private Quaternion m_Rot_Neck;
    private Quaternion m_Rot_ClavicleLeft;
    private Quaternion m_Rot_UpperArmLeft;
    private Quaternion m_Rot_LowerArmLeft;
    private Quaternion m_Rot_ClavicleRight;
    private Quaternion m_Rot_UpperArmRight;
    private Quaternion m_Rot_LowerArmRight;

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



    void Start()
    {
        IMUs = new Dictionary<BodyPart, IMU>(NumOfIMU);
        refQuatation = new Dictionary<BodyPart, Quaternion>(NumOfIMU);
        originQuatation = new Dictionary<BodyPart, Quaternion>(NumOfIMU);

        foreach (BodyPart item in Enum.GetValues(typeof(BodyPart)))
        {
            IMUs.Add(item, new IMU());
            refQuatation.Add(item, Quaternion.identity);
            originQuatation.Add(item, Quaternion.identity);
        }


        string str = "#1;2;3";
        string ss = str.Substring(1);
        string[] strArray = ss.Split(';');

        Debug.LogWarning(string.Format("{0}  {1}  {2}", strArray[0], strArray[1], strArray[2]));

        server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        server.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9999));//绑定端口号和IP
        point = new IPEndPoint(IPAddress.Any, 0);//用来保存发送方的ip和端口号
        pointEnd = (EndPoint)point;

        UnityEngine.Debug.LogWarning("服务端已经开启");
        Thread t = new Thread(ReciveMsg);//开启接收消息线程
        //t.IsBackground = true;
        t.Start();

        // Thread t2 = new Thread(sendMsg);//开启发送消息线程
        // t2.Start();

        UnityEngine.Debug.LogWarning("执行完毕！");

    }
    void test()
    {
        for (int i = 0; i < 10000; i++)
            Debug.LogError("1");
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.R))   //重置命令
        {
            foreach (KeyValuePair<BodyPart, Quaternion> item in refQuatation)
            {
                refQuatation[item.Key] = originQuatation[item.Key];
            }
        }

        setOri(IMUs);
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
            UnityEngine.Debug.LogWarning(message);
            UpdateVal(message);
            UnityEngine.Debug.LogWarning(point.ToString() + message);
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
        UnityEngine.Debug.LogWarning("UpdateVal");
        if (s == null) return;
        char leading = s[0];
        if (leading != '#') return;

        string str = s.Substring(1);
        string[] strArray = str.Split(';');

        if (strArray.Length < NumOfQuaternion)
        {
            UnityEngine.Debug.LogError("strArray's length i less than 1");
        }
        else
        {
            for (int i = 0; i < IMUs.Count; i++)
            {
                int index = i * 4;
                float RotX = (float)Math.Round(float.Parse(strArray[index]),1);
                float RotY = (float)Math.Round(float.Parse(strArray[index + 1]),1);
                float RotZ = (float)Math.Round(float.Parse(strArray[index + 2]),1);
                float RotW = (float)Math.Round(float.Parse(strArray[index + 3]),1);

                //Vector3 NewAngle = new Quaternion(RotX, RotY, RotZ, RotW).eulerAngles;
                //Vector3 OldAngle = new Quaternion(IMUs[i].RotX_Q, IMUs[i].RotY_Q, IMUs[i].RotZ_Q, IMUs[i].RotW_Q).eulerAngles;
                //Changes[i] =(NewAngle - OldAngle);
                //IMUs[i].RotX_Q = RotX;
                //IMUs[i].RotY_Q = RotY;
                //IMUs[i].RotZ_Q = RotZ;
                //IMUs[i].RotW_Q = RotW;   
                //refQuatation[(BodyPart)i] = new Quaternion(, -RotX, -RotY, RotW);
                refQuatation[(BodyPart)i] = new Quaternion(-RotZ, -RotX, -RotY, RotW);

            }
        }
    }


    void setOri(Dictionary<BodyPart,IMU> IMUs)
    {
        //m_Neck.rotation = new Quaternion(IMUs[0].RotX_Q,IMUs[0].RotY_Q,
        //    IMUs[0].RotZ_Q,IMUs[0].RotW_Q);
        //m_ClavicleLeft.rotation = new Quaternion(IMUs[1].RotX_Q, IMUs[1].RotY_Q,
        //     IMUs[1].RotZ_Q, IMUs[1].RotW_Q);
        //m_UpperArmLeft.rotation = new Quaternion(IMUs[2].RotX_Q, IMUs[2].RotY_Q,
        //    IMUs[2].RotZ_Q, IMUs[2].RotW_Q);
        //m_LowerArmLeft.rotation = new Quaternion(IMUs[3].RotX_Q, IMUs[3].RotY_Q,
        //    IMUs[3].RotZ_Q, IMUs[3].RotW_Q);
        //m_ClavicleRight.rotation = new Quaternion(IMUs[4].RotX_Q, IMUs[4].RotY_Q,
        //    IMUs[4].RotZ_Q, IMUs[4].RotW_Q);
        //m_UpperArmRight.rotation = new Quaternion(IMUs[5].RotX_Q, IMUs[5].RotY_Q,
        //    IMUs[5].RotZ_Q, IMUs[5].RotW_Q);
        //m_LowerArmRight.rotation = new Quaternion(IMUs[6].RotX_Q, IMUs[6].RotY_Q,
        //    IMUs[6].RotZ_Q, IMUs[6].RotW_Q);

        //m_Neck.rotation = new Quaternion(IMUs[BodyPart.Neck].RotZ_Q,-IMUs[BodyPart.Neck].RotX_Q,
        //    -IMUs[BodyPart.Neck].RotY_Q, IMUs[BodyPart.Neck].RotW_Q);
        //m_ClavicleLeft.rotation = new Quaternion(IMUs[BodyPart.ClavicleLeft].RotZ_Q, -IMUs[BodyPart.ClavicleLeft].RotX_Q,
        //    -IMUs[BodyPart.ClavicleLeft].RotY_Q, IMUs[BodyPart.ClavicleLeft].RotW_Q);
        //m_UpperArmLeft.rotation = new Quaternion(IMUs[BodyPart.UpperArmLeft].RotZ_Q, -IMUs[BodyPart.UpperArmLeft].RotX_Q,
        //    -IMUs[BodyPart.UpperArmLeft].RotY_Q, IMUs[BodyPart.UpperArmLeft].RotW_Q);
        //m_LowerArmLeft.rotation = new Quaternion(IMUs[BodyPart.LowerArmLeft].RotZ_Q, -IMUs[BodyPart.LowerArmLeft].RotX_Q,
        //    -IMUs[BodyPart.LowerArmLeft].RotY_Q, IMUs[BodyPart.LowerArmLeft].RotW_Q);
        //m_ClavicleRight.rotation = new Quaternion(IMUs[BodyPart.ClavicleRight].RotZ_Q, -IMUs[BodyPart.ClavicleRight].RotX_Q,
        //    -IMUs[BodyPart.ClavicleRight].RotY_Q, IMUs[BodyPart.ClavicleRight].RotW_Q);
        //m_UpperArmRight.rotation = new Quaternion(IMUs[BodyPart.UpperArmRight].RotZ_Q, -IMUs[BodyPart.UpperArmRight].RotX_Q,
        //    -IMUs[BodyPart.UpperArmRight].RotY_Q, IMUs[BodyPart.UpperArmRight].RotW_Q);
        //m_LowerArmRight.rotation = new Quaternion(IMUs[BodyPart.LowerArmRight].RotZ_Q, -IMUs[BodyPart.LowerArmRight].RotX_Q,
        //    -IMUs[BodyPart.LowerArmRight].RotY_Q, IMUs[BodyPart.LowerArmRight].RotW_Q);


        m_Neck.localRotation = Quaternion.Inverse(refQuatation[BodyPart.Neck]) * originQuatation[BodyPart.Neck];
        m_ClavicleLeft.localRotation = Quaternion.Inverse(refQuatation[BodyPart.ClavicleLeft]) * originQuatation[BodyPart.ClavicleLeft];
        m_UpperArmLeft.localRotation = Quaternion.Inverse(refQuatation[BodyPart.UpperArmLeft]) * originQuatation[BodyPart.UpperArmLeft];
        m_LowerArmLeft.localRotation = Quaternion.Inverse(refQuatation[BodyPart.LowerArmLeft]) * originQuatation[BodyPart.LowerArmLeft];
        m_ClavicleRight.localRotation = Quaternion.Inverse(refQuatation[BodyPart.ClavicleRight]) * originQuatation[BodyPart.ClavicleRight];
        m_UpperArmRight.localRotation = Quaternion.Inverse(refQuatation[BodyPart.UpperArmRight]) * originQuatation[BodyPart.UpperArmRight];
        m_LowerArmRight.rotation = Quaternion.Inverse(refQuatation[BodyPart.LowerArmRight]) * originQuatation[BodyPart.LowerArmRight];




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




