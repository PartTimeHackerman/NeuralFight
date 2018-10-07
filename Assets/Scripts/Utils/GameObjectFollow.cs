using UnityEngine;
using System.Collections;


public class GameObjectFollow : MonoBehaviour
{
    public Transform objectToFollow;
    public bool setX = true;
    public bool setY = true;
    private Transform transform;
    
    void Start()
    {
        transform = GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        var oldPos = transform.position;
        var newPos = objectToFollow.position;
        oldPos.x = setX ? newPos.x : oldPos.x;
        oldPos.y = setY ? newPos.y : oldPos.y;
        transform.position = oldPos;
    }

}
