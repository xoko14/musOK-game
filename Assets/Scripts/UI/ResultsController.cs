using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultsController : MonoBehaviour
{
    private Text songName, songArtist;
    private RawImage songImage;
    private Text bellsValue;
    private Text bellsTotal;
    private Text flicksValue;
    private Text flicksTotal;
    private Text perfectValue;
    private Text almostValue;
    private Text failValue;
    private Text scoreValue;
    private Text reason;
    private RectTransform lifeBar;

    void Start()
    {
        songName = GameObject.Find("SongName").GetComponent<Text>();
        songArtist = GameObject.Find("SongArtist").GetComponent<Text>();
        songImage = GameObject.Find("Image").GetComponent<RawImage>();
        bellsTotal = GameObject.Find("BellsTotal").GetComponent<Text>();
        bellsValue = GameObject.Find("BellsValue").GetComponent<Text>();
        flicksTotal = GameObject.Find("FlicksTotal").GetComponent<Text>();
        flicksValue = GameObject.Find("FlicksValue").GetComponent<Text>();
        perfectValue = GameObject.Find("PerfectValue").GetComponent<Text>();
        almostValue = GameObject.Find("AlmostValue").GetComponent<Text>();
        scoreValue = GameObject.Find("Score").GetComponent<Text>();
        lifeBar = GameObject.Find("Bar").GetComponent<RectTransform>();
        reason = GameObject.Find("Reason").GetComponent<Text>();

        songName.text = PlayerSongStats.Instance.chart.SongInfo.title;
        songArtist.text = PlayerSongStats.Instance.chart.SongInfo.artist;

        if (PlayerSongStats.Instance.chart.SongInfo.jacket != null)
        {
            PlayerSongStats.Instance.chart.Jacket = File.ReadAllBytes(Path.Combine(PlayerSongStats.Instance.chart.FolderPath, PlayerSongStats.Instance.chart.SongInfo.jacket.file));
        }

        Texture2D sampleTexture = new Texture2D(2, 2);
        bool isLoaded = sampleTexture.LoadImage(PlayerSongStats.Instance.chart.Jacket);
        if (isLoaded)
        {
            songImage.GetComponent<RawImage>().texture = sampleTexture;
        }

        bellsTotal.text = PlayerSongStats.Instance.totalBells.ToString();
        bellsValue.text = PlayerSongStats.Instance.bellsCatched.ToString();
        flicksTotal.text = PlayerSongStats.Instance.totalFlicks.ToString();
        flicksValue.text = PlayerSongStats.Instance.flicksPerformed.ToString();
        perfectValue.text = PlayerSongStats.Instance.perfects.ToString();
        almostValue.text = PlayerSongStats.Instance.almosts.ToString();
        scoreValue.text = PlayerSongStats.Instance.score.ToString();
        lifeBar.sizeDelta = new Vector2(PlayerSongStats.Instance.life/100f * lifeBar.sizeDelta.x, lifeBar.sizeDelta.y);

        reason.text = PlayerSongStats.Instance.reason switch {
            ExitReason.Completed => "Song complete!",
            ExitReason.Failed => "Song failed!",
            ExitReason.Quitted => "Song quitted",
            _ => "Something wrong happened"
        };
    }

    public void Return(){
        SceneManager.LoadScene(1);
    }
}
