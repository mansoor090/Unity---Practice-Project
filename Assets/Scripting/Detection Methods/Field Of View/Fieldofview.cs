using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius;
    public float actualviewRadius;
    public float fireRadius;
    public float viewAngle;
    public LayerMask TargetMask;

    public LayerMask ObstacleMask;
    public Transform RequiredTarget;

    [SerializeField] private Mesh viewMesh;
    [SerializeField] private MeshFilter viewMeshFilter;
    [SerializeField] private Material viewMeshMaterial;
    [SerializeField] private int stepAngleSize;
    [SerializeField] private int maxResolutions;

    [SerializeField] private Color normalColor;

    [SerializeField] internal bool targetIsNotinView;


    //  [SerializeField] private MeshCollider Collider;


    private const int zero = 0;
    private const int one = 1;
    private const int two = 2;
    private const int three = 3;
    private const int ten = 10;

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float distance;
        public float angle;

        public ViewCastInfo(bool hit, Vector3 point, float distance, float angle)
        {
            this.hit = hit;
            this.point = point;
            this.distance = distance;
            this.angle = angle;
        }
    }

    void Start()
    {
//        levelManager = FindObjectOfType<LeveLManager>();
//        levelManager.Eggs.Add(this);
        setViewMeshProperties();

        fireRadius = actualviewRadius * 4;
    }

    void setViewMeshProperties()
    {
        viewMesh = new Mesh();
        viewMesh.name = "ViewMesh";
        viewMeshFilter.sharedMesh = viewMesh;
        viewMeshMaterial = viewMeshFilter.GetComponent<MeshRenderer>().material;
        normalColor = viewMeshMaterial.color;
    }

    void Update()
    {
        DrawFieldOfView();
    }

    void LateUpdate()
    {
       
  
    }

   


    void DrawFieldOfView()
    {
        if (stepAngleSize <= 0)
        {
            return;
        }

        int stepCounts = Mathf.RoundToInt(viewAngle * maxResolutions);
        //stepAngleSize = viewAngle / stepCounts;
        List<Vector3> viewPoints = new List<Vector3>();
        for (int i = zero; i <= stepAngleSize; i++)
        {
            float angle = transform.eulerAngles.y - (viewAngle / two) + (i * stepAngleSize)/(stepCounts);
//            float angle = transform.eulerAngles.y - (viewAngle / two) + (i * stepAngleSize);
//            Debug.DrawLine(transform.position,transform.position+ ReturnAngle(angle,true) * viewRadius,Color.cyan, 0.01f );

            ViewCastInfo newViewCast = ViewCast(angle);
            viewPoints.Add(newViewCast.point);
        }

        int vertexCount = viewPoints.Count + one;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - two) * three];
        vertices[zero] = Vector3.zero;

        for (int i = zero; i < vertexCount - two; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);
            if (i < vertexCount - two)
            {
                triangles[i * three] = zero;
                triangles[i * three + one] = i + one;
                triangles[i * three + two] = i + two;
            }
        }

        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
        //  Collider.sharedMesh = viewMesh;
    }

    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = ReturnAngle(globalAngle, true);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, viewRadius, ObstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
        }
    }

    public Vector3 ReturnAngle(float angleinDegrees, bool AngleIsGlobal)
    {
        if (!AngleIsGlobal)
        {
            angleinDegrees += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Sin(angleinDegrees * Mathf.Deg2Rad), zero, Mathf.Cos(angleinDegrees * Mathf.Deg2Rad));
    }
}
