using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class highScoreTable : MonoBehaviour {
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<Transform> hightScoreEntryTransformList;

    private const int MaxScoresInTable = 10; // максимальное кол-во рекордов в таблице
    private const string ScoreTableSaveKey = "ScoreTable.saveData"; // имя для сохранения в PlayerPrefs

    [Space]
    [Header("DebugSaveData")]
    [SerializeField]
    private ScoreTable _scoreTable; // отображение таблицы в инспектрое для дебага

    public void Awake() {
        entryContainer = transform.Find("highScoreEntryContainer");
        entryTemplate = entryContainer.Find("highScoreEntryTemplate");
        entryTemplate.gameObject.SetActive(false);

        hightScoreEntryTransformList = new List<Transform>();
        LoadTableOrDefault(); // загрузили таблицу, либо создали пустую, если сохранений не было

        // если таблица пустая, то добавляем тестовый сейв
        if (_scoreTable.IsEmpty) {
            AddHightScoreAndSave(score: 1, name: "test");
        }
        // обновляем UI таблицы
        RedrawTableUI();
        Debug.Log(JsonUtility.ToJson(_scoreTable));
    }
   
    public void AddHightScoreAndSave(int score, string name) { // Добавление рекорда в таблицу и пересохранение 
        _scoreTable.AddScore(score, name);
        Debug.Log(JsonUtility.ToJson(_scoreTable));
        SaveTable();
        // Debug.Log(JsonUtility.ToJson(_scoreTable));
    }

    public void SaveTable() { // сохранение таблицы в PlayerPrefs 
        SortAndCutTable();
        PlayerPrefs.SetString(ScoreTableSaveKey, JsonUtility.ToJson(_scoreTable));
    }

    public void LoadTableOrDefault() { // загружка таблицы из PlayerPrefs  
        _scoreTable = JsonUtility.FromJson<ScoreTable>(PlayerPrefs.GetString(ScoreTableSaveKey));
        _scoreTable = _scoreTable ?? new ScoreTable(); //-> если сохранения не было и вернуло NULL, то создаём новую таблицу
    }

    private void SortAndCutTable() { // корректировка таблицы
        _scoreTable.ScoresList = _scoreTable.ScoresList
                                                        .OrderByDescending(i => i.Score) //-> сортировка от большего к меньшему
                                                        .Take(MaxScoresInTable) //-> оставляем только нужное кол-во рекордов
                                                        .ToList();
    }

    private void RedrawTableUI() {
        foreach (ScoreData scoreData in _scoreTable.ScoresList) //отрисовка строк таблицы рекордов 
        { 
            createHightScoreEntryTransform(scoreData.Score, scoreData.Name, entryContainer, hightScoreEntryTransformList);
        }
    }

    private void createHightScoreEntryTransform(int score, string name, Transform container, List<Transform> transformList) {
        float templateHight = 45f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString;
        switch (rank) {
            default:
                rankString = rank + ""; break;
            case 1:
                rankString = rank + "ый"; break;
            case 2:
                rankString = rank + "ой"; break;
            case 3:
                rankString = rank + "ий"; break;
        }
        entryTransform.Find("position (1)").GetComponent<Text>().text = rankString;
        entryTransform.Find("score (1)").GetComponent<Text>().text = score.ToString();
        entryTransform.Find("name (1)").GetComponent<Text>().text = name;

        transformList.Add(entryTransform);
    }

    [System.Serializable]
    public class ScoreTable {
        public List<ScoreData> ScoresList = new List<ScoreData>();

        public bool IsEmpty => ScoresList.Count == 0;
        public void AddScore(int score, string name) {
            Debug.Log(PlayerPrefs.GetString(ScoreTableSaveKey));
            ScoresList.Add(new ScoreData(score, name));
            // Debug.Log(PlayerPrefs.GetString(ScoreTableSaveKey));
        }
    }

    [System.Serializable]
    public class ScoreData {
        public int Score;
        public string Name;

        public ScoreData(int score, string name) {
            Score = score;
            Name = name;
        }
    }
}