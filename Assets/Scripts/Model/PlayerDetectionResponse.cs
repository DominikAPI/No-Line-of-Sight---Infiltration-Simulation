public class PlayerDetectionResponse : DetectionResponse
{
    public override bool ShouldTriggerAlert(float visibleTime, GuardEntity guard) => visibleTime >= guard.DetectionTime;

}
