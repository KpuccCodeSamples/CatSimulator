using UnityEngine;
using System;

public enum OnCatImpactType
{
    Kick = 0,
    Play = 1,
    Feed = 2,
    Stroke = 3
}

public class Controller : MonoBehaviour
{
    public Cat m_MyPrettyCat;
    public View m_CatInfo;

    public static event Action<Cat, OnCatImpactType> OnCatImpactPerformed;

    //////////////
    private void Start()
    {
        m_CatInfo.InitView(m_MyPrettyCat.Mood);
    }

    //////////////
    public void PerformOnCatImpact(int impactTypeIdex)
    {
        OnCatImpactType impactType = (OnCatImpactType)impactTypeIdex;

        OnCatImpactPerformed?.Invoke(m_MyPrettyCat, impactType);
    }

    //////////////
    public void OnClickExit()
    {
        Application.Quit();
    }
}
