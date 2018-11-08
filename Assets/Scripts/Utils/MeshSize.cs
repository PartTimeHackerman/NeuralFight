using UnityEngine;
using System.Collections;

public class MeshSize : MonoBehaviour
{
    public float ScaleX = 1.0f;
    public float ScaleY = 1.0f;
    public float ScaleZ = 1.0f;
    public bool RecalculateNormals = false;
    public Color color = Color.white;
    private Vector3[] _baseVertices;
    private Renderer _renderer;
    private Vector3 colliderPivot;
    
    
    void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    void Reset()
    {
        var mesh = GetComponent<MeshFilter>().mesh;
        if (_baseVertices == null)
            _baseVertices = mesh.vertices;
        
        BoxCollider collider = GetComponent<BoxCollider>();
        colliderPivot = collider.center;
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
        var vertices = new Vector3[_baseVertices.Length];
        for (var i = 0; i < vertices.Length; i++)
        {
            var vertex = _baseVertices[i];
            var newVertex = vertices[i];
            newVertex.x = (vertex.x * ScaleX) + colliderPivot.x;
            newVertex.y = (vertex.y * ScaleY) + colliderPivot.y;
            newVertex.z = (vertex.z * ScaleZ) + colliderPivot.z;
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