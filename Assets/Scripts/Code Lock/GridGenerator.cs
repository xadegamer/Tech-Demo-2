using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [Header("Material")]
    [SerializeField] private Material normalMaterial;
    [SerializeField] private Material selectedMaterial;

    [SerializeField] private GameObject gridPiece;
    [SerializeField] private int gridWidth;
    [SerializeField] private int gridHeight;

    [SerializeField] private int offset;
    [SerializeField] private float tileSize;
    [SerializeField] private float tileMargin;

    [SerializeField] private GameObject[,] tiles;
    [SerializeField] private Transform startPos;

    [Header("Debug")]
    [SerializeField] private GameObject selectedTile;
    [SerializeField] private GameObject lastTile;
    [SerializeField] private int x = 0;
    [SerializeField] private int y = 0;

    protected void Start()
    {
        GenerateGrid();
        GenerateGameGrid();
    }

    void Update()
    {
        InputManager();
    }

    protected virtual void GenerateGameGrid()
    {
        for (int i = 0; i < gridWidth; i++)
        {
            tiles[i, gridWidth].GetComponent<Keypad>().SetUp(Keypad.KeyType.Number, i + 1);
            tiles[i, gridWidth - 1].GetComponent<Keypad>().SetUp(Keypad.KeyType.Number, i + 4);
            tiles[i, gridWidth - 2].GetComponent<Keypad>().SetUp(Keypad.KeyType.Number, i + 7);
        }

        for (int i = 0; i < gridWidth; i++)
        {
            switch (i)
            {
                case 0:
                    tiles[i, gridWidth - 3].GetComponent<Keypad>().SetUp(Keypad.KeyType.Delete);
                    break;
                case 1:
                    tiles[i, gridWidth - 3].GetComponent<Keypad>().SetUp(Keypad.KeyType.Number, 0);
                    break;
                case 2:
                    tiles[i, gridWidth - 3].GetComponent<Keypad>().SetUp(Keypad.KeyType.Clear);
                    break;
            }
        }
    }

    internal void RegenerateGrid()
    {
        GenerateGrid();
        GenerateGameGrid();
    }

    
    protected void GenerateGrid()
    {
        //Create actual grid
        tiles = new GameObject[gridWidth, gridHeight];

        Vector3 spawnPoint = startPos.transform.position;
        
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                GameObject piece = Instantiate(gridPiece, spawnPoint,  Quaternion.identity);
                
                piece.transform.SetParent(startPos.transform);
                
                piece.name = "TILE [" + i + "," + j + "]";

                piece.transform.Translate(new Vector3(
                    (i * offset) + (tileMargin * i),
                    (j * offset) + (tileMargin * j), 
                    0), Space.Self);



                piece.GetComponent<Keypad>().OnSelected(normalMaterial);

                piece.GetComponent<Keypad>().SetScale(new Vector3(tileSize, tileSize, tileSize));

                tiles[i, j] = piece;
            }
        }

        lastTile = Instantiate(gridPiece, startPos.transform);
        lastTile.transform.localPosition = new Vector3(0.1f, -0.1f, 0);
        lastTile.GetComponent<Keypad>().OnSelected(normalMaterial);
        lastTile.GetComponent<Keypad>().SetScale(new Vector3(tileSize * 3, tileSize, tileSize));
        lastTile.GetComponent<Keypad>().SetUp(Keypad.KeyType.Confirm);
        lastTile.name = "TILE [" + 1 + "," + -1 + "]";

        //Spawn position and colorize the selection box
        x = 0;
        y = 0;
        WarpTile(tiles[x, y]);
    }

    
    private void InputManager()
    {
        if (StarterAssetsInputs.Instance.left)
        {
            StarterAssetsInputs.Instance.left = false;
            if (KeypadPuzzle.Instance.isActive) Input_Left();
        }

        if (StarterAssetsInputs.Instance.right)
        {
            StarterAssetsInputs.Instance.right = false;
            if (KeypadPuzzle.Instance.isActive) Input_Right();
        }

        if (StarterAssetsInputs.Instance.up)
        {
            StarterAssetsInputs.Instance.up = false;
            if (KeypadPuzzle.Instance.isActive) Input_Up();
        }

        if (StarterAssetsInputs.Instance.down)
        {
            StarterAssetsInputs.Instance.down = false;
            if (KeypadPuzzle.Instance.isActive) Input_Down();
        }

        if (StarterAssetsInputs.Instance.enter)
        {
            StarterAssetsInputs.Instance.enter = false;
            if (KeypadPuzzle.Instance.isActive) Input_Click();
        }


        //if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) Input_Up();
        //if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) Input_Down();
        //if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) Input_Right();
        //if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) Input_Left();
        //if (Input.GetKeyDown(KeyCode.Return)) Input_Click();

    }

    protected virtual void Input_Click()
    {
        selectedTile.GetComponent<Keypad>().OnPressed();
    }
    
    public void WarpTile( GameObject newTile)
    {
        if(selectedTile) selectedTile.GetComponent<Keypad>().OnSelected(normalMaterial);
        selectedTile = newTile;
        selectedTile.GetComponent<Keypad>().OnSelected(selectedMaterial);
    }

    protected virtual void Input_Left()
    {
        if (selectedTile == lastTile) return;

        if (x > 0)
        {
            x--;
            WarpTile(tiles[x, y]);
        }
    }

    protected virtual void Input_Right()
    {
        if (selectedTile == lastTile) return;
        
        if (x < gridWidth - 1)
        {
            x++;
            WarpTile(tiles[x, y]);
        }
    }

    protected virtual void Input_Down()
    {
        if (selectedTile == lastTile) return;

        if (y > 0)
        {
            y--;
            WarpTile(tiles[x, y]);
        }
        else
        {
            WarpTile(lastTile);
        }             
    }

    protected virtual void Input_Up()
    {
        if(selectedTile == lastTile)
        {
            x = 1; y = 0;
            WarpTile(tiles[x, y]);
        
            return;
        }
        
        if (y < gridHeight - 1)
        {
            y++;
            WarpTile(tiles[x, y]);
        }
    }
 
}
