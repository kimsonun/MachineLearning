using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public Camera fullViewCamera;
    public Camera behindCamera;
    public Camera cinnematicCamera;

    public void ShowFullViewCamera()
    {
        behindCamera.enabled = false;
        cinnematicCamera.enabled = false;
        fullViewCamera.enabled = true;
    }

    public void ShowCinnematicCamera()
    {
        behindCamera.enabled = false;
        cinnematicCamera.enabled = true;
        fullViewCamera.enabled = false;
    }

    public void ShowBehindCamera()
    {
        behindCamera.enabled = true;
        cinnematicCamera.enabled = false;
        fullViewCamera.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            ShowFullViewCamera();
        if (Input.GetKeyDown(KeyCode.Alpha2))
            ShowBehindCamera();
        if (Input.GetKeyDown(KeyCode.Alpha3))
            ShowCinnematicCamera();
    }
}
