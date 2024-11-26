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

    // ���j���[�A�C�e�����쐬
    [MenuItem("VroidRigSetup/AutoSetup")]

    private static void AutoRigSetup()
    {
        if(CanSetupCheacker())
        {
            BeginSetUp(true);
        }
    }

    // ���j���[�A�C�e�����쐬
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
        // �ݒ�����Z�b�g
        rigList = null;

        // �I�����ꂽ�I�u�W�F�N�g���擾
        selectedObject = Selection.activeGameObject;

        if (selectedObject == null)
        {
            Debug.Log("�I�u�W�F�N�g���I������Ă��܂���B");
            return false;
        }

        // �I�����ꂽ�I�u�W�F�N�g��RigBuilderga���A�^�b�`����Ă��邩
        rigBuilder = selectedObject.GetComponent<RigBuilder>();
        if (rigBuilder == null || rigBuilder.layers == null)
        {
            Debug.Log("RigBuilder���Z�b�g�A�b�v����Ă��܂���B");
            return false;
        }

        // �I�����ꂽ�I�u�W�F�N�g�̎q�I�u�W�F�N�g��Rig���A�^�b�`����Ă��邩
        foreach (Rig child in selectedObject.GetComponentsInChildren<Rig>())
        {
            rigList = child.gameObject;
        }
        if (rigList == null)
        {
            Debug.Log("Rig������܂���B");
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
        // �㔼�g��Rig
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

        // �w��Rig
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

        // �����g��Rig
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
        // Hip��Rig��ݒ�
        Transform hipObj = selectedObject.transform.GetChild(0).GetChild(0);
        hipRig.transform.position = hipObj.position;
        PosRigSet(hipObj, hipRig);
        RotRigSet(hipObj, hipRig);

        // Spine��Rig��ݒ�
        Transform spineObj = hipObj.GetChild(0);
        spineRig.transform.SetPositionAndRotation(spineObj.position, spineObj.rotation);
        RotRigSet(spineObj, spineRig);

        // Aim��Rig��ݒ�
        Transform headObj = spineObj.GetChild(0).GetChild(0).GetChild(2).GetChild(0);
        aimRig.transform.position = new Vector3(headObj.position.x, headObj.position.y, headObj.position.z + 1f);
        AimRigSet(headObj, aimRig);

        // Arm��Rig�̐ݒ�
        Transform armObjL = spineObj.GetChild(0).GetChild(0).GetChild(3).GetChild(0);
        armRigL.transform.position = armObjL.GetChild(0).GetChild(0).position;
        armHintL.transform.position = armObjL.GetChild(0).position;
        TwoRigSet(armObjL, armRigL, armHintL);
        Transform armObjR = spineObj.GetChild(0).GetChild(0).GetChild(4).GetChild(0);
        armRigR.transform.position = armObjR.GetChild(0).GetChild(0).position;
        armHintR.transform.position = armObjR.GetChild(0).position;
        TwoRigSet(armObjR, armRigR, armHintR);

        // Finger��Rig�̐ݒ�
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

        // Leg��Rig�̐ݒ�
        Transform legLObj = hipObj.transform.GetChild(1);
        legRigL.transform.position = legLObj.GetChild(0).GetChild(legLObj.transform.GetChild(0).childCount - 1).position;
        legHintL.transform.position = new Vector3(legLObj.GetChild(0).position.x, legLObj.GetChild(0).position.y, legLObj.GetChild(0).position.z + 1f);
        TwoRigSet(legLObj, legRigL, legHintL);
        Transform legRObj = hipObj.transform.GetChild(2);
        legRigR.transform.position = legRObj.GetChild(0).GetChild(legLObj.GetChild(0).childCount - 1).position;
        legHintR.transform.position = new Vector3(legRObj.GetChild(0).position.x, legRObj.GetChild(0).position.y, legRObj.GetChild(0).position.z + 1f);
        TwoRigSet(legRObj, legRigR, legHintR);
    }

    // MultiPositionConstraint�̐ݒ�
    static void PosRigSet(Transform constrainedObj, GameObject sourceObj)
    {
        MultiPositionConstraint rigPos = sourceObj.AddComponent<MultiPositionConstraint>();
        rigPos.data.constrainedObject = constrainedObj;

        var posSourceObjects = rigPos.data.sourceObjects;
        posSourceObjects.Clear();
        posSourceObjects.Add(new WeightedTransform(sourceObj.transform, 1f));
        rigPos.data.sourceObjects = posSourceObjects;
    }

    // MultiRotationConstraint�̐ݒ�
    static void RotRigSet(Transform constrainedObj, GameObject sourceObj)
    {
        MultiRotationConstraint rigRot = sourceObj.AddComponent<MultiRotationConstraint>();
        rigRot.data.constrainedObject = constrainedObj;

        var rotSourceObjects = rigRot.data.sourceObjects;
        rotSourceObjects.Clear();
        rotSourceObjects.Add(new WeightedTransform(sourceObj.transform, 1f));
        rigRot.data.sourceObjects = rotSourceObjects;
    }

    // MultiAimConstraint�̐ݒ�
    static void AimRigSet(Transform constrainedObj, GameObject sourceObj)
    {
        MultiAimConstraint rigAim = sourceObj.AddComponent<MultiAimConstraint>();
        rigAim.data.constrainedObject = constrainedObj;
        var aimSourceObjects = rigAim.data.sourceObjects;
        aimSourceObjects.Clear();
        aimSourceObjects.Add(new WeightedTransform(sourceObj.transform, 1f));
        rigAim.data.sourceObjects = aimSourceObjects;
    }

    // TwoBoneIKConstraint�̐ݒ�
    static void TwoRigSet(Transform constrainedObj, GameObject targetObj, GameObject hintObj)
    {
        TwoBoneIKConstraint rigTwo = targetObj.AddComponent<TwoBoneIKConstraint>();
        rigTwo.data.root = constrainedObj;
        rigTwo.data.mid = constrainedObj.GetChild(0);
        rigTwo.data.tip = constrainedObj.GetChild(0).GetChild(constrainedObj.GetChild(0).childCount - 1);
        rigTwo.data.target = targetObj.transform;
        rigTwo.data.hint = hintObj.transform;
    }

    // �e�w�̐ݒ�
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

    // �e�w�ȊO�̐ݒ�
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

    // Effecter������
    static void SetUpEffecter()
    {
        // �ꎞ�I�ȃL���[�u���쐬���ă��b�V�����擾
        GameObject tempCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Mesh cube = tempCube.GetComponent<MeshFilter>().sharedMesh;

        // �ꎞ�I�ȃL���[�u���폜
        DestroyImmediate(tempCube);

        // �ꎞ�I�ȃX�t�B�A���쐬���ă��b�V�����擾
        GameObject tempSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Mesh sphere = tempSphere.GetComponent<MeshFilter>().sharedMesh;

        // �ꎞ�I�ȃX�t�B�A���폜
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

        // �擾�����L���[�u�̃��b�V����V�����I�u�W�F�N�g�ɐݒ�
        style.shape = mesh;
        rigBuilder.AddEffector(targetRig.transform, style);
    }

}
