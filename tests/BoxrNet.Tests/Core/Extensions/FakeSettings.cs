namespace BoxrNet.Tests.Core.Extensions
{
    public interface IFakeSettings
    {
    }

    public class FakeSettings
    {
        public string Name { get; set; }
    }
    
    public class FakeFakeSettingsWithInterface : FakeSettings, IFakeSettings
    {
    }
}