using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class LaneTest : MonoBehaviour
{
    public float barSize;
    public Material redLineMaterial;
    public Material greenLineMaterial;
    public Material blueLineMaterial;
    public Material wallLineMaterial;

    public GameObject notePrefab;
    public GameObject holdNotePrefab;
    public GameObject bellPrefab;
    public GameObject bulletPrefab;
    public GameObject leftFlickPrefab;
    public GameObject rightFlickPrefab;

    public string chartID;
    public Difficulty songDifficulty;

    public Chart chart;
    private ChartLoader chartLoader = new ChartLoader(Path.Combine(Directory.GetCurrentDirectory(), "charts"));


    private Conductor conductor;

    private Mesh mesh;

    private void SetupLane()
    {
        chart = chartLoader.LoadChart(/*chartID*/ SongSaver.SongID, SongSaver.Difficulty, barSize);
        GameObject.Find("SongName").GetComponent<Text>().text = chart.SongInfo.title;
        GameObject.Find("Author").GetComponent<Text>().text = chart.SongInfo.artist;

        Texture2D sampleTexture = new Texture2D(2, 2);
        bool isLoaded = sampleTexture.LoadImage(chart.Jacket);
        // apply this texure as per requirement on image or material
        GameObject image = GameObject.Find("RawImage");
        if (isLoaded)
        {
            image.GetComponent<RawImage>().texture = sampleTexture;
        }
    }

    public void Awake()
    {
        PlayerSongStats.Init();
        Application.targetFrameRate = 60;
        SetupLane();
        CreateMesh();
        string songpath = Path.Combine(chart.FolderPath, chart.SongInfo.music.file);
        GetComponent<Conductor>().musicSource = songpath;
        GetComponent<Conductor>().songBpm = chart.SongBPM[0].Value;
        GetComponent<Conductor>().beatsInBar = chart.SongMetrics[0].Numerator;
    }

    public void Start()
    {
        PlayerSongStats.Instance.chart = chart;
        PlayerSongStats.Instance.SetTotalBells(chart.Bells.Count);
        PlayerSongStats.Instance.SetTotalFlicks(chart.RightFlicks.Count + chart.LeftFlicks.Count);
        PlayerSongStats.Instance.SetTotalNotes(chart.Notes.Count);
        PlayerSongStats.Instance.CalculateScoreValues();

        DrawLines();
        DrawNotes();
        DrawBells();
        DrawBullets();
        DrawFlicks();
    }
    private void CreateMesh()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Lane";

        List<Vector3> lVertices = new List<Vector3>();
        int vmax = 0;

        if (chart.Lane.leftVerticesChart.Count > chart.Lane.rightVerticesChart.Count)
        {
            vmax = chart.Lane.rightVerticesChart.Count;
        }
        else
        {
            vmax = chart.Lane.leftVerticesChart.Count;
        }

        for (int i = 0; i < vmax; i++)
        {
            lVertices.Add(chart.Lane.leftVerticesChart[0 + i]);
            lVertices.Add(chart.Lane.rightVerticesChart[0 + i]);
        }
        Vector3[] verticesv = lVertices.ToArray();
        mesh.vertices = verticesv;

        int[] triangles = new int[verticesv.Length * 3 - 6];
        for (int tri = 0, vert = 0; tri < triangles.Length - 2; tri += 6, vert += 2)
        {
            //if(verticesv[1+vert].z = verticesv[3+vert].z){
            /*triangles[0+tri] = 0+vert;
            triangles[1 + tri] = triangles[4 + tri] = 2 + vert;
            triangles[2 + tri] = triangles[3 + tri] = 1 + vert;
            triangles[5 + tri] = 3 + vert;*/
            //} else {
            triangles[0 + tri] = 0 + vert;
            triangles[1 + tri] = triangles[4 + tri] = 2 + vert;
            triangles[2 + tri] = triangles[3 + tri] = 1 + vert;
            triangles[5 + tri] = 3 + vert;
            //vert++;
            //}
        }

        mesh.triangles = triangles;
        mesh.RecalculateNormals();
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

    private void DrawLines()
    {
        GameObject lineholder = GameObject.Find("LineDrawer");
        foreach (Line line in chart.RedLine)
        {
            GameObject myLine = CreateLine(line.vectors.ToArray(), 0.05f / 2f, 0.35f / 2f, redLineMaterial);

            myLine.tag = "redline";

            myLine.AddComponent<LineInfo>().line = line;

            myLine.transform.SetParent(lineholder.transform);
            //Instantiate(myLine, lineholder.transform.position, Quaternion.identity);
        }

        foreach (Line line in chart.GreenLine)
        {
            GameObject myLine = CreateLine(line.vectors.ToArray(), 0.05f / 2f, 0.35f / 2f, greenLineMaterial);

            myLine.tag = "greenline";

            myLine.AddComponent<LineInfo>().line = line;

            myLine.transform.SetParent(lineholder.transform);
            //Instantiate(myLine, lineholder.transform.position, Quaternion.identity);
        }

        foreach (Line line in chart.BlueLine)
        {
            GameObject myLine = CreateLine(line.vectors.ToArray(), 0.05f / 2f, 0.35f / 2f, blueLineMaterial);

            myLine.tag = "blueline";

            myLine.AddComponent<LineInfo>().line = line;

            myLine.transform.SetParent(lineholder.transform);
            //Instantiate(myLine, lineholder.transform.position, Quaternion.identity);
        }

        // left line
        GameObject leftLine = new GameObject("LeftWallSegment");
        leftLine.AddComponent<LineRenderer>();
        LineRenderer llr = leftLine.GetComponent<LineRenderer>();
        llr.material = wallLineMaterial;
        llr.startColor = Color.red;
        llr.endColor = Color.red;
        llr.positionCount = chart.Lane.leftVerticesChart.Count;
        llr.SetPositions(chart.Lane.leftVerticesChart.ToArray());

        llr.startWidth = 0.15f;
        llr.endWidth = 0.15f;
        MeshCollider lMeshCollider = leftLine.AddComponent<MeshCollider>();
        Mesh lMesh = new Mesh();
        llr.BakeMesh(lMesh, true);
        lMeshCollider.sharedMesh = lMesh;

        llr.startWidth = 0.05f;
        llr.endWidth = 0.05f;

        leftLine.tag = "leftwallline";
        Line lLine = new Line();
        lLine.vectors = chart.Lane.leftVerticesChart;
        leftLine.AddComponent<LineInfo>().line = lLine;

        leftLine.transform.SetParent(lineholder.transform);

        // right line
        GameObject rightLine = new GameObject("RightWallSegment");
        rightLine.AddComponent<LineRenderer>();
        LineRenderer rlr = rightLine.GetComponent<LineRenderer>();
        rlr.material = wallLineMaterial;
        rlr.startColor = Color.red;
        rlr.endColor = Color.red;
        rlr.positionCount = chart.Lane.rightVerticesChart.Count;
        rlr.SetPositions(chart.Lane.rightVerticesChart.ToArray());

        rlr.startWidth = 0.15f;
        rlr.endWidth = 0.15f;
        MeshCollider rightMeshCollider = rightLine.AddComponent<MeshCollider>();
        Mesh rightMesh = new Mesh();
        rlr.BakeMesh(rightMesh, true);
        rightMeshCollider.sharedMesh = rightMesh;

        rlr.startWidth = 0.05f;
        rlr.endWidth = 0.05f;

        rightLine.tag = "rightwallline";
        Line rLine = new Line();
        rLine.vectors = chart.Lane.rightVerticesChart;
        rightLine.AddComponent<LineInfo>().line = rLine;

        rightLine.transform.SetParent(lineholder.transform);
    }

    private void DrawNotes()
    {
        GameObject notesHolder = GameObject.Find("NotesHolder");
        foreach (Vector3 note in chart.Notes)
        {
            Instantiate(notePrefab, note, Quaternion.identity, notesHolder.transform);
        }

        foreach (HoldNote hnote in chart.HoldNotes)
        {
            GameObject holdNote = Instantiate(holdNotePrefab, hnote.StartPosition, Quaternion.identity, notesHolder.transform);
            GameObject startNote = holdNote.transform.Find("LaneNoteS").gameObject;
            GameObject endNote = holdNote.transform.Find("LaneNoteE").gameObject;
            Vector3 startPos = new Vector3(hnote.StartPosition.x, 0, hnote.StartPosition.z);
            Vector3 endPos = new Vector3(hnote.EndPosition.x, 0, hnote.EndPosition.z);
            startNote.transform.position = startPos;
            endNote.transform.position = endPos;

            ParentHoldNote p = holdNote.AddComponent<ParentHoldNote>();
            p.start = hnote.StartPosition;
            p.end = hnote.EndPosition;
        }
    }

    private void DrawBells()
    {
        GameObject bellsHolder = GameObject.Find("BellsHolder");
        foreach (Vector3 bell in chart.Bells)
        {
            Instantiate(bellPrefab, bell, Quaternion.identity, bellsHolder.transform);
        }
    }

    private void DrawBullets()
    {
        GameObject bulletHolder = GameObject.Find("BulletsHolder");
        foreach (Bullet bullet in chart.Bullets)
        {
            GameObject bull = Instantiate(bulletPrefab, bullet.Position, Quaternion.identity, bulletHolder.transform);
            if (bullet.Pattern.Type == BulletType.Static && bullet.Pattern.Velocity != 1)
            {
                StaticBulletController sbc = bull.AddComponent<StaticBulletController>();
                sbc.pattern = bullet.Pattern;
            }
            else if (bullet.Pattern.Type == BulletType.Kinematic)
            {
                KinematicBulletController kbc = bull.AddComponent<KinematicBulletController>();
                kbc.pattern = bullet.Pattern;
            }
        }
    }

    private void DrawFlicks()
    {
        GameObject flickHolder = GameObject.Find("FlicksHolder");
        foreach (Vector3 flick in chart.LeftFlicks)
        {
            Vector3 vec = new Vector3(flick.x, flick.y - 0.5f, flick.z);
            Instantiate(leftFlickPrefab, vec, Quaternion.identity, flickHolder.transform);
        }
        foreach (Vector3 flick in chart.RightFlicks)
        {
            Vector3 vec = new Vector3(flick.x, flick.y - 0.5f, flick.z);
            Instantiate(rightFlickPrefab, vec, Quaternion.identity, flickHolder.transform);
        }
    }

    /*public void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(new Vector3(0, 0, 240), 0.2f);
        Gizmos.DrawSphere(new Vector3(0, 0, 250), 0.2f);
        Gizmos.DrawSphere(new Vector3(0, 0, 260), 0.2f);

        Gizmos.color = Color.black;
        for (int i = 0; i < chart.Lane.leftVerticesChart.Count - 1; i++)
        {
            Gizmos.DrawLine(chart.Lane.leftVerticesChart[i], chart.Lane.leftVerticesChart[i + 1]);
            Gizmos.DrawSphere(chart.Lane.leftVerticesChart[i], 0.1f);
        }

        Gizmos.color = Color.black;
        for (int i = 0; i < chart.Lane.rightVerticesChart.Count - 1; i++)
        {
            Gizmos.DrawLine(chart.Lane.rightVerticesChart[i], chart.Lane.rightVerticesChart[i + 1]);
            Gizmos.DrawSphere(chart.Lane.rightVerticesChart[i], 0.1f);
        }

        Gizmos.color = Color.red;
        foreach (Line line in chart.RedLine)
        {
            for (int i = 0; i < line.vectors.Count - 1; i++)
            {
                Gizmos.DrawLine(line.vectors[i], line.vectors[i + 1]);
            }
        }

        Gizmos.color = Color.green;
        foreach (Line line in chart.GreenLine)
        {
            for (int i = 0; i < line.vectors.Count - 1; i++)
            {
                Gizmos.DrawLine(line.vectors[i], line.vectors[i + 1]);
            }
        }

        Gizmos.color = Color.blue;
        foreach (Line line in chart.BlueLine)
        {
            for (int i = 0; i < line.vectors.Count - 1; i++)
            {
                Gizmos.DrawLine(line.vectors[i], line.vectors[i + 1]);
            }
        }

        Gizmos.color = Color.white;
        foreach (Vector3 note in chart.Notes)
        {
            Gizmos.DrawCube(note, new Vector3(0.3f, 0.2f, 0.2f));
        }
    }*/
}

