using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class VroidAutoRigSetup : MonoBehaviour
{
    #region
    static GameObject selectedObject;
    static RigBuilder rigBuilder;
    static GameObject rigList;
    static GameObject hipRig;
    static GameObject spineRig;
    static GameObject aimRig;
    static GameObject armRigL;
    static GameObject armHintL;
    static GameObject armRigR;
    static GameObject armHintR;
    static GameObject oyaRigL;
    static GameObject hitoRigL;
    static GameObject nakaRigL;
    static GameObject kusuriRigL;
    static GameObject koRigL;
    static GameObject oyaRigR;
    static GameObject hitoRigR;
    static GameObject nakaRigR;
    static GameObject kusuriRigR;
    static GameObject koRigR;
    static GameObject legRigL;
    static GameObject legHintL;
    static GameObject legRigR;
    static GameObject legHintR;
    #endregion

    // メニューアイテムを作成
    [MenuItem("VroidRigSetup/AutoSetup")]

    private static void AutoRigSetup()
    {
        if(CanSetupCheacker())
        {
            BeginSetUp(true);
        }
    }

    // メニューアイテムを作成
    [MenuItem("VroidRigSetup/NoEffecterSetup")]

    private static void NoEffecterSetup()
    {
        if (CanSetupCheacker())
        {
            BeginSetUp(false);
        }
    }

    [MenuItem("VroidRigSetup/DeleteRig")]

    private static void DeleteRig()
    {
        if (CanSetupCheacker())
        {
            foreach(Transform child in rigList.transform)
            {
                DestroyImmediate(child.gameObject);
            }
        }
    }

    static bool CanSetupCheacker()
    {
        // 設定をリセット
        rigList = null;

        // 選択されたオブジェクトを取得
        selectedObject = Selection.activeGameObject;

        if (selectedObject == null)
        {
            Debug.Log("オブジェクトが選択されていません。");
            return false;
        }

        // 選択されたオブジェクトにRigBuildergaがアタッチされているか
        rigBuilder = selectedObject.GetComponent<RigBuilder>();
        if (rigBuilder == null || rigBuilder.layers == null)
        {
            Debug.Log("RigBuilderがセットアップされていません。");
            return false;
        }

        // 選択されたオブジェクトの子オブジェクトにRigがアタッチされているか
        foreach (Rig child in selectedObject.GetComponentsInChildren<Rig>())
        {
            rigList = child.gameObject;
        }
        if (rigList == null)
        {
            Debug.Log("Rigがありません。");
            return false;
        }
        return true;
    }

    static async void BeginSetUp(bool effecterSet)
    {
        
        await SetObj();
        SetIkRig();
        if (effecterSet)
        {
            SetUpEffecter();
        }
    }

    static async Task SetObj()
    {
        // 上半身のRig
        hipRig = new GameObject("HipRig");
        hipRig.transform.parent = rigList.transform;

        spineRig = new GameObject("SpineRig");
        spineRig.transform.parent = hipRig.transform;
        aimRig = new GameObject("AimRig");
        aimRig.transform.parent = spineRig.transform;
        GameObject arm = new GameObject("Arm");
        arm.transform.parent = spineRig.transform;
        armRigL = new GameObject("ArmRigL");
        armRigL.transform.parent = arm.transform;
        armHintL = new GameObject("ArmHintL");
        armHintL.transform.parent = arm.transform;
        armRigR = new GameObject("ArmRigR");
        armRigR.transform.parent = arm.transform;
        armHintR = new GameObject("ArmHintR");
        armHintR.transform.parent = arm.transform;

        // 指のRig
        oyaRigL = new GameObject("OyaL");
        oyaRigL.transform.parent = armRigL.transform;
        hitoRigL = new GameObject("HitoL");
        hitoRigL.transform.parent = armRigL.transform;
        nakaRigL = new GameObject("NakaL");
        nakaRigL.transform.parent = armRigL.transform;
        kusuriRigL = new GameObject("KusuriL");
        kusuriRigL.transform.parent = armRigL.transform;
        koRigL = new GameObject("KoL");
        koRigL.transform.parent = armRigL.transform;

        oyaRigR = new GameObject("OyaR");
        oyaRigR.transform.parent = armRigR.transform;
        hitoRigR = new GameObject("HitoR");
        hitoRigR.transform.parent = armRigR.transform;
        nakaRigR = new GameObject("NakaR");
        nakaRigR.transform.parent = armRigR.transform;
        kusuriRigR = new GameObject("KusuriR");
        kusuriRigR.transform.parent = armRigR.transform;
        koRigR = new GameObject("KoR");
        koRigR.transform.parent = armRigR.transform;

        // 下半身のRig
        GameObject leg = new GameObject("Leg");
        leg.transform.parent = hipRig.transform;
        legRigL = new GameObject("LegRigL");
        legRigL.transform.parent = leg.transform;
        legHintL = new GameObject("LegHintL");
        legHintL.transform.parent = leg.transform;
        legRigR = new GameObject("LegRigR");
        legRigR.transform.parent = leg.transform;
        legHintR = new GameObject("LegHintR");
        legHintR.transform.parent = leg.transform;
        await Task.Delay(1);
    }

    static void SetIkRig()
    {
        // HipのRigを設定
        Transform hipObj = selectedObject.transform.GetChild(0).GetChild(0);
        hipRig.transform.position = hipObj.position;
        PosRigSet(hipObj, hipRig);
        RotRigSet(hipObj, hipRig);

        // SpineのRigを設定
        Transform spineObj = hipObj.GetChild(0).GetChild(0).GetChild(0);
        spineRig.transform.position = spineObj.position;
        SpineRigSet(hipObj.GetChild(0), spineRig);

        // AimのRigを設定
        Transform headObj = spineObj.GetChild(2).GetChild(0);
        aimRig.transform.position = new Vector3(headObj.position.x, headObj.position.y, headObj.position.z + 1f);
        AimRigSet(headObj, aimRig);

        // ArmのRigの設定
        Transform armObjL = spineObj.GetChild(3).GetChild(0);
        armRigL.transform.position = armObjL.GetChild(0).GetChild(0).position;
        armHintL.transform.position = armObjL.GetChild(0).position;
        TwoRigSet(armObjL, armRigL, armHintL);
        Transform armObjR = spineObj.GetChild(4).GetChild(0);
        armRigR.transform.position = armObjR.GetChild(0).GetChild(0).position;
        armHintR.transform.position = armObjR.GetChild(0).position;
        TwoRigSet(armObjR, armRigR, armHintR);

        // FingerのRigの設定
        Transform oyaObjL = armObjL.GetChild(0).GetChild(0).GetChild(4);
        oyaRigL.transform.position = oyaObjL.GetChild(0).GetChild(0).position;
        OyaRigSet(oyaObjL, oyaRigL);
        Transform hitoObjL = armObjL.GetChild(0).GetChild(0).GetChild(0);
        hitoRigL.transform.position = hitoObjL.GetChild(0).GetChild(0).position;
        KoRigSet(hitoObjL, hitoRigL);
        Transform nakaObjL = armObjL.GetChild(0).GetChild(0).GetChild(2);
        nakaRigL.transform.position = nakaObjL.GetChild(0).GetChild(0).position;
        KoRigSet(nakaObjL, nakaRigL);
        Transform kusuriObjL = armObjL.GetChild(0).GetChild(0).GetChild(3);
        kusuriRigL.transform.position = kusuriObjL.GetChild(0).GetChild(0).position;
        KoRigSet(kusuriObjL, kusuriRigL);
        Transform koObjL = armObjL.GetChild(0).GetChild(0).GetChild(1);
        koRigL.transform.position = koObjL.GetChild(0).GetChild(0).position;
        KoRigSet(koObjL, koRigL);

        Transform oyaObjR = armObjR.GetChild(0).GetChild(0).GetChild(4);
        oyaRigR.transform.position = oyaObjR.GetChild(0).GetChild(0).position;
        OyaRigSet(oyaObjR, oyaRigR);
        Transform hitoObjR = armObjR.GetChild(0).GetChild(0).GetChild(0);
        hitoRigR.transform.position = hitoObjR.GetChild(0).GetChild(0).position;
        KoRigSet(hitoObjR, hitoRigR);
        Transform nakaObjR = armObjR.GetChild(0).GetChild(0).GetChild(2);
        nakaRigR.transform.position = nakaObjR.GetChild(0).GetChild(0).position;
        KoRigSet(nakaObjR, nakaRigR);
        Transform kusuriObjR = armObjR.GetChild(0).GetChild(0).GetChild(3);
        kusuriRigR.transform.position = kusuriObjR.GetChild(0).GetChild(0).position;
        KoRigSet(kusuriObjR, kusuriRigR);
        Transform koObjR = armObjR.GetChild(0).GetChild(0).GetChild(1);
        koRigR.transform.position = koObjR.GetChild(0).GetChild(0).position;
        KoRigSet(koObjR, koRigR);

        // LegのRigの設定
        Transform legLObj = hipObj.transform.GetChild(1);
        legRigL.transform.position = legLObj.GetChild(0).GetChild(legLObj.transform.GetChild(0).childCount - 1).position;
        legHintL.transform.position = new Vector3(legLObj.GetChild(0).position.x, legLObj.GetChild(0).position.y, legLObj.GetChild(0).position.z + 1f);
        TwoRigSet(legLObj, legRigL, legHintL);
        Transform legRObj = hipObj.transform.GetChild(2);
        legRigR.transform.position = legRObj.GetChild(0).GetChild(legLObj.GetChild(0).childCount - 1).position;
        legHintR.transform.position = new Vector3(legRObj.GetChild(0).position.x, legRObj.GetChild(0).position.y, legRObj.GetChild(0).position.z + 1f);
        TwoRigSet(legRObj, legRigR, legHintR);
    }

    // MultiPositionConstraintの設定
    static void PosRigSet(Transform constrainedObj, GameObject sourceObj)
    {
        MultiPositionConstraint rigPos = sourceObj.AddComponent<MultiPositionConstraint>();
        rigPos.data.constrainedObject = constrainedObj;

        var posSourceObjects = rigPos.data.sourceObjects;
        posSourceObjects.Clear();
        posSourceObjects.Add(new WeightedTransform(sourceObj.transform, 1f));
        rigPos.data.sourceObjects = posSourceObjects;
    }

    // MultiRotationConstraintの設定
    static void RotRigSet(Transform constrainedObj, GameObject sourceObj)
    {
        MultiRotationConstraint rigRot = sourceObj.AddComponent<MultiRotationConstraint>();
        rigRot.data.constrainedObject = constrainedObj;

        var rotSourceObjects = rigRot.data.sourceObjects;
        rotSourceObjects.Clear();
        rotSourceObjects.Add(new WeightedTransform(sourceObj.transform, 1f));
        rigRot.data.sourceObjects = rotSourceObjects;
    }

    // 腰の設定
    static void SpineRigSet(Transform constrainedObj, GameObject sourceObj)
    {
        TwistCorrection rigTwi = sourceObj.AddComponent<TwistCorrection>();
        rigTwi.data.sourceObject = sourceObj.transform;

        var rigTwistNodes = rigTwi.data.twistNodes;
        rigTwistNodes.Clear();
        rigTwistNodes.Add(new WeightedTransform(constrainedObj, 1f));
        rigTwistNodes.Add(new WeightedTransform(constrainedObj.GetChild(0), 1f));
        rigTwistNodes.Add(new WeightedTransform(constrainedObj.GetChild(0).GetChild(0), 1f));
        rigTwi.data.twistNodes = rigTwistNodes;
        rigTwi.data.twistAxis = TwistCorrectionData.Axis.Y;

        ChainIKConstraint rigChain = sourceObj.AddComponent<ChainIKConstraint>();
        rigChain.data.root = constrainedObj;
        rigChain.data.tip = constrainedObj.GetChild(0).GetChild(0);

        rigChain.data.target = sourceObj.transform;
    }

    // MultiAimConstraintの設定
    static void AimRigSet(Transform constrainedObj, GameObject sourceObj)
    {
        MultiAimConstraint rigAim = sourceObj.AddComponent<MultiAimConstraint>();
        rigAim.data.constrainedObject = constrainedObj;
        var aimSourceObjects = rigAim.data.sourceObjects;
        aimSourceObjects.Clear();
        aimSourceObjects.Add(new WeightedTransform(sourceObj.transform, 1f));
        rigAim.data.sourceObjects = aimSourceObjects;
    }

    // TwoBoneIKConstraintの設定
    static void TwoRigSet(Transform constrainedObj, GameObject targetObj, GameObject hintObj)
    {
        TwoBoneIKConstraint rigTwo = targetObj.AddComponent<TwoBoneIKConstraint>();
        rigTwo.data.root = constrainedObj;
        rigTwo.data.mid = constrainedObj.GetChild(0);
        rigTwo.data.tip = constrainedObj.GetChild(0).GetChild(constrainedObj.GetChild(0).childCount - 1);
        rigTwo.data.target = targetObj.transform;
        rigTwo.data.hint = hintObj.transform;
    }

    // 親指の設定
    static void OyaRigSet(Transform constrainedObj, GameObject sourceObj)
    {
        TwistCorrection rigTwi = sourceObj.AddComponent<TwistCorrection>();
        rigTwi.data.sourceObject = sourceObj.transform;

        var rigTwistNodes = rigTwi.data.twistNodes;
        rigTwistNodes.Clear();
        rigTwistNodes.Add(new WeightedTransform(constrainedObj.GetChild(0), 1f));
        rigTwistNodes.Add(new WeightedTransform(constrainedObj.GetChild(0).GetChild(0), 1f));
        rigTwi.data.twistNodes = rigTwistNodes;
        rigTwi.data.twistAxis = TwistCorrectionData.Axis.Z;
    }

    // 親指以外の設定
    static void KoRigSet(Transform constrainedObj, GameObject sourceObj)
    {
        TwistCorrection rigTwi = sourceObj.AddComponent<TwistCorrection>();
        rigTwi.data.sourceObject = sourceObj.transform;

        var rigTwistNodes = rigTwi.data.twistNodes;
        rigTwistNodes.Clear();
        rigTwistNodes.Add(new WeightedTransform(constrainedObj, 1f));
        rigTwistNodes.Add(new WeightedTransform(constrainedObj.GetChild(0), 1f));
        rigTwistNodes.Add(new WeightedTransform(constrainedObj.GetChild(0).GetChild(0), 1f));
        rigTwi.data.twistNodes = rigTwistNodes;
        rigTwi.data.twistAxis = TwistCorrectionData.Axis.Z;

        MultiRotationConstraint rigRot = sourceObj.AddComponent<MultiRotationConstraint>();
        rigRot.data.constrainedObject = constrainedObj;

        var rotSourceObjects = rigRot.data.sourceObjects;
        rotSourceObjects.Clear();
        rotSourceObjects.Add(new WeightedTransform(sourceObj.transform, 1f));
        rigRot.data.sourceObjects = rotSourceObjects;
        rigRot.data.constrainedZAxis = false;
    }

    // Effecterをつける
    static void SetUpEffecter()
    {
        // 一時的なキューブを作成してメッシュを取得
        GameObject tempCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Mesh cube = tempCube.GetComponent<MeshFilter>().sharedMesh;

        // 一時的なキューブを削除
        DestroyImmediate(tempCube);

        // 一時的なスフィアを作成してメッシュを取得
        GameObject tempSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Mesh sphere = tempSphere.GetComponent<MeshFilter>().sharedMesh;

        // 一時的なスフィアを削除
        DestroyImmediate(tempSphere);

        ShowEffecter(hipRig, sphere, Color.green);
        ShowEffecter(spineRig, sphere, Color.green);
        ShowEffecter(aimRig, sphere, Color.green);
        ShowEffecter(armRigL, sphere, Color.red);
        ShowEffecter(armHintL, cube, Color.red);
        ShowEffecter(armRigR, sphere, Color.blue);
        ShowEffecter(armHintR, cube, Color.blue);
        ShowEffecter(legRigL, sphere, Color.red);
        ShowEffecter(legHintL, cube, Color.red);
        ShowEffecter(legRigR, sphere, Color.blue);
        ShowEffecter(legHintR, cube, Color.blue);
    }

    static void ShowEffecter(GameObject targetRig, Mesh mesh, Color objColor)
    {
        RigEffectorData.Style style = new RigEffectorData.Style();
        style.color = new Color(objColor.r, objColor.g, objColor.b, 0.5f);
        style.size = 0.1f;

        // 取得したキューブのメッシュを新しいオブジェクトに設定
        style.shape = mesh;
        rigBuilder.AddEffector(targetRig.transform, style);
    }

}
