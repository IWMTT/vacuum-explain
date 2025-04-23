
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

// ベルの動作をコントロールするためのコントローラスクリプト
// このスクリプトを当てただけではベルは動作せず、外部からRing()関数を呼ぶ必要がある

public class BellController : UdonSharpBehaviour
{
    private Animator anim;
    private AudioSource bell_sound;
    private DateTime _pressed_time;
    private DateTime _updated_time;
    private float _volume;
    public bool isActivated = false; 
    [SerializeField] private float animation_duration = 2.0f;
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        bell_sound = gameObject.GetComponent<AudioSource>();
        _pressed_time = DateTime.Now;
        _updated_time = DateTime.Now;
    }

    void Update()
    {
        if (anim.GetBool("isRinging") == true) //is Ringing がtrueのとき＝ベルのアニメーションが呼ばれている状態
        {
            _updated_time = DateTime.Now; //経過時間の評価
            if(IsTimePassed(_pressed_time, _updated_time, animation_duration)) //押された時間から、アニメーション終了時間が経っているか判定
            {
                anim.SetBool("isRinging",false); //経っていたらfalseにする
                isActivated = false;
            }
        }
        else //is Ringingがfalseのとき
        {
            if (isActivated == true) //Ring()が呼ばれると、このif文内の処理が実行される
            {
                anim.SetBool("isRinging", true);
                _pressed_time = DateTime.Now;
                bell_sound.volume = _volume;
                bell_sound.Play();
            }

        }
    }

    // ベルを鳴らす関数。別のスクリプトやオブジェクトからベルを鳴らせるように、publicに指定
    public void Ring(float input_volume)
    {
        isActivated = true;
        _volume = input_volume;
    }

    // ある時間input_pressed_timeと、別の時間input_updated_timeを比較して、その差が基準時間thres_secを超えているかを判断する関数
    private bool IsTimePassed(DateTime input_pressed_time, DateTime input_updated_time,float thres_sec)
    {
        TimeSpan _dt = input_updated_time - input_pressed_time;
        float _dtsec = (float)_dt.TotalMilliseconds / 1000;
        if (_dtsec > thres_sec)
        {
            return true;

        }
        else
        {
            return false;
        }
    }
}
