using UnityEngine;
using System.Collections.Generic;
public class Lane
{
    public List<Vector3> leftVerticesChart = new List<Vector3>();
    public void AddLeftVertex(Vector3 v)
    {
        leftVerticesChart.Add(v);
    }

    public List<Vector3> rightVerticesChart = new List<Vector3>();
    public void AddRightVertex(Vector3 v)
    {
        rightVerticesChart.Add(v);
    }

    public void Order()
    {
        leftVerticesChart.Sort((Vector3 v1, Vector3 v2) => v1.z.CompareTo(v2.z));

        rightVerticesChart.Sort((Vector3 v1, Vector3 v2) => v1.z.CompareTo(v2.z));
    }

    public string GetRawData()
    {
        string data = "";
        foreach (Vector3 v in leftVerticesChart)
        {
            data += $"WL {v.x} {v.y} {v.z}\n";
        }
        foreach (Vector3 v in rightVerticesChart)
        {
            data += $"WR {v.x} {v.y} {v.z}\n";
        }
        return data;
    }

    public void DeleteDuplicates()
    {
        for (int i = 0; i < leftVerticesChart.Count - 3; i++)
        {
            if (leftVerticesChart[i].z == leftVerticesChart[i + 1].z && leftVerticesChart[i].z == leftVerticesChart[i + 2].z)
            {
                leftVerticesChart.RemoveAt(i + 1);
                i--;
            }
        }

        for (int i = 0; i < rightVerticesChart.Count - 3; i++)
        {
            if (rightVerticesChart[i].z == rightVerticesChart[i + 1].z && rightVerticesChart[i].z == rightVerticesChart[i + 2].z)
            {
                rightVerticesChart.RemoveAt(i + 1);
                i--;
            }
        }

        for (int i = 0; i < leftVerticesChart.Count - 1; i++)
        {
            if (leftVerticesChart[i].z == leftVerticesChart[i + 1].z && leftVerticesChart[i].x == leftVerticesChart[i + 1].x)
            {
                leftVerticesChart.RemoveAt(i);
                i--;
            }
        }

        for (int i = 0; i < rightVerticesChart.Count - 1; i++)
        {
            if (rightVerticesChart[i].z == rightVerticesChart[i + 1].z && rightVerticesChart[i].x == rightVerticesChart[i + 1].x)
            {
                rightVerticesChart.RemoveAt(i);
                i--;
            }
        }
    }

    public void EvenOutVertices()
    {
        for(int i = 0; i<rightVerticesChart.Count - 2; i++)
        {
            if (rightVerticesChart[i].z == rightVerticesChart[i + 1].z)
            {
                Vector3 vec = rightVerticesChart[i];
                for (int j = 0; i < leftVerticesChart.Count-2; j++)
                {
                    if(leftVerticesChart[j].z == vec.z)
                    {
                        if (leftVerticesChart[j].z != leftVerticesChart[j + 1].z)
                        {
                            leftVerticesChart.Insert(
                                j,
                                new Vector3(leftVerticesChart[j].x, leftVerticesChart[j].y, leftVerticesChart[j].z)
                            );
                        }
                        break;
                    }
                }
            }
        }

        for (int i = 0; i < leftVerticesChart.Count - 1; i++)
        {
            if (leftVerticesChart[i].z == leftVerticesChart[i + 1].z)
            {
                Vector3 vec = leftVerticesChart[i];
                for (int j = 0; i < rightVerticesChart.Count-1; j++)
                {
                    if (rightVerticesChart[j].z == vec.z)
                    {
                        if (rightVerticesChart[j].z != rightVerticesChart[j + 1].z)
                        {
                            rightVerticesChart.Insert(
                                j,
                                new Vector3(rightVerticesChart[j].x, rightVerticesChart[j].y, rightVerticesChart[j].z)
                            );

                        }
                        break;
                    }
                }
            }
        }
    }

    public void GenerateMeshableVertices()
    {

        // recorre vértices da esquerda
        for (int i = 0; i < leftVerticesChart.Count; i++)
        {
            Vector3 vec = leftVerticesChart[i];
            int lastSmaller = -1;
            // recorre vértices da dereita
            for (int j = 0; j < rightVerticesChart.Count; j++)
            {
                // en busca do último con z menor ca o actual
                if (rightVerticesChart[j].z < vec.z)
                {
                    lastSmaller = j;
                }
            }
            // se o seguinte vértice ao atopado non ten unha z igual ao actual
            if (rightVerticesChart[lastSmaller + 1].z != vec.z)
            {
                // inserta un novo vértice paralelo con respecto ao eixe z na
                // posición indicada coa posición calculada con respecto aos adxacentes
                rightVerticesChart.Insert(
                    lastSmaller + 1,
                    new Vector3((rightVerticesChart[lastSmaller].x + rightVerticesChart[lastSmaller + 1].x) / 2f, 0, vec.z)
                );
            }
        }

        // recorre vértices da dereita
        for (int i = 0; i < rightVerticesChart.Count-1; i++)
        {
            Vector3 vec = rightVerticesChart[i];
            int lastSmaller = -1;
            // recorre vértices da esquerda
            for (int j = 0; j < leftVerticesChart.Count; j++)
            {
                // en busca do último con z menor ca o actual
                if (leftVerticesChart[j].z < vec.z)
                {
                    lastSmaller = j;
                }
            }
            // se o seguinte vértice ao atopado non ten unha z igual ao actual
            if (leftVerticesChart[lastSmaller + 1].z != vec.z)
            {
                // inserta un novo vértice paralelo con respecto ao eixe z na
                // posición indicada coa posición calculada con respecto aos adxacentes
                leftVerticesChart.Insert(
                    lastSmaller + 1,
                    new Vector3((leftVerticesChart[lastSmaller].x + leftVerticesChart[lastSmaller + 1].x) / 2f, 0, vec.z)
                );
            }
        }

        leftVerticesChart.Insert(0, new Vector3(-2.4f, 0, -5));
        rightVerticesChart.Insert(0, new Vector3(2.4f, 0, -5));
    }
}