using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_CellLogic : MonoBehaviour
{
    Scr_GameOfLife ManagerInstance = Scr_GameOfLife.instance;
    [SerializeField] private int StartLifetime = 0;
    [SerializeField] private int CurrLifetime = 0;

    [SerializeField] private Scr_GameOfLife.GridNames StartCellType;
    [SerializeField] private Scr_GameOfLife.GridNames CurrCellType;

    [SerializeField] private float ChanceToMove = 0.5f;

    [SerializeField] private List<GameObject> ConnectedTiles;
    public int[,] Position;

    private void Awake()
    {
        ConnectedTiles = new List<GameObject>();
    }

    public void InitializeCellType(Scr_GameOfLife.GridNames Type)
    {
        StartCellType = Type;
        CurrCellType = StartCellType;
        name = Type.GetType().GetEnumName(Type);

        StartLifetime = 0;
        CurrLifetime = StartLifetime;
    }

    public void CreateConnection()
    {
        for (int i = 0; i < ManagerInstance.GridPieces.Count; i++)
        {
            if (Vector2.Distance(transform.position, ManagerInstance.GridPieces[i].transform.position) > 0
            && Vector2.Distance(transform.position, ManagerInstance.GridPieces[i].transform.position) <= 1.5f)
            {
                AddConnection(ManagerInstance.GridPieces[i]);
            }
        }
    }

    public void AddConnection(GameObject To)
    {
        ConnectedTiles.Add(To);
    }

    public void StartLife()
    {
        switch (CurrCellType)
        {
            case (Scr_GameOfLife.GridNames.Fire):
                FireLife();
                break;
            case (Scr_GameOfLife.GridNames.Grass):
                break;
            case (Scr_GameOfLife.GridNames.Sheep):
                SheepLife();
                break;
            case (Scr_GameOfLife.GridNames.Snail):
                break;
            case (Scr_GameOfLife.GridNames.Tree):
                TreeLife();
                break;
            case (Scr_GameOfLife.GridNames.Water):
                break;
            case (Scr_GameOfLife.GridNames.Dirt):
                DirtLife();
                break;
        }
        CurrLifetime++;
    }
    #region Fire Logic
    void FireLife()
    {
        if (Mathf.FloorToInt(Random.value * 1.99f) >= ChanceToMove || CurrLifetime > 3)
        {
            List<int[,]> PosOne = new List<int[,]>();
            List<int[,]> PosTwo = new List<int[,]>();
            PosOne = PositionsToCheck(Scr_GameOfLife.GridNames.Tree);
            PosTwo = PositionsToCheck(Scr_GameOfLife.GridNames.Sheep);

            PosOne.AddRange(PosTwo);
            if (PosOne.Count > 0)
            {
                int RandomLoc = Random.Range(0, PosOne.Count);
                ReplaceLocation(PosOne[RandomLoc], Scr_GameOfLife.GridNames.Fire, 1);
            }
            else
            {
                ReplaceLocation(Position, Scr_GameOfLife.GridNames.Dirt, 0);
            }
        }
    }
    #endregion

    #region Dirt Logic
    void DirtLife()
    {
        if (Mathf.FloorToInt(Random.value * 1.99f) > ChanceToMove || CurrLifetime > 3)
        {
            ReplaceLocation(Position, Scr_GameOfLife.GridNames.Grass, 2);
        }
    }
    #endregion

    #region Sheep Logic
    void SheepLife()
    {
        if(Mathf.FloorToInt(Random.value * 1.99f) >= ChanceToMove || CurrLifetime > 3)
        {
            List<int[,]> PosFound = new List<int[,]>();
            List<int[,]> NeighbourSheeps = new List<int[,]>();

            PosFound = PositionsToCheck(Scr_GameOfLife.GridNames.Grass);
            NeighbourSheeps = PositionsToCheck(Scr_GameOfLife.GridNames.Sheep);

            if (PosFound.Count > 0 && NeighbourSheeps.Count < 1)
            {
                int RandomLoc = Random.Range(0, PosFound.Count);
                ReplaceLocation(PosFound[RandomLoc], Scr_GameOfLife.GridNames.Sheep, 3);
                ReplaceLocation(Position, Scr_GameOfLife.GridNames.Dirt, 0);
            }
            else if(PosFound.Count > 0 && NeighbourSheeps.Count > 0)
            {
                int RandomLoc = Random.Range(0, PosFound.Count);
                ReplaceLocation(PosFound[RandomLoc], Scr_GameOfLife.GridNames.Sheep, 3);
            }
            else
            {
                ReplaceLocation(Position, Scr_GameOfLife.GridNames.Grass, 2);
            }
        }
    }
    #endregion

    #region Tree Logic
    void TreeLife()
    {
        if (Mathf.FloorToInt(Random.value * 1.99f) >= ChanceToMove || CurrLifetime > 3)
        {
            List<int[,]> PosFound = new List<int[,]>();
            PosFound = PositionsToCheck(Scr_GameOfLife.GridNames.Grass);
            if (PosFound.Count > 0)
            {
                int RandomLoc = Random.Range(0, PosFound.Count);
                ReplaceLocation(PosFound[RandomLoc], Scr_GameOfLife.GridNames.Tree, 5);
            }
            else
            {
                ReplaceLocation(Position, Scr_GameOfLife.GridNames.Dirt, 0);
            }
        }
    }
    #endregion

    List<int[,]> PositionsToCheck(Scr_GameOfLife.GridNames TypeToFind)
    {
        List<int[,]> PosFound = new List<int[,]>();

        for (int i = 0; i < ConnectedTiles.Count; i++)
        {
            if (ConnectedTiles[i].GetComponent<Scr_CellLogic>().CurrCellType == TypeToFind)
            {
                PosFound.Add(ConnectedTiles[i].GetComponent<Scr_CellLogic>().Position);
            }
        }

        return PosFound;
    }

    void ReplaceLocation(int[,] Pos, Scr_GameOfLife.GridNames newType, int CellPos)
    {
        ManagerInstance.GridCoordinates[Pos.GetLength(0), Pos.GetLength(1)].GetComponent<SpriteRenderer>().sprite = ManagerInstance.CellsToSpawn[CellPos];
        ManagerInstance.GridTypeFound[Pos.GetLength(0), Pos.GetLength(1)] = newType;
        ManagerInstance.GridCoordinates[Pos.GetLength(0), Pos.GetLength(1)].GetComponent<Scr_CellLogic>().InitializeCellType(newType);

    }
}
