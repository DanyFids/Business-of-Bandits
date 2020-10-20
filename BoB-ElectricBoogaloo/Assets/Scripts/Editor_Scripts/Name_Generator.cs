using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Name_Generator : MonoBehaviour
{
	private static string CHARACTERS = "abcdefghijklmnopqrstuvwxyz0123456789";
	private static int NAME_LENGTH = 8;

	public static string Run()
	{
		string ret = "";

		for (int c = 0; c < NAME_LENGTH; c++)
		{
			ret += CHARACTERS[Random.Range(0, CHARACTERS.Length)];
		}

		return ret;
	}

	public static string Run_Unique(GameObject parent)
	{
		string ret;
		bool found = false;

		do
		{
			ret = Run();
			found = false;

			for (int c = 0; c < parent.transform.childCount; c++)
			{
				if (parent.transform.GetChild(c).gameObject.name == ret)
				{
					found = true;
					break;
				}
			}

		} while (found);

		return ret;
	}
}
