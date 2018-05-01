using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGSystem
{
    [RequireComponent(typeof(Inventory))]
    public class QuestManager : MonoBehaviour
    {
        public List<Quest> ActiveQuests;
        public List<Quest> CompletedQuests;
        public List<Quest> FailedQuests;

        private Inventory inventory;
        // Use this for initialization
        void Start()
        {
            inventory = GetComponent<Inventory>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void AddQuest(Quest quest)
        {
            ActiveQuests.Add(quest);
            quest.OnQuestStateChanged.AddListener(OnQuestUpdate);
        }

        void OnQuestUpdate(Quest quest)
        {
            if (quest.State == QuestState.Completed)
            {
                ActiveQuests.Remove(quest);
                if (inventory.GetEmptySlotCount() >= quest.Rewards.Count)
                {
                    CompletedQuests.Add(quest);
                    foreach (ItemReward reward in quest.Rewards)
                    {
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
        }
    }
}
