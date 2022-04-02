using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour {
    [SerializeField] private float _widthScreen;
    [SerializeField] private float _hightScreen;
    public Transform Rocketka1;
    public Transform Rocketka2;
    public GameObject Ground;
    public GameObject LeftBoard;
    public GameObject RightBoard;

    public GameObject CurrentBall;
    public GameObject PrefabBall;
    public Transform PointStartBall;
    public GameObject PlayGround;

    [Header("Score")]
    public int Score;
    [SerializeField] private Text _textScore;
    public int TopScore;
    [SerializeField] private Text _textTopScore;
    public bool IsClearSettings;

    [Header("Menu")]
    [SerializeField] private Color _currentColor;
    public GameObject Menu;
    public GameObject MenuButton;
    public List<Color> ColorList = new List<Color>();
    public List<Image> ImageButtonsList = new List<Image>();

    void Start() {
        if (IsClearSettings) { // Можно сбросить настройки
            ClearAllSettings();
        }

        if (PlayerPrefs.HasKey("topScore")) {
            TopScore = PlayerPrefs.GetInt("topScore");
        } else {
            TopScore = 0;
        }

        int inxColor = 0;
        if (PlayerPrefs.HasKey("Color")) {
            Color tempCol = Color.green;
            inxColor = PlayerPrefs.GetInt("Color");
            Debug.Log(inxColor);
            //ColorUtility.ToHtmlStringRGBA(strColor, out tempCol);
            _currentColor = ColorList[inxColor];
        } else {
            _currentColor = ColorList[0];
        }

        for (int i = 0; i < ImageButtonsList.Count; i++) {
            Color col = ColorList[i];
            col.a = 1;
            ImageButtonsList[i].color = col;
            if (i == inxColor) {
                ImageButtonsList[i].transform.Find("Frame").gameObject.SetActive(true);
            } else {
                ImageButtonsList[i].transform.Find("Frame").gameObject.SetActive(false);
            }
        }

        CreateBall();
        RefreshScore();
    }

    private void FixedUpdate() {
        if (!CurrentBall) {
            CreateBall();
        }
    }


    public void CreateBall() {
        GameObject newBall = Instantiate(PrefabBall, PointStartBall.transform.position, Quaternion.identity, PlayGround.transform);
        CurrentBall = newBall;
        CurrentBall.GetComponent<Ball>().SetColor(_currentColor);
    }

    public void AddScore() {
        Score += 100;
        if (Score > TopScore) {
            TopScore = Score;
            PlayerPrefs.SetInt("topScore", TopScore);
        }
        RefreshScore();
    }

    public void MinusScore() {
        Score -= 75;
        RefreshScore();
    }

    public void RefreshScore() { // Вывод результатов на UI
        _textScore.text = Score.ToString();
        if (TopScore > 0) {
            _textTopScore.gameObject.SetActive(true);
            _textTopScore.text = TopScore.ToString("");
        } else {
            _textTopScore.gameObject.SetActive(false);
        }
    }

    public void ChangeColor(int colorInx) { // Меняется цвет шара
        PlayerPrefs.SetInt("Color", colorInx);
        _currentColor = ColorList[colorInx];
        CurrentBall.GetComponent<Ball>().SetColor(_currentColor);
        for (int i = 0; i < ImageButtonsList.Count; i++) {
            if (i == colorInx) {
                ImageButtonsList[i].transform.Find("Frame").gameObject.SetActive(true);
            } else {
                ImageButtonsList[i].transform.Find("Frame").gameObject.SetActive(false);
            }
        }
    }


    public void ShowMenu() {
        MenuButton.SetActive(false);
        Time.timeScale = 0f;
        Menu.SetActive(true);
    }

    public void CloseMenu() {
        MenuButton.SetActive(true);

        Time.timeScale = 1f;
        Menu.SetActive(false);
    }

    public void ClearAllSettings() {
        PlayerPrefs.DeleteAll();
    }
}
