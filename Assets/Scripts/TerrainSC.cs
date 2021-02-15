using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class TerrainSC : MonoBehaviour
{
    public float map_size = 20f;
    public int points_amount = 100;
    public float noise_koef = 0.3f;
    public float max_height = 2f;
    private Vector2[] points_list;

    public float functionWeight = 0.2f;


    // Start is called before the first frame update
    void Start()
    {

        this.generateMesh();
        this.generateCollider();
        GameObject.Find( "Main Camera" ).GetComponent<CameraMovement>().mapGenerated();
    }

    float shape_function( float x )
    {
        float mapped = x * 2.2f - 1.1f;
        //return ( float ) ( -( mapped * mapped ) + 1 );
        return GlobalVariables.Map( ( float ) ( ( ( -Math.Pow( mapped, 4 ) + Math.Pow( mapped, 2 ) ) / 0.25 ) * functionWeight ), -1f, 1f, 1f - 2 * this.functionWeight, 1f );
    }


    void generateMesh()
    {
        this.points_list = new Vector2[this.points_amount + 3];
        float x_step = this.map_size / this.points_amount;

        this.points_list[0] = new Vector2( -points_amount / 2 * x_step, -5 );

        for ( int i = -points_amount / 2; i < points_amount / 2 + 1; i++ )
        {
            float mapped_i = ( i + ( points_amount / 2f ) ) / ( float ) ( points_amount + 1 );
            this.points_list[i + points_amount / 2 + 1] = new Vector2( i * x_step, Mathf.PerlinNoise( mapped_i * this.noise_koef, 0.0f ) * max_height * this.shape_function( mapped_i ) );
        }

        this.points_list[this.points_amount + 2] = new Vector2( points_amount / 2 * x_step, -5 );

        Triangulator tr = new Triangulator( this.points_list );
        int[] indices = tr.Triangulate();

        Vector3[] vertices = new Vector3[this.points_list.Length];
        for ( int i = 0; i < vertices.Length; i++ )
        {
            vertices[i] = new Vector3( this.points_list[i].x, this.points_list[i].y, 0 );
        }

        Mesh msh = new Mesh();
        msh.vertices = vertices;
        msh.triangles = indices;
        msh.RecalculateNormals();
        msh.RecalculateBounds();


        MeshFilter filter = gameObject.AddComponent( typeof( MeshFilter ) ) as MeshFilter;
        filter.mesh = msh;
    }

    // Update is called once per frame

    void generateCollider()
    {

        int[] triangles = GetComponent<MeshFilter>().mesh.triangles;
        Vector3[] vertices = GetComponent<MeshFilter>().mesh.vertices;

        Dictionary<string, KeyValuePair<int, int>> edges = new Dictionary<string, KeyValuePair<int, int>>();
        for ( int i = 0; i < triangles.Length; i += 3 )
        {
            for ( int e = 0; e < 3; e++ )
            {
                int vert1 = triangles[i + e];
                int vert2 = triangles[i + e + 1 > i + 2 ? i : i + e + 1];
                string edge = Mathf.Min( vert1, vert2 ) + ":" + Mathf.Max( vert1, vert2 );
                if ( edges.ContainsKey( edge ) )
                {
                    edges.Remove( edge );
                }
                else
                {
                    edges.Add( edge, new KeyValuePair<int, int>( vert1, vert2 ) );
                }
            }
        }

        Dictionary<int, int> lookup = new Dictionary<int, int>();
        foreach ( KeyValuePair<int, int> edge in edges.Values )
        {
            if ( lookup.ContainsKey( edge.Key ) == false )
            {
                lookup.Add( edge.Key, edge.Value );
            }
        }

        PolygonCollider2D polygonCollider = this.GetComponent<PolygonCollider2D>();
        polygonCollider.pathCount = 0;

        int startVert = 0;
        int nextVert = startVert;
        int highestVert = startVert;
        List<Vector2> colliderPath = new List<Vector2>();
        while ( true )
        {

            colliderPath.Add( vertices[nextVert] );

            nextVert = lookup[nextVert];

            if ( nextVert > highestVert )
            {
                highestVert = nextVert;
            }

            if ( nextVert == startVert )
            {
                polygonCollider.pathCount++;
                polygonCollider.SetPath( polygonCollider.pathCount - 1, colliderPath.ToArray() );
                colliderPath.Clear();

                if ( lookup.ContainsKey( highestVert + 1 ) )
                {

                    startVert = highestVert + 1;
                    nextVert = startVert;

                    continue;
                }
                break;
            }
        }

    }

    void Update()
    {

    }
}
