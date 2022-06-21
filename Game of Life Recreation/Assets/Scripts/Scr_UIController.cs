using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Scr_UIController : MonoBehaviour
{
    [SerializeField] Scr_GameOfLife Manager = Scr_GameOfLife.instance;
    [SerializeField]
    private TextMeshProUGUI GrassCurrent, GrassMax,
        TreeCurrent, TreeMax,
        DirtCurrent, DirtMax,
        WaterCurrent, WaterMax,
        SnailCurrent, SnailMax,
        SheepCurrent, SheepMax,
        FireCurrent, FireMax;

    [SerializeField]
    private int GrassCountCur, GrassCountMax,
        TreeCountCur, TreeCountMax,
        DirtCountCur, DirtCountMax,
        WaterCountCur, WaterCountMax,
        SnailCountCur, SnailCountMax,
        SheepCountCur, SheepCountMax,
        FireCountCur, FireCountMax;

    void Awake()
    {
        GrassCurrent.text = "0";
        GrassMax.text = "0";
        TreeCurrent.text = "0";
        TreeMax.text = "0";
        DirtCurrent.text = "0";
        DirtMax.text = "0";
        WaterCurrent.text = "0";
        WaterMax.text = "0";
        SnailCurrent.text = "0";
        SnailMax.text = "0";
        SheepCurrent.text = "0";
        SheepMax.text = "0";
        FireCurrent.text = "0";
        FireMax.text = "0";
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Manager.SetupComplete)
        {
            UpdateText();
        }
    }

    void UpdateText()
    {
        DirtCountCur = 0;
        TreeCountCur = 0;
        WaterCountCur = 0;
        SnailCountCur = 0;
        FireCountCur = 0;
        SheepCountCur = 0;
        GrassCountCur = 0;

        for (int x = 1; x < Manager.m_Width - 1; x++)
        {
            for (int y = 1; y < Manager.m_Height - 1; y++)
            {
                if (Manager.GridTypeFound[x, y] == Scr_GameOfLife.GridNames.Dirt)
                {
                    DirtCountCur++;
                    if (DirtCountCur > DirtCountMax)
                    {
                        DirtCountMax = DirtCountCur;
                    }
                }
                else if (Manager.GridTypeFound[x, y] == Scr_GameOfLife.GridNames.Grass)
                {
                    GrassCountCur++;
                    if (GrassCountCur > GrassCountMax)
                    {
                        GrassCountMax = GrassCountCur;
                    }
                }
                else if (Manager.GridTypeFound[x, y] == Scr_GameOfLife.GridNames.Water)
                {
                    WaterCountCur++;
                    if (WaterCountCur > WaterCountMax)
                    {
                        WaterCountMax = WaterCountCur;
                    }
                }
                else if (Manager.GridTypeFound[x, y] == Scr_GameOfLife.GridNames.Fire)
                {
                    FireCountCur++;
                    if (FireCountCur > FireCountMax)
                    {
                        FireCountMax = FireCountCur;
                    }
                }
                else if (Manager.GridTypeFound[x, y] == Scr_GameOfLife.GridNames.Snail)
                {
                    SnailCountCur++;
                    if (SnailCountCur > SnailCountMax)
                    {
                        SnailCountMax = SnailCountCur;
                    }
                }
                else if (Manager.GridTypeFound[x, y] == Scr_GameOfLife.GridNames.Tree)
                {
                    TreeCountCur++;
                    if (TreeCountCur > TreeCountMax)
                    {
                        TreeCountMax = TreeCountCur;
                    }
                }
                else if (Manager.GridTypeFound[x, y] == Scr_GameOfLife.GridNames.Sheep)
                {
                    SheepCountCur++;
                    if (SheepCountCur > SheepCountMax)
                    {
                        SheepCountMax = SheepCountCur;
                    }
                }
            }
        }

        DirtCurrent.text = DirtCountCur.ToString();
        DirtMax.text = DirtCountMax.ToString();
        GrassCurrent.text = GrassCountCur.ToString();
        GrassMax.text = GrassCountMax.ToString();
        WaterCurrent.text = WaterCountCur.ToString();
        WaterMax.text = WaterCountMax.ToString();
        FireCurrent.text = FireCountCur.ToString();
        FireMax.text = FireCountMax.ToString();
        SnailCurrent.text = SnailCountCur.ToString();
        SnailMax.text = SnailCountMax.ToString();
        TreeCurrent.text = TreeCountCur.ToString();
        TreeMax.text = TreeCountMax.ToString();
        SheepCurrent.text = SheepCountCur.ToString();
        SheepMax.text = SheepCountMax.ToString();
    }
}
