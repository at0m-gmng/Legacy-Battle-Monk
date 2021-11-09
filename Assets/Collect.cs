using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collect : MonoBehaviour
{
    public static int score;
    public Text TextScore;
    // Start is called before the first frame update
    void Start()
    {
        TextScore = GetComponent<Text>();
    }

    void Update() {
        TextScore.text = "Счёт: " + score;
    }
}
