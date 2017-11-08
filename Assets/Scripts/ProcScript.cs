using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProcScript : MonoBehaviour
{
	private class ProcData
	{
		public string tag = string.Empty;
		public string procName = string.Empty;
		public IEnumerator routine = null;
		public object param = null;
	}

	[SerializeField]
	Text _debugLabel = null;

	public MonoBehaviour targetBehaviour;

	private ProcScript.ProcData currentProcData;

	private List<ProcScript.ProcData> procList = new List<ProcScript.ProcData>();

	public delegate void OnStoppedTrigger();
	public OnStoppedTrigger onStoppedTrigger;

	void LateUpdate()
	{
		if(_debugLabel == null) 
		{
			return;
		}

		if(currentProcData == null) 
		{
			_debugLabel.text = "no proc";
		}
		else 
		{
			_debugLabel.text = currentProcData.procName;
		}
	}

	public void OutputProcList()
	{
		Debug.LogWarning("ProcList(CurrentProc) :: " + CurrentProcName());
		using (List<ProcScript.ProcData>.Enumerator enumerator = procList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ProcScript.ProcData current = enumerator.Current;
				Debug.LogWarning("ProcList(in list) :: " + current.procName);
			}
		}
	}

	public bool IsProcPending(string procName)
	{
		return procList.Find((ProcScript.ProcData o) => o.procName == procName) != null;
	}

	public string CurrentProcName()
	{
		if (currentProcData == null)
		{
			return null;
		}
		return currentProcData.procName;
	}

	public void DoneProc()
	{       
		currentProcData = null;
		onStoppedTrigger = null;
		NextProc();
    }

	private void NextProc()
	{
		while (currentProcData == null && procList.Count > 0)
		{
			ProcScript.ProcData procData = currentProcData = procList[0];
			procList.RemoveAt(0);

			if(procData.routine == null)
			{
				targetBehaviour.StartCoroutine(currentProcData.procName, currentProcData.param);
			}
			else
			{
				targetBehaviour.StartCoroutine(currentProcData.routine);
			}
		}
	}

	public void AddProc(string procName, object param = null)
	{
		ProcScript.ProcData procData = new ProcScript.ProcData();
		procData.procName = procName;
		procData.param = param;
		procList.Add(procData);
		NextProc();


	}
		
	public void AddProc(string procName, IEnumerator routine)
	{
		ProcScript.ProcData procData = new ProcScript.ProcData();
		procData.procName = procName;
		procData.routine = routine;
		procList.Add(procData);
		NextProc();
	}

	public void AddProcWithTag(string procName, object param, string tag)
	{
		ProcScript.ProcData procData = new ProcScript.ProcData();
		procData.procName = procName;
		procData.param = param;
		procData.tag = tag;
		procList.Add(procData);
		NextProc();
	}

	public void AddProcWithTag(string procName, IEnumerator routine, string tag)
	{
		ProcScript.ProcData procData = new ProcScript.ProcData();
		procData.procName = procName;
		procData.routine = routine;
		procData.tag = tag;
		procList.Add(procData);
		NextProc();
	}

	public void StopCurrentProc()
	{
		//if (currentProc == null || currentProcData == null)
		//unicon7 : Proc을 시작하자 마자 yield 하기전 외부에서 StopCurrentProc(), DontCurrentProc() 호출시 currentProc은 아직 null이기때문에 종료 될수가 없다.
		if (currentProcData == null)
		{
			return;
		}

		Debug.LogWarning("StopCurrentProc() ProcList Begin");
		OutputProcList();
		Debug.LogWarning("StopCurrentProc() ProcList End");

		//targetBehaviour
		if(onStoppedTrigger != null) 
		{
			onStoppedTrigger();
			onStoppedTrigger = null;
		}

		if(currentProcData.routine == null)
		{
			targetBehaviour.StopCoroutine(currentProcData.procName);
		}
		else
		{
			targetBehaviour.StopCoroutine(currentProcData.routine);
		}

		DoneProc();
	}

	public void DeleteProc(string procName)
	{
		procList.RemoveAll((p) => p.procName == procName);
		if(currentProcData != null && currentProcData.procName == procName)
		{
			StopCurrentProc();
		}
	}

	public void DeleteProcWithTag(string tag)
	{
		procList.RemoveAll((p) => p.tag == tag);
		if(currentProcData != null && currentProcData.tag == tag)
		{
			StopCurrentProc();
		}
	}

	public void DeleteAllProc()
	{       
        OutputProcList();
        procList.Clear();
		StopCurrentProc();
	}
}

