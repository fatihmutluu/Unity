using System;
using System.Collections;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class script : MonoBehaviour
{
    public Sprite defaultSprite;
    bool isFirstClicked;
    GameObject firstButton;
    GameObject secondButton;

    string firstButtonName;
    string secondButtonName;

    public AudioSource[] audioSources;
    public Image[] siblings;

    float startingTime = 120;
    public TextMeshProUGUI timeText;

    public void GetButtonObject(GameObject obj)
    {
        if (isFirstClicked)
        {
            secondButton = obj;
            secondButton.GetComponent<Image>().sprite = secondButton
                .GetComponentInChildren<SpriteRenderer>()
                .sprite;

            firstButtonName = firstButton.GetComponent<Image>().sprite.name;
            secondButtonName = secondButton.GetComponent<Image>().sprite.name;

            foreach (Image sibling in siblings)
            {
                if (sibling != null)
                    sibling.raycastTarget = false;
            }
            Debug.Log("ciktim");

            if (firstButtonName == secondButtonName)
                audioSources[1].Play();
            else
                audioSources[2].Play();
            SecondButtonClicked();
        }
        else
        {
            firstButton = obj;
            firstButton.GetComponent<Image>().sprite = firstButton
                .GetComponentInChildren<SpriteRenderer>()
                .sprite;
            firstButton.GetComponent<Image>().raycastTarget = false;
            isFirstClicked = true;
        }
    }

    public void SecondButtonClicked()
    {
        StartCoroutine(Wait(1));
    }

    IEnumerator Wait(int i)
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

        foreach (Image sibling in siblings)
        {
            if (sibling != null)
                sibling.raycastTarget = true;
        }

        isFirstClicked = false;
        firstButton = null;
        secondButton = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        isFirstClicked = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (startingTime > 0)
        {
            startingTime -= Time.deltaTime;
            timeText.text = startingTime.ToString("0");
        }
    }
}
