﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPGSystem
{
    [System.Serializable]
    public class Dialogue
    {
        public string name;

        [TextArea(3, 10)]
        public string[] sentences;
    }
}
