using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentHoldNote : MonoBehaviour
{
    private GameObject startNote;
    private GameObject endNote;
    private Chart chart;

    public Material redLineMaterial;
    public Material greenLineMaterial;
    public Material blueLineMaterial;

    public Vector3 start;
    public Vector3 end;

    private float lineY = 0.1f;

    void Start()
    {
        chart = GameObject.Find("Lane").GetComponent<LaneTest>().chart;
        startNote = transform.Find("LaneNoteS").gameObject;
        endNote = transform.Find("LaneNoteE").gameObject;
        string color = startNote.GetComponent<NoteColorTrigger>().color;
        Line thisLine = startNote.GetComponent<NoteColorTrigger>().line;
        List<Line> lines = new List<Line>();
        Material material = Resources.Load<Material>("RedHoldMaterial");

        switch (color)
        {
            case "red": material = Resources.Load<Material>("RedHoldMaterial"); lineY = 0.103f; break;
            case "green": material = Resources.Load<Material>("GreenHoldMaterial"); lineY = 0.102f; break;
            case "blue": material = Resources.Load<Material>("BlueHoldMaterial"); lineY = 0.101f; break;
            case "leftwall": material = Resources.Load<Material>("WallHoldMaterial"); break;
            case "rightwall": material = Resources.Load<Material>("WallHoldMaterial"); break;
        }

        //Debug.Log($"Hold note: {color} Line:");
        //Debug.Log(thisLine.vectors.ToArray());


        List<Vector3> vectors = new List<Vector3>();
        vectors.Add(new Vector3(startNote.transform.position.x, lineY, startNote.transform.position.z));
        foreach (Vector3 vec in thisLine.vectors)
        {
            if (vec.z >= start.z && vec.z <= end.z)
            {
                //Debug.Log("VectorFound");
                vectors.Add(new Vector3(vec.x, lineY, vec.z));
            }
        }
        //Debug.Log(vectors.Count);
        if (vectors[vectors.Count - 1].z < end.z)
        {
            vectors.Add(new Vector3(endNote.transform.position.x, lineY, endNote.transform.position.z));
        }
        GameObject line = CreateLine(vectors.ToArray(), 0.15f, 0.15f, material);
        line.transform.SetParent(transform);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private GameObject CreateLine(Vector3[] vectors, float width, float colliderWidth, Material material)
    {
        GameObject myLine = new GameObject("LineSegment");

        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = material;
        lr.startColor = Color.red;
        lr.endColor = Color.red;
        lr.positionCount = vectors.Length;
        lr.SetPositions(vectors);

        lr.startWidth = width;
        lr.endWidth = width;


        Mesh colliderMesh = new Mesh();
        myLine.AddComponent<MeshCollider>().sharedMesh = colliderMesh;

        Vector3[] colliderVectors = new Vector3[vectors.Length * 2 + 2];
        if (vectors.Length == 2 && vectors[0].z == vectors[1].z)
        {
            colliderVectors = new Vector3[4];
            Vector3 vec = vectors[0];
            colliderVectors[0] = new Vector3(vec.x - colliderWidth, vec.y, vec.z - 0.01f);
            colliderVectors[1] = new Vector3(vec.x + colliderWidth, vec.y, vec.z - 0.01f);
            colliderVectors[2] = new Vector3(vec.x - colliderWidth, vec.y, vec.z + 0.01f);
            colliderVectors[3] = new Vector3(vec.x + colliderWidth, vec.y, vec.z + 0.01f);
        }
        else
        {
            for (int i = 0, v = 0; i < vectors.Length; i++, v += 2)
            {
                Vector3 vec = vectors[i];
                colliderVectors[v] = new Vector3(vec.x - colliderWidth, vec.y, vec.z);
                colliderVectors[v + 1] = new Vector3(vec.x + colliderWidth, vec.y, vec.z);
            }

            // fix odd note placement
            colliderVectors[colliderVectors.Length - 2] = new Vector3(vectors[vectors.Length - 1].x - colliderWidth, vectors[vectors.Length - 1].y, vectors[vectors.Length - 1].z + 0.01f);
            colliderVectors[colliderVectors.Length - 1] = new Vector3(vectors[vectors.Length - 1].x + colliderWidth, vectors[vectors.Length - 1].y, vectors[vectors.Length - 1].z + 0.01f);
        }
        colliderMesh.vertices = colliderVectors;

        int[] triangles = new int[colliderVectors.Length * 3 - 6];
        for (int tri = 0, vert = 0; tri < triangles.Length - 2; tri += 6, vert += 2)
        {
            triangles[0 + tri] = 0 + vert;
            triangles[1 + tri] = triangles[4 + tri] = 2 + vert;
            triangles[2 + tri] = triangles[3 + tri] = 1 + vert;
            triangles[5 + tri] = 3 + vert;
        }
        colliderMesh.triangles = triangles;
        colliderMesh.RecalculateNormals();

        return myLine;
    }
}