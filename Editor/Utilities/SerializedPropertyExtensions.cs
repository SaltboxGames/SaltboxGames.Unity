using SaltboxGames.Core.Shims;
using UnityEditor;

namespace SaltboxGames.Unity.Editor.Utilities
{
    public static class SerializedPropertyExtensions
    {
        public static SafeGuid GetGuidValue(this SerializedProperty p)
        {
            int s1 = p.FindPropertyRelative("segment1").intValue;
            int s2 = p.FindPropertyRelative("segment2").intValue;
            int s3 = p.FindPropertyRelative("segment3").intValue;
            int s4 = p.FindPropertyRelative("segment4").intValue;
            return SafeGuid.FromSegments(s1, s2, s3, s4);
        }

        public static void SetGuidValue(this SerializedProperty p, SafeGuid guid)
        {
            guid.Decompose(out int s1, out int s2, out int s3, out int s4);
            p.FindPropertyRelative("segment1").intValue = s1;
            p.FindPropertyRelative("segment2").intValue = s2;
            p.FindPropertyRelative("segment3").intValue = s3;
            p.FindPropertyRelative("segment4").intValue = s4;
        }
    }
}
