using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Util
{
    public class Returnable<T>
    {
        public T value { get; set; }

        public Returnable(T value)
        {
            this.value = value;
        }
    }
}

