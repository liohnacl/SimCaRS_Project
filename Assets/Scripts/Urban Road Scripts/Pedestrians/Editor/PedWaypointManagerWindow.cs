using PlasticPipe.PlasticProtocol.Messages;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PedWaypointManagerWindow : EditorWindow
{
    [MenuItem("Tools/PedWaypoint Editor")]
    public static void Open()
    {
        GetWindow<PedWaypointManagerWindow>();
    }

    public Transform PedwaypointRoot;

    private void OnGUI()
    {
        SerializedObject obj = new SerializedObject(this);

        EditorGUILayout.PropertyField(obj.FindProperty("PedwaypointRoot"));

        if(PedwaypointRoot == null)
        {
            EditorGUILayout.HelpBox("Root transform must be selected. Please assign a root transform.", MessageType.Warning);
        }
        else
        {
            EditorGUILayout.BeginVertical("box");
            DrawButtons();
            EditorGUILayout.EndVertical();
        }
        obj.ApplyModifiedProperties();
    }

    void DrawButtons()
    {
        if (GUILayout.Button("Create Waypoint"))
        {
            CreateWaypoint();
        }
        if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<PedWaypoint>())
        {
            if(GUILayout.Button("Add Branch Waypoint"))
            {
                CreateBranch();
            }
            if (GUILayout.Button("Create Waypoint Before"))
            {
                CreateWaypointBefore();
            }
            if (GUILayout.Button("Create Waypoint After"))
            {
                CreateWaypointAfter();
            }
            if (GUILayout.Button("Remove Waypoint"))
            {
                RemoveWaypoint();
            }
        }
    }

    void CreateWaypoint()
    {
        GameObject PedwaypointObject = new GameObject("Waypoint " + PedwaypointRoot.childCount, typeof(PedWaypoint));
        PedwaypointObject.transform.SetParent(PedwaypointRoot, false);

        PedWaypoint waypoint = PedwaypointObject.GetComponent<PedWaypoint>();
        if (PedwaypointRoot.childCount > 1)
        {
            waypoint.previousWaypoint = PedwaypointRoot.GetChild(PedwaypointRoot.childCount - 2).GetComponent<PedWaypoint>();
            waypoint.previousWaypoint.nextWaypoint = waypoint;

            //Place the waypoint at the last position
            waypoint.transform.position = waypoint.previousWaypoint.transform.position;
            waypoint.transform.forward = waypoint.previousWaypoint.transform.forward;
        }

        Selection.activeGameObject = waypoint.gameObject;
    }

    void CreateWaypointBefore()
    {
        GameObject PedwaypointObject = new GameObject("Waypoint " + PedwaypointRoot.childCount, typeof(PedWaypoint));
        PedwaypointObject.transform.SetParent(PedwaypointRoot, false);

        PedWaypoint newWaypoint = PedwaypointObject.GetComponent<PedWaypoint>();
        
        PedWaypoint selectedWaypoint = Selection.activeGameObject.GetComponent<PedWaypoint>();

        PedwaypointObject.transform.position = selectedWaypoint.transform.position;
        PedwaypointObject.transform.forward = selectedWaypoint.transform.forward;

        if(selectedWaypoint.previousWaypoint != null)
        {
            newWaypoint.previousWaypoint = selectedWaypoint.previousWaypoint;
            selectedWaypoint.previousWaypoint.nextWaypoint = newWaypoint;
        }

        newWaypoint.nextWaypoint = selectedWaypoint;
        selectedWaypoint.previousWaypoint = newWaypoint;
        newWaypoint.transform.SetSiblingIndex(selectedWaypoint.transform.GetSiblingIndex());

        Selection.activeGameObject = newWaypoint.gameObject;

    }

    void CreateWaypointAfter()
    {
        GameObject PedwaypointObject = new GameObject("Waypoint " + PedwaypointRoot.childCount, typeof(PedWaypoint));
        PedwaypointObject.transform.SetParent(PedwaypointRoot, false);

        PedWaypoint newWaypoint = PedwaypointObject.GetComponent<PedWaypoint>();

        PedWaypoint selectedWaypoint = Selection.activeGameObject.GetComponent<PedWaypoint>();

        PedwaypointObject.transform.position = selectedWaypoint.transform.position;
        PedwaypointObject.transform.forward = selectedWaypoint.transform.forward;

        newWaypoint.previousWaypoint = selectedWaypoint;

        if (selectedWaypoint.nextWaypoint != null)
        {
            selectedWaypoint.nextWaypoint.previousWaypoint = newWaypoint;
            newWaypoint.nextWaypoint= selectedWaypoint.nextWaypoint;
        }

        selectedWaypoint.nextWaypoint = newWaypoint;

        newWaypoint.transform.SetSiblingIndex(selectedWaypoint.transform.GetSiblingIndex());

        Selection.activeGameObject = newWaypoint.gameObject;

    }

    void RemoveWaypoint()
    {
        PedWaypoint selectedWaypoint = Selection.activeGameObject.GetComponent<PedWaypoint>();

        if(selectedWaypoint.nextWaypoint != null)
        {
            selectedWaypoint.nextWaypoint.previousWaypoint = selectedWaypoint.previousWaypoint;
        }
        if (selectedWaypoint.previousWaypoint != null)
        {
            selectedWaypoint.previousWaypoint.nextWaypoint = selectedWaypoint.nextWaypoint;
            Selection.activeGameObject = selectedWaypoint.previousWaypoint.gameObject;
        }

        DestroyImmediate(selectedWaypoint.gameObject);
    }

    void CreateBranch()
    {
        GameObject PedwaypointObject = new GameObject("Waypoint " + PedwaypointRoot.childCount, typeof(PedWaypoint));
        PedwaypointObject.transform.SetParent(PedwaypointRoot, false);

        PedWaypoint waypoint = PedwaypointObject.GetComponent<PedWaypoint>();

        PedWaypoint branchedFrom = Selection.activeGameObject.GetComponent<PedWaypoint>();
        branchedFrom.branches.Add(waypoint);

        waypoint.transform.position = branchedFrom.transform.position;
        waypoint.transform.forward = branchedFrom.transform.forward;

        Selection.activeGameObject = waypoint.gameObject;
    }
}

