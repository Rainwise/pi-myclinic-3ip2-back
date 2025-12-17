namespace myclinic_back.Interfaces
{
    public interface IReadMapping<Tinput,  Toutput>
    {
        Task<Toutput> GetMapping(Tinput input, Toutput output);
    }
}
