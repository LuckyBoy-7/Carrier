using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButtonGenerator : MonoBehaviour
{
    public int number;
    public GameObject buttonPrefab;
    public static List<int> hard = new List<int>() { 5, 15 };
    public static List<int> recommended = new List<int>() { 1, 3, 4, 7, 13, 14, 16, 17, 18, 19 };

    private void Awake()
    {
        for (int i = 1; i <= number; i++)
        {
            int j = i;
            var go = Instantiate(buttonPrefab, transform);
            go.GetComponentInChildren<Button>().onClick.AddListener(
                () => { LevelManager.Instance.LoadLevel(j); }
            );
            go.GetComponentInChildren<Button>().GetComponent<Image>().color = GetColorByLevel(j);
            go.GetComponentInChildren<TMP_Text>().text = i.ToString();
        }
    }

    public static Color GetColorByLevel(int level)
    {
        if (hard.Contains(level))
            return Color.red;
        if (recommended.Contains(level))
            return Color.green;
        return Color.white;
    }
}