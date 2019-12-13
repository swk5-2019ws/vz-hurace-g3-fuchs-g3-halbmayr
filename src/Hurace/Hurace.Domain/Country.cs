namespace Hurace.Domain
{
    public class Country : DomainObjectBase
    {
        public Country(int countryId)
            : base(countryId)
        {
            PropertiesChanged = false;
        }
    }
}