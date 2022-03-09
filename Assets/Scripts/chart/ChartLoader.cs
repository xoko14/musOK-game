using System;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using System.Threading.Tasks;
using UnityEngine.Networking;

public class ChartLoader{

    private string location;

    public ChartLoader(string location){
        this.location = location;
    }

    public static Chart LoadInfo(string songLocation)
    {
        SongInfo song = XmlUtil.ImportXml<SongInfo>(songLocation);
        Chart chart = new Chart();
        chart.SongInfo = song;
        chart.FolderPath = songLocation;
        return chart;
    }

    public Chart LoadChart(string id, Difficulty dif, float barSize){
        List<BulletPattern> bps = new List<BulletPattern>();

        string songLocation = Path.Combine(location, id);
        Debug.Log(Path.Combine(songLocation, "song.xml"));
        SongInfo song = XmlUtil.ImportXml<SongInfo>(Path.Combine(songLocation, "song.xml"));
        Debug.Log(song.title);

        
        
        Chart chart = new Chart();
        chart.SongInfo = song;
        chart.FolderPath = songLocation;
        string chartString = "";

        if (chart.SongInfo.jacket != null)
        {
            chart.Jacket = File.ReadAllBytes(Path.Combine(songLocation, song.jacket.file));
        }

        switch(dif){
            case Difficulty.Easy:
                chartString = File.ReadAllText(Path.Combine(songLocation, song.easy.file));
                break;
            
            case Difficulty.Normal:
                chartString = File.ReadAllText(Path.Combine(songLocation, song.normal.file));
                break;
            
            case Difficulty.Hard:
                Debug.Log(Path.Combine(songLocation, song.hard.file));
                chartString = File.ReadAllText(Path.Combine(songLocation, song.hard.file));
                break;
        }

        using var sr = new StringReader(chartString);
        string line;
        Line rline = new Line();
        Line gline = new Line();
        Line bline = new Line();
        while ((line = sr.ReadLine()) != null)
        {
            if (line.Length > 3)
            {
                string[] lline = line.Split(' ');
                if (lline[0] == "WR")
                {
                    if (lline[4] == "S")
                    {
                        chart.Lane.AddRightVertex(new Vector3(24 / 10f, 0, (float.Parse(lline[1]) + (float.Parse(lline[2]) / 1920f)) * barSize));
                    }

                    Vector3 vector = new Vector3(float.Parse(lline[3]) / 10f, 0, (float.Parse(lline[1]) + (float.Parse(lline[2]) / 1920f)) * barSize);
                    chart.Lane.AddRightVertex(vector);

                    if (lline[4] == "E")
                    {
                        chart.Lane.AddRightVertex(new Vector3(24 / 10f, 0, (float.Parse(lline[1]) + (float.Parse(lline[2]) / 1920f)) * barSize));
                    }
                }
                else if (lline[0] == "WL")
                {
                    if (lline[4] == "S")
                    {
                        chart.Lane.AddLeftVertex(new Vector3(-24 / 10f, 0, (float.Parse(lline[1]) + (float.Parse(lline[2]) / 1920f)) * barSize));
                    }

                    Vector3 vector = new Vector3(float.Parse(lline[3]) / 10f, 0, (float.Parse(lline[1]) + (float.Parse(lline[2]) / 1920f)) * barSize);
                    chart.Lane.AddLeftVertex(vector);

                    if (lline[4] == "E")
                    {
                        chart.Lane.AddLeftVertex(new Vector3(-24 / 10f, 0, (float.Parse(lline[1]) + (float.Parse(lline[2]) / 1920f)) * barSize));
                    }
                }
                else if (lline[0] == "LL")
                {
                    Vector3 vector = new Vector3(float.Parse(lline[3]) / 10f, 0.05f, (float.Parse(lline[1]) + (float.Parse(lline[2]) / 1920f)) * barSize);
                    if (lline[4] == "S")
                    {
                        rline = new Line();
                        rline.vectors.Add(vector);
                    }
                    else if (lline[4] == "E")
                    {
                        rline.vectors.Add(vector);
                        chart.RedLine.Add(rline);
                    }
                    else
                    {
                        rline.vectors.Add(vector);
                    }
                }
                else if (lline[0] == "LC")
                {
                    Vector3 vector = new Vector3(float.Parse(lline[3]) / 10f, 0.05f, (float.Parse(lline[1]) + (float.Parse(lline[2]) / 1920f)) * barSize);
                    if (lline[4] == "S")
                    {
                        gline = new Line();
                        gline.vectors.Add(vector);
                    }
                    else if (lline[4] == "E")
                    {
                        gline.vectors.Add(vector);
                        chart.GreenLine.Add(gline);
                    }
                    else
                    {
                        gline.vectors.Add(vector);
                    }
                }
                else if (lline[0] == "LR")
                {
                    Vector3 vector = new Vector3(float.Parse(lline[3]) / 10f, 0.05f, (float.Parse(lline[1]) + (float.Parse(lline[2]) / 1920f)) * barSize);
                    if (lline[4] == "S")
                    {
                        bline = new Line();
                        bline.vectors.Add(vector);
                    }
                    else if (lline[4] == "E")
                    {
                        bline.vectors.Add(vector);
                        chart.BlueLine.Add(bline);
                    }
                    else
                    {
                        bline.vectors.Add(vector);
                    }
                }
                else if (lline[0] == "NT" || lline[0] == "CT")
                {
                    Vector3 vector = new Vector3(float.Parse(lline[3]) / 10f, 0, (float.Parse(lline[1]) + (float.Parse(lline[2]) / 1920f)) * barSize);
                    chart.Notes.Add(vector);
                }

                else if(lline[0] == "NH" || lline[0] == "CH")
                {
                    Vector3 vectorS = new Vector3(float.Parse(lline[3]) / 10f, 0, (float.Parse(lline[1]) + (float.Parse(lline[2]) / 1920f)) * barSize);
                    Vector3 vectorE = new Vector3(float.Parse(lline[6]) / 10f, 0, (float.Parse(lline[4]) + (float.Parse(lline[5]) / 1920f)) * barSize);
                    HoldNote hold = new HoldNote(vectorS, vectorE);
                    chart.HoldNotes.Add(hold);
                }

                else if (lline[0] == "BE")
                {
                    Vector3 vector = new Vector3(float.Parse(lline[3]) / 10f, 0.5f, (float.Parse(lline[1]) + (float.Parse(lline[2]) / 1920f)) * barSize);
                    chart.Bells.Add(vector);
                }

                else if (lline[0] == "BP")
                {
                    //Debug.Log(lline.Length);
                    BulletPattern bp = new BulletPattern();
                    bp.Name = lline[1];
                    bp.Type = CheckBulletPatternType(lline[2]);
                    bp.PosX = float.Parse(lline[3])/10f;
                    bp.Velocity = float.Parse(lline[5], NumberStyles.Any, CultureInfo.InvariantCulture);
                    bps.Add(bp);

                }

                else if (lline[0] == "BU")
                {
                    Vector3 vector = new Vector3(float.Parse(lline[3]) / 10f, 0.5f, (float.Parse(lline[1]) + (float.Parse(lline[2]) / 1920f)) * barSize);
                    BulletPattern bp;
                    bp = bps.Find(x => x.Name == lline[4]);
                    Bullet bullet = new Bullet(vector, bp);
                    chart.Bullets.Add(bullet);
                }

                else if (lline[0] == "BM"){
                    BpmValue bpm = new BpmValue(float.Parse(lline[3], NumberStyles.Any, CultureInfo.InvariantCulture), int.Parse(lline[1]), int.Parse(lline[2]));
                    chart.SongBPM.Add(bpm);
                }

                else if (lline[0] == "ME"){
                    MetricValue me = new MetricValue(int.Parse(lline[3]), int.Parse(lline[4]), int.Parse(lline[1]), int.Parse(lline[2]));
                    chart.SongMetrics.Add(me);
                }

                else if(lline[0] == "NF" || lline[0] == "CF")
                {
                    if(lline[4] == "L")
                    {
                        chart.LeftFlicks.Add(new Vector3(float.Parse(lline[3]) / 10f, 0.5f, (float.Parse(lline[1]) + (float.Parse(lline[2]) / 1920f)) * barSize));
                    }
                    else if (lline[4] == "R")
                    {
                        chart.RightFlicks.Add(new Vector3(float.Parse(lline[3]) / 10f, 0.5f, (float.Parse(lline[1]) + (float.Parse(lline[2]) / 1920f)) * barSize));
                    }
                }
            }
        }

        chart.Lane.DeleteDuplicates();
        chart.Lane.GenerateMeshableVertices();
        chart.Lane.EvenOutVertices();
        //lane.Order();

        Debug.Log($"Left: {chart.Lane.leftVerticesChart.Count}");
        Debug.Log($"Right: {chart.Lane.rightVerticesChart.Count}");

        return chart;
    }

    private BulletType CheckBulletPatternType(string type)
    {
        switch (type)
        {
            case "ENE":
            case "CEN": return BulletType.Kinematic;
            default: return BulletType.Static;
        }
    }
}