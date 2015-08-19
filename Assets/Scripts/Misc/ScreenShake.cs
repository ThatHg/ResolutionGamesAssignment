using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour {
    private struct ValuePair {
        public readonly float amplitude;
        public readonly float duration;

        public ValuePair(float amplitude, float duration) {
            this.amplitude = amplitude;
            this.duration = duration;
        }
    }

    private float currentTime;
    private float addedTime;
    private float amplitude;
    private readonly List<ValuePair> shake = new List<ValuePair>();
        
    private void Update() {
        while (shake.Count != 0) {
            amplitude = 0;
            amplitude += shake[0].amplitude;
            if (addedTime < shake[0].duration)
                addedTime = shake[0].duration;

            currentTime = addedTime + Time.timeSinceLevelLoad;

            shake.RemoveAt(0);
        }

        if (currentTime > Time.timeSinceLevelLoad) {
            Vector3 euler = transform.localEulerAngles;
            euler.z = amplitude * Random.Range(-10, 10);
            transform.localEulerAngles = euler;

            euler = transform.position;
            euler.x += amplitude * Random.Range(-10, 10) * 0.1f;
            euler.y += amplitude * Random.Range(-10, 10) * 0.1f;
            transform.position = euler;
        }
        else {
            transform.localEulerAngles = Vector3.zero;
        }
    }

    public void AddShake(float amplitude, float duration) {
        ValuePair vp = new ValuePair(amplitude, duration);
        shake.Add(vp);
    }
}
