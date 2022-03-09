using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlmostJudgment : MonoBehaviour
{
    Controls controls;
    public GameObject prefab;
    void Start()
    {
        controls = GameObject.Find("Player").GetComponent<PlayerController>().controls;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay(Collider c)
    {
        if (controls.Gameplay.BTA.WasPressedThisFrame() && c.tag == "red_note")
        {
            //Debug.Log("perfect red note performed");
            Instantiate(prefab).transform.position = new Vector3(c.transform.position.x, 0, transform.position.z);
            Destroy(c.gameObject);
        }
        else if (controls.Gameplay.BTB.WasPressedThisFrame() && c.tag == "green_note")
        {
            //Debug.Log("perfect green note performed");
            Instantiate(prefab).transform.position = new Vector3(c.transform.position.x, 0, transform.position.z);
            Destroy(c.gameObject);
        }
        else if (controls.Gameplay.BTC.WasPressedThisFrame() && c.tag == "blue_note")
        {
            //Debug.Log("perfect blue note performed");
            Instantiate(prefab).transform.position = new Vector3(c.transform.position.x, 0, transform.position.z);
            Destroy(c.gameObject);
        }
        else if (controls.Gameplay.BTL.WasPressedThisFrame() && c.tag == "left_note")
        {
            //Debug.Log("perfect left note performed");
            Instantiate(prefab).transform.position = new Vector3(c.transform.position.x, 0, transform.position.z);
            Destroy(c.gameObject);
        }
        else if (controls.Gameplay.BTR.WasPressedThisFrame() && c.tag == "right_note")
        {
            //Debug.Log("perfect right note performed");
            Instantiate(prefab).transform.position = new Vector3(c.transform.position.x, 0, transform.position.z);
            Destroy(c.gameObject);
        }
    }
}
