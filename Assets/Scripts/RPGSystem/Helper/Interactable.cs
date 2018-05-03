using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGSystem
{
    [System.Serializable]
    public enum InteractionType
    {
        Inventory,
        Dialogue
    }

    public class Interactable : MonoBehaviour
    {
        public InteractionType type;

        //[HideInInspector]
        //public MonoBehaviour Get
        //{
        //    get
        //    {
        //        switch (type)
        //        {
        //            case InteractionType.Inventory:
        //                return GetComponent<Inventory>();
        //            case InteractionType.Dialogue:
        //                return GetComponent<DialogueTrigger>();
        //            default:
        //                return null;
        //        }
        //    }
        //    private set
        //    {
        //        // Nothing to see here
        //    }
        //}
    }
}
