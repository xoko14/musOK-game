using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    private Conductor c;
    private LaneTest lt;
    public float delay;
    void Start()
    {
        delay = SongSaver.Delay;
        GameObject lane = GameObject.Find("Lane");
        c = lane.GetComponent<Conductor>();
        lt = lane.GetComponent<LaneTest>();
        transform.Translate(0f,0f,(lt.barSize/c.secPerBar)*delay);
    }

    // Update is called once per frame
    void Update()
    {
        if(c.audioSource != null && c.audioSource.isPlaying && c.isPlaying)
            transform.Translate(Vector3.forward*Time.deltaTime*(lt.barSize/c.secPerBar));
    }
}
