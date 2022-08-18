using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    private Animator _animator;

    private const int BONE_LAYER = 9;
    private const int ROOT_BONE_INDEX = 0;

    public List<Bone> Bones { get; private set; }
    public Bone Root => Bones != null && Bones.Count > 0 ? Bones[ROOT_BONE_INDEX] : null;
    public bool IsActive { get; private set; }

    void Awake()
    {
        _animator = GetComponentInChildren<Animator>();

        InitializeBones();
    }

    private void InitializeBones()
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
        Bones = new List<Bone>(rigidbodies.Length);

        foreach (Rigidbody rigidbody in rigidbodies)
        {
            Bone newBone = rigidbody.gameObject.AddComponent<Bone>();
            newBone.gameObject.layer = BONE_LAYER;
            Bones.Add(newBone);
        }
    }

    void Start()
    {
        DisableRagdoll();
    }

    public void EnableRagdoll()
    {
        if (IsActive) return;

        _animator.enabled = false;

        foreach (Bone bone in Bones)
        {
            bone.Collider.isTrigger = false;
            bone.Rigidbody.isKinematic = false;
        }

        IsActive = true;
    }

    public void DisableRagdoll()
    {
        _animator.enabled = true;

        foreach (Bone bone in Bones)
        {
            bone.Collider.isTrigger = true;
            bone.Rigidbody.isKinematic = true;
        }
        IsActive = false;
    }

    public void AddForceToRootBone(Vector3 force)
    {
        Root.Rigidbody.AddForce(force, ForceMode.Impulse);
    }
}