using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    [SerializeField] private GameObject[] neighbourTiles;
    [SerializeField] private Color original;

    public TilePanel tilePanel;
    public TileStatus status = TileStatus.UNVISITED;
    public float cost = 999.9f;
    
    public void SetNeighbourTile(int index, GameObject tile)
    {
        neighbourTiles[index] = tile;
    }

    public void SetColor(Color color, bool newColor = false)
    {
        if (!newColor)
            original = color;
        gameObject.GetComponent<SpriteRenderer>().color = color;
    }

    internal void SetStatus(TileStatus stat)
    {
        status = stat;
        switch (stat)
        {
            case TileStatus.UNVISITED:
                gameObject.GetComponent<SpriteRenderer>().color = original;
                tilePanel.statusText.text = "U";
                break;

            case TileStatus.OPEN:
                tilePanel.statusText.text = "O";
                break;

            case TileStatus.CLOSED:
                tilePanel.statusText.text = "C";
                break;

            case TileStatus.IMPASSABLE:
                gameObject.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0f, 0f, 0.5f);
                tilePanel.statusText.text = "I";
                break;

            case TileStatus.GOAL:
                gameObject.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0f, 0.5f);
                tilePanel.statusText.text = "G";
                break;

            case TileStatus.START:
                gameObject.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0f, 0.5f);
                tilePanel.statusText.text = "S";
                break;
        }
    }
    //public void ToggleImpassable(bool impass = true)
    //{
    //    if (impass)
    //    {
    //        status = TileStatus.IMPASSABLE;
    //        gameObject.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0f, 0f, 0.5f);
    //    }
    //    else
    //    {
    //        status = TileStatus.UNVISITED;
    //        gameObject.GetComponent<SpriteRenderer>().color = original;
    //    }
    //}
}
