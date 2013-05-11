namespace NuCheck
{
    public sealed class Package
    {
        public Package(string id, string version)
        {
            Id = id;
            Version = version;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Package;
            return other != null && string.Equals(Id, other.Id) && string.Equals(Version, other.Version);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() ^ Version.GetHashCode();
        }

        public string Id { get; private set; }
        public string Version { get; private set; }
    }
}