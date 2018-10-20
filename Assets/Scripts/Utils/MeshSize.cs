using UnityEngine;
using System.Collections;

public class MeshSize : MonoBehaviour
{
    public float ScaleX = 1.0f;
    public float ScaleY = 1.0f;
    public float ScaleZ = 1.0f;
    public bool RecalculateNormals = false;
    private Vector3[] _baseVertices;
    private Renderer _renderer;
    void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    void Reset()
    {
        BoxCollider collider = GetComponent<BoxCollider>();
        ScaleX = collider.size.x;
        ScaleY = collider.size.y;
        ScaleZ = collider.size.z;
        setSize();
    }

    public void Start()
    {
        setColor();
    }

    void setSize()
    {
        var mesh = GetComponent<MeshFilter>().mesh;
        if (_baseVertices == null)
            _baseVertices = mesh.vertices;
        var vertices = new Vector3[_baseVertices.Length];
        for (var i = 0; i < vertices.Length; i++)
        {
            var vertex = _baseVertices[i];
            vertex.x = vertex.x * ScaleX;
            vertex.y = vertex.y * ScaleY;
            vertex.z = vertex.z * ScaleZ;
            vertices[i] = vertex;
        }

        mesh.vertices = vertices;
        if (RecalculateNormals)
            mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    void setColor()
    {
        _renderer.material.SetColor("_Color", Random.ColorHSV());
    }
}