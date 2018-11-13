using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;



[CreateAssetMenu]
public class BlinkPreset : ScriptableObject
{
    [Header("瞬きのパターンをランダムに選択します")]
    public BlinkCurve[] curve;
    

#if UNITY_EDITOR
   
    private void OnValidate()
    {
        if (curve == null) return;

        for (int i = 0; i < curve.Length; i++)
        {
           

            if (!curve[i].test)
            {
                curve[i].blinkCurveTime = 0.35f;
                curve[i].blinkCurve = AnimationCurve.Linear(0, 0, 1, 1);
                curve[i].test = true;


            }
        }
    }
#endif
}



[System.Serializable]
public class BlinkCurve
{
    [Header("カーブ")]
    public float blinkCurveTime = 0.35f;

    public AnimationCurve blinkCurve;

#if UNITY_EDITOR
    [HideInInspector]
    public bool test;
#endif

}

