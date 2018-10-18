using UnityEngine;


public class DragRigidbody : MonoBehaviour
{
    public float force = 600;
    public float damping = 6;
    public Transform jointTrans;
    float dragDepth;

    bool isDragging = false;
    public Rigidbody draggingObject;
    public Vector3 pos;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Rigidbody rb = GetObjectFromMouseRaycast();
            draggingObject = rb == null ? draggingObject : rb;
            pos = CalculateMouse3DVector();
            if (jointTrans)
            {
                jointTrans.position = pos;
            }
            else
            {
                jointTrans = AttachJoint(draggingObject, pos);
            }
        }
        else
        {
            if (jointTrans)
                Destroy(jointTrans.gameObject);
        }
    }

    private Rigidbody GetObjectFromMouseRaycast()
    {
        Rigidbody rb = null;
        RaycastHit hitInfo = new RaycastHit();
        bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
        if (hit)
        {
            if (hitInfo.collider.gameObject.GetComponent<Rigidbody>())
            {
                rb = hitInfo.collider.gameObject.GetComponent<Rigidbody>();
            }
        }

        return rb;
    }

    private Vector3 CalculateMouse3DVector()
    {
        Vector3 v3 = Input.mousePosition;
        v3.z = 0f;
        v3 = Camera.main.ScreenToWorldPoint(v3);
        return v3;
    }

    Transform AttachJoint(Rigidbody rb, Vector3 attachmentPosition)
    {
        attachmentPosition.z = 0f;
        GameObject go = new GameObject("Attachment Point");
        //go.hideFlags = HideFlags.HideInHierarchy;
        go.transform.position = attachmentPosition;

        var newRb = go.AddComponent<Rigidbody>();
        newRb.isKinematic = true;

        var joint = go.AddComponent<ConfigurableJoint>();
        joint.connectedBody = rb;
        joint.configuredInWorldSpace = true;
        joint.xDrive = NewJointDrive(force, damping);
        joint.yDrive = NewJointDrive(force, damping);
        joint.zDrive = NewJointDrive(force, damping);
        joint.slerpDrive = NewJointDrive(force, damping);
        joint.rotationDriveMode = RotationDriveMode.Slerp;

        return go.transform;
    }

    private JointDrive NewJointDrive(float force, float damping)
    {
        JointDrive drive = new JointDrive();
        drive.mode = JointDriveMode.Position;
        drive.positionSpring = force;
        drive.positionDamper = damping;
        drive.maximumForce = Mathf.Infinity;
        return drive;
    }
}