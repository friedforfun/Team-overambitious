public interface IHealable
{
    void Heal(int healAmount);

    void AddBuff<T>(T buff) where T : Buff;
}
