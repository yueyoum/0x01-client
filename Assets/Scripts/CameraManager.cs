using UnityEngine;
using System.Collections;

public class CameraManager
{
    public static Camera CameraMain { get; set; }

    public static float ResizeSpeed { get; set; }
    public static float MaxSize { get; set; }
    public static float MinSize { get; set; }
    public static float InitSize { get; set; }

    private float CurrentSize { get; set; }

    private static CameraScript cameraScript = null;
    private static CameraManager instance = null;
    private CameraManager()
    {
        CurrentSize = InitSize;
        cameraScript = CameraMain.GetComponent<CameraScript>();
        cameraScript.ResizeSpeed = ResizeSpeed;
    }


    public static CameraManager GetInstance()
    {
        if (instance == null)
        {
            instance = new CameraManager();
        }

        return instance;
    }


    public void ExpandMainCameraView()
    {
        float percent = ResizeSpeed * Time.deltaTime * -1;
        ResizeMainCameraView(percent);
    }

    public void ShrinkMainCameraView()
    {
        float percent = ResizeSpeed * Time.deltaTime;
        ResizeMainCameraView(percent);
    }

    private void ResizeMainCameraView(float percent)
    {

        float newSize = CurrentSize * (1 + percent);
        if (newSize >= MaxSize)
        {
            CurrentSize = MaxSize;
            return;
        }

        if (newSize <= MinSize)
        {
            CurrentSize = MinSize;
            return;
        }

        CurrentSize = newSize;
        cameraScript.ReSize(CurrentSize);
    }

    public void MoveMainCamera(Vector3 targetPosition)
    {
        CameraMain.transform.position = new Vector3(
            targetPosition.x,
            targetPosition.y,
            CameraMain.transform.position.z
            );
    }
}
