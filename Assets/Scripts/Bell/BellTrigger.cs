using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellTrigger : MonoBehaviour
{
    private GameObject sfxPlayer;
    private AudioSource sfxSource;
    void Start()
    {
        sfxPlayer = GameObject.Find("SFXPlayer");
        sfxSource = sfxPlayer.GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter(Collider c){
        if(c.gameObject.tag == "player_chara")
        {
            PlayerSongStats.Instance.CatchBell();
            sfxSource.PlayOneShot(sfxSource.clip);
            Destroy(this.gameObject);
        }
    }

    private IEnumerator GoToPlayer()
    {

        yield return null;
    }
}
