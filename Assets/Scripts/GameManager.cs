using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Player player;
    [SerializeField] private Text scoreText;
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject getReady;
    
    public Text result;
    private int score;
    
    const string FILE_DIR = "/DATA/";
    const string DATA_FILE = "highScores.txt";
    string FILE_FULL_PATH;
    
    public int Score
    {
        get
        {
            return score;
        }

        set { score = value; }

    }
    string highScoresString = "";
    
    List<int> highScores;

    public List<int> HighScores
    {
        get
        {
            if (highScores == null && File.Exists(FILE_FULL_PATH))
            {
                
                highScores = new List<int>();

                highScoresString = File.ReadAllText(FILE_FULL_PATH);

                highScoresString = highScoresString.Trim();
                
                string[] highScoreArray = highScoresString.Split("\n");

                for (int i = 0; i < highScoreArray.Length; i++)
                {
                    int currentScore = Int32.Parse(highScoreArray[i]);
                    highScores.Add(currentScore);
                }
            }
            else if(highScores == null)
            {
                highScores = new List<int>();
                highScores.Add(3);
                highScores.Add(2);
                highScores.Add(1);
                highScores.Add(0);
            }
            return highScores;
        }
    }
            
    // Start is called before the first frame update
    void Start()
    {
        FILE_FULL_PATH = Application.dataPath + FILE_DIR + DATA_FILE;
    }       
            
    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Pause();
        }
        gameOver.SetActive(false);
    }

    public void Play()
    {
        score = 0;
        scoreText.text = score.ToString();

        //hide ui
        playButton.SetActive(false);
        gameOver.SetActive(false);
        getReady.SetActive(false);
        
        Time.timeScale = 1f;
        player.enabled = true;
        
        Pipe[] pipe = FindObjectsOfType<Pipe>();

        for (int i = 0; i < pipe.Length; i++) {
            Destroy(pipe[i].gameObject);
        }
    }

    public void GameOver()
    {
        //show ui
        playButton.SetActive(true);
        gameOver.SetActive(true);

        Pause();
        SetHighScore();
        result.text = "\nHigh Scores:\n" + highScoresString;
                      
        
    }

    public void Pause()
    {
        //disable player
        Time.timeScale = 0f;
        player.enabled = false;
    }

    public void IncreaseScore()
    {
        //count the score
        score++;
        scoreText.text = score.ToString();
    }
    bool IsHighScore(int score)
    {

        for (int i = 0; i < HighScores.Count; i++)
        {
            if (highScores[i] < score)
            {
                return true;
            }
        }

        return false;
    }

    void SetHighScore()
    {
        if (IsHighScore(score))
        {
            int highScoreSlot = -1;

            for (int i = 0; i < HighScores.Count; i++)
            {
                if (score > highScores[i])
                {
                    highScoreSlot = i;
                    break;
                }
            }
                
            highScores.Insert(highScoreSlot, score);

            highScores = highScores.GetRange(0, 5);

            string scoreBoardText = "";

            foreach (var highScore in highScores)
            {
                scoreBoardText += highScore + "\n";
            }

            highScoresString = scoreBoardText;
                
            File.WriteAllText(FILE_FULL_PATH, highScoresString);
        }
    }
}


