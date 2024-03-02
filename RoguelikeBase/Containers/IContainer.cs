using Arch.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeBase.Containers
{
    internal interface IContainer
    {
        EntityReference CreateForOwner(World world, EntityReference owner);
        EntityReference CreateAtPosition(World world, Point point);
    }
}
