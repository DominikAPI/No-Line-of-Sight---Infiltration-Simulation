
public class CorpseDetectionResponse : DetectionResponse
{
    public override bool ShouldTriggerAlert(float visibleTime, GuardEntity guard) => true;
}
