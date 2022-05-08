using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_SheepLogic : MonoBehaviour
{
    Scr_GameOfLife GameOfLifeManagerInstance = Scr_GameOfLife.instance;

    private void Awake()
    {
        GameOfLifeManagerInstance.GridType = Scr_GameOfLife.GridNames.Sheep;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
