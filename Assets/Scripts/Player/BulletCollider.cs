using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BulletCollider : MonoBehaviour
{
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("bullet"))
        {
            PlayerSongStats.Instance.TakeDamage(10);
            Destroy(other.gameObject);
            audioSource.PlayOneShot(audioSource.clip);
            if(PlayerSongStats.Instance.life <= 0)
            {
                PlayerSongStats.Instance.reason = ExitReason.Failed;
                SceneManager.LoadScene(3);
            }
        }
    }
}
