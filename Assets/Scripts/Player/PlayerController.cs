using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Controls controls;

    public float sensibility = 0;

    private Conductor c;
    private LaneTest lt;


    void Awake(){
        controls = new Controls();
        //controls.Gameplay.Click.performed += _ => Debug.Log(controls.Gameplay.MoveHorizontal.ReadValue<float>());
        controls.Gameplay.Esc.performed += _ => SceneManager.LoadScene(3);
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        GameObject lane = GameObject.Find("Lane");
        c = lane.GetComponent<Conductor>();
        lt = lane.GetComponent<LaneTest>();
        float barZ = GameObject.Find("Capsule").transform.position.z;
        float z1 = transform.position.z;
        float z2 = barZ-((lt.barSize / c.secPerBar) * SongSaver.Delay);
        float sizeZ = z2-z1;

        BoxCollider bc = GetComponent<BoxCollider>();
        Vector3 wtr = transform.TransformPoint(bc.center);
        bc.size = new Vector3(bc.size.x, bc.size.y, sizeZ*2);
        bc.center = transform.InverseTransformPoint(new Vector3(
            wtr.x,
            wtr.y,
            transform.position.z+(sizeZ/2)
        ));
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalMovement = controls.Gameplay.MoveHorizontal.ReadValue<float>() / 100;
        if(transform.position.x + horizontalMovement>= -2.4f && transform.position.x + horizontalMovement<= 2.4f){
            transform.Translate(horizontalMovement*sensibility, 0, 0);
        }
    }

    private void OnEnable(){
        controls.Gameplay.Enable();
    }

    private void OnDisable(){
        controls.Gameplay.Disable();
    }

    private void OnDestroy()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}
