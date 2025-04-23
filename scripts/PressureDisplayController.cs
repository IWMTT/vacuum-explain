
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using static UnityEditor.PlayerSettings;

//7segモデルを基に、表示される数字を変えたりするためのスクリプト。ちょっと無理やりな実装。


public class PressureDisplayController : UdonSharpBehaviour
{


    [SerializeField] private GameObject SevenSeg100000;
    [SerializeField] private GameObject SevenSeg10000;
    [SerializeField] private GameObject SevenSeg1000;
    [SerializeField] private GameObject SevenSeg100;
    [SerializeField] private GameObject SevenSeg10;
    [SerializeField] private GameObject SevenSeg1;
    [SerializeField] private GameObject SevenSeg01;
    [SerializeField] private GameObject SevenSeg001;
    [SerializeField] private GameObject SevenSeg0001;
    [SerializeField] private GameObject SevenSeg00001;
    [SerializeField] private GameObject SevenSeg000001;

    [SerializeField] public float PressureValue;


    void Start()
    {

    }
    void Update()
    {
        //小数点の位置の設定
        ToggleDp(ref SevenSeg100000, false);
        ToggleDp(ref SevenSeg10000, false);
        ToggleDp(ref SevenSeg1000, false);
        ToggleDp(ref SevenSeg100, false);
        ToggleDp(ref SevenSeg10, false);
        ToggleDp(ref SevenSeg1, true);
        ToggleDp(ref SevenSeg01, false);
        ToggleDp(ref SevenSeg001, false);
        ToggleDp(ref SevenSeg0001, false);
        ToggleDp(ref SevenSeg00001, false);
        ToggleDp(ref SevenSeg000001, false);

        //PressureValueを、読み込んで、各桁に分解する
        string floatString = PressureValue.ToString("F5");

        // char型の配列に変換
        char[] charArray = floatString.ToCharArray();

        // char型の配列をint型の配列に変換
        int[] intArray = new int[charArray.Length];

        for (int i = 0; i < charArray.Length; i++)
        {
            if (char.IsDigit(charArray[i]))
            {
                intArray[i] = (int)char.GetNumericValue(charArray[i]);
            }
            else
            {
                //小数点「.」だけ、整数に変換できないので、代わりに-1を代入しておく（特に意味は無い状態）
                intArray[i] = -1;
            }
        }
        int numberOfDigits = intArray.Length;

        //桁数に応じて、数字の表示を消したい箇所が出てきたり、intArray上の小数点（-1が代わりに代入されている）のインデックス位置が異なるため、力技で場合分け
        switch (numberOfDigits)
        {
            case 12: //少数点を含んで12 = 100000.00000の状態,小数点は[6]
                ChangeDisplayValue(ref SevenSeg100000, intArray[0]);
                ChangeDisplayValue(ref SevenSeg10000, intArray[1]);
                ChangeDisplayValue(ref SevenSeg1000, intArray[2]);
                ChangeDisplayValue(ref SevenSeg100, intArray[3]);
                ChangeDisplayValue(ref SevenSeg10, intArray[4]);
                ChangeDisplayValue(ref SevenSeg1, intArray[5]);
                ChangeDisplayValue(ref SevenSeg01, intArray[7]);
                ChangeDisplayValue(ref SevenSeg001, intArray[8]);
                ChangeDisplayValue(ref SevenSeg0001, intArray[9]);
                ChangeDisplayValue(ref SevenSeg00001, intArray[10]);
                ChangeDisplayValue(ref SevenSeg000001, intArray[11]);
                break;
            case 11://少数点を含んで11 = 90000.00000みたいな状態,小数点は[5]
                ChangeDisplayValue(ref SevenSeg100000, -1);
                ChangeDisplayValue(ref SevenSeg10000, intArray[0]);
                ChangeDisplayValue(ref SevenSeg1000, intArray[1]);
                ChangeDisplayValue(ref SevenSeg100, intArray[2]);
                ChangeDisplayValue(ref SevenSeg10, intArray[3]);
                ChangeDisplayValue(ref SevenSeg1, intArray[4]);
                ChangeDisplayValue(ref SevenSeg01, intArray[6]);
                ChangeDisplayValue(ref SevenSeg001, intArray[7]);
                ChangeDisplayValue(ref SevenSeg0001, intArray[8]);
                ChangeDisplayValue(ref SevenSeg00001, intArray[9]);
                ChangeDisplayValue(ref SevenSeg000001, intArray[10]);
                break;
            case 10://少数点を含んで10 = 9000.00000みたいな状態,小数点は[4]
                ChangeDisplayValue(ref SevenSeg100000, -1);
                ChangeDisplayValue(ref SevenSeg10000, -1);
                ChangeDisplayValue(ref SevenSeg1000, intArray[0]);
                ChangeDisplayValue(ref SevenSeg100, intArray[1]);
                ChangeDisplayValue(ref SevenSeg10, intArray[2]);
                ChangeDisplayValue(ref SevenSeg1, intArray[3]);
                ChangeDisplayValue(ref SevenSeg01, intArray[5]);
                ChangeDisplayValue(ref SevenSeg001, intArray[6]);
                ChangeDisplayValue(ref SevenSeg0001, intArray[7]);
                ChangeDisplayValue(ref SevenSeg00001, intArray[8]);
                ChangeDisplayValue(ref SevenSeg000001, intArray[9]);
                break;
            case 9://少数点を含んで9 = 900.00000みたいな状態,小数点は[3]
                ChangeDisplayValue(ref SevenSeg100000, -1);
                ChangeDisplayValue(ref SevenSeg10000, -1);
                ChangeDisplayValue(ref SevenSeg1000, -1);
                ChangeDisplayValue(ref SevenSeg100, intArray[0]);
                ChangeDisplayValue(ref SevenSeg10, intArray[1]);
                ChangeDisplayValue(ref SevenSeg1, intArray[2]);
                ChangeDisplayValue(ref SevenSeg01, intArray[4]);
                ChangeDisplayValue(ref SevenSeg001, intArray[5]);
                ChangeDisplayValue(ref SevenSeg0001, intArray[6]);
                ChangeDisplayValue(ref SevenSeg00001, intArray[7]);
                ChangeDisplayValue(ref SevenSeg000001, intArray[8]);
                break;
            case 8://少数点を含んで8 = 90.00000みたいな状態,小数点は[2]
                ChangeDisplayValue(ref SevenSeg100000, -1);
                ChangeDisplayValue(ref SevenSeg10000, -1);
                ChangeDisplayValue(ref SevenSeg1000, -1);
                ChangeDisplayValue(ref SevenSeg100, -1);
                ChangeDisplayValue(ref SevenSeg10, intArray[0]);
                ChangeDisplayValue(ref SevenSeg1, intArray[1]);
                ChangeDisplayValue(ref SevenSeg01, intArray[3]);
                ChangeDisplayValue(ref SevenSeg001, intArray[4]);
                ChangeDisplayValue(ref SevenSeg0001, intArray[5]);
                ChangeDisplayValue(ref SevenSeg00001, intArray[6]);
                ChangeDisplayValue(ref SevenSeg000001, intArray[7]);
                break;
            case 7://少数点を含んで7 = 9.00000みたいな状態,小数点は[1]
                ChangeDisplayValue(ref SevenSeg100000, -1);
                ChangeDisplayValue(ref SevenSeg10000, -1);
                ChangeDisplayValue(ref SevenSeg1000, -1);
                ChangeDisplayValue(ref SevenSeg100, -1);
                ChangeDisplayValue(ref SevenSeg10, -1);
                ChangeDisplayValue(ref SevenSeg1, intArray[0]);
                ChangeDisplayValue(ref SevenSeg01, intArray[2]);
                ChangeDisplayValue(ref SevenSeg001, intArray[3]);
                ChangeDisplayValue(ref SevenSeg0001, intArray[4]);
                ChangeDisplayValue(ref SevenSeg00001, intArray[5]);
                ChangeDisplayValue(ref SevenSeg000001, intArray[6]);
                break;

        }


    }

