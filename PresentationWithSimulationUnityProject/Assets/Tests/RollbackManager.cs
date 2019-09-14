using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollbackManager : MonoBehaviour
{
    [SerializeField] private int m_FrameController;

    [SerializeField] private int m_MaxFrameSimulated;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void RunSystem()
    {
        
    }

    private void NextFrame()
    {
        if (m_FrameController < m_MaxFrameSimulated)
            m_FrameController++;
    }
    private void BackFrame()
    {
        if (m_FrameController > 0)
            m_FrameController--;
    }
    
    private void RollbackToFrame(int pFrame)
    {
        m_FrameController = pFrame;
    }
}
