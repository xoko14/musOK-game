using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicBulletController : MonoBehaviour
{
    public bool isVisible = false, jobDone = false;
    public BulletPattern pattern;

    private float journeyLength, startTime;
    private Vector3 targetPos, initPos, direction;
    private float speed = 1f;

    private Conductor c;
    private LaneTest lt;

    private GameObject capsule;

    void Start()
    {
        GameObject lane = GameObject.Find("Lane");
        c = lane.GetComponent<Conductor>();
        lt = lane.GetComponent<LaneTest>();
        capsule = GameObject.Find("Capsule");
        //Debug.Log(pattern.Velocity);
        targetPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        initPos = new Vector3(pattern.PosX, transform.position.y, transform.position.z);
        direction = new Vector3(transform.position.x-pattern.PosX, transform.position.y, transform.position.z);
        speed = pattern.Velocity;
    }

    // Update is called once per frame
    void Update()
    {

        if (!isVisible)
        {
            // check if in frustum
            Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
            isVisible = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

            // set initial position
            transform.position = initPos;
        }
        
        if (isVisible && !jobDone)
        {
            jobDone = true;
            StartCoroutine(GoTo());
        }
    }

    private IEnumerator GoTo()
    {
        float elapsedTime = 0;
        float timeToMove = (transform.position.z-capsule.transform.position.z)/(lt.barSize/c.secPerBar);
        initPos = new Vector3(pattern.PosX, transform.position.y, ((lt.barSize/c.secPerBar*speed)-(lt.barSize/c.secPerBar))*timeToMove+targetPos.z);

        while (elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(initPos, targetPos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
