using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Drawing;

public class Tenna : MonoBehaviour
{
    [DllImport("user32.dll")]
    public static extern bool GetCursorPos(out POINT lpPoint);

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int x;
        public int y;
    }

    [DllImport("user32.dll")]
    public static extern bool SetCursorPos(int X, int Y);

    public CanvasGroup canvasGroup;

    public Button button;

    public GameObject textArea;
    public GameObject selectButton;
    public Button yesBtn;
    public Button noBtn;

    public Text myText;

    public Animator anim;

    public RectTransform mousePos1;
    public RectTransform mousePos2;

    private void Start()
    {
        button.onClick.AddListener(() => ShowText("╫н©Л╫г?", () =>
        {
            selectButton.SetActive(true);
            canvasGroup.interactable = true;
        }));
        yesBtn.onClick.AddListener(SetYes);
        noBtn.onClick.AddListener(SetNo);
    }

    private void ShowText(string text, Action endAction = null)
    {
        textArea.SetActive(true);
        StartCoroutine(PrintingText(text, endAction));
        canvasGroup.interactable = false;
    }

    private IEnumerator PrintingText(string text, Action endAction)
    {
        float progress = 0f;

        while(progress <= 1f)
        {
            progress += Time.deltaTime * 2f;

            int maxIndex = (int)(text.Length * progress);

            myText.text = text[0..maxIndex];

            yield return null;
        }

        endAction?.Invoke();

        yield break;
    }

    private void SetYes()
    {
        selectButton.SetActive(false);
        textArea.SetActive(false);
        StartCoroutine(TennaKick());
        canvasGroup.interactable = false;

    }

    private void SetNo()
    {
        selectButton.SetActive(false);
        textArea.SetActive(false);
        StartCoroutine(TennaLaugh());
        canvasGroup.interactable = false;
    }

    private IEnumerator TennaKick()
    {
        Vector3 readyPos = mousePos1.anchoredPosition + new Vector2(Screen.width / 2f, -(Screen.height / 2f));
        readyPos.y = -readyPos.y;

        yield return SetMousePos(readyPos, 0.5f);

        anim.SetTrigger("Kick");

        yield return new WaitForSeconds(0.5f);

        Vector3 kickPos = mousePos2.anchoredPosition + new Vector2(Screen.width / 2f, -(Screen.height / 2f));
        kickPos.y = -kickPos.y;

        yield return SetMousePos(kickPos, 0.1f);

        canvasGroup.interactable = true;

        yield break;
    }

    private IEnumerator SetMousePos(Vector3 pos, float duration)
    {
        float progress = 0f;

        GetCursorPos(out POINT currentMousePos);

        Vector3 startPos = new Vector3(currentMousePos.x, currentMousePos.y);

        while(progress <= 1f)
        {
            progress += Time.deltaTime / duration;

            Vector3 finalPos = Vector2.Lerp(startPos, pos, progress);

            SetCursorPos((int)finalPos.x, (int)finalPos.y);
            yield return null;
        }
        yield break;
    }

    private IEnumerator TennaLaugh()
    {
        ShowText("бл? ╓╩");

        anim.SetTrigger("Laugh");

        yield return new WaitForSeconds(1f);

        Vector2 startPos = (gameObject.transform as RectTransform).anchoredPosition;
        Vector2 endPos = startPos - new Vector2(0, 700);

        float progress = 0f;

        while(progress <= 1f)
        {
            progress += Time.deltaTime / 5f;

            if(progress >= 0.2f && textArea.activeSelf)
                textArea.SetActive(false);

            (gameObject.transform as RectTransform).anchoredPosition = Vector3.Lerp(startPos, endPos, progress);

            yield return null;
        }

        Application.Quit();
        yield break;
    }
}
