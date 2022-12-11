using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerController))]
[CanEditMultipleObjects]
public class PlayerInspectorEditor : Editor
{
    // Properties for PlayerController
    SerializedProperty canFall;
    SerializedProperty isJumping;
    SerializedProperty dashReady;
    SerializedProperty canOnlyDashOnGround;
    SerializedProperty overrideDashTime;
    SerializedProperty dashTime;
    SerializedProperty dashCooldown;
    SerializedProperty stopHorizontalMomenumOnRelease;
    bool showProperties;

    // Parameters for PlayerController
    SerializedProperty velocity;
    SerializedProperty gravity;
    SerializedProperty maxFallSpeed;
    SerializedProperty maxHorizontalSpeed;
    SerializedProperty walkAcceleration;
    SerializedProperty groundDecelerationTime;
    SerializedProperty airDecelerationTime;
    SerializedProperty horizontalInput;
    SerializedProperty jumpHeight;
    SerializedProperty timeToMaxHeight;
    SerializedProperty jumpVelocity;
    SerializedProperty dashVelocity;
    SerializedProperty knockbackScalar;
    SerializedProperty knockbackDuration;
    bool showPhysicsValues;

    private void OnEnable()
    {
        // Properties for PlayerController
        canFall = serializedObject.FindProperty("canFall");
        isJumping = serializedObject.FindProperty("isJumping");
        dashReady = serializedObject.FindProperty("dashReady");
        canOnlyDashOnGround = serializedObject.FindProperty("canOnlyDashOnGround");
        overrideDashTime = serializedObject.FindProperty("overrideDashTime");
        dashTime = serializedObject.FindProperty("dashTime");
        dashCooldown = serializedObject.FindProperty("dashCooldown");
        stopHorizontalMomenumOnRelease = serializedObject.FindProperty("stopHorizontalMomentumOnRelease");

        // Parameters for PlayerController
        velocity = serializedObject.FindProperty("velocity");
        gravity = serializedObject.FindProperty("gravity");
        maxFallSpeed = serializedObject.FindProperty("maxFallSpeed");
        maxHorizontalSpeed = serializedObject.FindProperty("maxHorizontalSpeed");
        walkAcceleration = serializedObject.FindProperty("walkAcceleration");
        groundDecelerationTime = serializedObject.FindProperty("groundDecelerationTime");
        airDecelerationTime = serializedObject.FindProperty("airDecelerationTime");
        horizontalInput = serializedObject.FindProperty("horizontalInput");
        jumpHeight = serializedObject.FindProperty("jumpHeight");
        timeToMaxHeight = serializedObject.FindProperty("timeToMaxHeight");
        jumpVelocity = serializedObject.FindProperty("jumpVelocity");
        dashVelocity = serializedObject.FindProperty("dashVelocity");
        knockbackScalar = serializedObject.FindProperty("knockbackScalar");
        knockbackDuration = serializedObject.FindProperty("knockbackDuration");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Organizing the properties for PlayerController
        showProperties = EditorGUILayout.Foldout(showProperties, "Properties & Toggles");
        if (showProperties)
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(isJumping);
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.PropertyField(canFall);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(canOnlyDashOnGround, GUILayout.ExpandWidth(true));
            EditorGUILayout.PropertyField(dashReady, GUILayout.ExpandWidth(true));
            EditorGUILayout.PropertyField(overrideDashTime, GUILayout.ExpandWidth(true));
            if (overrideDashTime.boolValue)
            {
                EditorGUILayout.PropertyField(dashTime, GUILayout.ExpandWidth(true));
            }
            EditorGUILayout.PropertyField(dashCooldown, GUILayout.ExpandWidth(true));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(stopHorizontalMomenumOnRelease, GUILayout.ExpandWidth(true));
        }

        // Organizing the parameters for PlayerController
        showPhysicsValues = EditorGUILayout.Foldout(showPhysicsValues, "Physics Values");
        if (showPhysicsValues)
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(velocity);
            EditorGUILayout.PropertyField(gravity);
            EditorGUILayout.PropertyField(jumpVelocity);
            EditorGUILayout.PropertyField(horizontalInput);
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(maxFallSpeed);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(maxHorizontalSpeed);
            EditorGUILayout.PropertyField(walkAcceleration);
            if (!stopHorizontalMomenumOnRelease.boolValue)
            {
                EditorGUILayout.PropertyField(groundDecelerationTime);
                EditorGUILayout.PropertyField(airDecelerationTime);
            }
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(jumpHeight);
            EditorGUILayout.PropertyField(timeToMaxHeight);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(dashVelocity);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(knockbackScalar);
            EditorGUILayout.PropertyField(knockbackDuration);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
