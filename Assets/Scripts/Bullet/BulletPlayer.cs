using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    private Conductor c;
    private LaneTest lt;

    void Start()
    {
        GameObject lane = GameObject.Find("Lane");
        c = lane.GetComponent<Conductor>();
        lt = lane.GetComponent<LaneTest>();
        audioSource = GetComponent<AudioSource>();
        BoxCollider bc = GetComponent<BoxCollider>();
        Vector3 wtr = transform.TransformPoint(bc.center);
        bc.center = transform.InverseTransformPoint(new Vector3(
            wtr.x, 
            wtr.y, 
            wtr.z - ((lt.barSize / c.secPerBar) * SongSaver.Delay)
        ));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "bullet")
        {
            audioSource.PlayOneShot(audioSource.clip);
            //Destroy(other.gameObject);
        }
    }
}
