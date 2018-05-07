using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGSystem
{
    [RequireComponent(typeof(Inventory))]
    public class QuestManager : MonoBehaviour
    {
        public List<Quest> ActiveQuests;
        public List<Quest> FinishedQuests;
        public List<Quest> CompletedQuests;
        public List<Quest> FailedQuests;

        private Inventory inventory;
        // Use this for initialization
        void Start()
        {
            inventory = GetComponent<Inventory>();
        }

        public void AddQuest(Quest quest)
        {
            // Add a quest the the ActiveQuests list and listen for updates
            ActiveQuests.Add(quest);
            quest.OnQuestStateChanged.AddListener(OnQuestUpdate);
        }

        void OnQuestUpdate(Quest quest)
        {
            // If the quest we're monitoring gets completed
            if (quest.State == QuestState.Completed)
            {
                // Remove it from the active quests list
                ActiveQuests.Remove(quest);
                if (inventory.GetEmptySlotCount() >= quest.Rewards.Count)
                {
                    // If the inventory has room for the rewards
                    // Add the quest to the completed quests list
                    CompletedQuests.Add(quest);
                    foreach (ItemReward reward in quest.Rewards)
                    {
                        // Add quest rewards to the inventory
                        Item item = Item.Copy(reward.item);
                        item.currentStacks = reward.amount > item.stackLimit ? item.stackLimit : reward.amount;
                    }
                }
                else
                {
                    //TODO
                    // No room in inventory for quest rewards
                }
            }

            // If the quest becomes finished
            else if (quest.State == QuestState.Finished)
            {
                // Remove from ActiveQuests list and add to FinishedQuests list
                ActiveQuests.Remove(quest);
                FinishedQuests.Add(quest);
            }
        }
    }
}
