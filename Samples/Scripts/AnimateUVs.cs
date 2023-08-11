using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateUVs : MonoBehaviour
{
	public List<AnimationModel> AnimationModels;
	private AnimationModel CurrentModel = null;
	public string PlayOnAwake;
	private float FrameTime = 0.0f;
	public bool IsMoving = true;
    private Vector2 Speed;
	private Vector2 Offset;
    public List<string> UVNames;
	private MeshRenderer MeshRenderer;
	private int CurrentFrame;
    
    void Start()
    {
	    MeshRenderer = gameObject.GetComponentInChildren<MeshRenderer>();
	    if(MeshRenderer == null)
	    {
		    MeshRenderer = gameObject.GetComponent<MeshRenderer>();
	    }
	    if(AnimationModels.Count>0)
	    {
	    	CurrentModel = AnimationModels[0];
	    }
	    if(PlayOnAwake!=null&&PlayOnAwake!="")
	    {
	    	Play(PlayOnAwake);
	    }

    }

    void Update()
    {
        
	    if(IsMoving)
	    {
	    	
		    	if(Time.time > FrameTime+(1.0f/CurrentModel.FrameRate))
			    {
			    	CurrentFrame++;
				    Offset += CurrentModel.Speed;
				    if(Offset.x>=1)
				    {
				    	if(CurrentModel.Looping)
				    	{
					    	Offset = Vector2.zero;
				    	CurrentFrame=0;
				    	}
				    	else{
				    		IsMoving = false; return;
				    	}
				    		
				    }
				    FrameTime = Time.time;
				    MoveFrameForModel(CurrentModel);
	    		
			    }
	    	
		  
	    }
    }
    
	private void MoveFrame()
	{
		foreach(var UVName in UVNames)
		{
			MeshRenderer.material.SetTextureOffset(UVName, Offset);
		}
	}
	
	private void MoveFrameForModel(AnimationModel model){
		if(MeshRenderer == null)
		{
			MeshRenderer = gameObject.GetComponent<MeshRenderer>();
		}
		foreach(var UVName in UVNames)
		{
			//Debug.Log(MeshRenderer +"-"+ model);
			MeshRenderer.material = model.Material;
			MeshRenderer.material.SetTextureOffset(UVName, Offset);
		}
	}
	
	public void Play(string animationModelName)
	{
		IsMoving = true;
		Offset = Vector2.zero;
		FrameTime = Time.time;
		CurrentModel = AnimationModels.First(x=>x.Name == animationModelName);
		if(MeshRenderer == null)
		{
			MeshRenderer = gameObject.GetComponent<MeshRenderer>();
		}
		MoveFrameForModel(CurrentModel);
	}
}

[System.Serializable]
public class AnimationModel
{
	public string Name;
	public Material Material;
	public float FrameRate = 24.0f;
	public float NumberOfFrames = 24.0f;
	public Vector2 Speed;
	public bool Looping;
	AnimationModel(string name, Material mat, float frameRate, Vector2 speed)
	{
		Name = name;
		Material = mat;
		FrameRate = frameRate;
		Speed = speed;
		
	}AnimationModel(string name, Material mat, float frameRate, float numberOfFrames)
	{
		Name = name;
		Material = mat;
		FrameRate = frameRate;
		Speed = Vector2.left /numberOfFrames;
		NumberOfFrames = numberOfFrames;
		
	}
}
