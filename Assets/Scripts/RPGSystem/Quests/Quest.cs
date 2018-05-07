using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

namespace RPGSystem
{
    [System.Serializable]
    public struct ItemReward
    {
        public Item item;
        public int amount;
    }

    [System.Serializable]
    public class ObjectiveStateChangedEvent : UnityEvent<QuestState>
    { }

    [System.Serializable]
    public class QuestStateChangedEvent : UnityEvent<Quest>
    { }

    public enum QuestState
    {
        Active,
        Finished,
        Completed,
        Failed
    }

    public enum ObjectiveType
    {
        Kill,
        KeepAlive,
        RequireItem
    }

    [System.Serializable]
    public class QuestObjective
    {
        [HideInInspector]
        public QuestState State = QuestState.Active;
        public ObjectiveType Type;

        // Require Item objective variables
        // TODO hide this if ObjectiveType is not RequireItem
        public Inventory inventory;
        public Item requiredItem;
        public int itemCount;

        public ObjectiveStateChangedEvent OnStateChanged;

        public void Update()
        {
            if (State == QuestState.Active)
            {
                switch (Type)
                {
                    case ObjectiveType.KeepAlive:
                        break;
                    case ObjectiveType.Kill:
                        break;
                    case ObjectiveType.RequireItem:
                        if (inventory.GetItemCount(requiredItem) >= itemCount)
                        {
                            // If the inventory has the number of items to complete the quest
                            // Update the State of the objective and fire the StateChanged event
                            State = QuestState.Completed;
                            OnStateChanged.Invoke(State);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        public override string ToString()
        {
            switch (Type)
            {
                case ObjectiveType.RequireItem:
                    return string.Format("{0} {1}/{2}", requiredItem.name, inventory.GetItemCount(requiredItem), itemCount);
                default:
                    return "";
            }
        }
    }

    [CreateAssetMenu(fileName = "New Quest", menuName = "RPG System/Quest")]
    public class Quest : ScriptableObject
    {
        [HideInInspector]
        public QuestState State = QuestState.Active;
        public string Description;
        public List<QuestObjective> Objectives;
        public List<ItemReward> Rewards;
        public bool AutoComplete = false;

        public QuestStateChangedEvent OnQuestStateChanged;

        // Use this for initialization
        void Start()
        {
            // Add a StateChanged listener to event QuestObjective
            foreach (QuestObjective qo in Objectives)
                qo.OnStateChanged.AddListener(OnQuestUpdate);
        }

        void OnQuestUpdate(QuestState state)
        {
            if (State == QuestState.Active)
            {
                foreach (QuestObjective qo in Objectives)
                    if (qo.State == QuestState.Failed)
                    {
                        // if you fail any of the objectives
                        // Change the Quest's State to failed
                        State = QuestState.Failed;

                        // Fire the QuestStateChanged event
                        OnQuestStateChanged.Invoke(this);
                        return;
                    }

                if (State != QuestState.Failed)
                {
                    // If you haven't failed the quest
                    // check if all the objectives are finished
                    QuestState temp = Objectives.All(a => a.State == QuestState.Finished) ? QuestState.Finished : State;
                    if (temp == QuestState.Finished)
                    {
                        // If all the objectives are finished
                        // Update the state of the quest
                        State = AutoComplete ? QuestState.Completed : QuestState.Finished;

                        // Fire the QuestStateChanged event
                        OnQuestStateChanged.Invoke(this);
                    }
                }
            }
        }
    }
}