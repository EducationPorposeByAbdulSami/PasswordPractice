using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BitWave_Labs.AnimatedTextReveal
{
    public class AnimateText : MonoBehaviour
    {
        private enum FadeMode { FadeIn, FadeOut, FadeInAndOut }

        [SerializeField] private AnimatedTextReveal animatedTextReveal;
        [SerializeField] private List<string> lines;
        [SerializeField] private FadeMode fadeMode;
        [SerializeField] private bool fadeLastLine;
        [SerializeField] private float delayBeforeFadeOut = 1f;
        [SerializeField] private float delayBeforeFadeIn = 1f;

        [Header("UI Elements")]
        [SerializeField] private Button continueButton;
        [SerializeField] private Button homeButton;
        [SerializeField] private GameObject popupImage; // assign in Inspector

        private int currentIndex = 0;
        private bool linePlaying = false;

        private void Start()
        {
            if (continueButton != null && homeButton != null)
            {
                continueButton.gameObject.SetActive(false);
                homeButton.gameObject.SetActive(false);
            }

            if (popupImage != null)
                popupImage.SetActive(false);

            ShowNextLine();
        }

        private void Update()
        {
            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && !linePlaying)
            {
                ShowNextLine();
            }
        }

        private void ShowNextLine()
        {
            if (currentIndex < lines.Count)
                StartCoroutine(PlayLine(lines[currentIndex]));
            else
                ShowPopup();
        }

        private IEnumerator PlayLine(string line)
        {
            linePlaying = true;

            animatedTextReveal.TextMesh.alignment = TextAlignmentOptions.TopLeft;
            animatedTextReveal.TextMesh.text = line;
            animatedTextReveal.SetAllCharactersAlpha(0);

            if (fadeMode is FadeMode.FadeIn or FadeMode.FadeInAndOut)
                yield return StartCoroutine(animatedTextReveal.FadeText(true));

            if (currentIndex == lines.Count - 1)
            {
                if (!fadeLastLine)
                {
                    ShowPopup();
                    linePlaying = false;
                    yield break;
                }
            }

            if (fadeMode is FadeMode.FadeOut or FadeMode.FadeInAndOut)
            {
                yield return new WaitForSeconds(delayBeforeFadeOut);
                yield return StartCoroutine(animatedTextReveal.FadeText(false));
            }

            yield return new WaitForSeconds(delayBeforeFadeIn);

            currentIndex++;
            linePlaying = false;
        }

        private void ShowPopup()
        {
            if (continueButton != null && homeButton != null)
            {
                continueButton.gameObject.SetActive(true);
                homeButton.gameObject.SetActive(true);
            }

            if (popupImage != null)
                popupImage.SetActive(true);
        }
    }
}