    //ある7segモデルで、小数点を表示・非表示にする処理
    private void ToggleDp(ref GameObject obj, bool tf)
    {
        Transform seg__dp_on = obj.transform.Find("7seg__dp_on");
        Transform seg__dp_off = obj.transform.Find("7seg__dp_off");
        if (tf == true)
        {
            seg__dp_off.gameObject.SetActive(false);
            seg__dp_on.gameObject.SetActive(true);
        }
        if (tf == false)
        {
            seg__dp_off.gameObject.SetActive(true);
            seg__dp_on.gameObject.SetActive(false);
        }
    }

    //ある1つの7segモデルに表示される数字を、整数valの値に沿って変える関数
    private void ChangeDisplayValue(ref GameObject obj, int val)
    {
        // 7segモデル内の_on/_offのモデルを取得
        Transform seg_a_off = obj.transform.Find("7seg_a_off");
        Transform seg_b_off = obj.transform.Find("7seg_b_off");
        Transform seg_c_off = obj.transform.Find("7seg_c_off");
        Transform seg_d_off = obj.transform.Find("7seg_d_off");
        Transform seg_e_off = obj.transform.Find("7seg_e_off");
        Transform seg_f_off = obj.transform.Find("7seg_f_off");
        Transform seg_g_off = obj.transform.Find("7seg_g_off");

        Transform seg_a_on = obj.transform.Find("7seg_a_on");
        Transform seg_b_on = obj.transform.Find("7seg_b_on");
        Transform seg_c_on = obj.transform.Find("7seg_c_on");
        Transform seg_d_on = obj.transform.Find("7seg_d_on");
        Transform seg_e_on = obj.transform.Find("7seg_e_on");
        Transform seg_f_on = obj.transform.Find("7seg_f_on");
        Transform seg_g_on = obj.transform.Find("7seg_g_on");

        // 以下、valの値に沿ってモデルの表示・非表示を切り替えて、表示される数字を変える処理
        if (val == -1) //全オフ用、
        {
            seg_a_off.gameObject.SetActive(true);
            seg_b_off.gameObject.SetActive(true);
            seg_c_off.gameObject.SetActive(true);
            seg_d_off.gameObject.SetActive(true);
            seg_e_off.gameObject.SetActive(true);
            seg_f_off.gameObject.SetActive(true);
            seg_g_off.gameObject.SetActive(true);

            seg_a_on.gameObject.SetActive(false);
            seg_b_on.gameObject.SetActive(false);
            seg_c_on.gameObject.SetActive(false);
            seg_d_on.gameObject.SetActive(false);
            seg_e_on.gameObject.SetActive(false);
            seg_f_on.gameObject.SetActive(false);
            seg_g_on.gameObject.SetActive(false);
        }


        if (val == 0)//gだけオフ
        {
            seg_a_off.gameObject.SetActive(false);
            seg_b_off.gameObject.SetActive(false);
            seg_c_off.gameObject.SetActive(false);
            seg_d_off.gameObject.SetActive(false);
            seg_e_off.gameObject.SetActive(false);
            seg_f_off.gameObject.SetActive(false);
            seg_g_off.gameObject.SetActive(true);

            seg_a_on.gameObject.SetActive(true);
            seg_b_on.gameObject.SetActive(true);
            seg_c_on.gameObject.SetActive(true);
            seg_d_on.gameObject.SetActive(true);
            seg_e_on.gameObject.SetActive(true);
            seg_f_on.gameObject.SetActive(true);
            seg_g_on.gameObject.SetActive(false);
        }

        if (val == 1) //bcだけオン
        {
            seg_a_off.gameObject.SetActive(true);
            seg_b_off.gameObject.SetActive(false);
            seg_c_off.gameObject.SetActive(false);
            seg_d_off.gameObject.SetActive(true);
            seg_e_off.gameObject.SetActive(true);
            seg_f_off.gameObject.SetActive(true);
            seg_g_off.gameObject.SetActive(true);

            seg_a_on.gameObject.SetActive(false);
            seg_b_on.gameObject.SetActive(true);
            seg_c_on.gameObject.SetActive(true);
            seg_d_on.gameObject.SetActive(false);
            seg_e_on.gameObject.SetActive(false);
            seg_f_on.gameObject.SetActive(false);
            seg_g_on.gameObject.SetActive(false);
        }
        if (val == 2) //abgedだけオン
        {
            seg_a_off.gameObject.SetActive(false);
            seg_b_off.gameObject.SetActive(false);
            seg_c_off.gameObject.SetActive(true);
            seg_d_off.gameObject.SetActive(false);
            seg_e_off.gameObject.SetActive(false);
            seg_f_off.gameObject.SetActive(true);
            seg_g_off.gameObject.SetActive(false);

            seg_a_on.gameObject.SetActive(true);
            seg_b_on.gameObject.SetActive(true);
            seg_c_on.gameObject.SetActive(false);
            seg_d_on.gameObject.SetActive(true);
            seg_e_on.gameObject.SetActive(true);
            seg_f_on.gameObject.SetActive(false);
            seg_g_on.gameObject.SetActive(true);
        }
        if (val == 3)//abcdg
        {
            seg_a_off.gameObject.SetActive(false);
            seg_b_off.gameObject.SetActive(false);
            seg_c_off.gameObject.SetActive(false);
            seg_d_off.gameObject.SetActive(false);
            seg_e_off.gameObject.SetActive(true);
            seg_f_off.gameObject.SetActive(true);
            seg_g_off.gameObject.SetActive(false);

            seg_a_on.gameObject.SetActive(true);
            seg_b_on.gameObject.SetActive(true);
            seg_c_on.gameObject.SetActive(true);
            seg_d_on.gameObject.SetActive(true);
            seg_e_on.gameObject.SetActive(false);
            seg_f_on.gameObject.SetActive(false);
            seg_g_on.gameObject.SetActive(true);
        }
        if (val == 4)//bcfg
        {
            seg_a_off.gameObject.SetActive(true);
            seg_b_off.gameObject.SetActive(false);
            seg_c_off.gameObject.SetActive(false);
            seg_d_off.gameObject.SetActive(true);
            seg_e_off.gameObject.SetActive(true);
            seg_f_off.gameObject.SetActive(false);
            seg_g_off.gameObject.SetActive(false);

            seg_a_on.gameObject.SetActive(false);
            seg_b_on.gameObject.SetActive(true);
            seg_c_on.gameObject.SetActive(true);
            seg_d_on.gameObject.SetActive(false);
            seg_e_on.gameObject.SetActive(false);
            seg_f_on.gameObject.SetActive(true);
            seg_g_on.gameObject.SetActive(true);
        }
        if (val == 5)//acdfg
        {
            seg_a_off.gameObject.SetActive(false);
            seg_b_off.gameObject.SetActive(true);
            seg_c_off.gameObject.SetActive(false);
            seg_d_off.gameObject.SetActive(false);
            seg_e_off.gameObject.SetActive(true);
            seg_f_off.gameObject.SetActive(false);
            seg_g_off.gameObject.SetActive(false);

            seg_a_on.gameObject.SetActive(true);
            seg_b_on.gameObject.SetActive(false);
            seg_c_on.gameObject.SetActive(true);
            seg_d_on.gameObject.SetActive(true);
            seg_e_on.gameObject.SetActive(false);
            seg_f_on.gameObject.SetActive(true);
            seg_g_on.gameObject.SetActive(true);
        }
        if (val == 6) //acdefg
        {
            seg_a_off.gameObject.SetActive(false);
            seg_b_off.gameObject.SetActive(true);
            seg_c_off.gameObject.SetActive(false);
            seg_d_off.gameObject.SetActive(false);
            seg_e_off.gameObject.SetActive(false);
            seg_f_off.gameObject.SetActive(false);
            seg_g_off.gameObject.SetActive(false);

            seg_a_on.gameObject.SetActive(true);
            seg_b_on.gameObject.SetActive(false);
            seg_c_on.gameObject.SetActive(true);
            seg_d_on.gameObject.SetActive(true);
            seg_e_on.gameObject.SetActive(true);
            seg_f_on.gameObject.SetActive(true);
            seg_g_on.gameObject.SetActive(true);
        }
        if (val == 7) //abc
        {
            seg_a_off.gameObject.SetActive(false);
            seg_b_off.gameObject.SetActive(false);
            seg_c_off.gameObject.SetActive(false);
            seg_d_off.gameObject.SetActive(true);
            seg_e_off.gameObject.SetActive(true);
            seg_f_off.gameObject.SetActive(true);
            seg_g_off.gameObject.SetActive(true);

            seg_a_on.gameObject.SetActive(true);
            seg_b_on.gameObject.SetActive(true);
            seg_c_on.gameObject.SetActive(true);
            seg_d_on.gameObject.SetActive(false);
            seg_e_on.gameObject.SetActive(false);
            seg_f_on.gameObject.SetActive(false);
            seg_g_on.gameObject.SetActive(false);
        }
        if (val == 8) //全部つく
        {
            seg_a_off.gameObject.SetActive(false);
            seg_b_off.gameObject.SetActive(false);
            seg_c_off.gameObject.SetActive(false);
            seg_d_off.gameObject.SetActive(false);
            seg_e_off.gameObject.SetActive(false);
            seg_f_off.gameObject.SetActive(false);
            seg_g_off.gameObject.SetActive(false);

            seg_a_on.gameObject.SetActive(true);
            seg_b_on.gameObject.SetActive(true);
            seg_c_on.gameObject.SetActive(true);
            seg_d_on.gameObject.SetActive(true);
            seg_e_on.gameObject.SetActive(true);
            seg_f_on.gameObject.SetActive(true);
            seg_g_on.gameObject.SetActive(true);
        }
        if (val == 9) //eだけオフ
        {
            seg_a_off.gameObject.SetActive(false);
            seg_b_off.gameObject.SetActive(false);
            seg_c_off.gameObject.SetActive(false);
            seg_d_off.gameObject.SetActive(false);
            seg_e_off.gameObject.SetActive(true);
            seg_f_off.gameObject.SetActive(false);
            seg_g_off.gameObject.SetActive(false);

            seg_a_on.gameObject.SetActive(true);
            seg_b_on.gameObject.SetActive(true);
            seg_c_on.gameObject.SetActive(true);
            seg_d_on.gameObject.SetActive(true);
            seg_e_on.gameObject.SetActive(false);
            seg_f_on.gameObject.SetActive(true);
            seg_g_on.gameObject.SetActive(true);
        }



    }

}
