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

                piece.GetComponent<Keypad>().SetUp(new Vector3(tileSize, tileSize, tileSize), Keypad.KeyType.Number, i);

                tiles[i, j] = piece;
            }
        }

        lastTile = Instantiate(gridPiece, startPos.transform);
        lastTile.transform.localPosition = new Vector3(0.1f, -0.1f, 0);
        lastTile.GetComponent<Keypad>().OnSelected(normalMaterial);
        lastTile.GetComponent<Keypad>().SetUp(new Vector3(tileSize * 3, tileSize, tileSize) ,Keypad.KeyType.Confirm);
        lastTile.name = "TILE [" + 1 + "," + -1 + "]";

        //Spawn position and colorize the selection box
        x = 0;
        y = 0;
        WarpTile(tiles[x, y]);
    }

    
    private void InputManager()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) Input_Up();
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) Input_Down();
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) Input_Right();
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) Input_Left();
        if (Input.GetKeyDown(KeyCode.Space)) Input_Click();

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
