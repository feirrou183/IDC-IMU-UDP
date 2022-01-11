
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecorderShow : MonoBehaviour
{
    public RawImage rawImage;//�����Ⱦ��UI
    private WebCamTexture webCamTexture;

    void Start()
    {
        ToOpenCamera();
    }

    /// <summary>
    /// �������
    /// </summary>
    public void ToOpenCamera()
    {
        StartCoroutine("OpenCamera");
    }
    public IEnumerator OpenCamera()
    {

        int maxl = Screen.width;
        if (Screen.height > Screen.width)
        {
            maxl = Screen.height;
        }

        // ��������ͷȨ��
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            if (webCamTexture != null)
            {
                webCamTexture.Stop();
            }

            //����Ⱦͼ
            if (rawImage != null)
            {
                rawImage.gameObject.SetActive(true);
            }

            // ��ص�һ����Ȩ���Ƿ��õ��豸����Ϊ�ܿ��ܵ�һ����Ȩ�ˣ����ǻ�ò����豸�������������⣩
            // ��� ��û�л���豸�����ܾ�����û������ͷ��������ȡ camera
            int i = 0;
            while (WebCamTexture.devices.Length <= 0 && 1 < 300)
            {
                yield return new WaitForEndOfFrame();
                i++;
            }
            WebCamDevice[] devices = WebCamTexture.devices;//��ȡ�����豸
            if (WebCamTexture.devices.Length <= 0)
            {
                Debug.LogError("û������ͷ�豸������");
            }
            else
            {
                string devicename = devices[0].name;
                webCamTexture = new WebCamTexture(devicename, maxl, maxl == Screen.height ? Screen.width : Screen.height, 30)
                {
                    wrapMode = TextureWrapMode.Repeat
                };

                // ��Ⱦ�� UI ���� ��Ϸ������
                if (rawImage != null)
                {
                    rawImage.texture = webCamTexture;
                }



                webCamTexture.Play();
            }

        }
        else
        {
            Debug.LogError("δ��ö�ȡ����ͷȨ��");
        }
    }

    private void OnApplicationPause(bool pause)
    {
        // Ӧ����ͣ��ʱ����ͣcamera��������ʱ�����ʹ��
        if (webCamTexture != null)
        {
            if (pause)
            {
                webCamTexture.Pause();
            }
            else
            {
                webCamTexture.Play();
            }
        }

    }


    private void OnDestroy()
    {
        if (webCamTexture != null)
        {
            webCamTexture.Stop();
        }
    }
}