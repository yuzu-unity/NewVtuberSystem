using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRM;


public class BlendShapeChange : MonoBehaviour
{
    public VRMBlendShapeProxy vRM;

    [Header("リップシンク")]
    OVRLipSyncContextMorphTarget lipsync;

    [Header("表情切り替え")]
    public FaceCtrlPreset faceCtrlPreset;
    private FacePreset[] preset;
    private bool isFaceOn;
    private int presetType;
    private float faceChangeTime;

    [Header("オート瞬き")]

    //リップシンクから自動取得
    private SkinnedMeshRenderer mesh;
    //瞬きのブレンドシェイプ番号
    public int blendShapeNum;
    public BlinkPreset blinkPreset;
    public int randomTimeLow = 4, randomTimehigh = 6;
    private BlinkCurve[] Curve;
    private int blinkNum;
    private float blinkTime;

    void Start()
    {

        lipsync = GetComponent<OVRLipSyncContextMorphTarget>();
        mesh = lipsync.skinnedMeshRenderer;
        preset = faceCtrlPreset.preset;
        Curve = blinkPreset.curve;
    }



    // 表情制御記述
    void Update()
    {

        //表情切り替え
        FaceChange();

        //自動瞬き
        CurveBlink();


    }

    //表情切り替え
    private void FaceChange()
    {
        faceChangeTime -= Time.deltaTime;

        //入力
        for (int i = 0; i < preset.Length; i++)
        {
            if (Input.GetKeyDown(preset[i].keyCode))
            {
                vRM.SetValue(BlendShapePreset.Neutral, 1);
                presetType = i;
                if (preset[i].startChangeTime <= 0)
                {
                    faceChangeTime = 0;
                }
                else
                {
                    faceChangeTime = preset[i].startChangeTime;
                }
                if (!isFaceOn) isFaceOn = true;

                if (!preset[i].lipsync)
                {
                    if (lipsync.enabled)
                        lipsync.enabled = false;
                }

                if (!preset[i].blink)
                {
                    blinkTime = float.MaxValue;
                }

            }
            else if (Input.GetKeyUp(preset[i].keyCode))
            {
                if (preset[i].endChangeTime <= 0)
                {
                    faceChangeTime = 0;
                }
                else if (presetType == i && faceChangeTime > 0)
                {
                    faceChangeTime = preset[i].endChangeTime * (faceChangeTime / preset[i].startChangeTime);
                }
                else
                {
                    faceChangeTime = preset[i].endChangeTime;
                }

                if (isFaceOn) isFaceOn = false;

                if (!preset[i].lipsync)
                {
                    if (!lipsync.enabled)
                        lipsync.enabled = true;
                }

                if (!preset[i].blink)
                {
                    RandomTime();
                }

            }

        }

        //出力結果代入
        if (isFaceOn)
        {
            vRM.SetValue(preset[presetType].blendShapeName, 1 - Mathf.Clamp01(faceChangeTime / preset[presetType].startChangeTime));
        }
        else
        {
            vRM.SetValue(preset[presetType].blendShapeName, Mathf.Clamp01(faceChangeTime / preset[presetType].endChangeTime));
        }

    }


    //カーブで動作
    private void CurveBlink()
    {
        float weight;

        if (blinkTime < 0)
        {
            RandomTime();
        }
        else
        {
            blinkTime -= Time.deltaTime;
        }

        if (blinkTime < 0)
        {
            weight = 0;
        }
        else if (blinkTime < Curve[blinkNum].blinkCurveTime)
        {
            weight = Curve[blinkNum].blinkCurve.Evaluate(1 - blinkTime / Curve[blinkNum].blinkCurveTime);
        }
        else
        {
            weight = 0;
        }

        mesh.SetBlendShapeWeight(blendShapeNum, weight * 100);
    }

    private void RandomTime()
    {
        blinkTime = Random.Range(randomTimeLow, randomTimehigh);
        blinkNum = Random.Range(0, Curve.Length);
    }







}
