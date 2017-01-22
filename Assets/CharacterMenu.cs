using UnityEngine;
using System.Collections;
using System;

public class CharacterMenu : MonoBehaviour
{
	private Animator _animator;
	public GameObject mainMenu;
	public GameObject tutorialMenu;
	public GameObject topBarMenu;


	void Start ()
	{
		_animator = GetComponent<Animator> ();

		Utility.Instance.WaitTillAnimationTime (_animator, .95f, 1, () => {
			_animator.SetTrigger ("StopWalking");

			StartCoroutine(
				WaitForAnyKey(()=>{
					CanvasGroup maincgroup = mainMenu.GetComponent<CanvasGroup>();
					LeanTween.alphaCanvas(maincgroup, 0, .2f);

					tutorialMenu.SetActive(true);

					StartCoroutine(
						WaitForAnyKey(()=>{

							CanvasGroup tutcgroup = tutorialMenu.GetComponent<CanvasGroup>();
							LeanTween.alphaCanvas(tutcgroup, 0, .2f);
							topBarMenu.SetActive(true);
							gameObject.SetActive(false);
							GameManager.Instance.StartGame();
						})
					);
				})
			);
		});
	}

	IEnumerator WaitForAnyKey(Action callback)
	{
		while(!Input.anyKeyDown)
		{
			yield return true;
		}
		yield return true;
		callback ();

	}

}

