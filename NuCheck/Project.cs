namespace NuCheck
{
    public class Project
    {
        public Project(string name, string fileName)
        {
            Name = name;
            FileName = fileName;
        }

        public string Name { get; private set; }
        public string FileName { get; private set; }
    }
}