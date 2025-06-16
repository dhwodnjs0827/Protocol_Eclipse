using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Ratio
{
    Ratio1x1,
    Ratio2x1
}

public class ItemImageCapturer : MonoBehaviour
{
    public Camera captureCamera;  // ĸó�� ����� ī�޶�
    public string fileName = "WeaponImage";  // ���� ���� �̸�
    int imageWidth = 512;  // �̹��� ���� ũ��
    int imageHeight = 512; // �̹��� ���� ũ��
    public Ratio ratio = Ratio.Ratio1x1;

    private void Start()
    {
        CaptureImage();
    }

    public void CaptureImage()
    {
        if (ratio == Ratio.Ratio1x1)
        {
            imageWidth = 512;
            imageHeight = 512;
            Camera.main.orthographicSize = 0.48f;
        }
        else if (ratio == Ratio.Ratio2x1)
        {
            imageWidth = 512;
            imageHeight = 256;
            Camera.main.orthographicSize = 0.28f;
        }

        // RenderTexture ����
        RenderTexture renderTexture = new RenderTexture(imageWidth, imageHeight, 24);
        captureCamera.targetTexture = renderTexture;

        // ī�޶� ������
        Texture2D screenshot = new Texture2D(imageWidth, imageHeight, TextureFormat.RGBA32, false);
        captureCamera.Render();

        // RenderTexture���� �ؽ�ó �б�
        RenderTexture.active = renderTexture;
        screenshot.ReadPixels(new Rect(0, 0, imageWidth, imageHeight), 0, 0);
        screenshot.Apply();

        // RenderTexture ����
        captureCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTexture);

        // �̹��� ����
        byte[] bytes = screenshot.EncodeToPNG();
        //string path = $"{Application.dataPath}/{fileName}.png";
        string path = $"{Application.dataPath}/Resources/Item/Weapon/Sprites/{fileName}.png";
        System.IO.File.WriteAllBytes(path, bytes);
        Debug.Log($"�̹����� ����Ǿ����ϴ�: {path}");
    }
}
