using UnityEngine;

public class ArenaCameraFollow : MonoBehaviour
{
    private Camera camera;

    public Transform LeftObs;
    public Transform RightObs;

    public float MaxZ = -5f;
    public float MinZ = -9f;
    public float MaxX = 5f;
    public float MinX = -5f;
    public float CameraSpeed = 1;
    public float ZoomFactor = 1.5f;

    private Vector3 BasePosition;
    private bool Follow = false;
    private bool ResetPos = false;

    void Start()
    {
        camera = GetComponent<Camera>();
        BasePosition = camera.transform.position;
    }

    public void SetTransforms(Transform left, Transform right)
    {
        LeftObs = left;
        RightObs = right;
    }

    public void ResetPosition()
    {
        ResetPos = true;
        Follow = false;
    }

    public void StartFollow()
    {
        Follow = true;
    }
    
    public void StopFollow()
    {
        Follow = false;
    }

    void Update()
    {
        if (ResetPos)
        {
            Vector3 CamResetPos = Vector3.Slerp(camera.transform.position, BasePosition, Time.deltaTime * CameraSpeed);
            camera.transform.position = CamResetPos;
            if (Vector3.Distance(BasePosition, camera.transform.position) <= 0.05f)
            {
                ResetPos = false;
            }
        }

        if (Follow)
        {
            Vector3 midpoint = (LeftObs.position + RightObs.position) / 2f;
            float distance = (LeftObs.position - RightObs.position).magnitude;
            Vector3 cameraDestination = midpoint - camera.transform.forward * distance * ZoomFactor;
            cameraDestination.y = BasePosition.y;
            cameraDestination.z = Mathf.Clamp(cameraDestination.z, MinZ, MaxZ);
            cameraDestination.x = Mathf.Clamp(cameraDestination.x, MinX, MaxX);
            camera.transform.position = Vector3.Slerp(camera.transform.position, cameraDestination, Time.deltaTime * CameraSpeed);
        }
    }
}