using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogeManager : MonoBehaviour
{
  private Queue<EachDialoge> dialogesQueue;
  public TextMeshProUGUI DialogeText;
  public TextMeshProUGUI NameText;
  [SerializeField] private Animator animator;
  [SerializeField] private Animator eButtonAnimator;
  [SerializeField] private DisablePlayerMechanics player;

  public bool dialogeActive = true;
  private bool lastDialoge = false;

  void Start()
  {
		dialogesQueue = new Queue<EachDialoge>();
  }

  public void startDialoge(Dialoge dialoge, bool lastDialgueBool)
  {
		lastDialoge = lastDialgueBool;

		dialogesQueue.Clear();
    dialogeActive = true;
    foreach (EachDialoge currentDialoge in dialoge.dialoges)
    {
			dialogesQueue.Enqueue(currentDialoge);
    }

    eButtonAnimator.SetBool("isOpen", false);
    animator.SetBool("isOpen", true);
    DisplayNextSentence();

    player.disableMovement();
  }

  public void DisplayNextSentence()
  {
    if (dialogesQueue.Count == 0)
    {
      EndDialoge();
      return;  
    }
    EachDialoge currentDialoge = dialogesQueue.Dequeue();
    NameText.text = currentDialoge.name;
    StopAllCoroutines();
    StartCoroutine(TypeSentence(currentDialoge.sentence));
  }
  IEnumerator TypeSentence(string sentence)
  {
    DialogeText.text = "";
    foreach (char i in sentence)
    {
      DialogeText.text += i;
      yield return new WaitForSeconds(0.03f);
    }
  }

  public void EndDialoge()
  {
    animator.SetBool("isOpen", false);
		dialogeActive = false;

    Debug.Log(lastDialoge);
    if (lastDialoge)
    {
      player.startNextSceneCollider = true;
		}
    player.enableMovement();
  }
}
