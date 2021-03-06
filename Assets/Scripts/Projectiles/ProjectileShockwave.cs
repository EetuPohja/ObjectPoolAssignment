﻿using UnityEngine;

namespace CursedWoods
{
    public class ProjectileShockwave : ProjectileBase
    {
        /// <summary>
        /// Determinates how large of an area the shockwave covers at it's maximum scale.
        /// </summary>
        [SerializeField]
        private float targetScale = 10f;

        /// <summary>
        /// How fast the shockwave scales up.
        /// </summary>
        [SerializeField]
        private float scaleSpeed = 5f;

        /// <summary>
        /// The starting scale of the shockwave.
        /// </summary>
        private float startScale = 0.1f;

        /// <summary>
        /// The current scale of the shockwave.
        /// </summary>
        private float currentScale = 0.1f;

        /// <summary>
        /// What is the acceptable difference between shockwave's current scale and targetscale while lerping.
        /// </summary>
        private float targetScaleOffAmount = 0.1f;

        private void Update()
        {
            Scale(Time.deltaTime);
        }

        public override void Activate(Vector3 pos, Quaternion rot)
        {
            base.Activate(pos, rot);
            currentScale = startScale;
            transform.localScale = new Vector3(currentScale, currentScale, currentScale);
        }

        /// <summary>
        /// Scales the shockwave to it's targetscale and then deactivates it.
        /// </summary>
        /// <param name="deltaTime">Takes delta time, makes sure scaling is framerate independent.</param>
        private void Scale(float deltaTime)
        {
            currentScale = Mathf.Lerp(currentScale, targetScale, scaleSpeed * deltaTime);
            transform.localScale = new Vector3(currentScale, currentScale, currentScale);
            if (currentScale >= targetScale - targetScaleOffAmount)
            {
                Deactivate();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            //TODO: affect enemies
        }
    }
}