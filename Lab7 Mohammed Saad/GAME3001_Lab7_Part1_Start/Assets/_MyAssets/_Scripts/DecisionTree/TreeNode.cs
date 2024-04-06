using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TreeNodeType
{
    LEFT_TREE_NODE, //If condition is false, we use left tree node
    RIGHT_TREE_NODE //If condition is true, we use right tree node
};

public abstract class TreeNode
{ 
    // We keep recording the connections of node,  SEE Lab7 tree diagram
    public string name;
    public TreeNode left = null;
    public TreeNode right = null;
    public TreeNode parent = null;
    public bool isLeaf = false; // If its a leaf, that means there is no longer condition. It is hanging without sub branches.
}
