﻿using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000963 RID: 2403
[RequireComponent(typeof(Renderer))]
[DefaultExecutionOrder(100)]
public class HotObject : MonoBehaviour
{

#if UNITY_EDITOR	
    [ContextMenu("Fill Heat and Haze Bounds")]
    private void FillBounds()
    {        
        GameObject centerObject = FindChildByName("center");
        GameObject extentObject = FindChildByName("extent");
        GameObject hazeObject = FindChildByName("haze");
        
        if (centerObject != null && extentObject != null && hazeObject != null)
        {
            // Get the positions of the game objects
            Vector3 centerPosition = centerObject.transform.localPosition;
            Vector3 extentPosition = extentObject.transform.localPosition;
            Vector3 hazePosition = hazeObject.transform.localPosition;

            // Insert the position
            HeatBounds = new Bounds(centerPosition, extentPosition * 4);          
            HeatHazeBounds = new Bounds(centerPosition, hazePosition * 4);

            Debug.Log("HeatBound and HazeBound filled successfully!");
        }
        else
        {
            Debug.LogError("One or more game objects not found. Please check their names. \"center, extent, haze\" Make sure they are named like this, case sensitive");
        }
    }

    // Loops to all GameObject within a prefab to find the specific gameobject with said name.
    private GameObject FindChildByName(string name)
    {
        Transform[] children = GetComponentsInChildren<Transform>(true);
        foreach (Transform child in children)
        {
            if (child.name == name)
            {
                return child.gameObject;
            }
        }
        return null;
    }

	[ContextMenu("Create Points")]
	private void CreateChildren()
	{
        GameObject centerObject = FindChildByName("center");
        GameObject extentObject = FindChildByName("extent");
        GameObject hazeObject = FindChildByName("haze");

        if (centerObject == null && extentObject == null && hazeObject == null)
		{
            GameObject center = new GameObject("center");
            center.transform.parent = transform;
            center.transform.localPosition = Vector3.zero;
            center.transform.localEulerAngles = Vector3.zero;

            GameObject extent = new GameObject("extent");
            extent.transform.parent = center.transform;
            extent.transform.localPosition = Vector3.zero;
            extent.transform.localEulerAngles = Vector3.zero;

            GameObject haze = new GameObject("haze");
            haze.transform.parent = center.transform;
            haze.transform.localPosition = Vector3.zero;
            haze.transform.localEulerAngles = Vector3.zero;
        }
		else
		{
			Debug.LogWarning("An existing game object already exist, no game object will be added. Please delete the existing one and run \"Create Points\" again.");
		}

		
    }
#endif

    // Token: 0x04003B6C RID: 15212
    [Tooltip("Apply to all materials on Renderer")]
	[Header("Sets the temperature for a specific object")]
	[SerializeField]
	public bool IsApplyAllMaterials;

	// Token: 0x04003B6D RID: 15213
	[Tooltip("(min, max, factor)")]
	[SerializeField]
	public Vector3 Temperature = new Vector3(0.1f, 1f, 3.5f);

	// Token: 0x04003B6E RID: 15214
	[Tooltip("The Temperature multiplier.z to control the temperature from the script")]
	[SerializeField]
	public float TemperatureCelsio = 29f;

	// Token: 0x04003B6F RID: 15215
	[Tooltip("The serial number of the material in the Renderer to which the temperature should be applied")]
	[SerializeField]
	private List<int> materialsId = new List<int>();

	// Token: 0x04003B70 RID: 15216
	[SerializeField]
	public Bounds HeatBounds = new Bounds(new Vector3(0f, 0f, 0f), new Vector3(0.03f, 0.08f, 0.03f));

	// Token: 0x04003B71 RID: 15217
	[SerializeField]
	public float VisibleHeatAlpha = 1f;

	// Token: 0x04003B72 RID: 15218
	[Space]
	[Header("Heat Haze Effect")]
	[SerializeField]
	public bool UseHeatHaze;

	// Token: 0x04003B73 RID: 15219
	[SerializeField]
	public Bounds HeatHazeBounds = new Bounds(new Vector3(0f, 0f, 0f), new Vector3(0.018f, 0.06f, 0.018f));

	// Token: 0x04003B74 RID: 15220
	[SerializeField]
	private Vector2 HeatParticleLifetimeDelta = new Vector2(0.3f, 0.5f);

	// Token: 0x04003B75 RID: 15221
	[SerializeField]
	private AnimationCurve HeatParticleCountByTemp = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f),
		new Keyframe(1f, 20f)
	});

	// Token: 0x04003B76 RID: 15222
	[SerializeField]
	private AnimationCurve HeatParticleLifeTimeByTemp = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0.15f),
		new Keyframe(1f, 0.3f)
	});

	// Token: 0x04003B77 RID: 15223
	[SerializeField]
	private AnimationCurve HeatParticleSizeByTemp = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0.01f),
		new Keyframe(1f, 0.14f)
	});
}
