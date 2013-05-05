namespace NuCheck
{
    public class Package
    {
        public Package(string id, string version)
        {
            Id = id;
            Version = version;
        }

        public string Id { get; private set; }
        public string Version { get; private set; }
    }
}