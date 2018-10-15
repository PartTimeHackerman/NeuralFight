using UnityEngine;

public class PunchPointSpawner : MonoBehaviour
{
    public Rigidbody root;
    public Rigidbody upper;
    public Rigidbody end;
    public ConfigurableJoint upperJoint;

    private float dist = 0f;
    private float minAngle = 0f, maxAngle = 0f;
    public bool spawn = false;
    public GameObject point;

    void Awake()
    {
        dist = Vector3.Distance(upper.position, end.position);
        minAngle = upperJoint.lowAngularXLimit.limit;
        maxAngle = upperJoint.highAngularXLimit.limit;
    }

    void Update()
    {
        if (spawn)
        {
            for (int i = 0; i < 100; i++)
            {
                spawnRandom();
            }


            spawn = false;
        }
    }

    public Vector3 spawnRandom()
    {
        if (point == null)
        {
            point = new GameObject("punch_point");
            DontDestroyOnLoad(point);
        }

        float rootAngle = root.rotation.eulerAngles.z;
        float randAngle = Random.Range(minAngle + 10f, maxAngle - 10f) - rootAngle;

        Vector3 pointPos = upper.position;
        pointPos.x += dist;

        point.transform.position = pointPos;
        point.transform.RotateAround(upper.position, Vector3.back, randAngle);
        pointPos = point.transform.position;

        /*
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Destroy(sphere.GetComponent<SphereCollider>());
        sphere.transform.localScale = new Vector3(.1f, .1f, .1f);
        sphere.GetComponent<MeshRenderer>().material.color = Color.red;
        sphere.transform.position = pointPos;
        */


        return pointPos;
    }
}