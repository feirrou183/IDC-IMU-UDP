using DataVisualization.Charts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Drawer{
	///<summary>
	///
	///</summary>
	public class DrawController : MonoBehaviour
	{
		//每个bodyPart对应XYZ三个chart
		public Dictionary<BodyPart, Tuple<Chart, Chart, Chart>> drawDic = new Dictionary<BodyPart, Tuple<Chart, Chart, Chart>>();
		public Transform line_graph;


		//初始化，绑定所有线表
        private void Start()
        {

			Transform neck = line_graph.Find("Neck");
			Transform spine = line_graph.Find("Spine");
			Transform UpperArmLeft = line_graph.Find("UpperArmLeft");
			Transform UpperArmRight = line_graph.Find("UpperArmRight");
			Transform LowerArmLeft = line_graph.Find("LowerArmLeft");
			Transform LowerArmRight = line_graph.Find("LowerArmRight");
			//Chart neck_x = gui_scene.

			drawDic[BodyPart.Neck] = new Tuple<Chart, Chart, Chart>(
				neck.Find("X").GetComponentInChildren<Chart>(),
				neck.Find("Y").GetComponentInChildren<Chart>(),
				neck.Find("Z").GetComponentInChildren<Chart>());

			drawDic[BodyPart.Spine] = new Tuple<Chart, Chart, Chart>(
				spine.Find("X").GetComponentInChildren<Chart>(),
				spine.Find("Y").GetComponentInChildren<Chart>(),
				spine.Find("Z").GetComponentInChildren<Chart>());

			drawDic[BodyPart.UpperArmLeft] = new Tuple<Chart, Chart, Chart>(
				UpperArmLeft.Find("X").GetComponentInChildren<Chart>(),
				UpperArmLeft.Find("Y").GetComponentInChildren<Chart>(),
				UpperArmLeft.Find("Z").GetComponentInChildren<Chart>());

			drawDic[BodyPart.UpperArmRight] = new Tuple<Chart, Chart, Chart>(
				UpperArmRight.Find("X").GetComponentInChildren<Chart>(),
				UpperArmRight.Find("Y").GetComponentInChildren<Chart>(),
				UpperArmRight.Find("Z").GetComponentInChildren<Chart>());

			drawDic[BodyPart.LowerArmLeft] = new Tuple<Chart, Chart, Chart>(
				LowerArmLeft.Find("X").GetComponentInChildren<Chart>(),
				LowerArmLeft.Find("Y").GetComponentInChildren<Chart>(),
				LowerArmLeft.Find("Z").GetComponentInChildren<Chart>());

			drawDic[BodyPart.LowerArmRight] = new Tuple<Chart, Chart, Chart>(
				LowerArmRight.Find("X").GetComponentInChildren<Chart>(),
				LowerArmRight.Find("Y").GetComponentInChildren<Chart>(),
				LowerArmRight.Find("Z").GetComponentInChildren<Chart>());


			ClearGraph();
		}

		public void ClearGraph()
		{
			foreach (BodyPart bodyPart in Enum.GetValues(typeof(BodyPart)))
			{
				drawDic[bodyPart].Item1.Clear();
				drawDic[bodyPart].Item2.Clear();
				drawDic[bodyPart].Item3.Clear();
			}

		}

		public void drawLine(int timeStep,BodyPart bodyPart,Quaternion quaternion)
		{
			drawDic[bodyPart].Item1.AddData(new TwoDimensionalData(timeStep, quaternion.eulerAngles.x));
			drawDic[bodyPart].Item2.AddData(new TwoDimensionalData(timeStep, quaternion.eulerAngles.y));
			drawDic[bodyPart].Item3.AddData(new TwoDimensionalData(timeStep, quaternion.eulerAngles.z));

			Debug.Log(string.Format("{0}---{1}---{2}", quaternion.eulerAngles.x, quaternion.eulerAngles.y, quaternion.eulerAngles.z));
		}





    }

	



}
