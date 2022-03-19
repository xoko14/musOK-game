using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SongSelectionController : MonoBehaviour
{
    public GameObject songScrollContainer;
    public GameObject songCardPrefab;
    public GameObject songImage;
    public GameObject songName;
    public GameObject songArtist;
    public GameObject selector;

    private List<Chart> songs = new List<Chart>();
    private List<string> songNames;

    private Chart currentChart = null;
    private ChartLoader loader = new ChartLoader(Path.Combine(Directory.GetCurrentDirectory(), "charts"));

    private void Awake()
    {
        songNames = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "charts"), "*.xml", SearchOption.AllDirectories).ToList();
        foreach (string s in songNames)
        {
            songs.Add(ChartLoader.LoadInfo(s));
        }
    }

    void Start()
    {
        foreach (var chart in songs)
        {
            var item = Instantiate(songCardPrefab);
            item.transform.SetParent(songScrollContainer.transform, false);
            item.transform.GetChild(0).GetComponent<Text>().text = chart.SongInfo.title;
            item.transform.GetChild(1).GetComponent<Text>().text = chart.SongInfo.artist;
            item.GetComponent<Button>().onClick.AddListener(() => SetCurrentSong(chart));
        }
        SetCurrentSong(songs[0]);
    }

    public void SetCurrentSong(Chart chart)
    {
        if (chart.SongInfo.jacket != null)
        {
            chart.Jacket = File.ReadAllBytes(Path.Combine(Path.GetDirectoryName(chart.FolderPath), chart.SongInfo.jacket.file));
        }

        Texture2D sampleTexture = new Texture2D(2, 2);
        bool isLoaded = sampleTexture.LoadImage(chart.Jacket);
        // apply this texure as per requirement on image or material
        if (isLoaded)
        {
            songImage.GetComponent<RawImage>().texture = sampleTexture;
        }
        songName.GetComponent<Text>().text = chart.SongInfo.title;
        songArtist.GetComponent<Text>().text = chart.SongInfo.artist;
        currentChart = chart;
    }

    public void PlayChart()
    {
        if (currentChart != null)
        {
            SongSaver.SongID = Path.GetFileName(Path.GetDirectoryName(currentChart.FolderPath));
            SongSaver.Delay = PlayerPrefs.GetFloat("audioDelay", 0);
            SongSaver.SpeedMulti = PlayerPrefs.GetFloat("chartSpeed", 1);
            SongSaver.Difficulty = selector.GetComponent<TMPro.TMP_Dropdown>().value switch
            {
                0 => Difficulty.Easy,
                1 => Difficulty.Normal,
                2 => Difficulty.Hard,
                _ => Difficulty.Easy,
            };
            SceneManager.LoadScene(2);
        }
    }

    public void ReturnMenu(){
        SceneManager.LoadScene(0);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
