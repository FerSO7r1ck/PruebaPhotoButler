
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace PhotoButler.AEUnity
{
	public enum SpriteType{
		SpriteRenderer=1,
		UGUI,
		NGUI
	}
	public enum BuildAtlasType
	{
		AllImageInDirectory=1,
		ReferenceImage=2,
		NotBuildAtlas=3,
		//BuildAtlasCustom=4,
	}
	public enum SortLayerType
	{
		Depth=0,
		Z=1,
	}
	public enum AnimationStyle
	{
		Loop,
		Normal
	}
	[Preserve]
	public class AEPInfoDataImage
	{ 
		public string name;
		public Rect rect;
		public Vector2 pivot;
		public AEPInfoDataImage(string _name,Rect _rect,float x,float y)
		{
			name=_name;
			rect=_rect;
			pivot=new Vector2(x,y);
		}
	}
	[Preserve]
	public class ModuleImage{
		public int x;
		public int y;
		public int w;
		public int h;
		public float pivotX;
		public float pivotY;
	}
	[Preserve]
	public class Composition
	{
		public string name;//unique
		public float duration;
		public float frameDuration;
		public float frameRate;
		public float width = 1;
		public float height = 1;
		public string workFrameDuration;
		public float displayStartTime = 0;


	}
	[Preserve]
	public class BoneElement
	{
		public string name;//unique
		public string parent;
		public float x;
		public float y;
		public float z;
		public float zoom;
		public float scaleX=1;
		public float scaleY=1;
		public float scaleZ=1;
		public float ox = 0;
		public float oy = 0;
		public float oz = 0;
		public float rx = 0;
		public float ry = 0;
		public float rz = 0;
		public float length=0;
		public float rotation;
		public int index=0;
		public string layerType="";
		public string _3DTrackerName = "";
		public float layerInPoint = 0;
		public float layerOutPoint = 0;
		public string frameInPoint = "";
		public string frameOutPoint = "";
		public float[] anchorPointArray;
		public float[] lightColor;
		public float lightIntensity = 0;
		public float lightConeAngle = 0;
		public float lightConeFeather = 0;
		public int cameraIndex = -1;
		public bool isASolidLayer = false;
		public bool hasAudio = true;
        public bool isFront = false;
		public Dictionary<string, object> layerSpecificData;
		public Dictionary<string, object> sourceRects;
		public Dictionary<string, float[]> contents;
	}
	[Preserve]
	class BoneElementCompare : IComparer<BoneElement>
	{
		public int Compare(BoneElement x, BoneElement y)
		{
			if (x.index > y.index) {
				return -1;
			} else if (x.index < y.index) {
				return 1;
			}
			return 0;
		}
	}
	[Preserve]
	public class SlotElelement
	{
		public string name;//unique
		public string bone;
		public string color="ffffffff";
		public string attachment="";//attachmentDefault
		public List<string> listAcceptAttachment=new List<string>();
		public List<EAPInfoAttachment> listAcceptObj=new List<EAPInfoAttachment>();
		public int index=0;

	}
	[Preserve]
	public class EAPInfoAttachment
	{
		public string slotName;
		public string spriteName;
		public float x;
		public float y;
		public int depth=-100;
		public float width;
		public float height;

		public float scaleX=1;
		public float scaleY=1;
		public float rotation=0;
		//just use for optimize
		public int startX;
		public int startY;
		//public IntRect originalRect;
		//public IntRect optimizeRect;
		public bool isOptimze=false;
		public float offsetX=0;
		public float offsetY=0;
		/*public void SetCache(int _startX,int _startY,IntRect _originalRect, IntRect _optimizeRect)
		{
			isOptimze=true;
			startX=_startX;
			startY=_startY;
			originalRect=_originalRect;
			optimizeRect=_optimizeRect;
		}*/

		public EAPInfoAttachment(string _spriteName,string _Slotname,int _depth,float x,float y,float _offsetX,float _offsetY)
		{
			this.slotName=_Slotname;
			this.spriteName=_spriteName;

			this.x=x;
			this.y=y;
			this.depth=_depth;
			this.offsetX=_offsetX;
			this.offsetY=_offsetY;
			//this.rotation=raw.rotation;
			//this.scaleX=raw.scaleX;
			//this.scaleY=raw.scaleY;
		}
	}
	[Preserve]
	public class AEPAnimationRaw
	{
		public Dictionary<string,object> animation;
	}
	[Preserve]
	public class RawAEPJson
	{
		public List<Composition> comps;
		public List<BoneElement> bones;
		public List<SlotElelement> slots;
		public Dictionary<string,object> skins;
		public AEPAnimationRaw animations;
	}
	[Preserve]
	public class RawAEPJsonAttachment
	{
		public string name;
		public float x;
		public float y;
		public float width;
		public float height;
		public float scaleX=1;
		public float scaleY=1;
		public float rotation=0;
	}

	#region Animation Data Tranform
	[Preserve]
	public class AEPAnimationTranslate
	{
		public float time;
		public float x;
		public float y;
		public string tangentType="linear";
	}

	[Preserve]
	public class AEPAnimationScale
	{
		public float time;
		public float x;
		public float y;
		public string tangentType="linear";
	}

	[Preserve]
	public class AEPAnimationRotate
	{
		public float time;
		public float angle;
		public float angleChange=0;
		public string tangentType="linear";
	}
	[Preserve]
	public class AEPBoneAnimationElement
	{
		public string name;
		public List<AEPAnimationTranslate> translate;
		public List<AEPAnimationScale> scale;
		public List<AEPAnimationRotate> rotate;
	}
	[Preserve]
	public class AEPAnimationAttachment
	{
		public float time;
		public string name;
	}
	#endregion
	[Preserve]
	public class AEPAnimationColor
	{
		public float time;
		public string color;
		public string tangentType="linear";
	}
	[Preserve]
	public class AEPSlotAnimationElement
	{
		public string name;
		public List<AEPAnimationAttachment> attachment;
		public List<AEPAnimationColor> color;
	}
	[Preserve]
	public class AEPJsonFinal
	{
		public List<BoneElement> bones;
		public List<SlotElelement> slots;

		public Dictionary<string,BoneElement> dicBones;
		public Dictionary<string,SlotElelement> dicSlots;
		public Dictionary<string,object> skins;

		public Dictionary<string,EAPInfoAttachment> dicPivot=new Dictionary<string,EAPInfoAttachment>();
		public Dictionary<string,AEPBoneAnimationElement> dicAnimation=new Dictionary<string,AEPBoneAnimationElement>();
		public Dictionary<string,AEPSlotAnimationElement> dicSlotAttactment=new Dictionary<string,AEPSlotAnimationElement>();

		public List<SlotElelement> GetSlotMappingWithBone(string bone)
		{
			List<SlotElelement> list=new List<SlotElelement>();
			foreach(KeyValuePair<string,SlotElelement> pair in dicSlots)
			{
				if(pair.Value.bone==bone)
				{
					list.Add(pair.Value);
				}
			}
			return list;
		}

		public AEPJsonFinal(RawAEPJson raw)
		{
			#region Generate Bones Info
			this.bones=raw.bones;
			dicBones=new Dictionary<string, BoneElement>();
			for(int i=0;i<bones.Count;i++)
			{
				//bones[i].index=i;
				dicBones[bones[i].name]=bones[i];
			}
			#endregion

			#region attactment
			Dictionary<string,bool> dicAttact = new Dictionary<string, bool> ();
			if(raw.animations!=null&&raw.animations.animation!=null)
			{
				foreach(KeyValuePair<string,object> pair1 in raw.animations.animation)
				{
					if(pair1.Key=="bones")// tam thoi chi su ly cho bone
					{
						Dictionary<string,object> temp1=(Dictionary<string,object>)pair1.Value;
						
						foreach(KeyValuePair<string,object> pair2 in temp1)
						{
							//Debug.LogError(Pathfinding.Serialization.JsonFx.JsonWriter.Serialize(pair2.Value));
							//AEPBoneAnimationElement full=Pathfinding.Serialization.JsonFx.JsonReader.Deserialize(
							//	Pathfinding.Serialization.JsonFx.JsonWriter.Serialize(pair2.Value), typeof(AEPBoneAnimationElement)) as AEPBoneAnimationElement;
							// AEPBoneAnimationElement full = JsonUtility.FromJson<AEPBoneAnimationElement>(JsonUtility.ToJson(pair2.Value));
							AEPBoneAnimationElement full = JsonConvert.DeserializeObject<AEPBoneAnimationElement>(JsonConvert.SerializeObject(pair2.Value));
							full.name=pair2.Key;
							dicAnimation[full.name]=full;
						}
						//Debug.LogError(Pathfinding.Serialization.JsonFx.JsonWriter.Serialize(dicAnimation));
					}
					else if(pair1.Key=="slots")// attachment object
					{
						Dictionary<string,object> temp1=(Dictionary<string,object>)pair1.Value;
						
						foreach(KeyValuePair<string,object> pair2 in temp1)
						{
							//AEPSlotAnimationElement slotAnim=Pathfinding.Serialization.JsonFx.JsonReader.Deserialize(
							//	Pathfinding.Serialization.JsonFx.JsonWriter.Serialize(pair2.Value), typeof(AEPSlotAnimationElement)) as AEPSlotAnimationElement;
							// AEPSlotAnimationElement slotAnim = JsonUtility.FromJson<AEPSlotAnimationElement>(JsonUtility.ToJson(pair2.Value));
							AEPSlotAnimationElement slotAnim = JsonConvert.DeserializeObject<AEPSlotAnimationElement>(JsonConvert.SerializeObject(pair2.Value));
							slotAnim.name=pair2.Key; 
							if(slotAnim.attachment!=null)
							{
								for(int yy=0;yy<slotAnim.attachment.Count;yy++)
								{
									string finalName=slotAnim.attachment[yy].name;
									if(!string.IsNullOrEmpty(finalName))
									{
										int index=finalName.LastIndexOf("/");
										finalName=finalName.Substring(index+1,finalName.Length-index-1);
									}
									slotAnim.attachment[yy].name=finalName;
								}
								dicAttact[slotAnim.name]=true;
							}
							dicSlotAttactment[slotAnim.name]=slotAnim;
						}
						//Debug.LogError(Pathfinding.Serialization.JsonFx.JsonWriter.Serialize(dicSlotAttactment));
					}
				}
			}
			#endregion

			#region generate Slots Info
			this.slots=raw.slots;
			if(this.slots==null)
			{
				this.slots=new List<SlotElelement>();
			}
			dicSlots=new Dictionary<string,SlotElelement>();
			for(int i=0;i<slots.Count;i++)
			{
				string slotAttachmentName=slots[i].attachment;
				if(slotAttachmentName==null)
				{
					if(dicAttact.ContainsKey(slots[i].name))
					{
						slotAttachmentName="";
					}
				}
				if(slotAttachmentName!=null)
				{
					//Debug.LogError(slotAttachmentName);
					int index=slotAttachmentName.LastIndexOf("/");
					slotAttachmentName=slotAttachmentName.Substring(index+1,slotAttachmentName.Length-index-1);
					slots[i].attachment=slotAttachmentName;
					slots[i].index=slots.Count-i;
					this.dicSlots[slots[i].name]=slots[i];

					BoneElement boneTemp=null;
					dicBones.TryGetValue(slots[i].bone,out boneTemp);
					if(boneTemp!=null){
						boneTemp.index=slots[i].index;
					}
				}
			}
			#endregion
			
			#region skin
			Dictionary<string,object> skins=raw.skins;
			foreach(KeyValuePair<string,object> pair1 in skins)
			{
				Dictionary<string,object> temp1=(Dictionary<string,object>)pair1.Value;
				int count=0;
				foreach(KeyValuePair<string,object> pair2 in temp1)
				{
					string slotsName=pair2.Key;
					SlotElelement slot=null;
					dicSlots.TryGetValue(slotsName,out slot);
					
					
					Dictionary<string,object> temp2=(Dictionary<string,object>)pair2.Value;
					foreach(KeyValuePair<string,object> pair3 in temp2)
					{
						int depth=-count;
						//Debug.LogError(pair3.Key);
						string attachmentName=pair3.Key;
						int index=attachmentName.LastIndexOf("/");
						attachmentName=attachmentName.Substring(index+1,attachmentName.Length-index-1);
						float x=0;
						float y=0;
						
						//RawAEPJsonAttachment rawPivot=Pathfinding.Serialization.JsonFx.JsonReader.Deserialize(
						//	Pathfinding.Serialization.JsonFx.JsonWriter.Serialize(pair3.Value), typeof(RawAEPJsonAttachment)) as RawAEPJsonAttachment;
						//RawAEPJsonAttachment rawPivot = JsonUtility.FromJson<RawAEPJsonAttachment>(JsonUtility.ToJson(pair3.Value));
						RawAEPJsonAttachment rawPivot = JsonConvert.DeserializeObject<RawAEPJsonAttachment>(JsonConvert.SerializeObject(pair3.Value));
						x= (rawPivot.width/2-rawPivot.x)/rawPivot.width;
						y= (rawPivot.height/2-rawPivot.y)/rawPivot.height;
						if(slot!=null)
						{
							depth=-slot.index;
						}
						if(string.IsNullOrEmpty(rawPivot.name))
						{
							rawPivot.name=attachmentName;
						} 
						//Debug.LogError(Pathfinding.Serialization.JsonFx.JsonWriter.Serialize(rawPivot));
						EAPInfoAttachment  pivot=new EAPInfoAttachment(attachmentName,slotsName,depth,x,y,rawPivot.x,rawPivot.y);
						
						count++;
						//Debug.LogError(pivot.name);
						pivot.spriteName=pivot.spriteName.Trim();
						dicPivot[pivot.spriteName]=pivot;
						if(slot!=null&&!slot.listAcceptAttachment.Contains(pivot.spriteName))
						{
							slot.listAcceptAttachment.Add(pivot.spriteName);
							slot.listAcceptObj.Add(pivot);
						}
					}
				}
			}
			#endregion

		}

		public BoneElement GetBoneElement(string boneName)
		{
			BoneElement element=null;
			dicBones.TryGetValue(boneName, out element);
			return element;
		}

		public string GetFullPathBone(string boneName)
		{
			Dictionary<string,BoneElement> dic=dicBones;
			string path="";
			BoneElement bone=null;
			if(dic.TryGetValue(boneName,out bone))
			{
				while(true)
				{
					if(bone.parent==null)
					{
						path+="";
						break;
					}
					else
					{
						if(path.Length<1)
							path=bone.name;
						else
							path=bone.name+"/"+path;
						if(!dic.TryGetValue(bone.parent,out bone))
						{
							break;
						}
					}
				}
			}
			return path;
		}

	} 
}
