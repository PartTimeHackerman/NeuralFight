using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class CameraFollow : MonoBehaviour
{
    public GameObject obj;
    public float xOffset = 0f;
    public float yOffset = 3.6f;
    public float velCamSize = 10f;
    private Rigidbody objRb;
    private Camera camera;
    public Observations obs;
    private float distToFloor = 0f;
    public float camZoomSpeed = .1f;
    private float camZoom = 0f;

    private float camPosY = 0f;
    public float camPosYSpeed = .1f;
    public float speedXOffset = 0f;
    void Start()
    {
        camera = GetComponent<Camera>();
        objRb = obj.GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector3 objPos = obj.transform.position;
        float rbVel = Mathf.Abs(Mathf.Clamp(objRb.velocity.x, -velCamSize, velCamSize));
        float zoom = RewardFunctions.toleranceInvNoBounds(Mathf.Clamp((rbVel / velCamSize), 0f, 1f), .4f, .1f, RewardFunction.LONGTAIL);

        if (objPos.y > 0)
        {
            zoom += objPos.y;
        }

        distToFloor = obs.distToFloor();
        if (distToFloor <= 0)
        {
            distToFloor = objPos.y;
        }

        /*if (Mathf.Abs(camZoom - distToFloor) > camZoomThreshold)
        {
            if (camZoom > distToFloor)
            { 
                camZoom -= camZoomSpeed;
            }
            else if (camZoom < distToFloor)
            {
                camZoom += camZoomSpeed;
            }
        }*/
        camZoom = Mathf.Lerp(camZoom, distToFloor, Time.deltaTime * camZoomSpeed);
        camPosY = Mathf.Lerp(camPosY, objPos.y, Time.deltaTime * camPosYSpeed);
        camera.orthographicSize = 5f + camZoom;

        float cameraX = objPos.x + xOffset;
        float cameraY = camPosY + yOffset;
        transform.position = new Vector3(cameraX, cameraY, transform.position.z);
    }


}
