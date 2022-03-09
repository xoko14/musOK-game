using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteColorTrigger : MonoBehaviour
{
    public string color = "Unasigned";
    public Material redNoteMaterial;
    public Material greenNoteMaterial;
    public Material blueNoteMaterial;
    public Material wallNoteMaterial;

    public GameObject noteStart;
    public GameObject noteEnd;

    public Line line;

    private Renderer thisRenderer;
    void Start()
    {
        thisRenderer = GetComponent<Renderer>();

        Debug.DrawRay(transform.position + new Vector3(0, 0.3f, 0), Vector3.down * 0.3f, Color.green, 100f);

        RaycastHit[] hits = Physics.RaycastAll(transform.position + new Vector3(0, 0.3f, 0), Vector3.down, 1f);
        RaycastHit hit = new RaycastHit();
        float order = -100;
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.tag == "redline" || hits[i].collider.tag == "greenline" || hits[i].collider.tag == "blueline" || hits[i].collider.tag == "leftwallline" || hits[i].collider.tag == "rightwallline")
            {
                if (hits[i].collider.gameObject.GetComponent<LineInfo>().line.vectors[0].z > order)
                {
                    hit = hits[i];
                    order = hits[i].collider.gameObject.GetComponent<LineInfo>().line.vectors[0].z;
                }
            }
        }

        if (hit.collider.CompareTag("redline"))
        {
            color = "red";
            thisRenderer.material = redNoteMaterial;
            line = hit.collider.gameObject.GetComponent<LineInfo>().line;
            tag = "red_note";

        }
        else if (hit.collider.CompareTag("greenline"))
        {
            color = "green";
            thisRenderer.material = greenNoteMaterial;
            line = hit.collider.gameObject.GetComponent<LineInfo>().line;
            tag = "green_note";
        }
        else if (hit.collider.CompareTag("blueline"))
        {
            color = "blue";
            thisRenderer.material = blueNoteMaterial;
            line = hit.collider.gameObject.GetComponent<LineInfo>().line;
            tag = "blue_note";
        }
        else if (hit.collider.CompareTag("leftwallline"))
        {
            color = "leftwall";
            thisRenderer.material = wallNoteMaterial;
            line = hit.collider.gameObject.GetComponent<LineInfo>().line;
            tag = "left_note";
        }
        else if (hit.collider.CompareTag("rightwallline"))
        {
            color = "rightwall";
            thisRenderer.material = wallNoteMaterial;
            line = hit.collider.gameObject.GetComponent<LineInfo>().line;
            tag = "right_note";
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
