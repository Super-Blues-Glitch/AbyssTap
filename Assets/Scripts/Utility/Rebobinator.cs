using PrimeTween;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rebobinator : MonoBehaviour
{
    [SerializeField] private Stack<Vector3> positions = new();
    [SerializeField] private Stack<Quaternion> rotations = new();
    [SerializeField] bool saveOnawake;
    [SerializeField] bool save;
    [SerializeField] float rebobinationSpeed;

    public Action RevoDone;

    private void Start()
    {
        save = saveOnawake;
    }

    private void Update()
    {
        if (!save) { return; }

        positions.TryPeek(out Vector3 savedPos);
        rotations.TryPeek(out Quaternion savedRot);

        positions.Push(transform.position);
        rotations.Push(transform.rotation);
    }

    public void DoRebobine(Rigidbody objToRebobine)
    {
        StopRebobine();
        //StartCoroutine(Rebobine(objToRebobine));
        Debug.Log("THE REVO");
        objToRebobine.useGravity = false;
        objToRebobine.constraints = RigidbodyConstraints.FreezeAll;
        objToRebobine.TryGetComponent(out PlayerCordMovement mov);
        mov.enabled = false;

        positions.TryPop(out Vector3 reboPos);
        rotations.TryPop(out Quaternion reboRot);

        objToRebobine.transform.position = reboPos;
        objToRebobine.transform.rotation = reboRot;

        if (positions.Count > 0)
        {
            Tween.Delay(rebobinationSpeed, () => DoRebobine(objToRebobine));
        }
        else
        {
            objToRebobine.useGravity = true;
            objToRebobine.constraints = RigidbodyConstraints.FreezeRotation;
            positions.Clear();
            rotations.Clear();
            mov.enabled = true;

            RevoDone?.Invoke();
        }
    }

    public void StopRebobine()
    {
        save = false;
    }
}
