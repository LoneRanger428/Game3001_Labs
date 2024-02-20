    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class PathNode
    {
        public GameObject Tile {  get; private set; }
        public List<PathConnection> connection;

        public PathNode(GameObject tile)
        {
            Tile = tile;
            connection = new List<PathConnection>();
        }

        public void AddConnection(PathConnection c)
        {
            connection.Add(c);
        }
    }

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

    [System.Serializable]
    public class PathManager : MonoBehaviour
    {
        public List<NodeRecord> openlist;
        public List<NodeRecord> closeList;

        public List<PathConnection> path; // What will be the shortest path

        public static PathManager Instance { get; private set; } // Static object of the Class

        private void Awake()
        {
            if (Instance == null) // If the object/instance doesn;t exist yet
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Initialize()
        {
            openlist = new List<NodeRecord>();
            closeList = new List<NodeRecord>();
            path = new List<PathConnection>();
        }

        public void GetShortestPath(PathNode start, PathNode end)
        {

        }

        public NodeRecord GetSmallestNode()
        {
            NodeRecord smallestNode = openlist[0];
            for(int i = 1; i < openlist.Count; i++)
            {
                if (openlist[i].CostSoFar < smallestNode.CostSoFar)
                {
                    smallestNode = openlist[i];
                }
                // If they're the same, fllip a coin
                else if (openlist[i].CostSoFar == smallestNode.CostSoFar)
                {
                    smallestNode = (Random.value < 0.5f ? openlist[i] : smallestNode);
                }
            }
            return smallestNode; //Return the ModeRecord with the smallest CostSoFar
        }

        public bool ConstainsNode(List<NodeRecord> list, PathNode node)
        {
            foreach(NodeRecord record in list)
            {
                if (record.Node == node) return true;
            }
            return false;
        }

        public NodeRecord GetNodeRecord(List<NodeRecord> list, PathNode node)
        {
            foreach(NodeRecord record in list)
            {
                if(record.Node == node) return record;
            }
            return null;
        }
    }