using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MemoryGameScript : MonoBehaviour
{
    #region Fields
    // Game state fields
    private GameObject firstButton;
    private GameObject secondButton;
    private string firstButtonName;
    private string secondButtonName;
    private bool isFirstClicked;
    private bool win = false;
    private bool lose = false;
    private int level = 2;
    public float startingTime = 98f;

    // UI elements
    private List<GameObject> siblings = new List<GameObject>();
    public GameObject grid;
    public GameObject exampleButton;
    public TextMeshProUGUI timeText;
    public GameObject endGameScreens;
    public GameObject point;
    private int pointValue = 0;

    // Audio elements
    public AudioSource[] audioSources;

    // Constants and configuration values
    public float waitTime = 1f;
    public float cellSizeStartingPoint = 1600f;

    #endregion

    #region MonoBehaviour Methods

    private void Awake()
    {
        InitializeGrid();
    }

    private void Start()
    {
        ConfigureGrid();
        ResetGame();
    }

    private void Update()
    {
        CheckGameTime();

        if (IsWinConditionMet())
        {
            HandleWinState();
        }
        else if (IsLoseConditionMet())
        {
            HandleLoseState();
        }
    }

    #endregion

    #region Game State Management

    private void InitializeGrid()
    {
        grid.SetActive(true);

        for (int i = 0; i < level; i++)
        {
            GameObject button = Instantiate(exampleButton, grid.transform);
            ConfigureButton(button, i);
        }
    }

    private void ConfigureGrid()
    {
        float cellSize = cellSizeStartingPoint / Mathf.CeilToInt(MathF.Sqrt(level));
        grid.GetComponent<GridLayoutGroup>().cellSize = new Vector2(cellSize, cellSize);
        SetSiblingClickability(true);
    }

    private void ResetGame()
    {
        isFirstClicked = false;
        win = false;
        lose = false;
    }

    #endregion

    #region Game Logic

    private void CheckGameTime()
    {
        if (startingTime <= 0)
        {
            lose = true;
        }

        if (!win && !lose)
        {
            startingTime -= Time.deltaTime;
            timeText.text = startingTime.ToString("0");
        }
    }

    private bool IsWinConditionMet()
    {
        win = true;
        foreach (GameObject sibling in siblings)
        {
            if (sibling.GetComponent<Image>().enabled)
            {
                win = false;
                break;
            }
        }

        return win;
    }

    private bool IsLoseConditionMet()
    {
        return startingTime <= 0;
    }

    private void HandleWinState()
    {
        ProceedToNextLevel();
        ShuffleAndAssignPositions();
        ResetGame();
    }

    private void HandleLoseState()
    {
        audioSources[0].Stop();
        endGameScreens.SetActive(true);
        endGameScreens.transform.GetChild(0).gameObject.SetActive(true);
    }

    #endregion

    #region Button Interaction

    private void ConfigureButton(GameObject button, int index)
    {
        button
            .transform.GetChild(1)
            .GetComponent<TextMeshProUGUI>()
            .SetText(((int)(index / 2)).ToString());
        button.name = ((int)(index / 2)).ToString();
        siblings.Add(button);
    }

    public void OnButtonClicked(GameObject obj)
    {
        if (!isFirstClicked)
        {
            firstButton = obj;
            OnFirstButtonClicked(firstButton);
        }
        else
        {
            secondButton = obj;
            OnSecondButtonClicked(secondButton);
            ProcessButtonClick();
        }
    }

    private void OnFirstButtonClicked(GameObject button)
    {
        ToggleButton(button);
        button.GetComponent<Button>().interactable = false;
        isFirstClicked = true;
    }

    private void OnSecondButtonClicked(GameObject button)
    {
        ToggleButton(button);
        firstButtonName = firstButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text;
        secondButtonName = secondButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text;
    }

    private void ProcessButtonClick()
    {
        SetSiblingClickability(false);
        if (firstButtonName == secondButtonName)
        {
            audioSources[1].Play();
        }
        else
        {
            audioSources[2].Play();
        }
        StartCoroutine(HandleButtonMatchResult(waitTime));
    }

    private IEnumerator HandleButtonMatchResult(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (firstButtonName == secondButtonName)
        {
            DisableButton(firstButton);
            DisableButton(secondButton);

            siblings.Remove(firstButton);
            siblings.Remove(secondButton);
            pointValue += 2;
        }
        else
        {
            ResetButton(firstButton);
            ResetButton(secondButton);
            pointValue -= 1;
        }

        ResetButtonState();
    }

    #endregion

    #region Helper Methods

    private void ShuffleAndAssignPositions()
    {
        foreach (GameObject sibling in siblings)
        {
            sibling.transform.SetSiblingIndex(UnityEngine.Random.Range(0, siblings.Count));
        }
    }

    private void SetSiblingClickability(bool isClickable)
    {
        foreach (GameObject sibling in siblings)
        {
            sibling.GetComponent<Button>().interactable = isClickable;
        }
    }

    private void ToggleButton(GameObject obj)
    {
        obj.transform.GetChild(0).gameObject.SetActive(false);
        obj.transform.GetChild(1).gameObject.SetActive(true);
    }

    private void DisableButton(GameObject button)
    {
        button.GetComponent<Image>().enabled = false;
        button.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = false;
        button.transform.GetChild(1).gameObject.SetActive(false);
    }

    private void ResetButton(GameObject button)
    {
        button.transform.GetChild(0).gameObject.SetActive(true);
        button.transform.GetChild(1).gameObject.SetActive(false);
    }

    private void ResetButtonState()
    {
        point.GetComponent<TextMeshProUGUI>().SetText(pointValue.ToString());
        SetSiblingClickability(true);
        isFirstClicked = false;
        firstButton = null;
        secondButton = null;
    }

    private void ProceedToNextLevel()
    {
        startingTime += level;
        pointValue += level;
        point.GetComponent<TextMeshProUGUI>().SetText(pointValue.ToString());

        level += 2;
        float cellSize = cellSizeStartingPoint / Mathf.CeilToInt(MathF.Sqrt(level));
        grid.GetComponent<GridLayoutGroup>().cellSize = new Vector2(cellSize, cellSize);

        siblings = new List<GameObject>();

        for (int i = 0; i < grid.transform.childCount; i++)
        {
            Destroy(grid.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < level; i++)
        {
            GameObject button = Instantiate(exampleButton, grid.transform);
            ConfigureButton(button, i);
        }
    }

    #endregion

    #region Screen Navigation

    public void OnMainScreenClicked()
    {
        SceneManager.LoadScene("MainScreen");
    }

    public void OnRestartClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    #endregion
}
