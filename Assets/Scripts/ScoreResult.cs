using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

public class ScoreResult : MonoBehaviour
{
    public Text ResultText;
    public Image icon;
    public Text CommentText;
    public Text Star;

    public Sprite teacher, kawai, mensetu, izou;

    private string star;

    private AudioSource audiosource;

    private float animationduration = 3.0f;   //アニメーションの実行時間
    public bool finished;
    public string senario;

    //各ステージの満点
    public int max12 = 1000;
    public int clear12 = 500;
    public int max3 = 213400;
    public int clear3 = 60000;
    public int max4 =  2001221580;
    public int clear4 = 2000000000;

    public int max;
    public int standard;
    public int value;

    // Start is called before the first frame update
    public void Start()
    {
        audiosource = GetComponent<AudioSource>();
        ResultText.text = "0";                       //最初は０に設定
        finished = false;
        senario = TitleManager.senario;
    }

    public void CheckValue(int score)
    {
        if (senario == "senario1" || senario == "senario2")
        {
            max = max12;
            standard = clear12;
        }
        else if (senario == "senario3")
        {
            max = max3;
            standard = clear3;
        }
        else
        {
            max = max4;
            standard = clear4;
        }

        if(score >= max)
        {
            value = 2;
        }
        else if(score >= (max/2))
        {
            value = 1;
        }
        else
        {
            value = 0;
        }

    }

    //スコアをカウントアップしていきながら表示、DOCounter(開始値、終了値、遷移時間、カンマの有無).SetEase(Ease.遷移の様子)
    public void CountUp(int score)
    {
        CheckValue(score);
        MakeComment(value);

        if (score > 1000)  //何かしらの理由で値がカンスト値を超えていた場合999999を表示
        {
            int resultscore = score / 10;
            DOVirtual.Int(0, resultscore, animationduration, updatingscore => { ResultText.text = updatingscore.ToString(); }).SetEase(Ease.OutCubic); //outcubicは(t - 1)^3 + 1(0<=t<=1)
        }
        else
        {
            float resultscore = (float)score / 10f;
            DOVirtual.Float(0f, resultscore, animationduration / 2, updatingscore => {ResultText.text = updatingscore.ToString("F1");}).SetEase(Ease.OutCubic);
        }

        if(value==2)
        {
            star = "★★★";
        }
        else if(value==1)
        {
            star = "★★";
        }
        else
        {
            star = "★";
        }
        string beforetext = Star.text;
        Star.DOText(star, animationduration).SetEase(Ease.Linear).OnUpdate(() => {
            string currenttext = Star.text;
            if (beforetext == currenttext)
            {
                return;
            }
            audiosource.Play();
            beforetext = currenttext;
            });
        Debug.Log(Star);

        ResultText.DOColor(new Color(1f, 1f, 0), animationduration).SetEase(Ease.Linear).OnComplete(() => {
            // 次の1.5秒で黒色に変化
            ResultText.DOColor(new Color(1f, 0, 0), animationduration / 2).SetEase(Ease.Linear);
        });

        ResultText.rectTransform.DOScale(new Vector3(1.2f, 1.2f, 1f), animationduration/2).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            // 次の1.5秒で元の大きさに戻す
            ResultText.rectTransform.DOScale(new Vector3(1f, 1f, 1f), animationduration/2).SetEase(Ease.Linear);
        });

        StartCoroutine(TimeCount());

    }

    private IEnumerator TimeCount()
    {
        Debug.Log("count start");
        yield return new WaitForSeconds(animationduration+4.0f);
        finished = true;
    }

    private void MakeComment(int value)
    {
        if(senario==null)
        {
            senario = "senario3";
        }
        string comment;
        Debug.Log(senario);
        if (senario=="senario1")
        {
            SetPosition(new Vector2(0, 300));
            SetScale(new Vector2(3f, 3f));
            icon.sprite = teacher;
            if (value==2)
            {
               TitleManager.cleard1 = true;
               comment = "クラスの人気者間違いなしですね";
            }
            else if (value == 1)
            {
                TitleManager.cleard1 = true;
                comment = "このクラスにもなじめそうですね";
            }
            else
            {
                comment = "いじめられそうですね";
            }
        }
        else if (senario == "senario2")
        {
            SetPosition(new Vector2(0, -550));
            SetScale(new Vector2(5f, 5f));
            icon.sprite = kawai;
            if (value == 2)
            {
                TitleManager.cleard2 = true;
                comment = "スギル君って面白いのね";
            }
            else if (value == 1)
            {
                TitleManager.cleard2 = true;
                comment = "今度大喜利合宿ね";
            }
            else
            {
                comment = "別れようかしら";
            }
        }
        else if (senario == "senario3")
        {
            SetPosition(new Vector2(30, 0));
            SetScale(new Vector2(4f, 4f));
            icon.sprite = mensetu;
            if (value == 2)
            {
                TitleManager.cleard3 = true;
                comment = "優勝おめでとう!!!";
            }
            else if (value == 1)
            {
                TitleManager.cleard3 = true;
                comment = "合格おめでとう!!!";
            }
            else
            {
                comment = "今回はご希望に添えない形になりましたが小諸様の今後のご活躍を\nお祈り申し上げます";
            }
        }
        else if (senario == "senario4")
        {
            SetPosition(new Vector2(-40, -300));
            SetScale(new Vector2(3f, 3f));
            icon.sprite = izou;
            if (value == 2)
            {
                TitleManager.cleard4 = true;
                comment = "まだまだ現世も面白そうだからもう少し長生きしてみるかの";
            }
            else if (value == 1)
            {
                TitleManager.cleard4 = true;
                comment = "おかげで成仏できそうじゃわい";
            }
            else
            {
                comment = "許さない許さない許さない許さない許さない許さない許さない許さない";
            }
        }
        else
        {
            comment = "エラー発生です";
        }

        CommentText.text = comment;
    }



    // スケールを設定する関数
    public void SetScale(Vector2 scale)
    {
        // ImageのRectTransformコンポーネントを取得
        RectTransform rectTransform = icon.GetComponent<RectTransform>();

        // スケールを設定
        rectTransform.localScale = scale;
    }

    public void SetPosition(Vector2 position)
    {
        // ImageのRectTransformコンポーネントを取得
        RectTransform rectTransform = icon.GetComponent<RectTransform>();

        // 位置を設定
        rectTransform.anchoredPosition = position;
    }




}

