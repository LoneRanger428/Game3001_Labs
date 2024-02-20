using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    public GameObject Tile {  get; private set; }
    public List<PathManager> connection;

    public PathNode(GameObject tile)
    {
        Tile = tile;
        connection.Add(c);
    }
}

[System.Serializable]

public class PathConnection
{
    public float Cost { get; set; }
    public PathNode FromNode { get; private set; }
    public PathNode ToNode { get; private set; }

    public PathConnection(PathNode from, PathNode to, float cost = 1f)
    {
        FromNode = from;
        ToNode = to;
        Cost = cost;
    }
}

public class NodeRecord
{
    public PathNode Node { get; set; }
    public NodeRecord FronRecord { get; set; }
    public PathConnection PathConnection { get; set; }
    public float CostSoFar { get; set; }
}

public class PathManager : MonoBehaviour
{
    
}