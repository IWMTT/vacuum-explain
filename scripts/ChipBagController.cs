
using Newtonsoft.Json.Linq;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

// ポテトの袋モデルに入っている、SkinnedMeshRenderer(≒Blenderで設定したシェイプキー)をコントロールするためのスクリプト

public class ChipBagController : UdonSharpBehaviour
{
    [SerializeField] private GameObject bag_object;
    [SerializeField] private float factor = 100.0f;
    private SkinnedMeshRenderer skinnedMeshRenderer;

    // BlendShapeのインデックス
    private int blendShapeIndex = 0;
    



    void Start()
    {
        // オブジェクトのSkinnedMeshRendererを取得
        skinnedMeshRenderer = bag_object.GetComponent<SkinnedMeshRenderer>();
    }

    void Update()
    {
        // BlendShapeの値を設定
        skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, factor);
    }

    public void ChangeBagExpand(float input_value)
    {
        private float clamped_input_value;
        clamped_input_value = Mathf.Clamp(input_value, 0.0f, 1.0f);
        factor =  clamped_input_value * 100.0f;
    }

}
