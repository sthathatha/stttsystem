# stttsystem

Package Managerから Packages:Unity Registry ＞ Input System をインストールしておく
ManagerSceneにプレハブStttSystemを配置する

各シーンの作成
Scene
├system	←MainScriptBaseまたはGameScriptBaseから派生したスクリプトを持つ
└parent	←オブジェクトの親　これのActiveが制御される
　├……いろいろ

ManagerSceneScript.GetInstance()からマネージャー操作
MainScriptBaseのシーンはLoadMainSceneで呼び出す→現在のMainSceneが閉じられて切り替わる
GameScriptBaseのシーンはStartGameSceneで呼び出す→現在のMainSceneが一時停止して切り替わり、終わると戻って来る
	GameScript内からExitGameを呼び出す

GlobalData.GetSaveData()　セーブデータアクセス

manager.soundManager　SEやボイス再生時にアクセス