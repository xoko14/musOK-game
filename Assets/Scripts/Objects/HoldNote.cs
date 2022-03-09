using UnityEngine;

public class HoldNote{
    public Vector3 StartPosition {get; set;}
    public Vector3 EndPosition {get; set;}

    public HoldNote(Vector3 s, Vector3 e){
        StartPosition = s;
        EndPosition = e;
    }
}