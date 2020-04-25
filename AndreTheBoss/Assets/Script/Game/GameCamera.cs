using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Camera))]
public class GameCamera : MonoBehaviour
{
    public HexMap hexMap;
    private Camera gameCamera;

    public float cameraSmooth = 3f;
    Vector3 targetPoint;

    public float cameraVerticalSpeed = 1f;
    public float cameraHorizontalSpeed = 1f;

    //[Range(30, 50)]
    //public float minFieldOfView = 50;
    //[Range(50, 80)]
    //public float maxFieldOfView = 70;
    //private float fieldOfView;
    //public float fovChangeSpeed = 2f;

    [Range(0, 5)]
    public float fieldOfViewChangeSpeed = 0.01f;
    [Range(5, 50)]
    public float cameraHeight = 15f;
    [Range(5, 30)]
    public float cameraZOffset = 5f;
    public float cameraMaxHeight = 20f;
    public float cameraMinHeight = 10f;
    public float cameraHeightChangeSpeed = 1f;



    private Vector3 mouseOldPos;
    private Vector3 mouseCurrentPos;

   

    private Rect cameraBorder;
    public void OnEnable()
    {
        Vector3 mapCenter = hexMap.GetCenterPoint();
        gameCamera = GetComponent<Camera>();
        gameCamera.transform.position = targetPoint = new Vector3(mapCenter.x, cameraHeight, mapCenter.z - cameraZOffset);
        //fieldOfView = gameCamera.fieldOfView;

        cameraBorder = hexMap.GetBorder();
        cameraBorder.yMin -= cameraZOffset;
        cameraBorder.yMax -= cameraZOffset;

        targetPoint = gameCamera.transform.position;
    }

    public void Update()
    {
        if(Input.mouseScrollDelta.y != 0f && !EventSystem.current.IsPointerOverGameObject())
        {
            //UpdateFieldOfView(-Input.mouseScrollDelta.y * fieldOfViewChangeSpeed);
            UpdateCameraHeight(-Input.mouseScrollDelta.y * cameraHeightChangeSpeed);
        }
        if(Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
        {
            mouseOldPos = Input.mousePosition;
        }
        if (Input.GetMouseButton(1) && !EventSystem.current.IsPointerOverGameObject())
        {
            mouseCurrentPos = Input.mousePosition;
            Vector2 offsets = mouseCurrentPos - mouseOldPos;
            mouseOldPos = mouseCurrentPos;
            UpdateCameraMovement(offsets);
        }
    }

    /*
    public void UpdateFieldOfView(float value)
    {
        value = gameCamera.fieldOfView + value;
        if (value > maxFieldOfView)
            fieldOfView = maxFieldOfView;
        else if (value < minFieldOfView)
            fieldOfView = minFieldOfView;
        else
            fieldOfView = value;
    }
    */

    public void UpdateCameraHeight(float value)
    {
        value = cameraHeight + value;
        if (cameraHeight > cameraMaxHeight)
            cameraHeight = cameraMaxHeight;
        else if (cameraHeight < cameraMinHeight)
            cameraHeight = cameraMinHeight;
        else
            cameraHeight = value;
        // update focus point
        targetPoint = new Vector3(targetPoint.x, cameraHeight, targetPoint.z);
    }

    public void UpdateCameraMovement(Vector2 offsets)
    {
        Vector3 position = new Vector3(transform.position.x - offsets.x * cameraHorizontalSpeed, 
            transform.position.y, transform.position.z - offsets.y * cameraVerticalSpeed);
        if (position.x < cameraBorder.xMin)
            position.x = cameraBorder.xMin;
        else if (position.x > cameraBorder.xMax)
            position.x = cameraBorder.xMax;
        if (position.z < cameraBorder.yMin)
            position.z = cameraBorder.yMin;
        else if (position.z > cameraBorder.yMax)
            position.z = cameraBorder.yMax;

        targetPoint = position;
    }

    public void FocusOnPoint(Vector3 point)
    {
        targetPoint = new Vector3(point.x, cameraHeight, point.z - cameraZOffset);
    }


    public void FixedUpdate()
    {
        if(targetPoint != null)
        {
            transform.position = Vector3.Lerp(transform.position, targetPoint, Time.deltaTime * cameraSmooth);
            //gameCamera.fieldOfView = Mathf.Lerp(gameCamera.fieldOfView, fieldOfView, Time.deltaTime * fovChangeSpeed);
        }  
    }

}
