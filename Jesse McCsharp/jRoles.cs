using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
namespace Jesse_McCsharp
{
    class jRoles
    {
        public jRoles(string aName, Role[] someRoles)
        {
            name = aName;
            roles = someRoles;
        }

        string name;
        Role[] roles;

        public Role[] Roles
        {
            get
            {
                return roles;
            }
            set
            {
                roles = value;
            }

        }
        public string Name
        {
            get
            {
                return name;
            }
        }
    }
}
