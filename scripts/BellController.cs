using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEditor;

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
        //is Ringing がtrueのとき＝ベルのアニメーションが呼ばれている状態
        if (anim.GetBool("isRinging") == true)
        {
            //経過時間の評価
            _updated_time = DateTime.Now;
            //押された時間から、アニメーション終了時間が経っているか判定
            if (IsTimePassed(_pressed_time, _updated_time, animation_duration))
            {
                //経っていたらfalseにする
                anim.SetBool("isRinging", false);
                isActivated = false;
            }
        }
        else //is Ringingがfalseのとき
        {

            if (isActivated == true)
            {
                anim.SetBool("isRinging", true);
                _pressed_time = DateTime.Now;
                bell_sound.volume = _volume;
                bell_sound.Play();
            }

        }

    }

    public void Ring(float input_volume)
    {
        isActivated = true;
        _volume = input_volume;
    }

    private bool IsTimePassed(DateTime input_pressed_time, DateTime input_updated_time, float thres_sec)
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

[CustomEditor(typeof(BellController))] 
public class BellControllertEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Ring"))
        {
            BellController bellController = (BellController)target;
            bellController.Ring(1.0f);
        }


    }
}