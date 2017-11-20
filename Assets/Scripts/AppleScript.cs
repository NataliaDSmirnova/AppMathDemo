using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AppleScript : MonoBehaviour {

    // public variables
    public float rotationSpeed = 6.0f;
    public float maxHeight = 4.0f;
    public float step = 0.1f;
    public Vector3 movement = new Vector3(0.0f, 0.1f, 0.0f);

    public Dictionary<string, int> timeStart = new Dictionary<string, int>();
    public Dictionary<string, int> timeEnd = new Dictionary<string, int>();
    public float[] times =  new float[20];

    // private variables
    private float currentHeight = 0.0f;

    public void Awake ()
    {
        if(timeStart.Count == 0) {
            MonoBehaviour[] tmp_ch_comps = GetComponentsInChildren<MonoBehaviour>();
            for(int i = 0; i < tmp_ch_comps.Length; ++i) {
                timeStart[tmp_ch_comps[i].name] = 2*i;
                timeEnd[tmp_ch_comps[i].name] = 2*i+1;
            }
        }

    }

    public void Rotate()
    {
        transform.RotateAround(Vector3.zero, Vector3.up, 360.0f / rotationSpeed * Time.deltaTime);

    foreach (Transform child in transform)
        print("Foreach loop: " + child);
    }
    public Transform getTransform()
    {
        return transform;
    }

    public void Jump()
    {
        transform.Translate(movement);
        currentHeight += step;
        if (currentHeight >= maxHeight)
        {
            movement *= -1;
            currentHeight = 0.0f;
        }
    }
}

 [CustomEditor(typeof(AppleScript))]
 public class AppleScriptEditor : Editor
 {
    void OnEnable () {
        // Setup the SerializedProperties.
    }

   public override void OnInspectorGUI()
   {
     AppleScript appSc = (AppleScript)target;
    //  serializedObject.Update ();
    //  EditorGUILayout.IntSlider (blobProp, 0, 100, new GUIContent ("blob"));
    //  serializedObject.ApplyModifiedProperties ();

     if(appSc.timeStart.Count == 0) {
        MonoBehaviour[] tmp_ch_comps = appSc.GetComponentsInChildren<MonoBehaviour>();
        for(int i = 0; i < tmp_ch_comps.Length; ++i) {
            appSc.timeStart[tmp_ch_comps[i].name] = 2*i;
            appSc.timeEnd[tmp_ch_comps[i].name] = 2*i+1;
        }
     }

     MonoBehaviour[] ch_comps = appSc.GetComponentsInChildren<MonoBehaviour>();
     if(ch_comps.Length == 0) return;
     EditorGUILayout.LabelField(ch_comps[0].name + " | from " + appSc.times[appSc.timeStart[ch_comps[0].name]] + " to " + appSc.times[appSc.timeEnd[ch_comps[0].name]]);
     EditorGUILayout.MinMaxSlider(ref appSc.times[appSc.timeStart[ch_comps[0].name]], ref  appSc.times[appSc.timeEnd[ch_comps[0].name]], 0, 100);
     EditorGUILayout.LabelField("________________________________________________________________");
     float minT = appSc.times[appSc.timeStart[ch_comps[0].name]], dT = appSc.times[appSc.timeEnd[ch_comps[0].name]] - minT;
     for(int i = 1; i < ch_comps.Length; ++i) {
        EditorGUILayout.LabelField(ch_comps[i].name + " | from " + (minT + appSc.times[appSc.timeStart[ch_comps[i].name]] * dT) + 
        " to " + (minT + appSc.times[appSc.timeEnd[ch_comps[i].name]] * dT));
        EditorGUILayout.MinMaxSlider(ref appSc.times[appSc.timeStart[ch_comps[i].name]], ref  appSc.times[appSc.timeEnd[ch_comps[i].name]], 0, 1);
     }
 
    //  if(appSc.times.Length == 0) {
    //      for(int i = 0; i < tr.childCount; ++i) totalC += tr.GetChild(i).GetComponents<MonoBehaviour>().Length;
    //      appSc.times = new float[totalC * 2];
    //  }
    //  totalC = 0;

    //  Transform tr = appSc.getTransform();
    //  for(int i = 0; i < tr.childCount; ++i) {
    //     EditorGUILayout.LabelField(tr.GetChild(i).name);
    //     MonoBehaviour[] cmps = tr.GetChild(i).GetComponents<MonoBehaviour>();
    //     for(int j = 0; j < cmps.Length; ++j) {
    //         EditorGUILayout.LabelField(cmps[j].name + " | from " + appSc.times[totalC] + " to " + appSc.times[totalC+1]);
    //         EditorGUILayout.MinMaxSlider(ref appSc.times[cmps[j].name], ref appSc.times[cmps[j].name], 1, 100);
    //     }
    //  }
     EditorUtility.SetDirty( target );
   }
 }