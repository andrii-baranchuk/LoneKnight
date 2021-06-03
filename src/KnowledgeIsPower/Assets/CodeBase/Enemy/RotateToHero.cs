using UnityEngine;

namespace CodeBase.Enemy
{
  public class RotateToHero : Follow
  {
    public float Speed;

    private Transform _heroTransform;
    private Vector3 _positionToLook;

    public void Construct(Transform heroTransform) => 
      _heroTransform = heroTransform; 

    private void Update()
    {
      if (Initialized())
        RotateTowardsHero();
    }

    private bool Initialized() => 
      _heroTransform != null;

    private void RotateTowardsHero()
    {
      UpdatePositionToLookAt();

      transform.rotation = SmoothedRotation(transform.rotation, _positionToLook);
    }

    private void UpdatePositionToLookAt()
    {
      Vector3 positionDiff = _heroTransform.position - transform.position;
      _positionToLook = new Vector3(positionDiff.x, transform.position.y, positionDiff.z);
    }

    private Quaternion SmoothedRotation(Quaternion rotation, Vector3 positionToLook) => 
      Quaternion.Lerp(rotation, TargerRotation(positionToLook), SpeedFactor());

    private float SpeedFactor() => 
      Speed * Time.deltaTime;

    private Quaternion TargerRotation(Vector3 positionToLook) => 
      Quaternion.LookRotation(positionToLook);
  }
}