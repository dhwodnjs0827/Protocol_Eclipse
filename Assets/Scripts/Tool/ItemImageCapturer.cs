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
    public Camera captureCamera;  // 캡처에 사용할 카메라
    public string fileName = "WeaponImage";  // 저장 파일 이름
    int imageWidth = 512;  // 이미지 가로 크기
    int imageHeight = 512; // 이미지 세로 크기
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

        // RenderTexture 설정
        RenderTexture renderTexture = new RenderTexture(imageWidth, imageHeight, 24);
        captureCamera.targetTexture = renderTexture;

        // 카메라 렌더링
        Texture2D screenshot = new Texture2D(imageWidth, imageHeight, TextureFormat.RGBA32, false);
        captureCamera.Render();

        // RenderTexture에서 텍스처 읽기
        RenderTexture.active = renderTexture;
        screenshot.ReadPixels(new Rect(0, 0, imageWidth, imageHeight), 0, 0);
        screenshot.Apply();

        // RenderTexture 해제
        captureCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTexture);

        // 이미지 저장
        byte[] bytes = screenshot.EncodeToPNG();
        //string path = $"{Application.dataPath}/{fileName}.png";
        string path = $"{Application.dataPath}/Resources/Item/Weapon/Sprites/{fileName}.png";
        System.IO.File.WriteAllBytes(path, bytes);
        Debug.Log($"이미지가 저장되었습니다: {path}");
    }
}
