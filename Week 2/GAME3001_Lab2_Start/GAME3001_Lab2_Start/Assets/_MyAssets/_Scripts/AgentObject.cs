using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentObject : MonoBehaviour
{
    //A parent class of all the agent objects
    [SerializeField] Transform m_target;

    public Vector3 TargetPosition
    {
        get { return m_target.position; }
        set { m_target.position = value;}
    }
    //Note I only want the above property here, so the class cannot be abstract

    // Start is called before the first frame update
    public void Start()
    {
        Debug.Log("Starting Agent...");
        TargetPosition = m_target.transform.position;
    }
}
