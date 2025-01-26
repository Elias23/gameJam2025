using System;
using UnityEngine;

namespace Core
{
    public abstract class TutorialStep
    {
        public string tutorialText;
        public bool isCompleted = false;
        public Action onCompleted;

        protected TutorialStep(Action onCompleted = null)
        {
            this.onCompleted = onCompleted;
        }

        public abstract bool CheckCompletion();

        public virtual void Initialize()
        {
        }
    }

    public class TimedStep : TutorialStep
    {
        private float duration;
        private float startTime;

        public TimedStep(string text, float duration, Action onCompleted = null) : base(onCompleted)
        {
            this.tutorialText = text;
            this.duration = duration;
        }

        public override void Initialize()
        {
            startTime = Time.time;
        }

        public override bool CheckCompletion()
        {
            return Time.time - startTime >= duration;
        }
    }

    public class EventCountStep : TutorialStep
    {
        public enum TutorialEvent
        {
            MovementKeysPressed,
            BubbleShot,
            ChargedBubbleShot,
            GarbageRepelled
        }

        private TutorialEvent eventToCheck;
        private int eventCount;
        private int currentEventCount = 0;

        public EventCountStep(string text, TutorialEvent eventToCheck, int eventCount, Action onCompleted = null) :
            base(onCompleted)
        {
            this.tutorialText = text;
            this.eventToCheck = eventToCheck;
            this.eventCount = eventCount;
        }

        public override bool CheckCompletion()
        {
            return currentEventCount >= eventCount;
        }

        public void OnEvent(TutorialEvent tutorialEvent)
        {
            if (tutorialEvent == eventToCheck)
            {
                currentEventCount++;
            }
        }
    }
}