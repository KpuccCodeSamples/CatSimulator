using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class View : MonoBehaviour
{
    public Image m_MoodIndicator;
    public TMP_Text m_MoodText;
    public TMP_Text m_ReactionDescription;

    private Color[] m_IndicatorColors = new Color[3] { Color.red, Color.yellow, Color.green };

    ///////////////
    private void OnEnable()
    {
        Cat.OnMoodChanged += UpdateMoodInfo;
        Cat.OnReactionPerformed += UpdateReactionDescription;
    }

    ///////////////
    private void OnDisable()
    {
        Cat.OnMoodChanged -= UpdateMoodInfo;
        Cat.OnReactionPerformed -= UpdateReactionDescription;
    }

    ///////////////
    public void InitView(CatMood mood)
    {
        UpdateMoodInfo(mood);
        m_ReactionDescription.text = string.Empty;
    }

    ///////////////
    private void UpdateMoodInfo(CatMood mood)
    {
        m_MoodText.text = mood.ToString();
        m_MoodIndicator.color = m_IndicatorColors[(int)mood];
    }

    ///////////////
    private void UpdateReactionDescription(string reacrtionDescription)
    {
        m_ReactionDescription.text = reacrtionDescription;
    }
}