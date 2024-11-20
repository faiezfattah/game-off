using DG.Tweening;

public class Spider : Bouncing
{
    protected override void Start()
    {
        base.Start();
        tween.SetEase(Ease.Linear);
    }
}
