using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;

namespace Unity.Tutorials.Core.Editor.Tests
{
    public class MaskingManagerTests
    {
        static IEnumerable GetHoleTestCases()
        {
            var rects = new List<Rect>()
            {
                new Rect(10, 10, 10, 10)
            };
            var area = new Rect(0, 0, 100, 100);
            var expected = new List<Rect>()
            {
                new Rect(0,  0, 100,  10),
                new Rect(0,  10, 10,  10),
                new Rect(20, 10, 80,  10),
                new Rect(0,  20, 100, 80),
            };
            yield return new TestCaseData(rects, area, expected).SetName("OneHoleCase");

            rects = new List<Rect>()
            {
                new Rect(10, 10, 10, 10),
                new Rect(30, 30, 10, 10)
            };
            area = new Rect(0, 0, 100, 100);
            expected = new List<Rect>()
            {
                new Rect(0,  0,  100, 10),
                new Rect(0,  10, 10,  10),
                new Rect(20, 10, 80,  10),
                new Rect(0,  20, 100, 10),
                new Rect(0,  30, 30,  10),
                new Rect(40, 30, 60,  10),
                new Rect(0,  40, 100, 60),
            };
            yield return new TestCaseData(rects, area, expected).SetName("TwoNonIntersectingHoles");

            rects = new List<Rect>()
            {
                new Rect(10, 10, 10, 10),
                new Rect(15, 15, 10, 10)
            };
            area = new Rect(0, 0, 100, 100);
            expected = new List<Rect>()
            {
                new Rect(0,  0,  100, 10),
                new Rect(0,  10, 10,  5),
                new Rect(20, 10, 80,  5),
                new Rect(0,  15, 10,  5),
                new Rect(25, 15, 75,  5),
                new Rect(0,  20, 15,  5),
                new Rect(25, 20, 75,  5),
                new Rect(0,  25, 100, 75),
            };
            yield return new TestCaseData(rects, area, expected).SetName("TwoIntersectingHoles");
        }

        [Test, TestCaseSource("GetHoleTestCases")]
        public void OneHoleTest(List<Rect> holes, Rect area, List<Rect> expectedRects)
        {
            var result = MaskingManager.GetNegativeSpaceRects(area, holes);

            var expected = expectedRects;

            AssertNoRectsIntersectHoles(result, holes);
            AssertNoRectsIntersectEachOther(result);

            Assert.AreEqual(expected, result);
        }

        private bool DoRectsIntersect(Rect a, Rect b)
        {
            return a.Overlaps(b);
        }

        private void AssertNoRectsIntersectRect(List<Rect> rects, Rect hole)
        {
            for (int j = 0; j < rects.Count; ++j)
            {
                var rj = rects[j];

                Assert.IsFalse(DoRectsIntersect(hole, rj), "There should be no Rects intersecting the hole");
            }
        }

        private void AssertNoRectsIntersectHoles(List<Rect> rects, List<Rect> holes)
        {
            for (int j = 0; j < holes.Count; ++j)
            {
                var hole = holes[j];
                AssertNoRectsIntersectRect(rects, hole);
            }
        }

        private void AssertNoRectsIntersectEachOther(List<Rect> rects)
        {
            for (int i = 0; i < rects.Count; ++i)
            {
                var ri = rects[i];
                for (int j = 0; j < rects.Count; ++j)
                {
                    if (i == j)
                        continue;
                    var rj = rects[j];

                    Assert.IsFalse(DoRectsIntersect(ri, rj), "Rects should not intersect each other");
                }
            }
        }
    }
}
