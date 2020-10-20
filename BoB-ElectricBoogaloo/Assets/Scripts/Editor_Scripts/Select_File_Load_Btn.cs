using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Select_File_Load_Btn : MonoBehaviour
{
	private string file_name;
	private TMP_Text file_display;
	private File_Plugin_Behavior File_Handler;

	public string GetFile()
	{
		return file_name;
	}

	public void SetFile(string f)
	{
		file_name = f;
		file_display.text = f;
	}

	public void Clicked()
	{
		File_Handler.SelectMapToLoad(file_name);

		GameObject file_btn;
		int c = 1;

		file_btn = transform.parent.Find("File" + c.ToString()).gameObject;
		do {
			if(file_btn != this.gameObject)
				file_btn.GetComponent<Button>().interactable = true;

			c++;

			Transform tmp = transform.parent.Find("File" + c.ToString());
			file_btn = (tmp != null) ? tmp.gameObject : null;
		} while (file_btn != null);

		this.GetComponent<Button>().interactable = false;
		File_Handler.SetLoadBtn(true);
	}

	private void Awake()
	{
		file_display = transform.Find("Text").GetComponent<TMP_Text>();
		File_Handler = GameObject.Find("File_Handler").GetComponent<File_Plugin_Behavior>();
	}
}
