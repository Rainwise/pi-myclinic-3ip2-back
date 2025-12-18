namespace myclinic_back.Interfaces
{
    public interface IMapping : IReadMapping<object, object>, ICreateMapping<object, object>, IUpdateMapping<object, object>, IDeleteService
    {
      
    }
}
