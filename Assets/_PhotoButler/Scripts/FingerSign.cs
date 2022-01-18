

using UnityEngine;
using System.Collections;

public class FingerSign : MonoBehaviour {


    [SerializeField]
    private bool _animateScale = true;
    public float ScaleMin = 0.8f;
    public float ScaleMax = 1.2f;
    public float ScaleCycleDuration = 2;

    private Vector3 _startLocalScale;

    private Transform _transform;
    private float _time;

    void Awake() {
        _transform = GetComponent<Transform>();

        _startLocalScale = _transform.localScale;
    }

    void Update() {

        ScaleBehaviour();
    }

    private void ScaleBehaviour()
    {
        if (_animateScale)
        {
            float scale;
            _time += Time.deltaTime;
            if (ScaleCycleDuration != 0)
            {
                float scaleT = Mathf.InverseLerp(-1, 1, Mathf.Sin(_time / ScaleCycleDuration * Mathf.PI * 2));
                scale = Mathf.Lerp(ScaleMin, ScaleMax, scaleT);
            }
            else
            {
                scale = 1;
            }

            this.transform.localScale = scale * _startLocalScale;
        }
    }

    private void OnEnable()
    {
        this.transform.localScale = Vector3.one;
        _startLocalScale = Vector3.one;
        _time = 0f;
    }
}

