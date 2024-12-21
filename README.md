# stttsystem  

## 準備
Package Managerから Packages:Unity Registry ＞ Input System をインストールしておく  

Package Managerで ＋ ＞ from git Uri ＞ https://github.com/sthathatha/stttsystem.git?path=/Assets/stttsystem
ManagerSceneにプレハブStttSystemを配置する

## 各シーンの作成
Scene  
├system	←MainScriptBase または GameScriptBase または SubScriptBase から派生したスクリプトを持つ  
└parent	←オブジェクトの親　これのActiveが制御される  
　├……いろいろ

## スクリプト使い方
ManagerSceneScript.GetInstance()からマネージャー操作  

### MainScriptBase
 LoadMainSceneで呼び出す→現在のMainSceneが閉じられて切り替わる  

### SubScriptBase
 LoadSubSceneで呼び出す→他のシーンに影響を与えない  
  InitParamをオーバーライドしてパラメータ受取
  自身のDeleteSceneで消える  
  またはManagerのDeleteSubSceneAllで全消し  

### GameScriptBase
 未実装
 StartGameSceneで呼び出す→現在のMainSceneが一時停止して切り替わり、終わると戻って来る  
  自身のExitGameを呼び出すとMainSceneに戻る

GlobalData.GetSaveData()　セーブデータアクセス

manager.soundManager　SEやボイス再生時にアクセス
