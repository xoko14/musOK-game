using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class Chart{
    public Lane Lane {get; set;}
    public List<Line> RedLine {get; set;}
    public List<Line> GreenLine {get; set;}
    public List<Line> BlueLine {get; set;}
    public List<Vector3> Notes {get; set;}
    public List<HoldNote> HoldNotes {get; set;}
    public List<Bullet> Bullets {get; set;}
    public List<Vector3> Bells {get; set;}
    public List<Vector3> LeftFlicks {get; set;}
    public List<Vector3> RightFlicks {get; set;}
    public byte[] Jacket {get; set;}

    public List<BpmValue> SongBPM { get; set;}
    public List<MetricValue> SongMetrics {get; set;} 

    public SongInfo SongInfo {get; set;}

    public String FolderPath {get; set;}

    public Chart(){
        Lane = new Lane();
        RedLine = new List<Line>();
        GreenLine = new List<Line>();
        BlueLine = new List<Line>();
        Notes = new List<Vector3>();
        HoldNotes = new List<HoldNote>();
        Bullets = new List<Bullet>();
        Bells = new List<Vector3>();
        LeftFlicks = new List<Vector3>();
        RightFlicks = new List<Vector3>();
        SongBPM = new List<BpmValue>();
        SongMetrics = new List<MetricValue>();
    }

}