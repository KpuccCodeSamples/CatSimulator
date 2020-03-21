using System;
using System.Collections;
using UnityEngine;

public enum CatMood
{
    Bad = 0,
    Good = 1,
    Awesome = 2
}

public class Cat : MonoBehaviour
{
    private CatMood m_Mood = CatMood.Good;
    public CatMood Mood
    {
        get
        {
            return m_Mood;
        }

        set
        {
            if (m_Mood == value)
                return;

            m_Mood = value;

            OnMoodChanged?.Invoke(m_Mood);
        }
    }

    public ParticleSystem m_KickParticles;
    public Animator m_Animator;

    private bool m_IsEating;
    private Coroutine m_EatingCoroutine;

    public static event Action<CatMood> OnMoodChanged;
    public static event Action<string> OnReactionPerformed;

    ///////////////
    private void OnEnable()
    {
        Controller.OnCatImpactPerformed += TryApplyImpactOnCat;
    }

    ///////////////
    private void OnDisable()
    {
        Controller.OnCatImpactPerformed -= TryApplyImpactOnCat;
    }

    ///////////////
    private void Start()
    {
        // инициализируем стартовое состояние кошки
        m_KickParticles.Stop();
    }

    ///////////////
    private void TryApplyImpactOnCat(Cat cat, OnCatImpactType impactType)
    {
        if (cat != this)
            return;

        if (TryInterruptEatingWithReaction())
            return;

        // останавливаем эффекты
        m_KickParticles.Stop();

        switch (impactType)
        {
            case OnCatImpactType.Feed:
                OnCatFeed();
                break;

            case OnCatImpactType.Kick:
                OnCatKicked();
                break;

            case OnCatImpactType.Play:
                OnPlayWithCat();
                break;

            case OnCatImpactType.Stroke:
                OnCatStroked();
                break;
        }
    }

    ///////////////
    private void OnPlayWithCat()
    {
        switch (Mood)
        {
            case CatMood.Bad:
                PerformReaction("Сидит на месте");
                break;

            case CatMood.Good:
                PerformReaction("Медленно бегает за мячиком");
                Mood = CatMood.Awesome;
                break;

            case CatMood.Awesome:
                PerformReaction("Носится, как угорелая");
                break;
        }
    }

    ///////////////
    private void OnCatKicked()
    {
        m_Animator.SetTrigger("Kicked");
        StartCoroutine(TryStartImpactParticles());

        switch (Mood)
        {
            case CatMood.Bad:
                PerformReaction("Прыгает и кусает за правое ухо");
                break;

            case CatMood.Good:
                PerformReaction("Убегает на ковер и писает");
                Mood = CatMood.Bad;
                break;

            case CatMood.Awesome:
                PerformReaction("Убегает в другую комнату");
                Mood = CatMood.Bad;
                break;
        }
    }

    ///////////////
    private void OnCatFeed()
    {
        switch (Mood)
        {
            case CatMood.Bad:
                if (m_EatingCoroutine != null)
                {
                    StopCoroutine(m_EatingCoroutine);
                    m_EatingCoroutine = null;
                }

                m_EatingCoroutine = StartCoroutine(StartEat());
                break;

            case CatMood.Good:
                PerformReaction("Быстро все съедает");
                Mood = CatMood.Awesome;
                break;

            case CatMood.Awesome:
                PerformReaction("Быстро все съедает");
                break;
        }
    }

    ///////////////
    private void OnCatStroked()
    {
        switch (Mood)
        {
            case CatMood.Bad:
                PerformReaction("ЦАП ЦАРАП");
                break;

            case CatMood.Good:
                PerformReaction("Мурлычет");
                Mood = CatMood.Awesome;
                break;

            case CatMood.Awesome:
                PerformReaction("Мурлычет и машет хвостом");
                break;
        }
    }

    ///////////////
    private void PerformReaction(string reactionDescription)
    {
        OnReactionPerformed?.Invoke(reactionDescription);
    }

    ///////////////
    private bool TryInterruptEatingWithReaction()
    {
        if (m_IsEating)
        {
            if (m_EatingCoroutine != null)
            {
                StopCoroutine(m_EatingCoroutine);
                m_EatingCoroutine = null;
            }

            PerformReaction("ЦАП ЦАРАП, АППЕТИТ ИСПОРТИЛ!");
            m_IsEating = false;
            return true;
        }

        return false;
    }

    ///////////////
    private IEnumerator StartEat()
    {
        m_IsEating = true;
        PerformReaction("Жую вискас, не мешай");

        yield return new WaitForSecondsRealtime(3f);

        m_IsEating = false;
        PerformReaction("Все съела");
        Mood = CatMood.Good;
    }

    ///////////////
    private IEnumerator TryStartImpactParticles()
    {
        m_KickParticles.Play();

        yield return new WaitForSecondsRealtime(3f);

        m_KickParticles.Stop();
    }
}
