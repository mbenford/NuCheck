namespace NuCheck
{
    public class Project
    {
        private readonly string name;
        private readonly string fileName;

        public Project(string name, string fileName)
        {
            this.name = name;
            this.fileName = fileName;
        }

        public string Name { get { return name; } }

        public string FileName { get { return fileName; } }
    }
}