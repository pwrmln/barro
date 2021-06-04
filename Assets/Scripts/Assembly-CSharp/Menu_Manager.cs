using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Manager : MonoBehaviour
{
	[Serializable]
	public class _ListOfMenuGameobject
	{
		public GameObject objList;

		public bool Desktop = true;

		public bool Mobile = true;
	}

	[Serializable]
	public class _List_gameObjectsByPage
	{
		public List<_ListOfMenuGameobject> listOfMenuGameobject = new List<_ListOfMenuGameobject>();
	}

	private IEnumerator Coroutine;

	public bool b_Coutine;

	public List<CanvasGroup> List_GroupCanvas = new List<CanvasGroup>();

	public List<bool> b_MoreOptions = new List<bool>();

	public int CurrentPage;

	public bool b_DesktopOrMobile = true;

	public List<_ListOfMenuGameobject> listOfMenuGameobject = new List<_ListOfMenuGameobject>();

	public List<_List_gameObjectsByPage> list_gameObjectByPage = new List<_List_gameObjectsByPage>();

	private void Start()
	{
		for (int i = 0; i < List_GroupCanvas.Count; i++)
		{
			if (List_GroupCanvas[i].gameObject.activeSelf)
			{
				CurrentPage = i;
				break;
			}
		}
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}

	public IEnumerator MoveToPosition(CanvasGroup G_canvas_01, CanvasGroup G_canvas_02, Button selected = null)
	{
		b_Coutine = true;
		G_canvas_01.interactable = false;
		G_canvas_01.blocksRaycasts = false;
		G_canvas_01.alpha = 0f;
		G_canvas_01.gameObject.SetActive(false);
		G_canvas_02.gameObject.SetActive(true);
		G_canvas_02.blocksRaycasts = true;
		G_canvas_02.alpha = 1f;
		G_canvas_02.interactable = true;
		b_Coutine = false;
		yield return null;
		if ((bool)selected)
		{
			selected.Select();
		}
	}

	public void GoToOtherPage(CanvasGroup newCanvas)
	{
		if (b_Coutine)
		{
			return;
		}
		Coroutine = MoveToPosition(List_GroupCanvas[CurrentPage], newCanvas);
		for (int i = 0; i < List_GroupCanvas.Count; i++)
		{
			if (List_GroupCanvas[i] == newCanvas)
			{
				CurrentPage = i;
				break;
			}
		}
		StartCoroutine(Coroutine);
	}

	public void GoToOtherPageWithHisNumber(int newCanvasNumber, Button selected = null)
	{
		if (!b_Coutine)
		{
			Coroutine = MoveToPosition(List_GroupCanvas[CurrentPage], List_GroupCanvas[newCanvasNumber], selected);
			for (int i = 0; i < List_GroupCanvas.Count; i++)
			{
				if (List_GroupCanvas[i] == List_GroupCanvas[newCanvasNumber])
				{
					CurrentPage = i;
					break;
				}
			}
			StartCoroutine(Coroutine);
		}
		if ((bool)selected)
		{
			selected.Select();
		}
	}
}
