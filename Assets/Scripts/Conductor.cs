using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Conductor : MonoBehaviour
{
    //Song beats per minute
    //This is determined by the song you're trying to sync up to
    public float songBpm;

    public float songPositionInBars;

    public float secPerBar;

    //an AudioSource attached to this GameObject that will play the music.
    public string musicSource;

    public AudioSource audioSource;

    public AudioClip bellSource, bulletSource;

    public int beatsInBar;

    public bool isPlaying = false;

    private Chart chart;

    private int metricIndex = 0;
    private int bpmIndex = 0;

    public event EventHandler BellAudioLoaded;

    protected virtual void OnBellAudioLoaded(EventArgs e)
    {
        EventHandler handler = BellAudioLoaded;
        handler?.Invoke(this, e);
    }

    void Start()
    {
        Debug.Log(Directory.GetCurrentDirectory());
        chart = GetComponent<LaneTest>().chart;
        secPerBar = 60f / songBpm * beatsInBar;

        Debug.Log("Reached here");

        StartCoroutine(MusicPlayer());
        songPositionInBars = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (audioSource!=null && audioSource.isPlaying && isPlaying)
        {
            if (chart.SongBPM.Count > bpmIndex + 1)
            {
                if (chart.SongBPM[bpmIndex + 1].Position <= songPositionInBars)
                {
                    songBpm = chart.SongBPM[bpmIndex + 1].Value;
                    secPerBar = 60f / songBpm * beatsInBar;
                    bpmIndex++;
                    Debug.Log("BPM changed");
                }
            }

            if (chart.SongMetrics.Count > metricIndex + 1)
            {
                //Debug.Log(chart.SongBPM[1].Position);
                if (chart.SongMetrics[metricIndex + 1].Position <= songPositionInBars)
                {
                    beatsInBar = chart.SongMetrics[metricIndex + 1].Numerator;
                    secPerBar = 60f / songBpm * beatsInBar;
                    metricIndex++;
                    Debug.Log("Metrics changed");
                }
            }
            songPositionInBars += (Time.deltaTime * songBpm / 60f / beatsInBar);
        }
    }

    IEnumerator ExitWhenMusicIsStopped(){
        yield return new WaitUntil(() => audioSource.isPlaying == false);
        yield return new WaitForSeconds(0.25f);
        PlayerSongStats.Instance.reason = ExitReason.Completed;
        SceneManager.LoadScene(3);
    }

    IEnumerator MusicPlayer()
    {
        //Ensure the AudioType matches your file extension type
        using (UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(GetFileLocation(musicSource), AudioType.WAV))
        {
            yield return uwr.SendWebRequest();

            if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                audioSource = gameObject.GetComponent<AudioSource>();
                if (audioSource == null)
                    audioSource = gameObject.AddComponent<AudioSource>();
                else if (audioSource.clip != null)
                {
                    //Unload the existing clip
                    audioSource.Stop();
                    AudioClip currentClip = audioSource.clip;
                    audioSource.clip = null;
                    currentClip.UnloadAudioData();
                    DestroyImmediate(currentClip, false);
                }

                audioSource.loop = false;
                audioSource.volume = .2f;
                audioSource.clip = DownloadHandlerAudioClip.GetContent(uwr);

                yield return new WaitForSeconds(2);
                audioSource.Play();
                isPlaying = true;
                StartCoroutine(ExitWhenMusicIsStopped());

            }
        }

    }

    public static string GetFileLocation(string relativePath)
    {
        return "file://" + relativePath;
    }
}
