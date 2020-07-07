using UnityEngine;
using System.Collections;

namespace Bolt.AdvancedTutorial
{
	public class LineFade : MonoBehaviour
	{
		[SerializeField] private Color color;

        [SerializeField] private float speed = 10f;

		LineRenderer lr;

		void Start ()
		{
			lr = GetComponent<LineRenderer> ();
		}

		void Update ()
		{
			
			color.a = Mathf.Lerp (color.a, 100, Time.deltaTime * speed);

			
			lr.startColor = color;
			lr.endColor = color;
		}
	}
}
