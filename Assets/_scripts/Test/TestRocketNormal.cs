using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace GellosGames
{
    // https://www.mathematik-oberstufe.de/vektoren/a/abstand-punkt-gerade-lfdpkt.html
    public class TestRocketNormal : MonoBehaviour
    {
        public Transform PlayerObj;
        [FormerlySerializedAs("PunktA")]
        public Vector3 P;
        public Vector3 PunktB;

        public Vector3 Scalar;
        [SerializeField]
        private float result;
        [FormerlySerializedAs("directionNextPoint")]
        [SerializeField]
        private Vector3 u;
        [FormerlySerializedAs("directionPlayer")]
        [SerializeField]
        private Vector3 AF;
        [SerializeField]
        private Vector3 t;
        
        void Update()
        {
            var A = transform.position;
            AF = P - A;
            var a = Vector3.Dot(AF, u);
            var b = Vector3.Dot(u, u);
            result = -(a / b);

            t = u  * result + P;
            // AF
            Debug.DrawLine(A, t, Color.black );
            // Gerade
            Debug.DrawLine(P, P + 300 * u, Color.magenta);
        }
    }
}
