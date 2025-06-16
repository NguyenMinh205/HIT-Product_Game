using UnityEngine;

namespace Gameplay
{
    public class PachinkoBox : MonoBehaviour
    {
        [SerializeField] private Transform leftLimit;
        [SerializeField] private Transform rightLimit;
        [SerializeField] private Rigidbody2D basket;
        [SerializeField] private Collider2D basketTrigger;
        [SerializeField] private Collider2D floorTrigger;
        [SerializeField] private float basketSpeed = 5f;

        public Collider2D BasketTrigger => basketTrigger;
        public Collider2D FloorTrigger => floorTrigger;

        private bool _moveRight = true;

        private void FixedUpdate()
        {
            if (basket == null || PachinkoMachine.Instance.State == GameState.Waiting || PachinkoMachine.Instance.State == GameState.Ended) return;

            Vector2 target = _moveRight ? rightLimit.position : leftLimit.position;
            basket.MovePosition(Vector2.MoveTowards(basket.position, target, basketSpeed * Time.fixedDeltaTime));

            if (Vector2.Distance(basket.position, rightLimit.position) < 0.1f)
                _moveRight = false;
            else if (Vector2.Distance(basket.position, leftLimit.position) < 0.1f)
                _moveRight = true;
        }
    }
}