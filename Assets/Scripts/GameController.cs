using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Lucky.Framework;
using Lucky.Kits.Collections;
using Lucky.Kits.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Input = Lucky.Kits.Inputs.Input;

public class GameController : ManagedBehaviour
{
    private List<Transform> holes = new();
    private List<Transform> rocks = new();
    public TMP_Text levelText;

    private void Awake()
    {
        holes.Extend(GameObject.FindGameObjectsWithTag("Hole").Select(go => go.transform).ToList());
        rocks.Extend(GameObject.FindGameObjectsWithTag("Rock").Select(go => go.transform).ToList());
    }

    private void Start()
    {
        levelText.text = $"Level {LevelManager.Instance.currentLevel}";
        levelText.color = LevelButtonGenerator.GetColorByLevel(LevelManager.Instance.currentLevel);
    }

    protected override void ManagedFixedUpdate()
    {
        base.ManagedFixedUpdate();
        if (Input.GetKeyDown(KeyCode.Q))
            ReturnToMenu();
        else if (Input.GetKeyDown(KeyCode.R))
            Restart();
        else if (Input.GetKeyDown(KeyCode.Z) && Player.instance.canOperate)
            Undo();
        else if (Input.GetKeyDown(KeyCode.X) && Player.instance.canOperate)
            Do();

        // win checker
        foreach (var hole in holes)
        {
            if (rocks.All(rock => Vector2.Distance(rock.position, hole.position) > 0.01f))
                return;
        }

        Player.instance.canOperate = false;
        StartCoroutine(
            () =>
            {
                print("Win");
                LevelManager.Instance.LoadNextLevel();
            }, 0.5f
        );
    }

    public void ReturnToMenu() => SceneManager.LoadScene("Menu");
    public void Restart() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    public void Undo()
    {
        CommandManager.Instance.Undo();
    }

    public void Do()
    {
        CommandManager.Instance.Do();
    }
}