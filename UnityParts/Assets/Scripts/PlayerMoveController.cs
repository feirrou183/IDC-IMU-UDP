using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//用于各个关节角度判断与检验


namespace Player{
	///<summary>
	///
	///</summary>
	public class PlayerMoveController : MonoBehaviour
	{
		[HideInInspector]
		public Transform Neck;
		[HideInInspector]
		public Transform Spine;
		[HideInInspector]
		public Transform UpperArmLeft;
		[HideInInspector]
		public Transform UpperArmRight;
		[HideInInspector]
		public Transform LowerArmLeft;
		[HideInInspector]
		public Transform LowerArmRight;

		private	Quaternion ChangeMatrix = new Quaternion();
		private Quaternion newLocalQuaternion = new Quaternion();




		public Quaternion CheckLimitation(BodyPart bodyPart,Quaternion ReadyChangeQuaterion)
		{
			switch (bodyPart)
			{
				case BodyPart.Neck:
					if (
						((ReadyChangeQuaterion.eulerAngles.z <= 212.6) || (ReadyChangeQuaterion.eulerAngles.z >= 302.6))
						||
						((ReadyChangeQuaterion.eulerAngles.x >= 45) && (ReadyChangeQuaterion.eulerAngles.x <= 315))
						||
						((ReadyChangeQuaterion.eulerAngles.y <= 190) || (ReadyChangeQuaterion.eulerAngles.y >= 350))
						)
					{
						return Neck.rotation;
					}
					else return ReadyChangeQuaterion;

				case BodyPart.Spine:
					if (
						((ReadyChangeQuaterion.eulerAngles.z <= 185) || (ReadyChangeQuaterion.eulerAngles.z >= 305))
						||
						((ReadyChangeQuaterion.eulerAngles.x >= 30) && (ReadyChangeQuaterion.eulerAngles.x <= 330))
						||
						((ReadyChangeQuaterion.eulerAngles.y <= 240) || (ReadyChangeQuaterion.eulerAngles.y >= 300))
						)
					{
						return Spine.rotation;
					}
					else return ReadyChangeQuaterion;

				case BodyPart.UpperArmLeft:

					//获取世界转本地变换矩阵
					Quaternion ChangeMatrix = UpperArmLeft.localRotation * Quaternion.Inverse(UpperArmLeft.rotation);
					//获取变换后的本地四元数
					Quaternion newLocalQuaternion = ChangeMatrix * ReadyChangeQuaterion;
					if (
						((newLocalQuaternion.eulerAngles.x >= 50 && newLocalQuaternion.eulerAngles.x <= 180))
						||
						((newLocalQuaternion.eulerAngles.z >= 90) && (newLocalQuaternion.eulerAngles.z <= 270))
						)
					{
						return UpperArmLeft.rotation;
					}
					else
					{
						//y轴默认不存在旋转
						newLocalQuaternion.eulerAngles = new Vector3(newLocalQuaternion.eulerAngles.x, 0 , newLocalQuaternion.eulerAngles.z);
						//变换为世界坐标
						return Quaternion.Inverse(ChangeMatrix) * newLocalQuaternion;
					}



				case BodyPart.UpperArmRight:

					//获取世界转本地变换矩阵
					ChangeMatrix = UpperArmRight.localRotation * Quaternion.Inverse(UpperArmRight.rotation);
					//获取变换后的本地四元数
					newLocalQuaternion = ChangeMatrix * ReadyChangeQuaterion;
					if (
						((newLocalQuaternion.eulerAngles.x >= 50 && newLocalQuaternion.eulerAngles.x <= 180))
						||
						((newLocalQuaternion.eulerAngles.z >= 60) && (newLocalQuaternion.eulerAngles.z <= 240))
						)
					{
						return UpperArmRight.rotation;
					}
					else
					{
						//y轴默认不存在旋转
						newLocalQuaternion.eulerAngles = new Vector3(newLocalQuaternion.eulerAngles.x, 0, newLocalQuaternion.eulerAngles.z);
						//变换为世界坐标
						return Quaternion.Inverse(ChangeMatrix) * newLocalQuaternion;
					}


                case BodyPart.LowerArmLeft:
					//获取世界转本地变换矩阵
					ChangeMatrix = LowerArmLeft.localRotation * Quaternion.Inverse(LowerArmLeft.rotation);
					//获取变换后的本地四元数
					newLocalQuaternion = ChangeMatrix * ReadyChangeQuaterion;
					if (
						(newLocalQuaternion.eulerAngles.y >= 150)
						||
						((newLocalQuaternion.eulerAngles.z <= 170) || (newLocalQuaternion.eulerAngles.z >= 310))
						)
					{
						return LowerArmLeft.rotation;
					}
					else
					{
						//y轴默认不存在旋转
						newLocalQuaternion.eulerAngles = new Vector3(0, newLocalQuaternion.eulerAngles.y, newLocalQuaternion.eulerAngles.z);
						//变换为世界坐标
						return Quaternion.Inverse(ChangeMatrix) * newLocalQuaternion;
					}

				case BodyPart.LowerArmRight:
					//获取世界转本地变换矩阵
					ChangeMatrix = LowerArmRight.localRotation * Quaternion.Inverse(LowerArmRight.rotation);
					//获取变换后的本地四元数
					newLocalQuaternion = ChangeMatrix * ReadyChangeQuaterion;
					if (
						((newLocalQuaternion.eulerAngles.y >= 120) && (newLocalQuaternion.eulerAngles.y<= 330))
						||
						((newLocalQuaternion.eulerAngles.z <= 170) || (newLocalQuaternion.eulerAngles.z >= 320))
						)
					{
						return LowerArmRight.rotation;
					}
					else
					{
						//y轴默认不存在旋转
						newLocalQuaternion.eulerAngles = new Vector3(110, newLocalQuaternion.eulerAngles.y, newLocalQuaternion.eulerAngles.z);
						//变换为世界坐标
						return Quaternion.Inverse(ChangeMatrix) * newLocalQuaternion;
					}

				default:
					return ReadyChangeQuaterion;
			}
		}



	}
}
