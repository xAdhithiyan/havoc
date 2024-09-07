using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// System.Serializble is used to serialize a class so that fields that use the class are also serialized (if the fields are public)
[System.Serializable]
public class EachDialoge
{
	public string name;

	[TextArea(3, 10)]
	public string sentence;
}


[CreateAssetMenu(fileName = "New Dialoge", menuName = "Dialoge")]
public class Dialoge : ScriptableObject
{
	public EachDialoge[] dialoges;
}
