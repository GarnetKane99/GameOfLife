using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_GameOfLife : MonoBehaviour
{
    public int m_Width = 40;
    public int m_Height = 30;

    public static Scr_GameOfLife instance { get; private set; }

    public float m_UpdateTimeSeconds;
    [SerializeField] private float m_StartingPopulatedChance;

    public List<Sprite> CellsToSpawn;
    [SerializeField] private GameObject ParentObject;

    private bool m_InitialisedCells = false;
    [HideInInspector] public GridNames GridType;
    [HideInInspector] public GridNames[,] GridTypeFound;

    private bool[,] m_CellLayout;
    [HideInInspector] public GameObject[,] GridCoordinates;
    [HideInInspector] public List<GameObject> GridPieces;

    public enum GridNames
    {
        Dirt,
        Fire,
        Grass,
        Sheep,
        Snail,
        Tree,
        Water,
        Empty
    }

    private void Awake()
    {
        GridCoordinates = new GameObject[m_Width, m_Height];
        m_CellLayout = new bool[m_Width, m_Height];
        GridTypeFound = new GridNames[m_Width, m_Height];
        GridPieces = new List<GameObject>();

        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        for (int x = 0; x < m_Width; x++)
        {
            for (int y = 0; y < m_Height; y++)
            {
                m_CellLayout[x, y] = (Random.value <= m_StartingPopulatedChance);
            }
        }

        DrawCells();
    }

    void DrawCells()
    {
        if (!m_InitialisedCells)
        {
            InitializeCells();

            for (int i = 1; i < m_Width - 1; i++)
            {
                for (int j = 1; j < m_Height - 1; j++)
                {
                    int RandomSprite = Random.Range(0, CellsToSpawn.Count);
                    GridCoordinates[i, j].GetComponent<SpriteRenderer>().sprite = m_CellLayout[i, j] ? CellsToSpawn[RandomSprite] : CellsToSpawn[2];
                    GridCoordinates[i, j].GetComponent<Scr_CellLogic>().InitializeCellType(m_CellLayout[i,j] ? (GridNames)System.Enum.GetValues(typeof(GridNames)).GetValue(RandomSprite) : GridNames.Grass);
                    GridCoordinates[i, j].GetComponent<Scr_CellLogic>().Position = new int[i, j];
                    GridTypeFound[i, j] = m_CellLayout[i, j] ? (GridNames)System.Enum.GetValues(typeof(GridNames)).GetValue(RandomSprite) : GridNames.Grass;
                }
            }

            for(int i = 0; i < GridPieces.Count; i++)
            {
                GridPieces[i].GetComponent<Scr_CellLogic>().CreateConnection();
                if(GridPieces[i].name == "Base")
                {
                    GridPieces[i].GetComponent<Scr_CellLogic>().InitializeCellType(GridNames.Empty);
                }
            }

            for(int i = 0; i < GridPieces.Count; i++)
            {
                GridPieces[i].GetComponent<Scr_CellLogic>().InvokeRepeating("StartLife", Random.Range(0.5f, 1.0f), Random.Range(0.25f, m_UpdateTimeSeconds));
            }
        }
    }

    void InitializeCells()
    {
        m_InitialisedCells = true;

        for (int y = 0; y < m_Height; y++)
        {
            for (int x = 0; x < m_Width; x++)
            {
                GridCoordinates[x, y] = CreateEmptyObject(x,y);
            }
        }
    }

    private GameObject CreateEmptyObject(int x, int y)
    {
        GameObject _GO = new GameObject("Base");
        _GO.transform.parent = ParentObject.transform;
        _GO.transform.position = new Vector2(x - (m_Width / 2), y - (m_Height / 2) + 0.5f);
        _GO.AddComponent<Scr_CellLogic>();
        _GO.AddComponent<SpriteRenderer>();
        GridPieces.Add(_GO);

        return _GO;
    }
}
