using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedMeshRendererController : MonoBehaviour
{
	public List<AnimatedMeshRendererModel> AnimatedMeshRendererModels;
	public List<string> UVNames;

	protected void Update()
	{
		UpdateActiveBulletsAnimation();
	}
	
	private void UpdateActiveBulletsAnimation()
	{
		AnimatedMeshRendererModels.ForEach(AnimatedMeshRendererModel=>
		{
			if(Time.time > AnimatedMeshRendererModel.FrameTime+(1.0f/AnimatedMeshRendererModel.AnimationModel.FrameRate))
			{
				AnimatedMeshRendererModel.CurrentFrame++;
				AnimatedMeshRendererModel.Offset += AnimatedMeshRendererModel.AnimationModel.Speed;
				if(AnimatedMeshRendererModel.Offset.x>=1)
				{
					AnimatedMeshRendererModel.Offset = Vector2.zero;
					AnimatedMeshRendererModel.CurrentFrame=0;
				}
				AnimatedMeshRendererModel.FrameTime = Time.time;
				AnimatedMeshRendererModel.Pool.ForEach(PooledMeshRenderer=>
				{
					if(PooledMeshRenderer.enabled)
					{
						MoveFrame(PooledMeshRenderer,AnimatedMeshRendererModel.AnimationModel, AnimatedMeshRendererModel.Offset);
					}
				});
			}
		});
	}

	private void MoveFrame(MeshRenderer meshRenderer,AnimationModel animationModel,Vector2 offset){
		foreach(var UVName in UVNames)
		{
			meshRenderer.material = animationModel.Material;
			meshRenderer.material.SetTextureOffset(UVName, offset); 
		}
	}
}

[System.Serializable]
public class AnimatedMeshRendererModel
{
	public string name;
	public List<MeshRenderer> Pool;
	public AnimationModel AnimationModel;
	public float FrameTime = 0.0f;
	public Vector2 Speed;
	public Vector2 Offset;
	public int CurrentFrame;
}
