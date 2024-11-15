# stttsystem  

## 準備
Package Managerから Packages:Unity Registry ＞ Input System をインストールしておく  

Package Managerで ＋ ＞ from git Uri ＞ https://github.com/sthathatha/stttsystem.git?path=/Assets/stttsystem
ManagerSceneにプレハブStttSystemを配置する

## 各シーンの作成
Scene  
├system	←MainScriptBaseまたはGameScriptBaseから派生したスクリプトを持つ  
└parent	←オブジェクトの親　これのActiveが制御される  
　├……いろいろ

## スクリプト使い方
ManagerSceneScript.GetInstance()からマネージャー操作  
MainScriptBaseのシーンはLoadMainSceneで呼び出す→現在のMainSceneが閉じられて切り替わる  
GameScriptBaseのシーンはStartGameSceneで呼び出す→現在のMainSceneが一時停止して切り替わり、終わると戻って来る  
	GameScript内からExitGameを呼び出す

GlobalData.GetSaveData()　セーブデータアクセス

manager.soundManager　SEやボイス再生時にアクセス
