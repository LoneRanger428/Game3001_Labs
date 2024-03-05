using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PathNode
{
    public GameObject Tile { get; private set; }
    public List<PathConnection> connections;

    public PathNode(GameObject tile) // A constructor
    {
        Tile = tile;
        connections = new List<PathConnection>();
    }

    public void AddConnection(PathConnection c) // ??
    {
        connections.Add(c);
    }
}

[System.Serializable] // we can access
public class PathConnection
{
    public float Cost { get; set; } // This is a new cost from tile to tile, we will use distance in units.
    public PathNode FromNode { get; private set; }
    public PathNode ToNode { get; private set; }

    public PathConnection(PathNode fromNode, PathNode toNode, float cost = 1f)
    {
        FromNode = fromNode;
        ToNode = toNode;
        Cost = cost;
    }
}

public class NodeRecord
{
    public PathNode Node { get; set; }
    public NodeRecord FromRecord { get; set; }
    public PathConnection PathConnection { get; set; }
    public float CostSoFar { get; set; }

    public NodeRecord(PathNode node = null)
    {
        Node = node;
        PathConnection = null;
        FromRecord = null;

    }
}

[System.Serializable]
public class PathManager : MonoBehaviour
{
    public List<NodeRecord> openList;
    public List<NodeRecord> closeList;

    public List<PathConnection> path; // what will be the shortest path.

    public static PathManager Instance { get; private set; } // Static object of the class

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Initialize()
    {
        openList = new List<NodeRecord>();
        closeList = new List<NodeRecord>();
        path = new List<PathConnection>();
    }

    // BiG Algorithm
    public void GetShortestPath(PathNode start, PathNode goal)
    {

        if (path.Count > 0)
        {
            GridManager.Instance.SetTileStatuses();
            path.Clear();
        }

        NodeRecord currentRecord = null;
        openList.Add(new NodeRecord(start));
        while (openList.Count > 0)
        {
            currentRecord = GetSmallestNode();

            if (currentRecord.Node == goal)
            {
                openList.Remove(currentRecord);
                closeList.Add(currentRecord);
                currentRecord.Node.Tile.GetComponent<TileScript>().SetStatus(TileStatus.CLOSED);
                break;
            }

            List<PathConnection> connections = currentRecord.Node.connections;
            for (int i = 0; i < connections.Count; i++)
            {
                PathNode endNode = connections[i].ToNode;
                NodeRecord endNodeRecord;
                float endNodeCost = currentRecord.CostSoFar + connections[i].Cost;

                if (ContainsNode(closeList, endNode)) continue;
                else if (ContainsNode(openList, endNode))
                {
                    endNodeRecord = GetNodeRecord(openList, endNode);
                    if (endNodeRecord.CostSoFar > endNodeCost)
                        continue;
                }
                else
                {
                    endNodeRecord = new NodeRecord();
                    endNodeRecord.Node = endNode;
                }
                endNodeRecord.CostSoFar = endNodeCost;
                endNodeRecord.PathConnection = connections[i];
                endNodeRecord.FromRecord = currentRecord;
                if (!ContainsNode(openList, endNode))
                {
                    openList.Add(endNodeRecord);
                    endNodeRecord.Node.Tile.GetComponentInParent<TileScript>().SetStatus(TileStatus.CLOSED);
                }
            }
            openList.Remove(currentRecord);
            closeList.Add(currentRecord);
            currentRecord.Node.Tile.GetComponent<TileScript>().SetStatus(TileStatus.CLOSED);

        }
        if (currentRecord != null)
        {
            if (currentRecord.Node != null)
            {
                Debug.Log("couldnot find path to goal");
            }
            else
            {
                Debug.Log("found path to goal");
                while (currentRecord.Node != start)
                {
                    path.Add(currentRecord.PathConnection);
                    currentRecord.Node.Tile.GetComponent<TileScript>().SetStatus(TileStatus.PATH);
                    currentRecord = currentRecord.FromRecord;
                }
                path.Reverse();
            }
            openList.Clear();
            closeList.Clear();
        }
    }

    //Some utility methods

    public NodeRecord GetSmallestNode()
    {
        NodeRecord smallestNode = openList[0];

        //Iterate through the rest if the noderecords in the list
        for (int i = 1; i < openList.Count; i++)
        {
            //if the current NodeRecord has a smaller CostSoFar than the smallestNode, update smallestNode with current NodeRecord
            if (openList[i].CostSoFar < smallestNode.CostSoFar)
            {
                smallestNode = openList[i];
            }
            else if (openList[i].CostSoFar == smallestNode.CostSoFar)
            {
                smallestNode = (Random.value < 0.5 ? openList[i] : smallestNode);
            }
        }
        return smallestNode;
    }

    public bool ContainsNode(List<NodeRecord> list, PathNode node)
    {
        foreach (NodeRecord record in list)
        {
            if (record.Node == node) return true;
        }
        return false;
    }

    public NodeRecord GetNodeRecord(List<NodeRecord> list, PathNode node)
    {
        foreach (NodeRecord record in list)
        {
            if (record.Node == node) return record;
        }
        return null;
    }
}