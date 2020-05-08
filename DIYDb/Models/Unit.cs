namespace DIYDb.Models
{
    /// <summary>
    /// Тип измерения товра - кг, шт, м3, м2 и тд
    /// </summary>
    /// ToDo: change to enuum
    public class Unit
    {
        public int UnitId { get; set; }

        public string ShortName { get; set; }

        public string Description { get; set; }
    }
}
