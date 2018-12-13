using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PartEditor : MonoBehaviour
{
    public Transform part;
    public Transform end;
    public Positioner positionerPref;

    private BoxCollider Collider;
    public Positioner positioner;
    private float minAngle;
    private float maxAngle;

    private float size;

    public float Size
    {
        get { return size; }
        set
        {
            if (Math.Abs(size * value - size) > .001f)
            {
                ChangeSize(value);
            }
        }
    }

    private float baseSize;
    private float minSize;
    private float maxSize;

    private Vector3 lastPositionerPos;

    public event ChangePart OnChangePart;

    public delegate void ChangePart();

    private void Start()
    {
    }

    public void Init(GameObject partGo)
    {
        part = partGo.transform;
        //end = ending.transform;

        List<Transform> childrens = part.GetComponentsInChildren<Transform>().ToList();
        childrens.Remove(part.transform);
        end = childrens[0];
        Collider = part.GetComponent<BoxCollider>();

        positioner = Instantiate(positionerPref);
        if (part.transform.position.z > 0)
        {
            positioner.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }

        positioner.transform.position = end.position;
        positioner.transform.parent = transform;
        JointInfo ji = part.GetComponent<JointInfo>();
        minAngle = !ji.isBackwards ? ji.xMinMax[0] : -ji.xMinMax[1];
        maxAngle = !ji.isBackwards ? ji.xMinMax[1] : -ji.xMinMax[0];
        size = Collider.size.y;
        baseSize = Part.GetPartByName(partGo.name).BaseSize;
        minSize = baseSize * .6f;
        maxSize = baseSize * 1.5f;
        lastPositionerPos = positioner.transform.position;
        UpdateSize();
    }

    private void LateUpdate()
    {
        /*positioner.rotation = part.rotation;
        float dst = Vector3.Distance(part.position, positioner.position);
        if (dst > maxDist)
        {
            Vector3 vect = part.position  - positioner.position;
            vect = vect.normalized;
            vect *= (dst-maxDist);
            positioner.position += vect;
        }*/

        positioner.indicator.position = end.position;
        Vector3 rot = positioner.transform.rotation.eulerAngles;
        rot.z = rot.y > 0 ? -part.rotation.eulerAngles.z : part.rotation.eulerAngles.z;
        positioner.transform.rotation = Quaternion.Euler(rot);
    }

    private void FixedUpdate()
    {
        if (positioner.active)
        {
            UpdatePart();
        }

        positioner.transform.position = end.position;
        Vector3 rot = positioner.transform.rotation.eulerAngles;
        rot.z = rot.y > 0 ? -part.rotation.eulerAngles.z : part.rotation.eulerAngles.z;
        positioner.transform.rotation = Quaternion.Euler(rot);
    }

    public void UpdatePart()
    {
        UpdateRot();
        UpdateSize();
        lastPositionerPos = positioner.transform.position;
        OnChangePart?.Invoke();
    }

    public void UpdateRot()
    {
        float RotZ = -part.parent.rotation.eulerAngles.z;
        float rotZ = getRotation(part, positioner.transform);
        rotZ = ClampAng(rotZ, minAngle + RotZ, maxAngle + RotZ);
        part.rotation = Quaternion.Euler(new Vector3(0f, 0f, -rotZ));
    }

    public void UpdateSize()
    {
        float dst = Vector3.Distance(part.position, positioner.transform.position);
        if (dst > minSize && dst < maxSize)
        {
            ChangeSize(dst / baseSize);
        }

        //part.GetComponent<MeshSizeChanger>().SetSize();
    }

    public static void UpdateSize(Transform part)
    {
        List<Transform> childrens = part.GetComponentsInChildren<Transform>().ToList();
        childrens.Remove(part.transform);

        Transform end = childrens[0];

        float baseSize = Part.GetPartByName(part.name).BaseSize;
        float minSize = baseSize * .6f;
        float maxSize = baseSize * 1.5f;

        float dst = Vector3.Distance(part.position, end.transform.position);
        if (dst > minSize && dst < maxSize)
        {
            ChangeSize(dst / baseSize, part, end, baseSize);
        }

        //part.GetComponent<MeshSizeChanger>().SetSize();
    }

    private static void ChangeSize(float precSize, Transform part, Transform end, float baseSize)
    {
        
        BoxCollider Collider = part.GetComponent<BoxCollider>();

        precSize = Mathf.Clamp(precSize, .6f, 1.5f);
        float size = baseSize * precSize;
        
        Vector3 newSize = Collider.size;
        newSize.y = size;
        Collider.size = newSize;

        
        Vector3 newCenter = Collider.center;
        newCenter.y = -size / 2f;
        Collider.center = newCenter;
        
        ////
        EditablePart editablePart = part.GetComponent<EditablePart>();
        EditableMesh editableMesh = editablePart.EditableMesh;
        EndMesh endMesh = editablePart.EndMesh;
        BoxCollider editableMeshCollider = editableMesh.GetComponent<BoxCollider>();
        Vector3 editableNewSize = editableMeshCollider.size;
        editableNewSize.y = newSize.y - (editableMesh.DownOffset + editableMesh.UpOffset);
        editableMeshCollider.size = editableNewSize;
        Vector3 newEditableMeshCenter = editableMeshCollider.center;
        
        newEditableMeshCenter.y = -editableNewSize.y / 2f;
        editableMeshCollider.center = newEditableMeshCenter;

        Vector3 endMeshPos = endMesh.transform.localPosition;
        endMeshPos.y = -size + endMesh.Offset;
        endMesh.transform.localPosition = endMeshPos;
        
        editableMesh.GetComponent<MeshSizeChanger>().SetSize();
        ////
        
        Vector3 childLocPos = end.localPosition;
        childLocPos.y = -size;
        end.localPosition = childLocPos;

        ConfigurableJoint cj = end.GetComponent<ConfigurableJoint>();
        if (cj != null)
        {
            cj.connectedAnchor = childLocPos;
        }
    }

    protected float getRotation(Transform from, Transform to)
    {
        var relativePos =
            InvTransformPoint(from.position, to.position, Quaternion.Euler(new Vector3(0, 0, -90)), from.localScale);
        Quaternion rotation = LookAt(relativePos);
        var eulerRot = rotation.eulerAngles;

        if (eulerRot.z > 180)
        {
            eulerRot.z = eulerRot.z - 360f;
        }


        return eulerRot.z;
    }

    private void ChangeSize(float precSize)
    {
        precSize = Mathf.Clamp(precSize, .6f, 1.5f);
        size = baseSize * precSize;
        Vector3 newSize = Collider.size;
        newSize.y = size;
        Collider.size = newSize;
        Vector3 newCenter = Collider.center;
        newCenter.y = -size / 2f;
        Collider.center = newCenter;
        Vector3 childLocPos = end.localPosition;
        childLocPos.y = -size;
        end.localPosition = childLocPos;
        
        ////
        EditablePart editablePart = part.GetComponent<EditablePart>();
        EditableMesh editableMesh = editablePart.EditableMesh;
        EndMesh endMesh = editablePart.EndMesh;
        BoxCollider editableMeshCollider = editableMesh.GetComponent<BoxCollider>();
        Vector3 editableNewSize = editableMeshCollider.size;
        editableNewSize.y = newSize.y - (editableMesh.DownOffset + editableMesh.UpOffset);
        editableMeshCollider.size = editableNewSize;
        Vector3 newEditableMeshCenter = editableMeshCollider.center;
        
        newEditableMeshCenter.y = -editableNewSize.y / 2f;
        editableMeshCollider.center = newEditableMeshCenter;

        Vector3 endMeshPos = endMesh.transform.localPosition;
        endMeshPos.y = -size + endMesh.Offset;
        endMesh.transform.localPosition = endMeshPos;
        
        editableMesh.GetComponent<MeshSizeChanger>().SetSize();
        ////


        ConfigurableJoint cj = end.GetComponent<ConfigurableJoint>();
        if (cj != null)
        {
            cj.connectedAnchor = childLocPos;
        }
    }

    protected Quaternion LookAt(Vector3 relativePos)
    {
        float rotationZ = Mathf.Atan2(relativePos.y, relativePos.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(0.0f, 0.0f, -rotationZ);
    }

    private Vector3 InvTransformPoint(Vector3 t, Vector3 p, Quaternion r, Vector3 s)
    {
        Vector3 sInv = new Vector3(1 / s.x, 1 / s.y, 1 / s.z);
        Vector3 q = Vector3.Scale(sInv, (Quaternion.Inverse(r) * (p - t)));
        return q;
    }

    public static float ClampAng(float angle, float min, float max)
    {
        float start = (min + max) * 0.5f - 180;
        float floor = Mathf.FloorToInt((angle - start) / 360) * 360;
        min += floor;
        max += floor;
        return Mathf.Clamp(angle, min, max);
    }


    public static float ClampAngle(float angle, float min, float max)
    {
        angle = Mathf.Repeat(angle, 360);
        min = Mathf.Repeat(min, 360);
        max = Mathf.Repeat(max, 360);
        bool inverse = false;
        var tmin = min;
        var tangle = angle;
        if (min > 180)
        {
            inverse = !inverse;
            tmin -= 180;
        }

        if (angle > 180)
        {
            inverse = !inverse;
            tangle -= 180;
        }

        var result = !inverse ? tangle > tmin : tangle < tmin;
        if (!result)
            angle = min;

        inverse = false;
        tangle = angle;
        var tmax = max;
        if (angle > 180)
        {
            inverse = !inverse;
            tangle -= 180;
        }

        if (max > 180)
        {
            inverse = !inverse;
            tmax -= 180;
        }

        result = !inverse ? tangle < tmax : tangle > tmax;
        if (!result)
            angle = max;
        return angle;
    }
}