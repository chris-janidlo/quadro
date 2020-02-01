using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HitText : MonoBehaviour, IDriverSubscriber
{
	public ADriver Driver { get; set; }

    public float InitialTime, FadeTime;

    public TextMeshProUGUI Text;

    void Start ()
    {
        Text.text = "";
        Driver.Player.Hit += hit => { StopAllCoroutines(); StartCoroutine(messageRoutine(hit)); };
    }

    IEnumerator messageRoutine (HitData hit)
    {
        Text.text = $"<#{ColorUtility.ToHtmlStringRGB(hit.Quality.Color())}>{hit.ShortDescription()}</color>";
        Text.alpha = 1;

        yield return new WaitForSeconds(InitialTime);

        float timer = 0;

        while (timer < FadeTime)
        {
            Text.alpha = Mathf.Lerp(1, 0, timer / FadeTime);
            timer += Time.deltaTime;
            yield return null;
        }

        Text.alpha = 0;
        Text.text = "";
    }
}
