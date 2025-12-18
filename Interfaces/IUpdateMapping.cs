namespace myclinic_back.Interfaces
{
    public interface IUpdateMapping<Tinput, Toutput>
    {
        Task<Toutput> UpdateMapping(Tinput input, Toutput output);
    }
}
