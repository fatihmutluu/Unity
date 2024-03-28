using System;
using System.Collections;
using System.Threading;
using ScreenScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class script : MonoBehaviour
{
    #region Fields
    GameObject firstButton;
    GameObject secondButton;
    public Sprite defaultSprite;
    public float startingTime = 10;

    bool isFirstClicked;
    string firstButtonName;
    string secondButtonName;

    bool win = false,
        lose = false;

    #region Public Fields
    public GameObject endGameScreens;
    public AudioSource[] audioSources;
    public Image[] siblings;
    public TextMeshProUGUI timeText;
    #endregion

    #endregion

    //! Fonksyionlar
    void SiblingClickability(bool isClickable)
    {
        foreach (Image sibling in siblings)
        {
            if (sibling != null)
                sibling.raycastTarget = isClickable;
        }
    }

    void SecondButtonClicked()
    {
        SiblingClickability(false);
        if (firstButtonName == secondButtonName)
            audioSources[1].Play();
        else
            audioSources[2].Play();
        StartCoroutine(WaitAndDo(1));
    }

    IEnumerator WaitAndDo(int i)
    {
        yield return new WaitForSeconds(i);

        if (firstButtonName == secondButtonName)
        {
            firstButton.GetComponent<Image>().enabled = false;
            secondButton.GetComponent<Image>().enabled = false;
        }
        else
        {
            firstButton.GetComponent<Image>().sprite = defaultSprite;
            secondButton.GetComponent<Image>().sprite = defaultSprite;
        }

        SiblingClickability(true);

        isFirstClicked = false;
        firstButton = null;
        secondButton = null;
    }

    void ButtonClicked(GameObject obj)
    {
        obj.GetComponent<Image>().sprite = obj.GetComponentInChildren<SpriteRenderer>().sprite;
    }

    public void GetButtonObject(GameObject obj)
    {
        if (isFirstClicked)
        {
            secondButton = obj;
            ButtonClicked(secondButton);

            firstButtonName = firstButton.GetComponent<Image>().sprite.name;
            secondButtonName = secondButton.GetComponent<Image>().sprite.name;

            SecondButtonClicked();
        }
        else
        {
            firstButton = obj;
            ButtonClicked(firstButton);
            firstButton.GetComponent<Image>().raycastTarget = false;
            isFirstClicked = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        isFirstClicked = false;
        SiblingClickability(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (startingTime <= 0)
        {
            lose = true;
        }

        win = true;
        foreach (Image sibling in siblings)
        {
            if (sibling.enabled == true)
            {
                win = false;
                break;
            }
        }

        if (!win && !lose)
        {
            startingTime -= Time.deltaTime;
            timeText.text = startingTime.ToString("0");
        }

        if (win)
        {
            audioSources[0].Stop();
            endGameScreens.SetActive(true);
            endGameScreens.transform.GetChild(0).gameObject.SetActive(true);
        }
        if (lose)
        {
            audioSources[0].Stop();
            endGameScreens.SetActive(true);
            endGameScreens.transform.GetChild(1).gameObject.SetActive(true);
        }
    }
}
