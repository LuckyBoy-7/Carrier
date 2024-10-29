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

    private void Awake()
    {
        for (int i = 1; i <= number; i++)
        {
            int j = i;
            var go = Instantiate(buttonPrefab, transform);
                go.GetComponentInChildren<Button>().onClick.AddListener(
                () =>
                {
                    LevelManager.Instance.LoadLevel(j);
                }
            );
            go.GetComponentInChildren<TMP_Text>().text = i.ToString();
        }
    }
}