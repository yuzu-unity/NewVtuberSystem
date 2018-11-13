using UnityEngine;
using VRM;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;



[CreateAssetMenu]
public class FaceCtrlPreset : ScriptableObject
{
    [Header("表情のプリセットを作成します")]
    public FacePreset[] preset;




#if UNITY_EDITOR
   
    private void OnValidate()
    {
        if (preset == null) return;

        for (int i = 0; i < preset.Length; i++)
        {
            if (preset[i].blendShapePreset != BlendShapePreset.Unknown)
            {
                preset[i].blendShapeName = preset[i].blendShapePreset.ToString();
            }


            if (!preset[i].test)
            {
                preset[i].startCurve = AnimationCurve.Linear(0, 0, 1, 1);
                preset[i].endCurve = AnimationCurve.Linear(0, 0, 1, 1);
                preset[i].test = true;
                preset[i].blendShapeName = preset[i].blendShapePreset.ToString();
            }
        }
    }
#endif



}



[System.Serializable]
public class FacePreset
{
    [Header("Unknownなら入力")]
    public string blendShapeName;
    public BlendShapePreset blendShapePreset;
    [Header("割当キー")]
    public KeyCode keyCode;
    [Header("瞬き併用かどうか")]
    public bool blink=false;
    [Header("リップシンク併用かどうか")]
    public bool lipsync = false;
    [Header("イーズインの調整")]
    public float startChangeTime=0;
    public AnimationCurve startCurve=AnimationCurve.Linear(0, 0, 1, 1);
    [Header("イーズアウトの調整")]
    public float endChangeTime=0;
    public AnimationCurve endCurve= AnimationCurve.Linear(0, 0, 1, 1);

#if UNITY_EDITOR
    [HideInInspector]
    public bool test;
#endif

}

