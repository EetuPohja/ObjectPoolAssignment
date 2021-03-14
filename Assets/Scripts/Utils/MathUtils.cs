﻿using UnityEngine;

namespace CursedWoods.Utils
{
    public static class MathUtils
    {
        public static Quaternion GetLookRotationYAxis(Vector3 pos1, Vector3 pos2, Vector3 up)
        {
            Quaternion wantedRot = Quaternion.LookRotation(pos1 - pos2, up);
            wantedRot.x = 0f;
            wantedRot.z = 0f;
            return wantedRot;
        }
    }
}