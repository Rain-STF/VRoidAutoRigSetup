# VRoidAutoRigSetup
UnityにインポートしたVroidのモデルに対し、Unityの機能であるAnimationRiggingのセットアップを行うスクリプトです。

・使い方

Unityにスクリプトファイルをインポートしていただくと、「VRoidRigSetup」のタブがメインウィンドウの上部に表示されます。

VRoidで作成したモデルをインポートし、ヒエラルキーに置きます。（Vrmファイルを読み込むためのモジュールが別途必要）
Unityの機能であるAnimationRiggingをインポートします。
インポートしたVRoidのモデルを選択した状態で「AnimationRigging」のタブの中にある「RigSetup」をクリックする。
選択中のモデルにRigBuilderのコンポーネントがアタッチされ、3Dモデルの子オブジェクトに「Rig1」が生成されたことを確認。
モデルを選択した状態で、「VRoidRigSetup」のタブから「AutoSetup」をクリックすると、エフェクターが付いた状態でRigの設定が完了します。
セットされるRigによって、身体の位置と角度、腰の角度、両手足、指をIKで動かすことができます。

モデルを選択した状態で、「VRoidRigSetup」のタブから「DeleteRig」をクリックすると、セットアップしたRigが削除されます。

モデルを選択した状態で、「VRoidRigSetup」のタブから「NoEffecterSetup」をクリックすると、エフェクターのないRig設定されます。
（Rigを使用したいが、Effecterが邪魔になる場合）


・動作環境

Unity2022.3
Vrm1.0
一般的なボーン構造、親子関係のある3Dモデルであれば使用可能
