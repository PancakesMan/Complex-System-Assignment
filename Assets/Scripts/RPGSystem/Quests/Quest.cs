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
        Failed,
        Completed
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
        public QuestState State;
        public ObjectiveType Type;

        // Require Item objective variables
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
                            State = QuestState.Completed;
                            OnStateChanged.Invoke(State);
                        }
                        break;
                    default:
                        break;
                }
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

        public QuestStateChangedEvent OnQuestStateChanged;

        // Use this for initialization
        void Start()
        {
            foreach (QuestObjective qo in Objectives)
                qo.OnStateChanged.AddListener(OnQuestUpdate);
        }

        // Update is called once per frame
        void Update()
        {
        }

        void OnQuestUpdate(QuestState state)
        {
            if (State == QuestState.Active)
            {
                foreach (QuestObjective qo in Objectives)
                    if (qo.State == QuestState.Failed)
                    {
                        State = QuestState.Failed;
                        OnQuestStateChanged.Invoke(this);
                        return;
                    }

                if (State != QuestState.Failed)
                {
                    QuestState temp = Objectives.All(a => a.State == QuestState.Completed) ? QuestState.Completed : State;
                    if (temp == QuestState.Completed)
                    {
                        State = QuestState.Completed;
                        OnQuestStateChanged.Invoke(this);
                    }
                }
            }
        }
    }
}