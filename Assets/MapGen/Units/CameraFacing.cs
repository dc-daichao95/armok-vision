﻿//	CameraFacing.cs 
//	original by Neil Carter (NCarter)
//	modified by Hayden Scott-Baron (Dock) - http://starfruitgames.com
//  allows specified orientation axis


using UnityEngine;
using System.Collections;

public class CameraFacing : MonoBehaviour
{
    Camera referenceCamera;

    public enum Axis { up, down, left, right, forward, back };
    public bool reverseFace = false;
    public bool stayVertical = false;
    public Axis axis = Axis.up;

    // return a direction based upon chosen axis
    public Vector3 GetAxis(Axis refAxis)
    {
        switch (refAxis)
        {
            case Axis.down:
                return Vector3.down;
            case Axis.forward:
                return Vector3.forward;
            case Axis.back:
                return Vector3.back;
            case Axis.left:
                return Vector3.left;
            case Axis.right:
                return Vector3.right;
        }

        // default is Vector3.up
        return Vector3.up;
    }

    void Awake()
    {
        // if no camera referenced, grab the main camera
        if (!referenceCamera)
            referenceCamera = Camera.main;
    }

    void Update()
    {
        //We need a different type of facing between VR and 2D
        if (SteamVR.active)
        {
            // rotates the object relative to the camera
            Vector3 targetPos = transform.position + referenceCamera.transform.rotation * (reverseFace ? Vector3.forward : Vector3.back);
            Vector3 targetOrientation = referenceCamera.transform.rotation * GetAxis(axis);
            if (stayVertical)
                targetPos = new Vector3(transform.position.x, transform.position.y, targetPos.z);
            transform.LookAt(targetPos, targetOrientation);
        }
        else
        {
            // look at players position with quaternion magic
            // code above matches camera rotation, this is position based       
            transform.rotation = Quaternion.LookRotation(transform.position - referenceCamera.transform.position);
        }
    }
}