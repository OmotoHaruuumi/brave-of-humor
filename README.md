# BRAVE OF HUMOR

> ボケ力で切り拓け。日本一の笑いを目指す、選択肢型コメディアドベンチャー。

> **Note:** このリポジトリは、学生時代に制作したゲーム *BRAVE OF HUMOR* のコードを整理・公開するために作成したものです。

![Unity](https://img.shields.io/badge/Unity-2020.3%20LTS-black?logo=unity)
![C#](https://img.shields.io/badge/C%23-.NET-239120?logo=csharp)
![DOTween](https://img.shields.io/badge/DOTween-Animation-ff6b35)
![TextMeshPro](https://img.shields.io/badge/TextMeshPro-Text%20Rendering-blue)
![Platform](https://img.shields.io/badge/Platform-Windows%20%2F%20Mac-lightgrey)

---

## 概要

日常生活の中でタイミングを見極めてボケよう!

タイミングやボケを間違えると大変な雰囲気に．．．

失敗しても何度でもリトライ可能！でもただのリトライじゃねぇぞ

ネタの使用を快諾してくださった大石浩二先生の作品"トマトイプーのリコピン"。
少年ジャンプ+にて2025年3月10日に完結しました！ぜひ単行本をご購入してください！!
---

## スクリーンショット

> ※ スクリーンショットは `docs/screenshots/` フォルダに追加予定

| タイトル画面 | ゲーム画面（選択肢） | リザルト画面 |
|:---:|:---:|:---:|
| *(coming soon)* | *(coming soon)* | *(coming soon)* |

---

## ゲームはこちらから遊べます！！

https://unityroom.com/games/legend_of_sugiru

---

## ゲームの特徴

- **5つのステージ**：転校、遊園地、就活、葬儀など多彩なシチュエーション
- **スコアシステム**：ボケの正確さとタイミングによるスコアで笑いを数値化
- **分岐シナリオ**：選択肢ごとに異なる展開・リアクションが発生
- **ボケフラグ制御**：特定の場面でのみ「一発逆転ボケ」が選択可能になる仕組み

---

## 技術スタック

| カテゴリ | 技術 |
|---|---|
| ゲームエンジン | Unity 2020.3.14f1 LTS |
| スクリプト | C# (.NET) |
| アニメーション | DOTween (Demigiant) |
| テキスト描画 | TextMeshPro |
| シェーダー | HLSL / CG (Shapes2D) |
| シナリオ形式 | 独自テキストマークアップ（後述） |

---

## アーキテクチャと工夫点

### テキストファイル駆動のシナリオシステム

**開発チームに非エンジニアが含まれていたため、コードを触らずにゲームシナリオを編集できる仕組みが必要でした。**

そこで、シナリオをすべてプレーンテキストファイル（`.txt`）で記述する独自の軽量マークアップ形式を設計しました。
シナリオライターは以下のような直感的な記法でシーンの全要素（台詞・キャラクター・BGM・背景・選択肢・スコア）を制御できます。

```
#scene=001
#bgm=転校
#background=normal_school
#speaker=先生
#chara=teacher1
{笑いの本場から引っ越してきた！彼の口からボケの大洪水！！}
#bokeflag=true
#options={
{仕事ができない父だとは思っていましたが群馬に飛ばされるとは思いませんでした,003}
{出身は小諸過で名前は大阪です,004}
{転校で、、人生好転！,005}
}
#next=006
```

| 記法 | 意味 |
|---|---|
| `#scene=ID` | シーン番号の定義 |
| `#speaker=名前` | 発話キャラクター |
| `#chara=スプライト名` | 表示するキャラクター画像 |
| `#background=背景名` | 背景画像の切り替え |
| `#bgm=BGM名` | BGM の再生 / 停止 |
| `#se=SE名` | 効果音の再生 |
| `#bokeflag=true/false` | ボケ選択肢の有効・無効 |
| `#options={...}` | 選択肢と遷移先シーンの定義 |
| `{台詞テキスト}` | 表示する台詞本文 |
| `#score=数値` | スコアの加算・減算 |
| `#next=シーンID` | 次のシーンへの遷移 |
| `#result` | リザルト画面へ移行 |

このフォーマットは `SceneParser.cs` が行単位でパースし、`TextParser.cs` が台詞をキャラクターごとに1文字ずつ逐次表示するタイプライターアニメーションを担当します。

---

### システム構成図

```
senario1.txt
    │
    ▼
SceneParser         ← # コマンドを行単位でパース
    │
    ├─ SceneController  ← シーン遷移・状態管理
    │       ├─ BGMManager     ← BGM 再生制御
    │       ├─ SEManager      ← SE 再生制御
    │       ├─ BGManager      ← 背景切り替え
    │       ├─ CharacterManager ← キャラクタースプライト表示
    │       └─ GUIManager     ← UI・選択肢表示
    │
    └─ TextParser       ← { } 内テキストを Queue<char> に変換
            └─ Coroutine でタイプライター表示
```

---

### その他の設計上の工夫

- **Manager パターン**：音声・背景・キャラクター・UI をそれぞれ独立した Manager クラスに分離し、シナリオ側から差し替えやすい構造に
- **DOTween 活用**：キャラクター出現アニメーションや画面演出を宣言的に記述し、コード量を削減
- **CoroutineManager**：Unity の Coroutine をラップして、タイプライター表示・BGM フェード等の非同期処理を一元管理

---

## ディレクトリ構成

```
brave of humor/
├── Assets/
│   ├── Scripts/          # ゲームロジック (C#)
│   │   ├── SceneParser.cs    # シナリオ解析
│   │   ├── TextParser.cs     # テキスト逐次表示
│   │   ├── GameManager.cs    # ゲーム全体管理
│   │   ├── BGMManager.cs     # BGM 制御
│   │   ├── CharacterManager.cs
│   │   └── ...
│   ├── Resources/
│   │   ├── Texts/        # シナリオテキストファイル (senario1.txt 〜 senario5.txt)
│   │   ├── Images/       # キャラクタースプライト
│   │   ├── Background/   # 背景画像
│   │   ├── BGM/          # BGM (WAV/MP3)
│   │   └── AudioClips/   # SE (MP3)
│   ├── Scenes/           # Unity シーンファイル
│   └── Plugins/
│       └── Demigiant/DOTween/
├── ProjectSettings/
└── Packages/
```

---

## 開発背景

学生時代にチームで制作したゲームです。チームメンバーに非エンジニア（シナリオライター）が含まれており、**「コードを書けなくてもゲームの内容を更新できる」** ことが最大の要件でした。
この制約から生まれたテキストファイル駆動のシナリオ設計は、データとロジックの分離という設計原則を自然な形で体現しており、後のソフトウェア開発でも活きている考え方です。

---

## ライセンス

本リポジトリのコードは [MIT License](LICENSE) のもとで公開しています。
ゲーム内のアセット（画像・音声・テキスト）の著作権は制作チームに帰属します。





