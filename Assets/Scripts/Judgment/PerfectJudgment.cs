using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PerfectJudgment : MonoBehaviour
{
    Controls controls;
    private GameObject prefab;
    public GameObject perfect;
    public GameObject almost;

    private Conductor c;
    private LaneTest lt;
    private float judgmentWindow;
    private float judgmentWindowPerfect;
    private string[] _noteTags = { "red_note", "green_note", "blue_note", "left_note", "right_note" };

    void Start()
    {
        GameObject lane = GameObject.Find("Lane");
        c = lane.GetComponent<Conductor>();
        lt = lane.GetComponent<LaneTest>();

        controls = GameObject.Find("Player").GetComponent<PlayerController>().controls;
        controls.Gameplay.BTA.performed += ctx => DetectNote("red_note");
        controls.Gameplay.BTB.performed += ctx => DetectNote("green_note");
        controls.Gameplay.BTC.performed += ctx => DetectNote("blue_note");
        controls.Gameplay.BTL.performed += ctx => DetectNote("left_note");
        controls.Gameplay.BTR.performed += ctx => DetectNote("right_note");
    }

    private void DetectNote(string note)
    {
        judgmentWindow = lt.barSize / c.secPerBar / c.beatsInBar * 0.5f;
        Debug.Log(judgmentWindow);
        judgmentWindowPerfect = judgmentWindow * 0.3f;
        Debug.Log(judgmentWindowPerfect);

        float currentZ = float.MaxValue;
        List<Collider>currentNotes = new List<Collider>();
        Collider[] hitColliders = Physics.OverlapBox(transform.position, new Vector3(5f/2, 2, judgmentWindow), Quaternion.identity);
        foreach (Collider col in hitColliders)
        {
            if (col.tag == note && currentZ > col.gameObject.transform.position.z)
            {
                currentZ = col.gameObject.transform.position.z;
                currentNotes.Clear();
                currentNotes.Add(col);
            }
            else if(col.tag == note && currentZ == col.gameObject.transform.position.z)
            {
                currentNotes.Add(col);
            }
        }

        foreach (Collider c in currentNotes)
        {

            evaluateNote(c);
        }
    }

    private void Update()
    {
        Vector3 centerBox = new Vector3(transform.position.x, transform.position.y, transform.position.z - judgmentWindow - 1);
        Vector3 growBox = new Vector3(30, 2, judgmentWindow);
        Collider[] failedNotes = Physics.OverlapBox(centerBox, growBox, Quaternion.identity);
        foreach (Collider c in failedNotes)
        {
            if(_noteTags.Contains(c.tag))
            {
                Debug.Log($"Deleted {c.tag}");
                //Destroy(c.gameObject);
            }
        }
    }


    private void evaluateNote(Collider c)
    {
        if (Math.Abs(c.gameObject.transform.position.z - transform.position.z) <= judgmentWindowPerfect)
        {
            prefab = perfect;
            PlayerSongStats.Instance.ScorePerfect();
        }
        else
        {
            prefab = almost;
            PlayerSongStats.Instance.ScoreAlmost();
        }

        GameObject score = Instantiate(prefab);
        score.transform.position = new Vector3(c.gameObject.transform.position.x, 0, /*c.gameObject.transform.position.z*/transform.position.z);
        score.transform.parent = gameObject.transform;
        Destroy(c.gameObject);
    }

    private void OnTriggerExit(Collider c)
    {
        //Destroy(c.gameObject);
    }
}
