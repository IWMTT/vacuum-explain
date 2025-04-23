
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using System;
using UnityEngine.UI;

// スタートボタンにアタッチするスクリプト。コントローラスクリプトが割り当てられたオブジェクトをここから制御する
public class VacuumExplainStartSwitch : UdonSharpBehaviour
{
    private float total_time = 40.0f;
    [SerializeField] GameObject bell_object;
    [SerializeField] GameObject ball_object;
    [SerializeField] GameObject chip_bag_object;
    [SerializeField] GameObject pressure_display_object;


    private bool isActivated = false;


    private DateTime _pressed_time;
    private DateTime _updated_time;

    private float _current_factor_lin;
    private float _current_factor_pow;

    void Start()
    {
        _pressed_time = DateTime.Now;
        _updated_time = DateTime.Now;
    }

    public override void Interact()
    {
        // すでにボタンが押されていたら、何もしない
        if (isActivated)
        {

        }

        // 
        else
        {
            isActivated = true;
            _pressed_time = DateTime.Now;
            _updated_time = DateTime.Now;
        }


    }


    void Update()
    {
        if (isActivated) //pressedされてシーケンスに入っていたら実行する。
        {
            _updated_time = DateTime.Now; //経過時間の評価

            //音量、ボールの動き、ポテトの袋の膨らみ具合を決めるパラメータの計算
            _current_factor_lin = GetCurrentCurveValue_lin(_pressed_time, _updated_time, total_time);
            _current_factor_pow = GetCurrentCurveValue_pow(_pressed_time, _updated_time, total_time);


            //パラメータに基づいて袋の膨張・収縮
            chip_bag_object.GetComponent<ChipBagController>().ChangeBagExpand(_current_factor_lin);

            //ベルを鳴らす
            if (bell_object.GetComponent<BellController>().isActivated == true)
            {
                //すでにactivated されていたら、終わるまでまつ
            }
            else
            {
                bell_object.GetComponent<BellController>().Ring(_current_factor_pow);
            }
            


            //ボールのdynamic factorを徐々に下げて、また徐々に上げる
            ball_object.GetComponent<BallController>().ChangeDynamicFactor(_current_factor_lin);

            //7segの気圧ディスプレイの値変更
            pressure_display_object.GetComponent<PressureDisplayController>().PressureValue = _current_factor_pow * 100000.0f;


            //押された時間から、動作終了時間が経っているか判定し、経っていたら動作をオフにする
            if (IsTimePassed(_pressed_time, _updated_time, total_time)) 
            {
                isActivated = false;
            }

        }
        else    // ベルは常に一定間隔で鳴らすため、isActivatedがfalseでも一定の音量でRing()を呼び出し
        {
            if (bell_object.GetComponent<BellController>().isActivated == true)
            {
                // ベルが既に鳴らされている最中だったら何もしない
            }
            else
            {
                // ベルを鳴らす
                bell_object.GetComponent<BellController>().Ring(1.0f);
            }

        }




    }

    // 気圧と音用に、動作パラメータを計算
    private float GetCurrentCurveValue_pow(DateTime input_pressed_time, DateTime input_updated_time, float input_total_sec)
    {
        TimeSpan _dt = input_updated_time - input_pressed_time;
        float _dtsec = (float)_dt.TotalMilliseconds / 1000;
        float _descendTime = 15.0f;// input_total_sec *0.25f;
        float _ascendTime = input_total_sec - 15.0f; // * 0.75f;
        const float minVal = 1e-10f;
        const float maxVal = 1.0f;
        const float logMin = -9.0f; // log10(1e-9)
        const float logMax = 0.0f;  // log10(1.0)


        if (_dtsec <= input_total_sec)
        {
            if (_dtsec <= _descendTime)
            {
                // 対数的に 1.0 → 1e-9 へ減衰
                float t = _dtsec / _descendTime; // 0 → 1
                float logValue = Mathf.Lerp(logMax, logMin, t);
                return Mathf.Pow(10f, logValue);
            }
            else if (_descendTime < _dtsec & _dtsec < _ascendTime)
            {
                return minVal;
            }
            else if (_dtsec >= _ascendTime)
            {
                // 対数的に 1e-9 → 1.0 へ上昇
                float t = (_dtsec - _ascendTime) / (input_total_sec - _ascendTime); // 0 → 1
                float logValue = Mathf.Lerp(logMin, logMax, t);
                return Mathf.Pow(10f, logValue);
            }
            else
            {
                return 1.0f;
            }

        }
        else
        {
            return 1.0f;
        }
    }


    //ボールとポテトの袋用に、動作パラメータを計算
    private float GetCurrentCurveValue_lin(DateTime input_pressed_time, DateTime input_updated_time, float input_total_sec)
    {
        TimeSpan _dt = input_updated_time - input_pressed_time;
        float _dtsec = (float)_dt.TotalMilliseconds / 1000;
        float _descendTime = 15.0f;// input_total_sec *0.25f;
        float _ascendTime = input_total_sec - 15.0f; // * 0.75f;
        if (_dtsec <= input_total_sec)
        {
            if (_dtsec <= _descendTime)
            {
                return 1.0f - (_dtsec / _descendTime);
            }
            else if (_descendTime < _dtsec & _dtsec < _ascendTime)
            {
                return 0.0f;
            }
            else if (_ascendTime <= _dtsec)
            {
                return 0.0f + (_dtsec - _ascendTime) / (input_total_sec - _ascendTime);

            }
            else
            {
                return 1.0f;
            }

        }
        else
        {
            return 1.0f;
        }
    }


    public void ChangeOwner()
    {

        if (!Networking.IsOwner(Networking.LocalPlayer, this.gameObject)) Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
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
