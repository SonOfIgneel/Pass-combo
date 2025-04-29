using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum DifficultyLevel
{
    Easy,
    Medium,
    Hard
}

public class GameManager : MonoBehaviour
{
    public List<GameObject> teammates;
    public List<GameObject> teammatesToUse;
    public float flashDuration = 2f;
    public int score = 0;
    public int comboCount = 0;
    public GameObject currentTarget;
    public float gameTime = 30f;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    public GameObject gameOverPanel;
    public GameObject gameStartPanel;
    public TextMeshProUGUI finalScoreText;
    private float remainingTime;
    private bool isGameStarted = false;
    public DifficultyLevel currentDifficulty = DifficultyLevel.Easy;
    public TMP_Dropdown difficulty_DropDown;
    public static GameManager Instance;

    void Start()
    {
        currentDifficulty = DifficultyLevel.Easy;
        SetDifficultySettings();
        Instance = this;
        difficulty_DropDown.onValueChanged.AddListener(OnDifficultyChanged);
    }

    public void StartGame()
    {
        remainingTime = gameTime;
        gameOverPanel.SetActive(false);
        gameStartPanel.SetActive(false);
        isGameStarted = true;
        score = 0;
        StartCoroutine(FlashTeammatesRoutine());
    }

    public void Reset()
    {
        SceneManager.LoadScene(0);
    }

    void Update()
    {
        if (!isGameStarted) return;

        remainingTime -= Time.deltaTime;
        timerText.text = "" + Mathf.CeilToInt(remainingTime);

        if (remainingTime <= 0)
        {
            EndGame();
        }
    }

    public void OnDifficultyChanged(int index)
    {
        Debug.Log("Dropdown index: " + index);
        switch (index)
        {
            case 0:
                currentDifficulty = DifficultyLevel.Easy;
                SetDifficultySettings();
                break;
            case 1:
                currentDifficulty = DifficultyLevel.Medium;
                SetDifficultySettings();
                break;
            case 2:
                currentDifficulty = DifficultyLevel.Hard;
                SetDifficultySettings();
                break;
        }

        Debug.Log("Selected difficulty: " + currentDifficulty);
    }

    void SetDifficultySettings()
    {
        switch (currentDifficulty)
        {
            case DifficultyLevel.Easy:
                flashDuration = 1.5f;
                teammatesToUse = teammates.GetRange(0, 4);
                for (int i = 0; i < teammates.Count; i++)
                {
                    if (i > 3)
                    {
                        teammates[i].SetActive(false);
                    }
                }
                break;
            case DifficultyLevel.Medium:
                flashDuration = 1f;
                teammatesToUse = teammates.GetRange(0, 6);
                for (int i = 0; i < teammates.Count; i++)
                {
                    if (i > 5)
                    {
                        teammates[i].SetActive(false);
                    }
                    else
                    {
                        teammates[i].SetActive(true);
                    }
                }
                break;
            case DifficultyLevel.Hard:
                flashDuration = 0.6f;
                teammatesToUse = teammates;
                for (int i = 0; i < teammates.Count; i++)
                {
                    teammates[i].SetActive(true);
                }
                break;
        }
    }

    public void AddScore(int value)
    {
        if(currentDifficulty == DifficultyLevel.Medium && comboCount == 3)
        {
            score += 2;
            comboCount = 0;
        }
        else if(currentDifficulty == DifficultyLevel.Hard && comboCount == 5)
        {
            score += 3;
            comboCount = 0;
        }
        score += value;
        scoreText.text = "" + score;
    }

    void EndGame()
    {
        isGameStarted = false;
        gameOverPanel.SetActive(true);
        finalScoreText.text = score.ToString();
    }

    IEnumerator FlashTeammatesRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.5f);

            currentTarget = teammatesToUse[Random.Range(0, teammatesToUse.Count)];

            SetHighlight(currentTarget, true);

            yield return new WaitForSeconds(flashDuration);

            if (currentTarget != null)
                SetHighlight(currentTarget, false);

            currentTarget = null;
        }
    }

    void SetHighlight(GameObject player, bool isOn)
    {
        SpriteRenderer sr = player.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = isOn ? Color.yellow : Color.white;
        }
    }

    public void RegisterCorrectPass()
    {
        AddScore(1);
        Debug.Log("Correct Pass");
        comboCount++;
    }

    public void RegisterWrongPass()
    {
        Debug.Log("Wrong Pass");
        comboCount = 0;
        if (currentDifficulty == DifficultyLevel.Hard)
        {
            score--;
            scoreText.text = "" + score;
        }
    }
}

