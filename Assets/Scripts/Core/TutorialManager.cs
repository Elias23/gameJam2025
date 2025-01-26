using System.Collections;
using System.Collections.Generic;
using Ship;
using TMPro;
using UnityEngine;

namespace Core
{
    public class TutorialManager : MonoBehaviour
    {
        [Header("Tutorial Settings")] [SerializeField]
        private TextMeshProUGUI tutorialText;

        [SerializeField] private CanvasGroup textContainer;
        [SerializeField] private float fadeTime = 0.3f;

        public static TutorialManager Instance { get; private set; }

        private List<TutorialStep> tutorialSteps = new List<TutorialStep>();
        private int currentStepIndex = -1;
        private TutorialStep currentStep = null;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            tutorialText.text = "";
        }

        private void Start()
        {
            SetupTutorial();
            StartNextStep();
        }

        private void SetupTutorial()
        {
            var ships = Object.FindObjectsByType<ShipBehaviour>(FindObjectsSortMode.None);
            foreach (var ship in ships)
            {
                ship.enabled = false;
            }

            tutorialSteps.Add(new TimedStep(
                "Just another peaceful day in your cozy home at the bottom of the sea...",
                4f
            ));

            tutorialSteps.Add(new EventCountStep(
                "You can move around using the arrow keys",
                EventCountStep.TutorialEvent.MovementKeysPressed,
                3
            ));

            tutorialSteps.Add(new EventCountStep(
                "Great! Now tap to shoot bubbles!",
                EventCountStep.TutorialEvent.BubbleShot,
                5
            ));

            tutorialSteps.Add(new EventCountStep(
                "Hold to charge a bigger bubble - it's faster and packs more punch!",
                EventCountStep.TutorialEvent.ChargedBubbleShot,
                1
            ));

            tutorialSteps.Add(new TimedStep(
                "Wait... what's that shadow above?",
                3f,
                () =>
                {
                    foreach (var ship in ships)
                    {
                        ship.enabled = true;
                    }
                }
            ));

            tutorialSteps.Add(new TimedStep(
                "A ship! And they're...",
                2f
            ));

            tutorialSteps.Add(new TimedStep(
                "...dumping their garbage into our ocean!?",
                2.5f
            ));

            tutorialSteps.Add(new TimedStep(
                "Those polluters! We need to protect our home!",
                2.5f
            ));

            tutorialSteps.Add(new TimedStep(
                "Quick! Use your bubbles to bounce their garbage back at them!",
                3f
            ));

            tutorialSteps.Add(new EventCountStep(
                "Hit them with their own trash! Try reflecting one piece",
                EventCountStep.TutorialEvent.GarbageRepelled,
                1
            ));

            tutorialSteps.Add(new TimedStep(
                "Perfect! Keep it up - let's teach them to respect our ocean!",
                4f
            ));
        }

        public void HandleTutorialEvent(EventCountStep.TutorialEvent tutorialEvent)
        {
            if (currentStep != null && currentStep is EventCountStep step)
            {
                step.OnEvent(tutorialEvent);
            }
        }

        private void Update()
        {
            if (currentStep != null && !currentStep.isCompleted)
            {
                if (currentStep.CheckCompletion())
                {
                    CompleteCurrentStep();
                }
            }
        }

        private void StartNextStep()
        {
            currentStepIndex++;
            if (currentStepIndex < tutorialSteps.Count)
            {
                currentStep = tutorialSteps[currentStepIndex];
                currentStep.Initialize();
                UpdateTutorialText(currentStep.tutorialText);
            }
            else
            {
                EndTutorial();
            }
        }

        private void CompleteCurrentStep()
        {
            if (currentStep != null)
            {
                currentStep.isCompleted = true;
                currentStep.onCompleted?.Invoke();
                StartNextStep();
            }
        }

        private void UpdateTutorialText(string text)
        {
            StartCoroutine(FadeTextRoutine(text));
        }

        private void EndTutorial()
        {
            currentStep = null;
            UpdateTutorialText("");
            tutorialText.enabled = false;

            // Hide tutorial UI
            gameObject.SetActive(false);
        }

        private IEnumerator FadeTextRoutine(string newText)
        {
            float elapsedTime = 0;
            while (elapsedTime < fadeTime)
            {
                textContainer.alpha = Mathf.Lerp(1, 0, elapsedTime / fadeTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            tutorialText.text = newText;

            elapsedTime = 0;
            while (elapsedTime < fadeTime)
            {
                textContainer.alpha = Mathf.Lerp(0, 1, elapsedTime / fadeTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }
}