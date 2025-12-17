namespace myclinic_back.Interfaces
{
    public interface ICreateMapping<Tinput, Toutput>
    {
        Task<Toutput> CreateMapping(Tinput input, Toutput output);
    }
}
