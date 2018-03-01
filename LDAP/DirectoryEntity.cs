using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDAP
{
    public abstract class DirectoryEntity
    {
        private DirectoryEntity parent;
        private string name;
        private string path;
        public DirectoryEntity Parent { get => parent; }
        public string Name { get => name; }
        public string Path { get => path;  }

        public DirectoryEntity(DirectoryEntity parent, string name, string path)
        {
            this.parent = parent;
            this.name = name;
            this.path = path;
        }
    }
}
