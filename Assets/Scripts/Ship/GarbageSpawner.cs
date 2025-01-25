using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Ship
{
    public class GarbageItem
    {
        public float WeightClass { get; }
        public float SpawnProbability { get; }

        public GameObject Prefab { get; }

        public GarbageItem(float weightClass, float spawnProbability, GameObject prefab)
        {
            WeightClass = weightClass;
            SpawnProbability = spawnProbability;
            Prefab = prefab;
        }
    }

    public class GarbageSpawner
    {
        // constructor parameters
        private readonly float baseWeightPerMin;
        private readonly float weightIncreasePerMin;
        private readonly float minDropInterval;
        private readonly float heavyItemCooldown;
        private readonly int recentWeightsMemory;

        // other properties
        private float gameTime = 0f;
        private float lastDropTime = 0f;
        private float targetWeightPerMinute;
        private float actualWeightThisMinute = 0f;
        private float minuteTimer = 0f;
        private float lastHeavyDropTime = 0f;
        private Queue<float> recentWeights = new Queue<float>();

        private List<GarbageItem> garbageItems;

        public GarbageSpawner(List<GarbageItem> garbageItems, float baseWeightPerMin, float weightIncreasePerMin,
            float minDropInterval,
            float heavyItemCooldown, int recentWeightsMemory)
        {
            this.garbageItems = garbageItems;
            this.baseWeightPerMin = baseWeightPerMin;
            this.weightIncreasePerMin = weightIncreasePerMin;
            this.minDropInterval = minDropInterval;
            this.heavyItemCooldown = heavyItemCooldown;
            this.recentWeightsMemory = recentWeightsMemory;
        }

        public void Update(float deltaTime)
        {
            gameTime += deltaTime;
            minuteTimer += deltaTime;

            // Reset minute counters
            if (minuteTimer >= 60f)
            {
                actualWeightThisMinute = 0f;
                minuteTimer = 0f;
            }

            // Calculate target weight based on time
            targetWeightPerMinute = baseWeightPerMin +
                                    (gameTime / 60f) * weightIncreasePerMin;
        }

        public bool ShouldDropNewGarbage(float currentTime)
        {
            if (currentTime - lastDropTime < minDropInterval) return false;

            // Check if we are behind on target difficulty
            var weightDeficit = (targetWeightPerMinute * (minuteTimer / 60f)) - actualWeightThisMinute;

            var spawnChance = Mathf.Clamp01(weightDeficit / targetWeightPerMinute);
            return Random.value < spawnChance;
        }

        public GarbageItem SelectGarbageType(float currentTime)
        {
            // Calculate average recent weight
            var avgRecentWeight = recentWeights.Count > 0 ? recentWeights.Average() : 0f;

            // Adjust probabilities based on recent drops
            var adjustedTypes = garbageItems.Select(item => new
            {
                Item = item,
                AdjustedProbability = CalculateAdjustedProbability(
                    item,
                    currentTime,
                    avgRecentWeight
                )
            }).ToList();

            // Normalize probabilities
            var totalProb = adjustedTypes.Sum(x => x.AdjustedProbability);
            var randomValue = Random.value * totalProb;

            var currentProb = 0f;
            foreach (var type in adjustedTypes)
            {
                currentProb += type.AdjustedProbability;
                if (randomValue <= currentProb)
                {
                    UpdateRecentWeights(type.Item.WeightClass);
                    lastDropTime = currentTime;
                    if (type.Item.WeightClass >= 5f)
                        lastHeavyDropTime = currentTime;
                    actualWeightThisMinute += type.Item.WeightClass;
                    return type.Item;
                }
            }

            return adjustedTypes[0].Item; // Fallback to first item
        }

        private float CalculateAdjustedProbability(
            GarbageItem item,
            float currentTime,
            float avgRecentWeight
        )
        {
            var probability = item.SpawnProbability;

            // Reduce heavy item probability if one was dropped recently
            if (item.WeightClass >= 5f &&
                currentTime - lastHeavyDropTime < heavyItemCooldown)
            {
                probability *= 0.2f;
            }

            // Adjust based on weight target
            var weightDiff = targetWeightPerMinute / 60f - avgRecentWeight;
            if ((weightDiff > 0 && item.WeightClass > avgRecentWeight) ||
                (weightDiff < 0 && item.WeightClass < avgRecentWeight))
            {
                probability *= 1.5f;
            }

            return probability;
        }

        private void UpdateRecentWeights(float weight)
        {
            recentWeights.Enqueue(weight);
            if (recentWeights.Count > recentWeightsMemory)
            {
                recentWeights.Dequeue();
            }
        }
    }
}