/*
 private GameObject CreateLine(Vector3[] vectors, float width, float colliderWidth, Material material)
    {
        GameObject myLine = new GameObject("LineSegment");
        Mesh lineMesh = new Mesh();
        Mesh colliderMesh = new Mesh();
        myLine.AddComponent<MeshRenderer>().material = material;
        myLine.AddComponent<MeshFilter>().mesh = lineMesh;
        myLine.AddComponent<MeshCollider>().sharedMesh = colliderMesh;

        Vector3[] lineVectors = new Vector3[vectors.Length * 2 + 2];
        Vector3[] colliderVectors = new Vector3[vectors.Length * 2 + 2];
        if (vectors.Length == 2 && vectors[0].z == vectors[1].z)
        {
            //Debug.Log("mini linea");
            lineVectors = new Vector3[4];
            colliderVectors = new Vector3[4];
            Vector3 vec = vectors[0];
            lineVectors[0] = new Vector3(vec.x - width, vec.y, vec.z - 0.01f);
            lineVectors[1] = new Vector3(vec.x + width, vec.y, vec.z - 0.01f);
            lineVectors[2] = new Vector3(vec.x - width, vec.y, vec.z + 0.01f);
            lineVectors[3] = new Vector3(vec.x + width, vec.y, vec.z + 0.01f);
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

                lineVectors[v] = new Vector3(vec.x - width, vec.y, vec.z);
                lineVectors[v + 1] = new Vector3(vec.x + width, vec.y, vec.z);
                colliderVectors[v] = new Vector3(vec.x - colliderWidth, vec.y, vec.z);
                colliderVectors[v + 1] = new Vector3(vec.x + colliderWidth, vec.y, vec.z);
            }

            // fix odd note placement
            lineVectors[lineVectors.Length - 2] = new Vector3(vectors[vectors.Length - 1].x - width, vectors[vectors.Length - 1].y, vectors[vectors.Length - 1].z + 0.01f);
            lineVectors[lineVectors.Length - 1] = new Vector3(vectors[vectors.Length - 1].x + width, vectors[vectors.Length - 1].y, vectors[vectors.Length - 1].z + 0.01f);
            colliderVectors[lineVectors.Length - 2] = new Vector3(vectors[vectors.Length - 1].x - colliderWidth, vectors[vectors.Length - 1].y, vectors[vectors.Length - 1].z + 0.01f);
            colliderVectors[lineVectors.Length - 1] = new Vector3(vectors[vectors.Length - 1].x + colliderWidth, vectors[vectors.Length - 1].y, vectors[vectors.Length - 1].z + 0.01f);
        }
        lineMesh.vertices = lineVectors;
        colliderMesh.vertices = colliderVectors;

        int[] triangles = new int[lineVectors.Length * 3 - 6];
        for (int tri = 0, vert = 0; tri < triangles.Length - 2; tri += 6, vert += 2)
        {
            triangles[0 + tri] = 0 + vert;
            triangles[1 + tri] = triangles[4 + tri] = 2 + vert;
            triangles[2 + tri] = triangles[3 + tri] = 1 + vert;
            triangles[5 + tri] = 3 + vert;
        }
        lineMesh.triangles = triangles;
        colliderMesh.triangles = triangles;
        lineMesh.RecalculateNormals();
        colliderMesh.RecalculateNormals();

        return myLine;
    }
 */
