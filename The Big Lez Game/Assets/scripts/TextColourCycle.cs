using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextColourCycle : MonoBehaviour
{

    [SerializeField]
    Outline[] m_outlines;
    [SerializeField]
    Image[] m_images;

    [SerializeField]
    AnimationCurve m_curve;
    [SerializeField]
    Color m_colourA, m_colourB;
    [SerializeField]
    float m_speed;
    float m_time;
    // Update is called once per frame
    void Update()
    {
        m_time += Time.deltaTime * m_speed;
        foreach (Outline o in m_outlines)
        {
            o.effectColor = Color.Lerp(m_colourA, m_colourB, m_curve.Evaluate(m_time));
        } 
        foreach (Image i in m_images)
        {
            i.color = Color.Lerp(m_colourA, m_colourB, m_curve.Evaluate(m_time));
        } 
    }
}
