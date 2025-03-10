using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    private void Awake()
    {
        Camera camera = GetComponent<Camera>();

        //카메라 컴포넌트의 Viewport Rect
        Rect rect = camera.rect;

        //현재 세로 모드 9 : 16, 반대로 가로 모드로 하고 싶다면 16 : 9로 입력
        float scaleHeight = ((float)Screen.width / Screen.height) / ((float)9 / 16); //(가로 / 세로)
        float scaleWidth = 1.0f / scaleHeight;

        if(scaleHeight < 1)
        {
            rect.height = scaleHeight;
            rect.y = (1.0f - scaleHeight) / 2.0f;
        }
        else
        {
            rect.width = scaleWidth;
            rect.x = (1.0f - scaleHeight) / 2.0f;
        }

        camera.rect = rect;

    }
}
