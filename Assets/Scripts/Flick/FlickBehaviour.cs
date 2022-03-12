using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    public FlickDirection flickDirection;
    private float enterX = 0;
    public GameObject particlePrefab;
    private GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "player_chara")
        {
            //Debug.Log("Entered collision with player");
            enterX = col.gameObject.transform.position.x;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if(col.gameObject.tag == "player_chara")
        {
            switch(flickDirection){
                case FlickDirection.Left:
                    if(enterX>col.gameObject.transform.position.x)
                    {
                        Debug.Log("Left flick performed");
                        PlayerSongStats.Instance.PerformFlick();
                    }
                    break;

                case FlickDirection.Right:
                    if(enterX<col.gameObject.transform.position.x)
                    {
                        Debug.Log("Right flick performed");
                        PlayerSongStats.Instance.PerformFlick();
                    }
                    break;
            }
            Instantiate(particlePrefab, player.transform);
        }
    }

    public enum FlickDirection{
        Left, Right
    }
}
