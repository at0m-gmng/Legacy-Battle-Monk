using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// using UnityEngine.SceneManagement;

public class erorHightScoreTable : MonoBehaviour {
    private Transform entryContainer;
    private Transform entryTemplate;
    // private List<HightScoreEntry> hightScoreEntryList;
    private List<Transform> hightScoreEntryTransformList;
    // private List<Transform> hightScoreEntryTransformList = new List<Transform>(10);

    // [SerializeField] private Transform entryContainer;
    // [SerializeField] private Transform entryTemplate;

    private HightScore hightSSS;

    public void Awake() {
        entryContainer = transform.Find("highScoreEntryContainer");
        entryTemplate = entryContainer.Find("highScoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);
        // AddHightScoreEntry(4572, "asd");
        //  AddHightScoreEntry(234, "asd");
        //   AddHightScoreEntry(234234, "asd");

        // string jsonString1 = PlayerPrefs.GetString("hightScoreTable1");
        // HightScore temp =  JsonUtility.FromJson<HightScore>(jsonString1);
        // PlayerPrefs.SetString("hightScoreTable", asd);
        // PlayerPrefs.Save();
        // PlayerPrefs.SetString("hightScoreTable", "{}");
        // PlayerPrefs.Save();
        // new HightScore();

        // string jsonString = PlayerPrefs.GetString("hightScoreTable");
        HightScore hightscore =  JsonUtility.FromJson<HightScore>(PlayerPrefs.GetString("hightScoreTable"));

        // {"hightScoreEntryList":[{"score":354334,"name":"dtjhfg"}]}
        if(hightscore.hightScoreEntryList.Count == 0) {
            Debug.Log("был вызов");
            new HightScore();
            // AddHightScoreEntry(1, "test");
        }
            // public List<HightScoreEntry> hightScoreEntryList;
            // hightScoreEntryTransformList = new List<Transform>();
            // AddHightScoreEntry(1, "test");
            // HightScore();
            // HightScoreEntry firstHightScore = new HightScoreEntry { score = 1, name = "test"};

            // string jsonString1 = PlayerPrefs.GetString("hightScoreTable");
            // HightScore hightscore1 =  JsonUtility.FromJson<HightScore>(jsonString1);
            
            // hightscore1.hightScoreEntryList.Add(firstHightScore);
            
            // string json = JsonUtility.ToJson(hightscore1);
            // PlayerPrefs.SetString("hightScoreTable", json);
            // PlayerPrefs.Save();
        // }
        // HightScore hightscore =  JsonUtility.FromJson<HightScore>(jsonString);


        for(int i = 0; i < hightscore.hightScoreEntryList.Count; i++) { // сортируем массив
            for(int j = i + 1; j < hightscore.hightScoreEntryList.Count; j++) {
                if(hightscore.hightScoreEntryList[j].score > hightscore.hightScoreEntryList[i].score) {
                    HightScoreEntry tmp = hightscore.hightScoreEntryList[i];
                    hightscore.hightScoreEntryList[i] = hightscore.hightScoreEntryList[j];
                    hightscore.hightScoreEntryList[j] = tmp;

                    // string json = JsonUtility.ToJson(hightscore); // перезаписываем отсортированный массив
                    PlayerPrefs.SetString("hightScoreTable", JsonUtility.ToJson(hightscore));
                    // PlayerPrefs.Save();
                }
            }
        }
        int n=10;
        int k = hightscore.hightScoreEntryList.Count;
        for (int i = 0; i < (hightscore.hightScoreEntryList.Count - n); i ++ ) { 
            if(hightscore.hightScoreEntryList.Count > n) { 
                hightscore.hightScoreEntryList.RemoveAt(k-1);

                // string json = JsonUtility.ToJson(hightscore);
                PlayerPrefs.SetString("hightScoreTable", JsonUtility.ToJson(hightscore));
                // PlayerPrefs.Save();
                Debug.Log(hightscore.hightScoreEntryList.Count);
                Debug.Log(PlayerPrefs.GetString("hightScoreTable"));
            }
        }

        Debug.Log(PlayerPrefs.GetString("hightScoreTable"));
        hightScoreEntryTransformList = new List<Transform>();
        foreach (HightScoreEntry hightScoreEntry in hightscore.hightScoreEntryList) {
            createHightScoreEntryTransform(hightScoreEntry, entryContainer, hightScoreEntryTransformList);
        }
    }

    public void createHightScoreEntryTransform(HightScoreEntry hightScoreEntry, Transform container, List<Transform> transformList) {
        float templateHight = 45f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count +1;
        string rankString;
        switch(rank) {
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

        int score = hightScoreEntry.score;
        entryTransform.Find("score (1)").GetComponent<Text>().text = score.ToString();
        
        string name = hightScoreEntry.name;
        entryTransform.Find("name (1)").GetComponent<Text>().text = name;

        transformList.Add(entryTransform);
    }

    public static void AddHightScoreEntry(int score, string name) {
        HightScoreEntry hightScoreEntry = new HightScoreEntry { score = score, name = name};

        // string jsonString = PlayerPrefs.GetString("hightScoreTable");
        HightScore hightscore =  JsonUtility.FromJson<HightScore>(PlayerPrefs.GetString("hightScoreTable"));
        
        hightscore.hightScoreEntryList.Add(hightScoreEntry);
        
        // string json = JsonUtility.ToJson(hightscore);
        PlayerPrefs.SetString("hightScoreTable", JsonUtility.ToJson(hightscore));
        // PlayerPrefs.Save();
    }

    public class HightScore {
        public List<HightScoreEntry> hightScoreEntryList;
    }

    [System.Serializable]
    public class HightScoreEntry {
        public int score;
        public string name;
    }

}
