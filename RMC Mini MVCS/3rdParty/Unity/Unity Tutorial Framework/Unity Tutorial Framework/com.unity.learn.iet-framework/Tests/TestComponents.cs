using System;
using UnityEngine;

namespace Unity.Tutorials.Core.Tests
{
    static class TestComponents
    {
        // Nest type to avoid it showing up in the "Add Component" menu
        public class ComponentWithNestedValues : MonoBehaviour
        {
            public A componentWithNestedValuesFieldA;

            [Serializable]
            public struct A
            {
                public int componentWithNestedValuesFieldB;
            }
        }
    }
}
