using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameOfLifeManager : MonoBehaviour
{
    [SerializeField] private int m_Width = 40;
    [SerializeField] private int m_Height = 30;
    [SerializeField] private float m_UpdateTimeSeconds = 1f;
    [SerializeField] private float m_StartingPopulatedChance = 0.1f;

    private bool[,] m_CellLayout;

    [SerializeField] private GridLayoutGroup m_GridLayoutGroup;
    private Image[,] m_Images;
    private bool m_InitialisedCells = false;

    [SerializeField] private List<Sprite> CharacterImages;
    private List<GameObject> ListSprite;

    private enum GridType
    {
        Dirt,
        Fire,
        Grass,
        Sheep,
        Tree,
        Water
    }

    private GridType[,] _TypeFound;

    void Awake()
    {
        m_CellLayout = new bool[m_Width, m_Height];
        _TypeFound = new GridType[m_Width, m_Height];
        ListSprite = new List<GameObject>();

        for (int i = 0; i < m_Width; i++)
        {
            for (int j = 0; j < m_Height; j++)
            {
                m_CellLayout[i, j] = (Random.value <= m_StartingPopulatedChance);
            }
        }
        DrawCells();
    }

    private void Update()
    {
        GetInput();
    }

    void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ResetImages();
        }
    }

    void ResetImages()
    {
        CancelInvoke("StartLife");
        for (int i = 0; i < ListSprite.Count; i++)
        {
            Destroy(ListSprite[i]);
        }
        ListSprite = new List<GameObject>();

        m_CellLayout = new bool[m_Width, m_Height];
        _TypeFound = new GridType[m_Width, m_Height];

        for (int i = 0; i < m_Width; i++)
        {
            for (int j = 0; j < m_Height; j++)
            {
                m_CellLayout[i, j] = (Random.value <= m_StartingPopulatedChance);
            }
        }
        m_InitialisedCells = false;
        DrawCells();
    }

    private void DrawCells()
    {
        if (!m_InitialisedCells)
        {
            InitialiseCells();
            for (int i = 1; i < m_Width - 1; i++)
            {
                for (int j = 1; j < m_Height - 1; j++)
                {
                    int RandomImg = Random.Range(0, CharacterImages.Count);
                    m_Images[i, j].GetComponent<Image>().sprite = m_CellLayout[i, j] ? CharacterImages[RandomImg] : CharacterImages[2];
                    _TypeFound[i, j] = m_CellLayout[i, j] ? (GridType)System.Enum.GetValues(typeof(GridType)).GetValue(RandomImg) : GridType.Grass;
                    m_Images[i, j].name = _TypeFound[i, j].GetType().GetEnumName(_TypeFound[i, j]);
                }
            }
        }

        InvokeRepeating("StartLife", 1.0f, m_UpdateTimeSeconds);
    }
    private void InitialiseCells()
    {
        m_InitialisedCells = true;

        //Set the cell size based on how many can fit in each row
        RectTransform _RectTransfrom = m_GridLayoutGroup.GetComponent<RectTransform>();

        float _MaxWidth = _RectTransfrom.rect.width;
        float _MaxHeight = _RectTransfrom.rect.height;

        m_GridLayoutGroup.cellSize = new Vector2(_MaxWidth / (float)m_Width, _MaxHeight / (float)m_Height);

        //Create all the image objects
        m_Images = new Image[m_Width, m_Height];

        for (int i = 0; i < m_Height; i++)
        {
            for (int j = 0; j < m_Width; j++)
            {
                m_Images[j, i] = CreateNewImage();
                ListSprite.Add(m_Images[j, i].gameObject);
            }
        }
    }

    private Image CreateNewImage()
    {
        GameObject _GO = new GameObject("Image");

        _GO.transform.SetParent(m_GridLayoutGroup.transform);

        return _GO.AddComponent<Image>();
    }

    private void StartLife()
    {
/*        DirtLife();
        FireLife();
        TreeLife();
        SheepLife();*/

        StartCoroutine(DirtLife());
        StartCoroutine(FireLife());
        StartCoroutine(TreeLife());
        StartCoroutine(SheepLife());
    }

    #region Fire Logic
    IEnumerator FireLife()
    {
        for (int i = 1; i < m_Width - 1; i++)
        {
            for (int j = 1; j < m_Height - 1; j++)
            {
                if (_TypeFound[i, j] == GridType.Fire)
                {
                    List<int[,]> foundPos = new List<int[,]>();

                    if (_TypeFound[i, j + 1] == GridType.Tree ||
                        _TypeFound[i, j + 1] == GridType.Sheep)
                    {
                        foundPos.Add(new int[i, j + 1]);
                    }
                    if (_TypeFound[i, j - 1] == GridType.Tree ||
                        _TypeFound[i, j - 1] == GridType.Sheep)
                    {
                        foundPos.Add(new int[i, j - 1]);
                    }
                    if (_TypeFound[i + 1, j] == GridType.Tree ||
                        _TypeFound[i + 1, j] == GridType.Sheep)
                    {
                        foundPos.Add(new int[i + 1, j]);
                    }
                    if (_TypeFound[i - 1, j] == GridType.Tree ||
                        _TypeFound[i - 1, j] == GridType.Sheep)
                    {
                        foundPos.Add(new int[i - 1, j]);
                    }
                    if (_TypeFound[i + 1, j - 1] == GridType.Tree ||
                        _TypeFound[i + 1, j - 1] == GridType.Sheep)
                    {
                        foundPos.Add(new int[i + 1, j - 1]);
                    }
                    if (_TypeFound[i - 1, j + 1] == GridType.Tree ||
                        _TypeFound[i - 1, j + 1] == GridType.Sheep)
                    {
                        foundPos.Add(new int[i - 1, j + 1]);
                    }
                    if (_TypeFound[i + 1, j + 1] == GridType.Tree ||
                        _TypeFound[i + 1, j + 1] == GridType.Sheep)
                    {
                        foundPos.Add(new int[i + 1, j + 1]);
                    }
                    if (_TypeFound[i - 1, j - 1] == GridType.Tree ||
                        _TypeFound[i - 1, j - 1] == GridType.Sheep)
                    {
                        foundPos.Add(new int[i - 1, j - 1]);
                    }

                    if (foundPos.Count > 0 && foundPos.Count < 3)
                    {
                        int randomLoc = Random.Range(0, foundPos.Count);
                        ReplaceLocation(foundPos[randomLoc], GridType.Fire, CharacterImages[1]);
                        ReplaceLocation(new int[i, j], GridType.Dirt, CharacterImages[0]);
                    }
                    else if (foundPos.Count >= 3)
                    {
                        for (int y = 0; y < foundPos.Count; y++)
                        {
                            List<int> updated = new List<int>();
                            int randomLoc = Random.Range(0, foundPos.Count);
                            foreach (int x in updated)
                            {
                                if (x == randomLoc)
                                {
                                    y--;
                                    break;
                                }
                            }
                            updated.Add(randomLoc);
                            ReplaceLocation(foundPos[randomLoc], GridType.Fire, CharacterImages[1]);
                            ReplaceLocation(new int[i, j], GridType.Dirt, CharacterImages[0]);
                        }
                    }
                    else
                    {
                        ReplaceLocation(new int[i, j], GridType.Sheep, CharacterImages[3]);
                    }
                }
            }
        }
        yield return new WaitForSeconds(0.015f);
    }
    #endregion

    #region Tree Logic
    IEnumerator TreeLife()
    {
        for (int i = 1; i < m_Width - 1; i++)
        {
            for (int j = 1; j < m_Height - 1; j++)
            {
                if (_TypeFound[i, j] == GridType.Tree)
                {
                    List<int[,]> foundPos = new List<int[,]>();

                    if (_TypeFound[i, j + 1] == GridType.Grass)
                    {
                        foundPos.Add(new int[i, j + 1]);
                    }
                    if (_TypeFound[i, j - 1] == GridType.Grass)
                    {
                        foundPos.Add(new int[i, j - 1]);
                    }
                    if (_TypeFound[i + 1, j] == GridType.Grass)
                    {
                        foundPos.Add(new int[i + 1, j]);
                    }
                    if (_TypeFound[i - 1, j] == GridType.Grass)
                    {
                        foundPos.Add(new int[i - 1, j]);
                    }
                    if (_TypeFound[i + 1, j - 1] == GridType.Grass)
                    {
                        foundPos.Add(new int[i + 1, j - 1]);
                    }
                    if (_TypeFound[i - 1, j + 1] == GridType.Grass)
                    {
                        foundPos.Add(new int[i - 1, j + 1]);
                    }
                    if (_TypeFound[i + 1, j + 1] == GridType.Grass)
                    {
                        foundPos.Add(new int[i + 1, j + 1]);
                    }
                    if (_TypeFound[i - 1, j - 1] == GridType.Grass)
                    {
                        foundPos.Add(new int[i - 1, j - 1]);
                    }

                    if (foundPos.Count > 0)// && foundPos.Count < 3)
                    {
                        int randomLoc = Random.Range(0, foundPos.Count);
                        ReplaceLocation(foundPos[randomLoc], GridType.Tree, CharacterImages[4]);
                    }
                }
            }
        }
        yield return new WaitForSeconds(0.015f);
    }
    #endregion

    #region Sheep Logic
    IEnumerator SheepLife()
    {
        for (int i = 1; i < m_Width - 1; i++)
        {
            for (int j = 1; j < m_Height - 1; j++)
            {
                if (_TypeFound[i, j] == GridType.Sheep)
                {
                    List<int[,]> foundPos = new List<int[,]>();
                    List<int[,]> sheepLocs = new List<int[,]>();
                    int SheepCount = 0;

                    if (_TypeFound[i, j + 1] == GridType.Grass)
                    {
                        foundPos.Add(new int[i, j + 1]);
                    }
                    else if (_TypeFound[i, j + 1] == GridType.Sheep)
                    {
                        sheepLocs.Add(new int[i, j + 1]);
                        SheepCount++;
                    }
                    if (_TypeFound[i, j - 1] == GridType.Grass)
                    {
                        foundPos.Add(new int[i, j - 1]);
                    }
                    else if (_TypeFound[i, j - 1] == GridType.Sheep)
                    {
                        sheepLocs.Add(new int[i, j - 1]);
                        SheepCount++;
                    }
                    if (_TypeFound[i + 1, j] == GridType.Grass)
                    {
                        foundPos.Add(new int[i + 1, j]);
                    }
                    else if (_TypeFound[i + 1, j] == GridType.Sheep)
                    {
                        sheepLocs.Add(new int[i + 1, j]);
                        SheepCount++;
                    }
                    if (_TypeFound[i - 1, j] == GridType.Grass)
                    {
                        foundPos.Add(new int[i - 1, j]);
                    }
                    else if (_TypeFound[i - 1, j] == GridType.Sheep)
                    {
                        sheepLocs.Add(new int[i - 1, j]);
                        SheepCount++;
                    }
                    if (_TypeFound[i + 1, j - 1] == GridType.Grass)
                    {
                        foundPos.Add(new int[i + 1, j - 1]);
                    }
                    else if (_TypeFound[i + 1, j - 1] == GridType.Sheep)
                    {
                        sheepLocs.Add(new int[i + 1, j - 1]);
                        SheepCount++;
                    }
                    if (_TypeFound[i - 1, j + 1] == GridType.Grass)
                    {
                        foundPos.Add(new int[i - 1, j + 1]);
                    }
                    else if (_TypeFound[i - 1, j + 1] == GridType.Sheep)
                    {
                        sheepLocs.Add(new int[i - 1, j + 1]);
                        SheepCount++;
                    }
                    if (_TypeFound[i + 1, j + 1] == GridType.Grass)
                    {
                        foundPos.Add(new int[i + 1, j + 1]);
                    }
                    else if (_TypeFound[i + 1, j + 1] == GridType.Sheep)
                    {
                        sheepLocs.Add(new int[i + 1, j + 1]);
                        SheepCount++;
                    }
                    if (_TypeFound[i - 1, j - 1] == GridType.Grass)
                    {
                        foundPos.Add(new int[i - 1, j - 1]);
                    }
                    else if (_TypeFound[i - 1, j - 1] == GridType.Sheep)
                    {
                        sheepLocs.Add(new int[i - 1, j - 1]);
                        SheepCount++;
                    }

                    if (foundPos.Count > 0 && SheepCount < 2)
                    {
                        int randomLoc = Random.Range(0, foundPos.Count);
                        ReplaceLocation(foundPos[randomLoc], GridType.Sheep, CharacterImages[3]);
                        ReplaceLocation(new int[i, j], GridType.Dirt, CharacterImages[0]);
                    }
                    else if (foundPos.Count > 0 && SheepCount >= 2)
                    {
                        int randomLoc = Random.Range(0, foundPos.Count);
                        ReplaceLocation(foundPos[randomLoc], GridType.Sheep, CharacterImages[3]);
                        ReplaceLocation(new int[i, j], GridType.Dirt, CharacterImages[0]);
                    }
                    else
                    {
                        ReplaceLocation(new int[i, j], GridType.Fire, CharacterImages[1]);
                    }
                }
            }
        }

        yield return new WaitForSeconds(0.015f);
    }
    #endregion

    #region Dirt Logic
    IEnumerator DirtLife()
    {
        for (int i = 1; i < m_Width - 1; i++)
        {
            for (int j = 1; j < m_Height - 1; j++)
            {
                if (_TypeFound[i, j] == GridType.Dirt)
                {
                    List<int[,]> foundPos = new List<int[,]>();
                    bool WaterFound = false;

                    if (_TypeFound[i, j + 1] == GridType.Grass)
                    {
                        foundPos.Add(new int[i, j + 1]);
                    }
                    else if (_TypeFound[i, j + 1] == GridType.Water)
                    {
                        WaterFound = true;
                    }
                    if (_TypeFound[i, j - 1] == GridType.Grass)
                    {
                        foundPos.Add(new int[i, j - 1]);
                    }
                    else if (_TypeFound[i, j - 1] == GridType.Water)
                    {
                        WaterFound = true;
                    }
                    if (_TypeFound[i + 1, j] == GridType.Grass)
                    {
                        foundPos.Add(new int[i + 1, j]);
                    }
                    else if (_TypeFound[i + 1, j] == GridType.Water)
                    {
                        WaterFound = true;
                    }
                    if (_TypeFound[i - 1, j] == GridType.Grass)
                    {
                        foundPos.Add(new int[i - 1, j]);
                    }
                    else if (_TypeFound[i - 1, j] == GridType.Water)
                    {
                        WaterFound = true;
                    }
                    if (_TypeFound[i + 1, j - 1] == GridType.Grass)
                    {
                        foundPos.Add(new int[i + 1, j - 1]);
                    }
                    else if (_TypeFound[i + 1, j - 1] == GridType.Water)
                    {
                        WaterFound = true;
                    }
                    if (_TypeFound[i - 1, j + 1] == GridType.Grass)
                    {
                        foundPos.Add(new int[i - 1, j + 1]);
                    }
                    else if (_TypeFound[i - 1, j + 1] == GridType.Water)
                    {
                        WaterFound = true;
                    }
                    if (_TypeFound[i + 1, j + 1] == GridType.Grass)
                    {
                        foundPos.Add(new int[i + 1, j + 1]);
                    }
                    else if (_TypeFound[i + 1, j + 1] == GridType.Water)
                    {
                        WaterFound = true;
                    }
                    if (_TypeFound[i - 1, j - 1] == GridType.Grass)
                    {
                        foundPos.Add(new int[i - 1, j - 1]);
                    }
                    else if (_TypeFound[i - 1, j - 1] == GridType.Water)
                    {
                        WaterFound = true;
                    }

                    if (foundPos.Count > 0 && !WaterFound)
                    {
                        ReplaceLocation(new int[i, j], GridType.Grass, CharacterImages[2]);
                    }
                    else if (WaterFound)
                    {
                        ReplaceLocation(new int[i, j], GridType.Tree, CharacterImages[4]);
                    }
                }
            }
        }
        yield return new WaitForSeconds(0.015f);
    }
    #endregion

    private void ReplaceLocation(int[,] pos, GridType newType, Sprite newSprite)
    {
        _TypeFound[pos.GetLength(0), pos.GetLength(1)] = newType;
        m_Images[pos.GetLength(0), pos.GetLength(1)].GetComponent<Image>().sprite = newSprite;
        m_Images[pos.GetLength(0), pos.GetLength(1)].name = _TypeFound[pos.GetLength(0), pos.GetLength(1)].GetType().GetEnumName(_TypeFound[pos.GetLength(0), pos.GetLength(1)]);
    }
}
