using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class MeshSizeChanger : MonoBehaviour
{
    public float BaseScaleX = 1.0f;
    public float BaseScaleY = 1.0f;
    public float BaseScaleZ = 1.0f;
    
    public float ScaleX = 1.0f;
    public float ScaleY = 1.0f;
    public float ScaleZ = 1.0f;
    public bool RecalculateNormals = false;
    public Color color = Color.white;
    private Vector3[] _baseVertices;
    private Renderer _renderer;
    private Vector3 colliderPivot;
    private BoxCollider collider;
    
    
    void Awake()
    {
        
        collider = GetComponent<BoxCollider>();
        _renderer = GetComponent<Renderer>();
        BaseScaleX = collider.size.x;
        BaseScaleY = collider.size.y;
        BaseScaleZ = collider.size.z;
    }

    
    public void SetSize()
    {
        var mesh = GetComponent<MeshFilter>().mesh;
        if (_baseVertices == null)
            _baseVertices = mesh.vertices;

        colliderPivot = Vector3.zero;
        ScaleX = collider.size.x;
        ScaleY = collider.size.y;
        ScaleZ = collider.size.z;
        var vertices = new Vector3[_baseVertices.Length];
        for (var i = 0; i < vertices.Length; i++)
        {
            var vertex = _baseVertices[i];
            var newVertex = vertices[i];
            newVertex.x = (vertex.x * (ScaleX/BaseScaleX)) + colliderPivot.x;
            newVertex.y = (vertex.y * (ScaleY/BaseScaleY)) + colliderPivot.y;
            newVertex.z = (vertex.z * (ScaleZ/BaseScaleZ)) + colliderPivot.z;
            vertices[i] = newVertex;
        }

        mesh.vertices = vertices;
        if (RecalculateNormals)
            mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    void setColor()
    {
        _renderer.material.SetColor("_Color", new HSBColor(Random.Range(0f, 1f),1f,1f,1f).ToColor());
        //_renderer.material.SetColor("_Color", color);
    }
}