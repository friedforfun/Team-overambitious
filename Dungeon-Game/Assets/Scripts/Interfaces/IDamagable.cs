public interface IDamagable
{
    void Damage(int damageTaken);

    void AddDebuff<T>(T debuff) where T : Debuff;
}
