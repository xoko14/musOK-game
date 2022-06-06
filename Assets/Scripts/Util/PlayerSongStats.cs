using UnityEngine;
using UnityEngine.UI;

public sealed class PlayerSongStats
{
    private static PlayerSongStats _instance = new PlayerSongStats();

    public int totalBells {get; private set;}
    public int totalFlicks {get; private set;}
    public int bellsCatched {get; private set;} = 0;
    public int flicksPerformed {get; private set;} =0;
    public int perfects {get; private set;}  = 0;
    public int almosts {get; private set;}  = 0;
    public int fails {get; private set;} = 0;
    public int life { get; private set;} = 100;

    private int noteScoreValue;
    private int scoreMax = 1000000;
    private int totalNotes;
    private float lifeBarMaxSize;
    public int score {get; private set;} = 0;
    public Chart chart;

    public ExitReason reason {get; set;} = ExitReason.Quitted;

    private Text bellsValue;
    private Text bellsTotal;
    private Text flicksValue;
    private Text flicksTotal;
    private Text perfectValue;
    private Text almostValue;
    private Text failValue;
    private Text scoreValue;
    private RectTransform lifeBar;

    private PlayerSongStats()
    {
        bellsTotal = GameObject.Find("BellsTotal").GetComponent<Text>();
        bellsValue = GameObject.Find("BellsValue").GetComponent<Text>();
        flicksTotal = GameObject.Find("FlicksTotal").GetComponent<Text>();
        flicksValue = GameObject.Find("FlicksValue").GetComponent<Text>();
        perfectValue = GameObject.Find("PerfectValue").GetComponent<Text>();
        almostValue = GameObject.Find("AlmostValue").GetComponent<Text>();
        scoreValue = GameObject.Find("Score").GetComponent<Text>();
        lifeBar = GameObject.Find("Bar").GetComponent<RectTransform>();
        lifeBarMaxSize = lifeBar.sizeDelta.x;

    }

    public static void Init()
    {
        _instance = new PlayerSongStats();
    }
 
    public static PlayerSongStats Instance
    {
        get
        {
            return _instance;
        }
    }

    public void SetTotalBells(int bells)
    {
        totalBells = bells;
        bellsTotal.text = $"{totalBells}";
    }

    public void SetTotalFlicks(int flicks)
    {
        totalFlicks = flicks;
        flicksTotal.text = $"{totalFlicks}";
    }

    public void SetTotalNotes(int notes)
    {
        totalNotes = notes;
    }

    public void PerformFlick()
    {
        flicksPerformed++;
        flicksValue.text = $"{flicksPerformed}";
        UpdateScore(noteScoreValue);
    }

    public void CatchBell(){
        bellsCatched++;
        bellsValue.text = $"{bellsCatched}";
        UpdateScore(noteScoreValue);
    }

    public void ScorePerfect()
    {
        perfects++;
        perfectValue.text = $"{perfects}";
        UpdateScore(noteScoreValue);
    }

    public void ScoreAlmost()
    {
        almosts++;
        almostValue.text = $"{almosts}";
        UpdateScore(noteScoreValue/2);
    }

    private void UpdateScore(int value){
        score+=value;
        scoreValue.text = $"{score}";
    }

    public void CalculateScoreValues(){
        var totalForScore = totalBells + totalFlicks + totalNotes;
        noteScoreValue = scoreMax / (totalForScore == 0? scoreMax: totalForScore);
    }

    public void TakeDamage(int damage)
    {
        life -= damage;
        lifeBar.sizeDelta = new Vector2(life/100f*lifeBarMaxSize, lifeBar.sizeDelta.y);
    }
}