
using System;
using UdonSharp;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using VRC.SDKBase;
using VRC.Udon;

public class BallController : UdonSharpBehaviour
{
    // dynamic_factor: ボールの動作量を決める数。小さくなると落ちて動いていない状態になる。
    [SerializeField] private float dynamic_factor = 4.0f;
    private Vector3 _acceleration;
    private Vector3 _velocity;

    private float _y_position;
    private Rigidbody _rigidbody;


    void Start()
    {
        // Start()関数の中で、加速度などを初期化
        _acceleration = Vector3.zero;
        Transform _transform = transform;
        _y_position = _transform.localPosition.y;
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Awake()
    {
        // ボールの剛体コンポーネントを取得
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    void Update() 
    {
        // Update()関数の中で、ボールが風に吹かれてゆらゆら動く様子を表現。※物理現象を忠実に再現しているわけではない。
        // ボールの現在の高さに応じて、[ランダムな加速度追加]/[高い場所にいる場合は自由落下]/[低い場所にいる場合は上昇方向の加速度のみ追加]、を行う
        Transform _transform = transform;
        _y_position = _transform.localPosition.y;

        // ボールの高さが低すぎず高すぎずの位置 ⇒ dynamic_factor * (-0.5 ~ + 10.5) の範囲の加速度に
        if (_y_position < dynamic_factor*0.8 & 0.0f < _y_position)
        {
            _acceleration = new Vector3(0.0f, dynamic_factor * UnityEngine.Random.Range(-0.5f, 10.5f), 0.0f);
        }
        // ボールが底にある ⇒ 加速度は正方向のみにしたいので、dynamic_factor * (0 ~ + 10.5) の範囲の加速度に
        else if (_y_position <= 0.0f)
        {
            //transform.localPosition = new Vector3(0, 0, 0);
            _acceleration = new Vector3(0.0f, dynamic_factor * UnityEngine.Random.Range(0, 10.5f), 0.0f);

        }
        //ボールが高い位置にいる ⇒ 風が届かなくなって自由落下してほしいので、-9.8の加速度に
        else if (dynamic_factor*0.8 <= _y_position)
        {
            _acceleration = new Vector3(0.0f, -9.8f, 0.0f);
        }

        // 上記の場合分けに基づいて決定した加速度を、剛体コンポーネントに与える処理
        _rigidbody.AddForce(_acceleration, ForceMode.Acceleration);

    }

    // 他のスクリプト・オブジェクトから、dynamic_factorを変更するための関数
    public void ChangeDynamicFactor(float input_factor)
    {
        float x = Mathf.Clamp(input_factor, 0.0f, 1.0f);
        dynamic_factor = Mathf.Sin(Mathf.PI/2.0f*x) * 4.0f; // なんでsinにしたかは忘れました。
    }

}
